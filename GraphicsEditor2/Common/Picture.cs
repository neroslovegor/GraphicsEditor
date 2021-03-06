using GraphicsEditor2.Figures;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;

namespace GraphicsEditor2.Common
{
    public class FigureSelectedEventArgs : EventArgs
    {
        public Figure FigureSelected { get; set; }
    }

    /// <summary>
    /// Режимы редактора
    /// </summary>
    public enum EditorMode
    {
        Mouse,
        Dragging,
        AddLine,
        AddTriangle,
        AddRectangle,
        AddSquare,
        AddCube
    }

    /// <summary>
    /// Описание класса хранилища фигур
    /// </summary>
    [Serializable]
    public class Picture
    {
        public Stroke DefaultStroke = new Stroke();
        public Fill DefaultFill = new Fill();

        /// <summary>
        /// Сброс редактора
        /// </summary>
        public void New()
        {
            _fileName = string.Empty;
            _selected.Clear();
            _figures.Clear();
            DefaultStroke = new Stroke();
            DefaultFill = new Fill();
            _container.Invalidate();
            OnEditorFarConnerUpdated();
        }

        public Point MouseDownLocation { get; private set; }

        private PointF _pasteOffset = Point.Empty;

        private Point _mouseOffset = Point.Empty;
        private Rectangle _ribbonRect;

        public string FileName
        {
            get { return _fileName; }
            set { _fileName = value; }
        }

        private bool _controlPressed;
        private bool _altPressed;

        // список созданных фигур
        private readonly List<Figure> _figures = new List<Figure>();

        // список выбранных фигур
        private readonly ObservableCollection<Figure> _selected = new ObservableCollection<Figure>();

        // контейнер для рисования фигур
        [NonSerialized]
        private readonly Control _container;

        // контейнер для нажатия клавиш
        [NonSerialized]
        private readonly Form _form;
        private bool _vertexChanging;

        /// <summary>
        /// Режим работы редактора
        /// </summary>
        public EditorMode EditorMode { get; set; }

        /// <summary>
        /// Текущий индекс маркера
        /// </summary>
        public Marker CurrentMarker { get; private set; }

        /// <summary>
        /// Режим выбора и перетаскивания узловых точек (изменения узлов)
        /// </summary>
        public bool VertexChanging
        {
            get { return _vertexChanging; }
            set
            {
                _vertexChanging = value;
                _container.Invalidate();
            }
        }

        /// <summary>
        /// Размерный прямоугольник, охватывающий все фигуры
        /// </summary>
        public RectangleF GetBounds
        {
            get
            {
                using (var gp = new GraphicsPath())
                {
                    foreach (var fig in _figures)
                        gp.AddRectangle(fig.Bounds);
                    return gp.GetBounds();
                }
            }
        }

        /// <summary>
        /// Размер всех фигур в совокупности, с учетом смещения от верхнего левого угла
        /// </summary>
        public RectangleF ClientRectangle
        {
            get
            {
                var rect = GetBounds;
                rect.Width += rect.Left;
                rect.Height += rect.Top;
                rect.Location = PointF.Empty;
                return rect;
            }
        }

        /// <summary>
        /// Точка подключения обработчика события выбора
        /// </summary>
        public event EventHandler<FigureSelectedEventArgs> FigureSelected;

        /// <summary>
        /// Точка подключения обработчика события изменения размера
        /// </summary>
        public event EventHandler EditorFarConnerUpdated;

