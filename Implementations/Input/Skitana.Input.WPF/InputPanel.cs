// MIT License - Copyright © Skitana Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.
using Skitana.App.Framework.Abstractions;
using Skitana.Input.Abstractions.Pointers;
using System;
using System.Numerics;
using System.Windows;
using System.Windows.Input;

using MouseWheelEventArgs = Skitana.Input.Abstractions.Pointers.MouseWheelEventArgs;

namespace Skitana.Input.WPF
{
    internal class InputPanel : IInputPanel
    {
        public event EventHandler<PointerEventArgs> PointerDown;
        public event EventHandler<PointerEventArgs> PointerUp;
        public event EventHandler<PointerEventArgs> PointerLost;
        public event EventHandler<PointerEventArgs> PointerMove;
        public event EventHandler<MouseWheelEventArgs> MouseWheel;

        private long FirstMouseButton = 0;
        private readonly FrameworkElement element;
        private readonly IApplicationStopwatch applicationStopwatch;

        private TimeSpan CurrentTime => applicationStopwatch.ElapsedTime;
        public Vector2 Scale { set; private get; }

        public InputPanel(FrameworkElement element, IApplicationStopwatch applicationStopwatch)
        {
            element.MouseMove += Element_MouseMove;
            element.MouseDown += Element_MouseDown;
            element.MouseUp += Element_MouseUp;
            element.MouseLeave += Element_MouseLeave;
            element.MouseWheel += Element_MouseWheel;
            this.element = element;
            this.applicationStopwatch = applicationStopwatch;
        }

        private void Element_MouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs args)
        {
            var position = GetPosition(args);
            var buttons = GetMouseButtons(args);
            var delta = args.Delta;

            MouseWheel?.Invoke(this, new MouseWheelEventArgs(CurrentTime, position, PointerId.FromMouse(PointerId.MouseNoButton), buttons, delta));
        }

        private void Element_MouseLeave(object sender, MouseEventArgs args)
        {
            var position = GetPosition(args);
            var buttons = GetMouseButtons(args);

            if ((buttons & PointerId.MouseLeftButton) != 0)
            {
                PointerLost?.Invoke(this, new PointerEventArgs(CurrentTime, position, PointerId.FromMouse(PointerId.MouseLeftButton), buttons));
            }

            if ((buttons & PointerId.MouseRightButton) != 0)
            {
                PointerLost?.Invoke(this, new PointerEventArgs(CurrentTime, position, PointerId.FromMouse(PointerId.MouseRightButton), buttons));
            }

            if ((buttons & PointerId.MouseMiddleButton) != 0)
            {
                PointerLost?.Invoke(this, new PointerEventArgs(CurrentTime, position, PointerId.FromMouse(PointerId.MouseMiddleButton), buttons));
            }

            PointerLost?.Invoke(this, new PointerEventArgs(CurrentTime, position, PointerId.FromMouse(PointerId.MouseNoButton), buttons));
        }

        private void Element_MouseUp(object sender, MouseButtonEventArgs args)
        {
            var id = GetMouseButton(args);
            var buttons = GetMouseButtons(args);
            var position = GetPosition(args);

            if (FirstMouseButton == id) FirstMouseButton = 0;

            PointerUp?.Invoke(this, new PointerEventArgs(CurrentTime, position, PointerId.FromMouse(id), buttons));
        }

        private void Element_MouseDown(object sender, MouseButtonEventArgs args)
        {
            var id = GetMouseButton(args);
            var buttons = GetMouseButtons(args);
            var position = GetPosition(args);

            if (FirstMouseButton == 0) FirstMouseButton = id;

            PointerDown?.Invoke(this, new PointerEventArgs(CurrentTime, position, PointerId.FromMouse(id), buttons));
        }

        private void Element_MouseMove(object sender, MouseEventArgs args)
        {
            var buttons = GetMouseButtons(args);
            var id = buttons & FirstMouseButton;
            var position = GetPosition(args);

            PointerMove?.Invoke(this, new PointerEventArgs(CurrentTime, position, PointerId.FromMouse(id), buttons));
        }

        private Vector2 GetPosition(MouseEventArgs args)
        {
            var position = args.GetPosition(element);
            return new Vector2((float)position.X * Scale.X, (float)position.Y * Scale.Y);
        }

        private long GetMouseButton(MouseButtonEventArgs args)
        {
            switch(args.ChangedButton)
            {
                case MouseButton.Left:
                    return PointerId.MouseLeftButton;

                case MouseButton.Right:
                    return PointerId.MouseRightButton;

                case MouseButton.Middle:
                    return PointerId.MouseMiddleButton;
            }
            return 0;
        }

        private long GetMouseButtons(MouseEventArgs args)
        {
            long buttonsId = 0;
            if (args.LeftButton == MouseButtonState.Pressed) buttonsId |= PointerId.MouseLeftButton;
            if (args.RightButton == MouseButtonState.Pressed) buttonsId |= PointerId.MouseRightButton;
            if (args.MiddleButton == MouseButtonState.Pressed) buttonsId |= PointerId.MouseMiddleButton;
            return buttonsId;
        }
    }
}
