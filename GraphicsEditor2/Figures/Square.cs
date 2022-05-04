using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using GraphicsEditor2.Common;

namespace GraphicsEditor2.Figures
{
    [Serializable]
    public class Square : Rect, IRotateSupport
    {
        public Square() : base()
        {
        }

        public Square(Point origin, Point offset)
            : base(origin, offset)
        {
        }

        public override List<Marker> CreateSizeMarkers()
        {
            var markers = new List<Marker>
                {
                    new TopMiddleSizeMarker {TargetFigure = this},
                    new MiddleRightSizeMarker {TargetFigure = this},
                    new BottomMiddleSizeMarker {TargetFigure = this},
                    new MiddleLeftSizeMarker {TargetFigure = this}
                };
            return markers;
        }

        public override void UpdateSize(PointF offset, Marker marker)
        {
            var oldrect = CalcFocusRect(PointF.Empty, marker is ISizeMarker ? marker : null);
            var rect = CalcFocusRect(offset, marker);
            var size = (oldrect.Width * oldrect.Height < rect.Width * rect.Height)
                           ? Math.Max(rect.Width, rect.Height)
                           : Math.Min(rect.Width, rect.Height);
            Basicrect.Location = rect.Location;
            Basicrect.Size = new SizeF(size, size);
            if (marker is MiddleRightSizeMarker)
                Basicrect.Y -= (Basicrect.Height - oldrect.Height) / 2;
            else if (marker is BottomMiddleSizeMarker)
                Basicrect.X -= (Basicrect.Width - oldrect.Width) / 2;
            else if (marker is MiddleLeftSizeMarker)
                Basicrect.Y += -(Basicrect.Height - oldrect.Height) / 2;
            else if (marker is TopMiddleSizeMarker)
                Basicrect.X += -(Basicrect.Width - oldrect.Width) / 2;
            UpdateMarkers();
        }

        public override void DrawFocusFigure(Graphics graphics, PointF offset, Marker marker)
        {
            var oldrect = CalcFocusRect(PointF.Empty, marker is ISizeMarker ? marker : null);
            var rect = CalcFocusRect(offset, marker);
            var size = (oldrect.Width * oldrect.Height < rect.Width * rect.Height)
                           ? Math.Max(rect.Width, rect.Height)
                           : Math.Min(rect.Width, rect.Height);
            rect.Size = new SizeF(size, size);
            if (marker is MiddleRightSizeMarker)
                rect.Y -= (rect.Height - oldrect.Height) / 2;
            else if (marker is BottomMiddleSizeMarker)
                rect.X -= (rect.Width - oldrect.Width) / 2;
            else if (marker is MiddleLeftSizeMarker)
                rect.Y += -(rect.Height - oldrect.Height) / 2;
            else if (marker is TopMiddleSizeMarker)
                rect.X += -(rect.Width - oldrect.Width) / 2;
            DrawCustomFigure(graphics, rect);
        }

        public void RotateAt(float angle, float cx, float cy)
        {
            using (var gp = new GraphicsPath())
            {
                //gp.AddRectangle(CalcFocusRect(offset, marker));
                using (var m = new Matrix())
                {
                    m.RotateAt(angle, new PointF(cx, cy));
                    gp.Transform(m);
                }
                var ps = gp.PathPoints;
                SetPoints(ps);
            }
        }
    }
}
