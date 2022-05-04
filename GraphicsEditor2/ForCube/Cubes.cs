using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Media3D;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Diagnostics;
using System.Windows;
using System.Threading;
using System.IO;

namespace GraphicsEditor2.ForCube
{
    public class Cubes : Cube
    {
        /// <summary>
        /// The cube will be size x size x size
        /// </summary>
        private int size;

        private Point3D origin;

        /// <summary>
        /// Length of the cube edge
        /// </summary>
        private double edge_len;

        /// <summary>
        /// Space between the cubes forming the bigger cube
        /// </summary>
        private double space;

        public Cube2D projection;
        private static Random rnd = new Random();


        private Dictionary<CubeFace, Material> faceColors = new Dictionary<CubeFace, Material> {
            {CubeFace.L, new DiffuseMaterial(new SolidColorBrush(Colors.MediumAquamarine))},
            {CubeFace.D, new DiffuseMaterial(new SolidColorBrush(Colors.LightSeaGreen))},
            {CubeFace.B, new DiffuseMaterial(new SolidColorBrush(Colors.LightSkyBlue))},
            {CubeFace.R, new DiffuseMaterial(new SolidColorBrush(Colors.Plum))},
            {CubeFace.U, new DiffuseMaterial(new SolidColorBrush(Colors.CornflowerBlue))},
            {CubeFace.F, new DiffuseMaterial(new SolidColorBrush(Colors.Olive))}
            //{CubeFace.L, new DiffuseMaterial(new SolidColorBrush(Color.FromRgb((byte)rnd.Next(0,255),(byte)rnd.Next(0,255),(byte)rnd.Next(0,255))))},
            //{CubeFace.D, new DiffuseMaterial(new SolidColorBrush(Color.FromRgb((byte)rnd.Next(0,255),(byte)rnd.Next(0,255),(byte)rnd.Next(0,255))))},
            //{CubeFace.B, new DiffuseMaterial(new SolidColorBrush(Color.FromRgb((byte)rnd.Next(0,255),(byte)rnd.Next(0,255),(byte)rnd.Next(0,255))))},
            //{CubeFace.R, new DiffuseMaterial(new SolidColorBrush(Color.FromRgb((byte)rnd.Next(0,255),(byte)rnd.Next(0,255),(byte)rnd.Next(0,255))))},
            //{CubeFace.U, new DiffuseMaterial(new SolidColorBrush(Color.FromRgb((byte)rnd.Next(0,255),(byte)rnd.Next(0,255),(byte)rnd.Next(0,255))))},
            //{CubeFace.F, new DiffuseMaterial(new SolidColorBrush(Color.FromRgb((byte)rnd.Next(0,255),(byte)rnd.Next(0,255),(byte)rnd.Next(0,255))))}
        };

        public Cubes(int size, Point3D o, TimeSpan duration, double len = 1, double space = 0.1)
        {
            this.size = size;
            this.origin = o;
            this.edge_len = len;
            this.space = space;
            this.projection = new Cube2D(size);

            createCube();
        }

        public Cubes(CubeFace[,] projection, int size, Point3D o, TimeSpan duration, double len = 1, double space = 0.1)
        {
            this.size = size;
            this.origin = o;
            this.edge_len = len;
            this.space = space;
            this.projection = new Cube2D(size, projection);

            createCubeFromProjection();
        }

        private void createCubeFromProjection()
        {
            Cube c;
            Dictionary<CubeFace, Material> colors;

            double x_offset, y_offset, z_offset;

            for (int y = 0; y < size; y++)
            {
                for (int z = 0; z < size; z++)
                {
                    for (int x = 0; x < size; x++)
                    {
                        if (y == 1 && x == 1 && z == 1)
                        {
                            continue;
                        }

                        x_offset = (edge_len + space) * x;
                        y_offset = (edge_len + space) * y;
                        z_offset = (edge_len + space) * z;

                        Point3D p = new Point3D(origin.X + x_offset, origin.Y + y_offset, origin.Z + z_offset);

                        colors = setFaceColorsFromProjection(x, y, z, projection.projection);

                        c = new Cube(p, edge_len, colors, getPossibleMoves(x, y, z));
                        this.Children.Add(c);
                    }
                }
            }
        }

