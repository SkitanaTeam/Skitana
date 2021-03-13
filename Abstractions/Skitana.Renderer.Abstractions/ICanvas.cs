// MIT License - Copyright © Skitana Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.
using Skitana.Renderer.Abstractions.Text;
using System;
using System.Drawing;
using System.Numerics;

namespace Skitana.Renderer.Abstractions
{
    public interface ICanvas : IDisposable
    {
        void RotateDegrees(float degrees, Vector2 origin);
        void RotateRadians(float radians, Vector2 origin);
        void Translate(Vector2 offset);
        void Scale(Vector2 scale, Vector2 origin);
        void Clip(RectangleF clip);

        void PopState();
        void PushState();

        void Clear(Color color);
        void FillRect(RectangleF rect, Color color);
        void FillPolygon(Color color, params Vector2[] points);

        void DrawRect(RectangleF rect, Color color, float lineThickness);
        void DrawLine(Vector2 p1, Vector2 p2, Color color, float lineThickness);

        void DrawText(IFont font, Vector2 position, string text, Color color, TextAlign align);
        void DrawText(IFont font, Rectangle target, string text, Color textColor, TextAlign align);

        void BeginDraw(object platformCanvas);
        void EndDraw();
        void DrawImage(IImage image, Vector2 position, Vector2? scale = null, Color? tint = null);
        void DrawImage(IImage icon, Rectangle target, Rectangle? source = null, Color? tint = null);
    }
}
