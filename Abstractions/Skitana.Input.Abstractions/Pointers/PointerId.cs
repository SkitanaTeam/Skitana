// MIT License - Copyright © Skitana Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.
namespace Skitana.Input.Abstractions.Pointers
{
    public struct PointerId
    {
        public const long MouseNoButton = 0;
        public const long MouseLeftButton = 1;
        public const long MouseRightButton = 2;
        public const long MouseMiddleButton = 4;

        public long Id { get; }
        public PointerType Type { get; }

        public PointerId(long id, PointerType type)
        {
            Id = id;
            Type = type;
        }

        public static PointerId FromMouse(long id)
        {
            return new PointerId(id, PointerType.Mouse);
        }
    }
}