        protected override void createCube()
        {
            Cube c;
            Dictionary<CubeFace, Material> colors;

            double x_offset, y_offset, z_offset;

            for (int y = 0; y < size; y++)
            {
                for (int z = 0; z < size; z++)
                {
                    for (int x = 0; x < size; x++)
                    {
                        if (y == 1 && x == 1 && z == 1)
                        {
                            continue;
                        }

                        x_offset = (edge_len + space) * x;
                        y_offset = (edge_len + space) * y;
                        z_offset = (edge_len + space) * z;

                        Point3D p = new Point3D(origin.X + x_offset, origin.Y + y_offset, origin.Z + z_offset);

                        colors = setFaceColors(x, y, z);

                        c = new Cube(p, edge_len, colors, getPossibleMoves(x, y, z));
                        this.Children.Add(c);
                    }
                }
            }
        }

        private HashSet<Move> getPossibleMoves(int x, int y, int z)
        {
            HashSet<Move> moves = new HashSet<Move>();

            if (y == 0)
            {
                moves.Add(Move.D);
            }
            else if (y == size - 1)
            {
                moves.Add(Move.U);
            }
            else
            {
                moves.Add(Move.E);
            }

            if (x == 0)
            {
                moves.Add(Move.L);
            }
            else if (x == size - 1)
            {
                moves.Add(Move.R);
            }
            else
            {
                moves.Add(Move.M);
            }

            if (z == 0)
            {
                moves.Add(Move.B);
            }
            else if (z == size - 1)
            {
                moves.Add(Move.F);
            }
            else
            {
                moves.Add(Move.S);
            }

            return moves;
        }

        public bool isUnscrambled()
        {
            return projection.isUnscrambled();
        }

        private Dictionary<CubeFace, Material> setFaceColors(int x, int y, int z)
        {
            Dictionary<CubeFace, Material> colors = new Dictionary<CubeFace, Material>();

            if (x == 0)
            {
                colors.Add(CubeFace.L, faceColors[CubeFace.L]);
            }

            if (y == 0)
            {
                colors.Add(CubeFace.D, faceColors[CubeFace.D]);
            }

            if (z == 0)
            {
                colors.Add(CubeFace.B, faceColors[CubeFace.B]);
            }

            if (x == size - 1)
            {
                colors.Add(CubeFace.R, faceColors[CubeFace.R]);
            }

            if (y == size - 1)
            {
                colors.Add(CubeFace.U, faceColors[CubeFace.U]);
            }

            if (z == size - 1)
            {
                colors.Add(CubeFace.F, faceColors[CubeFace.F]);
            }

            return colors;
        }

