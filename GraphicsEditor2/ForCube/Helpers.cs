using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Media3D;
using System.Windows.Media;
using System.Diagnostics;

namespace GraphicsEditor2.ForCube
{
    public static class Helpers
    {
        public static GeometryModel3D createTriangleModel(Point3D p0, Point3D p1, Point3D p2, Material m)
        {
            MeshGeometry3D triangleMesh = new MeshGeometry3D();
            triangleMesh.Positions.Add(p0);
            triangleMesh.Positions.Add(p1);
            triangleMesh.Positions.Add(p2);

            triangleMesh.TriangleIndices.Add(0);
            triangleMesh.TriangleIndices.Add(1);
            triangleMesh.TriangleIndices.Add(2);

            return new GeometryModel3D(triangleMesh, m);
        }

        public static Model3DGroup createRectangleModel(Point3D[] p, Material m, bool up = true)
        {
            if (p.Length != 4)
            {
                return null;
            }

            Model3DGroup rect = new Model3DGroup();

            if (up)
            {
                rect.Children.Add(createTriangleModel(p[0], p[1], p[2], m));
                rect.Children.Add(createTriangleModel(p[0], p[2], p[3], m));
            }
            else
            {
                rect.Children.Add(createTriangleModel(p[0], p[2], p[1], m));
                rect.Children.Add(createTriangleModel(p[0], p[3], p[2], m));
            }

            return rect;
        }
    }
}
