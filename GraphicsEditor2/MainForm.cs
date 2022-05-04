using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using GraphicsEditor2.Common;
using GraphicsEditor2.Figures;

namespace GraphicsEditor2
{
    public partial class MainForm : Form
    {
        private readonly Picture _editor;
        private Figure _focusedfigure;
        public MainForm()
        {
            InitializeComponent();
            // создаём хранилище созданных фигур, которое также и рисует их
            _editor = new Picture(pbCanvas);
            _editor.FigureSelected += EditorFigureSelected;
            _editor.EditorFarConnerUpdated += EditorFarConnerUpdated;
        }
        #region tsbButton
        private void tsbOpen_Click(object sender, EventArgs e)
        {
            if (_editor.FileChanged &&
                (!_editor.FileChanged || MessageBox.Show(@"Есть не сохранённые данные! Открыть новый файл?",
                                                         @"Редактор фигур",
                                                         MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question,
                                                         MessageBoxDefaultButton.Button3) != DialogResult.Yes)) return;
            if (openFiguresFileDialog.ShowDialog(this) != DialogResult.OK) return;
            _editor.LoadFromFile(openFiguresFileDialog.FileName);
            Text = "Graphics Editor" + " - " + _editor.FileName;
        }
        private void tsbSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_editor.FileName))
            {
                if (saveFiguresFileDialog.ShowDialog(this) != DialogResult.OK) return;
                _editor.SaveToFile(saveFiguresFileDialog.FileName);
                Text = "Graphics Editor" + " - " + _editor.FileName;
            }
            else
                _editor.SaveToFile(_editor.FileName);
        }
        private void tsbUndo_Click(object sender, EventArgs e)
        {
            _editor.UndoChanges();
        }
        private void tsbRedo_Click(object sender, EventArgs e)
        {
            _editor.RedoChanges();
        }
        private void tsbCut_Click(object sender, EventArgs e)
        {
            _editor.CutSelectedToClipboard();
        }
        private void tsbCopy_Click(object sender, EventArgs e)
        {
            _editor.CopySelectedToClipboard();
        }
        private void tsbPaste_Click(object sender, EventArgs e)
        {
            _editor.PasteFromClipboardAndSelected();
        }
        private void tsbSelectAll_Click(object sender, EventArgs e)
        {
            _editor.SelectAllFigures();
        }
        private void tsbSelectMode_Click(object sender, EventArgs e)
        {
            foreach (var item in tsbModes.Items)
            {
                if (item is ToolStripButton)
                {
                    ToolStripButton obj = (ToolStripButton)item;
                    obj.Checked = false;
                }
            }
            ((ToolStripButton)sender).Checked = true;
            if (tsbMouse.Checked)
            {
                _editor.EditorMode = EditorMode.Mouse;
            }
            else if (tsbLine.Checked)
            {
                _editor.EditorMode = EditorMode.AddLine;
            }
            else if (tsbTriangle.Checked)
            {
                _editor.EditorMode = EditorMode.AddTriangle;
            }
            else if (tsbRectangle.Checked)
            {
                _editor.EditorMode = EditorMode.AddRectangle;
            }
            else if (tsbCube.Checked)
            {
                _editor.EditorMode = EditorMode.AddCube;
            }
        }

        private void tsbBringToFront_Click(object sender, EventArgs e)
        {
            _editor.BringToFront();
        }
        private void tsbSendToBack_Click(object sender, EventArgs e)
        {
            _editor.SendToBack();
        }
        private void tsbTurnLeft_Click(object sender, EventArgs e)
        {
            try
            {
                float angle = Convert.ToInt32(tsbAngle.Text);
                if (angle > 0)
                {
                    if (angle <= 360)
                    {
                        _editor.TurnLeft(angle);
                    }
                    else
                    {
                        tsbAngle.Text = "360";
                        angle = 360;
                        _editor.TurnLeft(angle);
                    }
                }
                else
                {
                    if (angle * -1 <= 360)
                    {
                        tsbAngle.Text = $"{angle * -1}";
                        _editor.TurnLeft(angle * -1);
                    }
                    else
                    {
                        tsbAngle.Text = "360";
                        angle = 360;
                        _editor.TurnLeft(angle);
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Угол введен неверно!", "Редактор фигур");
            }
        }
        private void tsbTurnRight_Click(object sender, EventArgs e)
        {
            try
            {
                float angle = Convert.ToInt32(tsbAngle.Text);
                if (angle > 0)
                {
                    if (angle <= 360)
                    {
                        _editor.TurnRight(angle);
                    }
                    else
                    {
                        tsbAngle.Text = "360";
                        angle = 360;
                        _editor.TurnRight(angle);
                    }
                }
                else
                {
                    if (angle * -1 <= 360)
                    {
                        tsbAngle.Text = $"{angle * -1}";
                        _editor.TurnRight(angle * -1);
                    }
                    else
                    {
                        tsbAngle.Text = "360";
                        angle = 360;
                        _editor.TurnRight(angle);
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Угол введен неверно!", "Редактор фигур");
            }
        }
        private void tsbReflectVertically_Click(object sender, EventArgs e)
        {
            _editor.ReflectVertically();
        }
        private void tsbReflectHorizontally_Click(object sender, EventArgs e)
        {
            _editor.ReflectHorizontally();
        }
        private void tsbMorphing_Click(object sender, EventArgs e)
        {
            try
            {
                float t = (float)Convert.ToDouble(tsbT.Text);
                if (t >= 0)
                {
                    if (t <= 1)
                    {
                        _editor.Morphing(t);
                    }
                    else
                    {
                        tsbT.Text = "1";
                        t = 1;
                        _editor.Morphing(t);
                    }
                }
                else
                {
                    tsbT.Text = "0";
                    t = 0;
                    _editor.Morphing(t);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Коэф. введен неверно!", "Редактор фигур");
            }
        }
        #endregion

        private void MainForm_Resize(object sender, EventArgs e)
        {
            UpdateCanvasSize();
        }
        private void UpdateCanvasSize()
        {
            var editrect = _editor.ClientRectangle;
            pbCanvas.Size = Size.Ceiling(editrect.Size);
            var rect = panelForScroll.ClientRectangle;
            if (pbCanvas.Width < rect.Width) pbCanvas.Width = rect.Width;
            if (pbCanvas.Height < rect.Height) pbCanvas.Height = rect.Height;
        }

        #region Обработчики
        /// <summary>
        /// Обработчик смены выбора фигур
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void EditorFigureSelected(object sender, FigureSelectedEventArgs e)
        {
            var figure = _focusedfigure = e.FigureSelected;
            if (figure == null) return;
            _editor.DefaultStroke = (Stroke)figure.Stroke.Clone();
            if (figure is ISolidFigure)
                _editor.DefaultFill = (Fill)figure.Fill.Clone();
        }
        void EditorFarConnerUpdated(object sender, EventArgs e)
        {
            UpdateCanvasSize();
        }
        #endregion

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.C)
            {
                _editor.CopySelectedToClipboard();
            }
            if (e.Control && e.KeyCode == Keys.V)
            {
                _editor.PasteFromClipboardAndSelected();
            }
        }

        private void tsbCube_Click(object sender, EventArgs e)
        {
            new CubeControl().Show();
            Hide();
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