        private Dictionary<CubeFace, Material> setFaceColorsFromProjection(int x, int y, int z, CubeFace[,] p)
        {
            Dictionary<Tuple<int, int, int>, Dictionary<CubeFace, Material>> colors = new Dictionary<Tuple<int, int, int>, Dictionary<CubeFace, Material>> {
                {new Tuple<int, int, int>(0, 0, 0), new Dictionary<CubeFace, Material>{
                    {CubeFace.L, faceColors[p[3,2]]},
                    {CubeFace.B, faceColors[p[2,3]]},
                    {CubeFace.D, faceColors[p[3,3]]},
                }},
                {new Tuple<int, int, int>(1, 0, 0), new Dictionary<CubeFace, Material>{
                    {CubeFace.D, faceColors[p[3,4]]},
                    {CubeFace.B, faceColors[p[2,4]]},
                }},
                {new Tuple<int, int, int>(2, 0, 0), new Dictionary<CubeFace, Material>{
                    {CubeFace.R, faceColors[p[3,6]]},
                    {CubeFace.B, faceColors[p[2,5]]},
                    {CubeFace.D, faceColors[p[3,5]]},
                }},

                {new Tuple<int, int, int>(0, 0, 1), new Dictionary<CubeFace, Material>{
                    {CubeFace.L, faceColors[p[4,2]]},
                    {CubeFace.D, faceColors[p[4,3]]},
                }},
                {new Tuple<int, int, int>(1, 0, 1), new Dictionary<CubeFace, Material>{
                    {CubeFace.D, faceColors[p[4,4]]},
                }},
                {new Tuple<int, int, int>(2, 0, 1), new Dictionary<CubeFace, Material>{
                    {CubeFace.R, faceColors[p[4,6]]},
                    {CubeFace.D, faceColors[p[4,5]]},
                }},

                {new Tuple<int, int, int>(0, 0, 2), new Dictionary<CubeFace, Material>{
                    {CubeFace.L, faceColors[p[5,2]]},
                    {CubeFace.F, faceColors[p[6,3]]},
                    {CubeFace.D, faceColors[p[5,3]]},
                }},
                {new Tuple<int, int, int>(1, 0, 2), new Dictionary<CubeFace, Material>{
                    {CubeFace.F, faceColors[p[6,4]]},
                    {CubeFace.D, faceColors[p[5,4]]},
                }},
                {new Tuple<int, int, int>(2, 0, 2), new Dictionary<CubeFace, Material>{
                    {CubeFace.R, faceColors[p[5,6]]},
                    {CubeFace.F, faceColors[p[6,5]]},
                    {CubeFace.D, faceColors[p[5,5]]},
                }},

                {new Tuple<int, int, int>(0, 1, 0), new Dictionary<CubeFace, Material>{
                    {CubeFace.L, faceColors[p[3,1]]},
                    {CubeFace.B, faceColors[p[1,3]]},
                }},
                {new Tuple<int, int, int>(1, 1, 0), new Dictionary<CubeFace, Material>{
                    {CubeFace.B, faceColors[p[1,4]]},
                }},
                {new Tuple<int, int, int>(2, 1, 0), new Dictionary<CubeFace, Material>{
                    {CubeFace.R, faceColors[p[3,7]]},
                    {CubeFace.B, faceColors[p[1,5]]},
                }},

                {new Tuple<int, int, int>(0, 1, 1), new Dictionary<CubeFace, Material>{
                    {CubeFace.L, faceColors[p[4,1]]},
                }},/*
                {new Tuple<int, int, int>(1, 1, 1), new Dictionary<CubeFace, Material>{
                    {CubeFace.B, faceColors[p[1,4]]}, //empty because we are in the middle of the cube!
                }},*/
                {new Tuple<int, int, int>(2, 1, 1), new Dictionary<CubeFace, Material>{
                    {CubeFace.R, faceColors[p[4,7]]},
                }},

                {new Tuple<int, int, int>(0, 1, 2), new Dictionary<CubeFace, Material>{
                    {CubeFace.L, faceColors[p[5,1]]},
                    {CubeFace.F, faceColors[p[7,3]]},
                }},
                {new Tuple<int, int, int>(1, 1, 2), new Dictionary<CubeFace, Material>{
                    {CubeFace.F, faceColors[p[7,4]]},
                }},
                {new Tuple<int, int, int>(2, 1, 2), new Dictionary<CubeFace, Material>{
                    {CubeFace.R, faceColors[p[5,7]]},
                    {CubeFace.F, faceColors[p[7,5]]},
                }},

                {new Tuple<int, int, int>(0, 2, 0), new Dictionary<CubeFace, Material>{
                    {CubeFace.L, faceColors[p[3,0]]},
                    {CubeFace.B, faceColors[p[0,3]]},
                    {CubeFace.U, faceColors[p[11,3]]},
                }},
                {new Tuple<int, int, int>(1, 2, 0), new Dictionary<CubeFace, Material>{
                    {CubeFace.U, faceColors[p[11,4]]},
                    {CubeFace.B, faceColors[p[0,4]]},
                }},
                {new Tuple<int, int, int>(2, 2, 0), new Dictionary<CubeFace, Material>{
                    {CubeFace.R, faceColors[p[3,8]]},
                    {CubeFace.B, faceColors[p[0,5]]},
                    {CubeFace.U, faceColors[p[11,5]]},
                }},

                {new Tuple<int, int, int>(0, 2, 1), new Dictionary<CubeFace, Material>{
                    {CubeFace.L, faceColors[p[4,0]]},
                    {CubeFace.U, faceColors[p[10,3]]},
                }},
                {new Tuple<int, int, int>(1, 2, 1), new Dictionary<CubeFace, Material>{
                    {CubeFace.U, faceColors[p[10,4]]},
                }},
                {new Tuple<int, int, int>(2, 2, 1), new Dictionary<CubeFace, Material>{
                    {CubeFace.R, faceColors[p[4,8]]},
                    {CubeFace.U, faceColors[p[10,5]]},
                }},

                {new Tuple<int, int, int>(0, 2, 2), new Dictionary<CubeFace, Material>{
                    {CubeFace.L, faceColors[p[5,0]]},
                    {CubeFace.F, faceColors[p[8,3]]},
                    {CubeFace.U, faceColors[p[9,3]]},
                }},
                {new Tuple<int, int, int>(1, 2, 2), new Dictionary<CubeFace, Material>{
                    {CubeFace.U, faceColors[p[9,4]]},
                    {CubeFace.F, faceColors[p[8,4]]},
                }},
                {new Tuple<int, int, int>(2, 2, 2), new Dictionary<CubeFace, Material>{
                    {CubeFace.R, faceColors[p[5,8]]},
                    {CubeFace.F, faceColors[p[8,5]]},
                    {CubeFace.U, faceColors[p[9,5]]},
                }},
            };

            return colors[new Tuple<int, int, int>(x, y, z)];
        }
    }
}