        protected virtual void OnEditorFarConnerUpdated()
        {
            var handler = EditorFarConnerUpdated;
            if (handler != null) handler(this, EventArgs.Empty);
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="container">контейнер для рисования</param>
        public Picture(Control container)
        {
            _container = container;
            // подключаемся к необходимым событиям на контейнере
            _container.MouseDown += ContainerMouseDown;
            _container.MouseMove += ContainerMouseMove;
            _container.MouseUp += ContainerMouseUp;
            _container.Paint += ContainerPaint;
            // а здесь пробуем найти ссылку на форму, на которой расположен PaintBox
            var parent = _container.Parent;
            // пока не найдём форму или пустой Parent
            while (!(parent is Form))
            {
                if (parent == null) break;
                parent = parent.Parent;
            }
            _form = parent as Form;
            // если найдена форма
            if (_form != null)
            {
                // то подключим к ней обработчик нажатия клавиш
                _form.KeyDown += FormKeyDown;
                _form.KeyUp += FormKeyUp;
                // включим признак предварительного просмотра нажатия клавиш
                _form.KeyPreview = true;
            }
            // при изменении выбора выключаем режим изменения узлов
            _selected.CollectionChanged += (sender, args) =>
            {
                VertexChanging = false;
            };
            FileChanged = false;
        }

        /// <summary>
        /// Отражение всех выбранных слева-направо
        /// </summary>
        public void ReflectHorizontally()
        {
            if (_selected.Count > 0) FileChanged = true;
            foreach (var fig in _selected) fig.FlipHorizontal();
            _container.Invalidate();
        }

        /// <summary>
        /// Отражение всех выбранных сверху-вниз
        /// </summary>
        public void ReflectVertically()
        {
            if (_selected.Count > 0) FileChanged = true;
            foreach (var fig in _selected) fig.FlipVertical();
            _container.Invalidate();
        }

        /// <summary>
        /// Поворот всех выбранных налево на четверть
        /// </summary>
        public void TurnRight(float angle)
        {
            if (_selected.Count > 0) FileChanged = true;
            foreach (var fig in _selected) fig.Rotate(angle);
            _container.Invalidate();
        }

        /// <summary>
        /// Поворот всех выбранных направо на четверть
        /// </summary>
        public void TurnLeft(float angle)
        {
            if (_selected.Count > 0) FileChanged = true;
            foreach (var fig in _selected) fig.Rotate(angle * -1);
            _container.Invalidate();
        }

        /// <summary>
        /// Переместить фигуру ниже всех фигур
        /// </summary>
        public void SendToBack()
        {
            if (_selected.Count > 0) FileChanged = true;
            foreach (var fig in _selected)
            {
                _figures.Remove(fig);
                _figures.Insert(0, fig);
            }
            _container.Invalidate();
        }

        /// <summary>
        /// Переместить фигуру выше всех фигур
        /// </summary>
        public void BringToFront()
        {
            if (_selected.Count > 0) FileChanged = true;
            foreach (var fig in _selected)
            {
                _figures.Remove(fig);
                _figures.Add(fig);
            }
            _container.Invalidate();
        }

        // определение типа формата работы с буфером обмена Windows
        [NonSerialized]
        readonly DataFormats.Format _drawsFormat = DataFormats.GetFormat("clipboardVectorFiguresFormat");

        /// <summary>
        /// Вырезать выделенные в буфер обмена
        /// </summary>
        public void CutSelectedToClipboard()
        {
            FileChanged = true;
            _pasteOffset = Point.Empty;
            var forcopy = _selected.ToList();
            var clipboardDataObject = new DataObject(_drawsFormat.Name, forcopy);
            Clipboard.SetDataObject(clipboardDataObject, false);
            foreach (var fig in _selected) _figures.Remove(fig);
            _selected.Clear();
            GC.Collect();
            _container.Invalidate();
        }

        /// <summary>
        /// Копировать выделенные в буфер обмена
        /// </summary>
        public void CopySelectedToClipboard()
        {
            _pasteOffset = Point.Empty;
            var forcopy = _selected.ToList();
            var clipboardDataObject = new DataObject(_drawsFormat.Name, forcopy);
            Clipboard.SetDataObject(clipboardDataObject, false);
        }

        /// <summary>
        /// Признак возможности вставки данных из буфера обмена
        /// </summary>
        public bool CanPasteFromClipboard
        {
            get { return Clipboard.ContainsData(_drawsFormat.Name); }
        }

        /// <summary>
        /// Вставка ранее скопированных фигур из буфера обмена
        /// </summary>
        public void PasteFromClipboardAndSelected()
        {
            if (!Clipboard.ContainsData(_drawsFormat.Name)) return;
            FileChanged = true;
            var clipboardRetrievedObject = Clipboard.GetDataObject();
            if (clipboardRetrievedObject == null) return;
            var pastedObject = (List<Figure>)clipboardRetrievedObject.GetData(_drawsFormat.Name);
            _selected.Clear();
            _pasteOffset = PointF.Add(_pasteOffset, new SizeF(5, 5));
            foreach (var fig in pastedObject)
            {
                fig.Offset(_pasteOffset);
                _figures.Add(fig);
                _selected.Add(fig);
            }
            if (_selected.Count > 0) Focus(_selected[0]);
            _container.Invalidate();
        }

        /// <summary>
        /// Выбрать все фигуры
        /// </summary>
        public void SelectAllFigures()
        {
            _selected.Clear();
            foreach (var fig in _figures)
                _selected.Add(fig);
            _container.Invalidate();
        }

        /// <summary>
        /// Признак изменения данных.
        /// ВНИМАНИЕ! Для правильной работы логики Undo|Redo
        /// изменение этого свойства производить ДО изменения данных!
        /// </summary>
        public bool FileChanged
        {
            get
            {
                return (_fileChanged);
            }
            set
            {
                _fileChanged = value;
                PrepareToUndo(_fileChanged);
                PrepareToRedo(false);
            }
        }

        readonly StackMemory _undoStack = new StackMemory(100);
        readonly StackMemory _redoStack = new StackMemory(100);

        private bool _fileChanged;
        private string _fileName = string.Empty;
        private Point _pt1;
        private Point _pt2;

        /// <summary>
        /// Подготовка к отмене (сохранения состояния)
        /// </summary>
        /// <param name="changed">True - cохранить состояние</param>
        private void PrepareToUndo(bool changed)
        {
            if (changed)
            {
                using (var stream = new MemoryStream())
                {
                    SaveToStream(stream);
                    _undoStack.Push(stream);
                }
            }
            else
                _undoStack.Clear();
        }

        /// <summary>
        /// Подготовка к возврату (сохранения состояния)
        /// </summary>
        /// <param name="changed">True - cохранить состояние</param>
        private void PrepareToRedo(bool changed)
        {
            if (changed)
            {
                using (var stream = new MemoryStream())
                {
                    SaveToStream(stream);
                    _redoStack.Push(stream);
                }
            }
            else
                _redoStack.Clear();
        }

        /// <summary>
        /// Возможность возврата после отмены
        /// </summary>
        /// <returns>True - возврат возможен</returns>
        public bool CanRedoChanges
        {
            get { return (_redoStack.Count > 0); }
        }

        /// <summary>
        /// Возврат после отмены
        /// </summary>
        public void RedoChanges()
        {
            if (!CanRedoChanges) return;
            PrepareToUndo(true);
            _selected.Clear();
            _figures.Clear();
            GC.Collect();
            using (var stream = new MemoryStream())
            {
                _redoStack.Pop(stream);
                var list = LoadFromStream(stream);
                foreach (var fig in list) _figures.Add(fig);
            }
            _container.Invalidate();
        }

        /// <summary>
        /// Возможность отмены действий, изменений
        /// </summary>
        /// <returns>Отмена возможна</returns>
        public bool CanUndoChanges
        {
            get { return (_undoStack.Count > 0); }
        }

        /// <summary>
        /// Отмена действий, изменений
        /// </summary>
        public void UndoChanges()
        {
            if (!CanUndoChanges) return;
            PrepareToRedo(true);
            _selected.Clear();
            _figures.Clear();
            GC.Collect();
            using (var stream = new MemoryStream())
            {
                _undoStack.Pop(stream);
                var list = LoadFromStream(stream);
                foreach (var fig in list) _figures.Add(fig);
            }
            _container.Invalidate();
        }

        /// <summary>
        /// Сохранить все фигуры в поток
        /// </summary>
        /// <param name="stream">поток в памяти</param>
        /// <param name="listToSave">список для сохранения</param>
        private void SaveToStream(Stream stream, List<Figure> listToSave = null)
        {
            var formatter = new BinaryFormatter();
            var list = (listToSave ?? _figures).ToList();
            formatter.Serialize(stream, list);
            stream.Position = 0;
        }

        /// <summary>
        /// Восстановить фигуры из потока в памяти
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        private static IEnumerable<Figure> LoadFromStream(Stream stream)
        {
            try
            {
                var formatter = new BinaryFormatter();
                stream.Position = 0;
                return (List<Figure>)formatter.Deserialize(stream);
            }
            catch (SerializationException e)
            {
                Console.WriteLine(@"Failed to deserialize. Reason: " + e.Message);
                throw;
            }
        }

        /// <summary>
        /// Во внешней форме нажата клавиша
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormKeyDown(object sender, KeyEventArgs e)
        {
            // проверяем нажатие Ctrl
            if (e.Control) _controlPressed = true;
            // проверяем нажатие Alt
            if (e.Alt) _altPressed = true;
            float step = 0;
            if (_controlPressed) step = 1;  // точное позиционирование
            if (_altPressed) step = 10;     // быстрое позиционирование
            switch (e.KeyCode)
            {
                case Keys.Up:
                    FileChanged = true;
                    foreach (var fig in _selected)
                        fig.Offset(new PointF(0, -step));
                    _container.Invalidate();
                    break;
                case Keys.Down:
                    FileChanged = true;
                    foreach (var fig in _selected)
                        fig.Offset(new PointF(0, step));
                    _container.Invalidate();
                    break;
                case Keys.Left:
                    FileChanged = true;
                    foreach (var fig in _selected)
                        fig.Offset(new PointF(-step, 0));
                    _container.Invalidate();
                    break;
                case Keys.Right:
                    FileChanged = true;
                    foreach (var fig in _selected)
                        fig.Offset(new PointF(step, 0));
                    _container.Invalidate();
                    break;
                case Keys.Delete:
                    if ((_selected.Count > 0) &&
                        (MessageBox.Show(@"Удалить выделенные объекты?", @"Редактор фигур",
                                         MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question,
                                         MessageBoxDefaultButton.Button3) == DialogResult.Yes))
                    {
                        FileChanged = true;
                        foreach (var fig in _selected) _figures.Remove(fig);
                        _selected.Clear();
                        GC.Collect();
                        _container.Invalidate();
                    }
                    break;
            }
        }

        /// <summary>
        /// Во внешней форме отпущена клавиша
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormKeyUp(object sender, KeyEventArgs e)
        {
            // проверяем отпускание Ctrl
            if (!e.Control) _controlPressed = false;
            // проверяем отпускание Alt
            if (e.Alt) _altPressed = false;
        }

        /// <summary>
        /// Поиск верхней фигуры под курсором
        /// </summary>
        /// <param name="location">точка нажатия</param>
        /// <returns>найденая фигура</returns>
        private Figure PointInFigure(PointF location)
        {
            // просматриваем с конца списка, так как последние нарисованные фигуры вверху
            for (var i = _figures.Count - 1; i >= 0; i--)
            {
                // смотрим на все фигуры, начиная с хвоста списка
                var fig = _figures[i];
                // если точка не попала в фигуру, то берём следующую
                if (!fig.PointInFigure(location)) continue;
                return fig;
            }
            return null;
        }

        /// <summary>
        /// Проверка нажатия на маркер в фигуре
        /// положительные индексы - это маркеры размеров,
        /// отрицательные - маркеры узлов,
        /// ноль - тело фигуры
        /// </summary>
        /// <param name="location">точка выбора</param>
        /// <param name="figure">указатель на фигуру</param>
        /// <returns>индекс маркера</returns>
        private Marker PointInMarker(PointF location, out Figure figure)
        {
            figure = null;
            // проверка нажатия на маркерах
            for (var i = _selected.Count - 1; i >= 0; i--)
            {
                // смотрим на выбранные фигуры, начиная с хвоста списка
                var fig = _selected[i];
                var found = fig.MarkerSelected(location, VertexChanging);
                if (found == null) continue;
                figure = fig;
                return found;
            }
            return null;
        }

        /// <summary>
        /// Нажатие кнопки "мышки" на PaintBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ContainerMouseDown(object sender, MouseEventArgs e)
        {
            // запоминаем точку первую точку для прямоугольника выбора
            MouseDownLocation = _pt1 = _pt2 = e.Location;
            _mouseOffset = Point.Empty;
            _ribbonRect = Rectangle.Empty;

            // если установлен другой режим, кроме выбора прямоугольником
            if (EditorMode != EditorMode.Mouse)
                _selected.Clear();   // очищаем список выбранных
            // ищем маркер в точке нажатия
            Figure fig;
            CurrentMarker = PointInMarker(e.Location, out fig);
            // ищем фигуру в точке нажатия
            if (fig == null) fig = PointInFigure(e.Location);
            Focus(fig);
            if (e.Button == MouseButtons.Left)
            {
                _container.Capture = true;
                switch (EditorMode)
                {
                    case EditorMode.Mouse:
                        CheckChangeSelection(e.Location);
                        if (fig != null)
                            EditorMode = EditorMode.Dragging;
                        break;
                }
                return;
            }
            if (e.Button != MouseButtons.Right) return;
            CheckChangeSelection(e.Location);
            // переключаем режим на выбор рамкой
            EditorMode = EditorMode.Mouse;
            // просим перерисовать
            _container.Invalidate();
        }

        /// <summary>
        /// Проверка возможности выбора фигур
        /// </summary>
        public bool CanSelectFigures
        {
            get { return !VertexChanging && _figures.Count > 0; }
        }

        /// <summary>
        /// Проверка возможности работы, когда выбрана только одна фигура
        /// </summary>
        public bool CanOneFigureOperation
        {
            get { return !VertexChanging && _selected.Count == 1; }
        }

        private Figure _lastFocused;

        private void Focus(Figure figure)
        {
            if (figure == _lastFocused) return;
            _lastFocused = figure;
            OnFigureSelected(new FigureSelectedEventArgs
            {
                FigureSelected = figure
            });
        }

        /// <summary>
        /// Проверка попадания на фигуру и выбор или отмена выбора фигуры
        /// </summary>
        /// <param name="location">позиция выбора "мышкой"</param>
        private void CheckChangeSelection(Point location)
        {
            Figure fig;
            PointInMarker(location, out fig);
            // ищем фигуру в точке нажатия
            if (fig == null)
                fig = PointInFigure(location);
            // если фигура найдена
            if (fig != null)
            {
                // и если нажат Ctrl на клавиатуре
                if (_controlPressed)
                {
                    // если найденная фигура уже была в списке выбранных
                    var foundIndex = _selected.IndexOf(fig);
                    if (foundIndex >= 0)
                    {
                        // удаление из списка уже выделенного элемента
                        if (_selected.Count > 1) // последний элемент при Ctrl не убирается
                        {
                            Focus(foundIndex == 0 ? _selected[foundIndex + 1] : _selected[foundIndex - 1]);
                            _selected.Remove(fig);
                        }
                        else
                            Focus(fig);
                    }
                    else
                    {
                        _selected.Add(fig); // иначе добавление к списку
                        Focus(fig);
                    }
                }
                else // работаем без нажатия Ctrl на клавиатуре
                {
                    // если фигуры не было в списке выбранных, то
                    if (!_selected.Contains(fig))
                    {
                        _selected.Clear(); // очистка списков
                        _selected.Add(fig); // выделение одного элемента
                        Focus(fig);
                    }
                }
                // просим перерисовать контейнер
                _container.Invalidate();
            }
            else // никакая фигура не была найдена 
            {
                _selected.Clear(); // очистка списков     
                Focus(null);
                _container.Invalidate();
            }
        }

        /// <summary>
        /// Перемещение мышки над PaintBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ContainerMouseMove(object sender, MouseEventArgs e)
        {
            if (MouseDownLocation == e.Location) return;
            // если удерживается левая кнопка и мышка захвачена
            if (e.Button == MouseButtons.Left && _container.Capture)
            {
                // пересчитываем смещение мышки
                _mouseOffset.X = e.X - MouseDownLocation.X;
                _mouseOffset.Y = e.Y - MouseDownLocation.Y;
                // нормализация параметров для прямоугольника выбора
                // в случае, если мы "растягиваем" прямоугольник не только по "главной" диагонали
                _ribbonRect.X = Math.Min(MouseDownLocation.X, e.Location.X);
                _ribbonRect.Y = Math.Min(MouseDownLocation.Y, e.Location.Y);
                // размеры должны быть всегда положительные числа
                _ribbonRect.Width = Math.Abs(MouseDownLocation.X - e.Location.X);
                _ribbonRect.Height = Math.Abs(MouseDownLocation.Y - e.Location.Y);
                _pt1 = MouseDownLocation;
                _pt2 = e.Location;
                // просим перерисовать
                _container.Invalidate();
            }
            if (e.Button != MouseButtons.None) return;
            Figure fig;
            var marker = PointInMarker(e.Location, out fig);
            _container.Cursor = marker != null ? marker.Cursor : Cursors.Default;
        }

        /// <summary>
        /// Отпускание кнопки мышки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ContainerMouseUp(object sender, MouseEventArgs e)
        {
            // если мышь была захвачена
            if (!_container.Capture) return;
            // освобождаем захват мышки
            _container.Capture = false;
            // если нажата левая кнопка
            if (e.Button == MouseButtons.Left)
            {
                var rect = _ribbonRect;
                Figure figure;
                switch (EditorMode)
                {
                    case EditorMode.Mouse:
                        // нормализация параметров для прямоугольника выбора
                        // добавляем все фигуры, которые оказались охваченными прямоугольником выбора
                        // в список выбранных фигур
                        foreach (var fig in _figures.Where(fig => rect.Contains(Rectangle.Ceiling(fig.Bounds))))
                            _selected.Add(fig);
                        if (_selected.Count > 0) Focus(_selected[0]);
                        break;
                    // перетаскивание выбранных фигур "мышкой"
                    case EditorMode.Dragging:
                        if (CurrentMarker == null)
                        {
                            FileChanged = true;
                            // перебираем все выделенные фигуры и смещаем
                            foreach (var fig in _selected)
                                fig.UpdateLocation(_mouseOffset);
                            _container.Invalidate();
                            OnEditorFarConnerUpdated();
                        }
                        else if (CurrentMarker is ISizeMarker) // тянут за размерный маркер
                        {
                            FileChanged = true;
                            // перебираем все выделенные фигуры и меняем размер
                            foreach (var fig in _selected)
                                fig.UpdateSize(_mouseOffset, CurrentMarker);
                            _container.Invalidate();
                            OnEditorFarConnerUpdated();
                        }
                        else if ((CurrentMarker is VertexLocationMarker) && _selected.Count == 1) // тянут за маркер узла
                        {
                            FileChanged = true;
                            var fig = _selected[0];
                            fig.UpdateSize(_mouseOffset, CurrentMarker);
                            _container.Invalidate();
                            OnEditorFarConnerUpdated();
                        }
                        break;
                    //TODO Сделать что-то с этой "лестницей"
                    case EditorMode.AddLine:
                        figure = new Line(MouseDownLocation, _mouseOffset)
                        {
                            Stroke = (Stroke)DefaultStroke.Clone()
                        };
                        AddFigure(figure);
                        OnEditorFarConnerUpdated();
                        break;
                    case EditorMode.AddTriangle:
                        figure = new Triangle(rect.Location, new Point(rect.Width, rect.Height))
                        {
                            Stroke = (Stroke)DefaultStroke.Clone(),
                            Fill = (Fill)DefaultFill.Clone()
                        };
                        AddFigure(figure);
                        OnEditorFarConnerUpdated();
                        break;
                    case EditorMode.AddRectangle:
                        figure = new RectLine(rect.Location, new Point(rect.Width, rect.Height))
                        {
                            Stroke = (Stroke)DefaultStroke.Clone(),
                            Fill = (Fill)DefaultFill.Clone()
                        };
                        AddFigure(figure);
                        OnEditorFarConnerUpdated();
                        break;
                    case EditorMode.AddSquare:
                        figure = new Square(rect.Location,
                            new Point(Math.Min(rect.Width, rect.Height), Math.Min(rect.Width, rect.Height)))
                        {
                            Stroke = (Stroke)DefaultStroke.Clone(),
                            Fill = (Fill)DefaultFill.Clone()
                        };
                        AddFigure(figure);
                        OnEditorFarConnerUpdated();
                        break;
                }
            }
            // возвращаем режим
            EditorMode = EditorMode.Mouse;
            // обнуление прямоугольника выбора
            _ribbonRect = Rectangle.Empty;
            _pt1 = _pt2 = Point.Empty;
            _container.Invalidate();
        }

        /// <summary>
        /// Обработчик события рисования на поверхности контейнера
        /// </summary>
        /// <param name="sender">визуальный компонент с поверхностью для рисования</param>
        /// <param name="e">объект параметров события со свойством Graphics</param>
        private void ContainerPaint(object sender, PaintEventArgs e)
        {
            // рисуем все созданные фигуры
            foreach (var fig in _figures)
                fig.Draw(e.Graphics);
            if (EditorMode != EditorMode.Dragging)
            {
                if (VertexChanging)
                    // маркеры узлов рисуем круглыми
                    foreach (var figure in _selected.OfType<IVertexSupport>())
                        figure.DrawVertexMarkers(e.Graphics);
                else
                    // рисуем маркеры размеров у выбранных фигур
                    foreach (var fig in _selected)
                        fig.DrawSizeMarkers(e.Graphics);
                if (_ribbonRect.IsEmpty) return;
                // рисуем рамку прямоугольника выбора
                using (var pen = new Pen(Color.Black))
                {
                    switch (EditorMode)
                    {
                        case EditorMode.Mouse:
                            pen.DashStyle = DashStyle.Dot;
                            e.Graphics.DrawRectangle(pen, _ribbonRect);
                            break;
                        case EditorMode.AddLine:
                            pen.DashStyle = DashStyle.Solid;
                            e.Graphics.DrawLine(pen, _pt1, _pt2);
                            break;
                        case EditorMode.AddTriangle:
                            var triangle = new Rectangle(_ribbonRect.Location, new Size(_ribbonRect.Width, _ribbonRect.Height));
                            List<PointF> points = new List<PointF>
                            {
                                new PointF(triangle.Left + triangle.Width / 2, triangle.Top),
                                new PointF(triangle.Left, triangle.Top + triangle.Height),
                                new PointF(triangle.Right, triangle.Top + triangle.Height),
                                new PointF(triangle.Left + triangle.Width / 2, triangle.Top)
                            };
                            e.Graphics.FillPolygon(Brushes.White, points.ToArray());
                            pen.DashStyle = DashStyle.Solid;
                            e.Graphics.DrawLines(pen, points.ToArray());
                            break;
                        case EditorMode.AddRectangle:
                            e.Graphics.FillRectangle(Brushes.White, _ribbonRect);
                            pen.DashStyle = DashStyle.Solid;
                            e.Graphics.DrawRectangle(pen, _ribbonRect);
                            break;
                        case EditorMode.AddSquare:
                            var square = new Rectangle(_ribbonRect.Location,
                                                       new Size(Math.Min(_ribbonRect.Width, _ribbonRect.Height),
                                                                Math.Min(_ribbonRect.Width, _ribbonRect.Height)));
                            e.Graphics.FillRectangle(Brushes.White, square);
                            pen.DashStyle = DashStyle.Solid;
                            e.Graphics.DrawRectangle(pen, square);
                            break;
                    }
                }
                return;
            }
            // при перетаскивании
            foreach (var fig in _selected)
                fig.DrawFocusFigure(e.Graphics, _mouseOffset, CurrentMarker);
        }

        /// <summary>
        /// Добавить фигуру в список
        /// </summary>
        /// <param name="figure">объект фигуры</param>
        public void AddFigure(Figure figure)
        {
            FileChanged = true;
            _figures.Add(figure);
            figure.UpdateMarkers();
            _selected.Clear();
            _selected.Add(figure);
            Focus(figure);
            _container.Invalidate();
        }

        /// <summary>
        /// Метод инициации события по окончании процесса выбора фигуры
        /// </summary>
        /// <param name="e">объект параметров события со свойством DrawingSelected</param>
        protected virtual void OnFigureSelected(FigureSelectedEventArgs e)
        {
            // если на событие подписались, то вызываем его
            if (FigureSelected != null)
                FigureSelected(this, e);
        }

        /// <summary>
        /// Метод записи фигур в файл
        /// </summary>
        /// <param name="fileName"></param>
        public void SaveToFile(string fileName)
        {
            using (var stream = new MemoryStream())
            {
                SaveToStream(stream);
                File.WriteAllBytes(fileName, stream.GetBuffer());
            }
            _fileName = fileName;
            FileChanged = false;
        }

        /// <summary>
        /// Метод загрузки фигур из файла
        /// </summary>
        /// <param name="fileName"></param>
        public void LoadFromFile(string fileName)
        {
            _fileName = fileName;
            using (var stream = new MemoryStream())
            {
                _selected.Clear();
                var buff = File.ReadAllBytes(fileName);
                stream.Write(buff, 0, buff.Length);
                stream.Position = 0;
                var list = LoadFromStream(stream);
                _figures.Clear();
                foreach (var fig in list) _figures.Add(fig);
            }
            FileChanged = false;
            _container.Invalidate();
            OnEditorFarConnerUpdated();
        }

        /// <summary>
        /// Метод морфинга между фигурами
        /// </summary>
        /// <param name="t"></param>
        public void Morphing(float t)
        {
            if (_selected.Count == 2)
            {
                var d = _selected[0].GetPoints().Length;
                var g = _selected[1].GetPoints().Length;
                if (_selected[0].GetPoints().Length == _selected[1].GetPoints().Length)
                {
                    switch (_selected[0].GetPoints().Length)
                    {
                        case 2:
                            {
                                var masV1 = _selected[0].GetPoints();
                                var masV2 = _selected[1].GetPoints();
                                var points = new List<PointF>
                                {
                                    new PointF((1 - t) * masV1[0].X + t * masV2[0].X, (1 - t) * masV1[0].Y + t * masV2[0].Y),
                                    new PointF((1 - t) * masV1[1].X + t * masV2[1].X, (1 - t) * masV1[1].Y + t * masV2[1].Y)
                                };
                                Figure figure = new Line(points)
                                {
                                    Stroke = (Stroke)DefaultStroke.Clone()
                                };
                                AddFigure(figure);
                                OnEditorFarConnerUpdated();
                            }
                            break;
                        case 3:
                            {
                                var masV1 = _selected[0].GetPoints();
                                var masV2 = _selected[1].GetPoints();
                                var points = new List<PointF>
                                {
                                    new PointF((1 - t) * masV1[0].X + t * masV2[0].X, (1 - t) * masV1[0].Y + t * masV2[0].Y),
                                    new PointF((1 - t) * masV1[1].X + t * masV2[1].X, (1 - t) * masV1[1].Y + t * masV2[1].Y),
                                    new PointF((1 - t) * masV1[2].X + t * masV2[2].X, (1 - t) * masV1[2].Y + t * masV2[2].Y),
                                    new PointF((1 - t) * masV1[0].X + t * masV2[0].X, (1 - t) * masV1[0].Y + t * masV2[0].Y)
                                };
                                Figure figure = new Triangle(points)
                                {
                                    Stroke = (Stroke)DefaultStroke.Clone(),
                                    Fill = (Fill)DefaultFill.Clone()
                                };
                                AddFigure(figure);
                                OnEditorFarConnerUpdated();
                            }
                            break;
                        case 4:
                            {
                                var masV1 = _selected[0].GetPoints();
                                var masV2 = _selected[1].GetPoints();
                                var points = new List<PointF>
                                {
                                    new PointF((1 - t) * masV1[0].X + t * masV2[0].X, (1 - t) * masV1[0].Y + t * masV2[0].Y),
                                    new PointF((1 - t) * masV1[1].X + t * masV2[1].X, (1 - t) * masV1[1].Y + t * masV2[1].Y),
                                    new PointF((1 - t) * masV1[2].X + t * masV2[2].X, (1 - t) * masV1[2].Y + t * masV2[2].Y),
                                    new PointF((1 - t) * masV1[3].X + t * masV2[3].X, (1 - t) * masV1[3].Y + t * masV2[3].Y)
                                };
                                Figure figure = new RectLine(points)
                                {
                                    Stroke = (Stroke)DefaultStroke.Clone(),
                                    Fill = (Fill)DefaultFill.Clone()
                                };
 
                                AddFigure(figure);
                                OnEditorFarConnerUpdated();
                            }
                            break;
                    }
                }
                else
                    MessageBox.Show("Выберите две фигуры одного типа!");
            }
            else
                MessageBox.Show("Выберите две фигуры!");
        }
    }
}
