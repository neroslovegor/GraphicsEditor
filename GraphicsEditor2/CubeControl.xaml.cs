using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Diagnostics;
using GraphicsEditor2.ForCube;

namespace GraphicsEditor2
{
    /// <summary>
    /// Логика взаимодействия для CubeControl.xaml
    /// </summary>
    public partial class CubeControl : Window
    {
        public CubeControl()
        {
            InitializeComponent();
        }
        private void Window_Closed(object sender, EventArgs e)
        {
            Hide();
            new MainForm().Show();
        }
        private void Window_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta > 0)
                distanceFactor = distanceFactor - 0.1;

            else if (e.Delta < 0)
                distanceFactor = distanceFactor + 0.1;

            Update();
        }
        private void Update()
        {
            len = edge_len * size + space * (size - 1);

            //IO = new InputOutput(size);

            Point3D cameraPos = new Point3D(len * distanceFactor, len * distanceFactor, len * distanceFactor);
            PerspectiveCamera camera = new PerspectiveCamera(
                cameraPos,
                new Vector3D(-cameraPos.X, -cameraPos.Y, -cameraPos.Z),
                new Vector3D(0, 1, 0),
                45
            );

            this.mainViewport.Camera = camera;
        }

        double distanceFactor = 2.3;
        Point startMoveCamera;
        bool allowMoveCamera = false;
        int size = 3;
        double edge_len = 1;
        double space = 0;
        double len;

        Transform3DGroup rotations = new Transform3DGroup();
        Cubes c;
        HashSet<string> touchedFaces = new HashSet<string>();

        List<KeyValuePair<Move, RotationDirection>> doneMoves = new List<KeyValuePair<Move, RotationDirection>>();
        //InputOutput IO;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Update();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            allowMoveCamera = true;
            startMoveCamera = e.GetPosition(this);
            this.Cursor = Cursors.SizeAll;
        }

        private void Window_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            allowMoveCamera = false;
            this.Cursor = Cursors.Arrow;
        }

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            if (allowMoveCamera)
            {
                moveCamera(e.GetPosition(this));
            }
        }

        private void moveCamera(Point p)
        {
            double distX = p.X - startMoveCamera.X;
            double distY = p.Y - startMoveCamera.Y;

            startMoveCamera = p;

            RotateTransform3D rotationX = new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(1, 0, 0), distY), new Point3D(0, 0, 0));
            RotateTransform3D rotationY = new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 1, 0), distX), new Point3D(0, 0, 0));

            rotations.Children.Add(rotationX);
            rotations.Children.Add(rotationY);
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            init();
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Up)
            {
                if (size <= 6)
                {
                    size = size + 1;
                    init();
                }
                else
                    MessageBox.Show("Остановись!");
            }
            if (e.Key == Key.Down)
            {
                if (size > 3)
                {
                    size = size - 1;
                    init();
                }
                else
                    MessageBox.Show("Остановись!");
            }
            if (e.Key == Key.Left)
            {
                if (space > 0)
                {
                    space = space - 0.05;
                    init();
                }
                else
                    MessageBox.Show("Остановись!");
            }
            if (e.Key == Key.Right)
            {
                if (space < 4)
                {
                    space = space + 0.05;
                    init();
                }
                else
                    MessageBox.Show("Остановись!");
            }
        }

        private void init(string file = null)
        {
            this.mainViewport.Children.Remove(c);

            rotations.Children.Clear();
            doneMoves.Clear();

            c = new Cubes(size, new Point3D(-len / 2, -len / 2, -len / 2), TimeSpan.FromMilliseconds(0), edge_len, space);

            c.Transform = rotations;

            this.mainViewport.Children.Add(c);
        }
    }
}
