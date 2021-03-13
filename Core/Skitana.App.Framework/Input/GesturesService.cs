// MIT License - Copyright © Skitana Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using Skitana.App.Framework.Abstractions;
using Skitana.App.Framework.Helpers;
using Skitana.Input.Abstractions.Pointers;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Numerics;

namespace Skitana.App.Framework.Input
{
    internal class GesturesService : IUpdatable, IGesturesService
    {
        private readonly ObjectPool<PointerDownElement> pointerDownElementsPool = new ObjectPool<PointerDownElement>();

        private readonly ObjectPool<Gesture> gesturesPool = new ObjectPool<Gesture>();

        private ConcurrentDictionary<PointerId, PointerDownElement> pointersDown = new ConcurrentDictionary<PointerId, PointerDownElement>();
        private ConcurrentQueue<Gesture> gesturesToPublish = new ConcurrentQueue<Gesture>();

        private List<IGestureConsumer> consumers = new List<IGestureConsumer>();

        public float MinDragSize { get; set; } = 10;
        public TimeSpan HoldTime { get; set; } = TimeSpan.FromMilliseconds(1000);
        public TimeSpan HoldStartTime { get; set; } = TimeSpan.FromMilliseconds(250);


        public GesturesService(IInputPanel inputPanel, IUpdatablesService updatablesService)
        {
            updatablesService.Register(this);

            inputPanel.PointerDown += InputPanel_PointerDown;
            inputPanel.PointerMove += InputPanel_PointerMove;
            inputPanel.PointerLost += InputPanel_PointerLost;
            inputPanel.PointerUp += InputPanel_PointerUp;
            inputPanel.MouseWheel += InputPanel_MouseWheel;
        }

        private void InputPanel_MouseWheel(object sender, MouseWheelEventArgs args)
        {
            var gesture = gesturesPool.Get();

            gesture.Init(args.PointerId, args.Position, args.Position, args.Time);
            gesture.GestureType = GestureType.MouseWheel;
            gesture.Offset = new Vector2(0, args.WheelDelta);

            Publish(gesture);
        }

        private void InputPanel_PointerDown(object sender, PointerEventArgs args)
        {
            if(!pointersDown.TryGetValue(args.PointerId, out var pointer))
            {
                pointer = pointerDownElementsPool.Get();
                pointersDown.TryAdd(args.PointerId, pointer);
            }

            pointer.DownTime = args.Time;
            pointer.Origin = args.Position;
            pointer.Position = args.Position;

            pointer.LockedGesture = 0;
            pointer.LockedListener = null;

            var gesture = gesturesPool.Get();

            gesture.Init(args.PointerId, args.Position, args.Position, args.Time);
            gesture.GestureType = GestureType.Down;

            Publish(gesture);
        }

        private void Publish(Gesture gesture) => gesturesToPublish.Enqueue(gesture);

        private void InputPanel_PointerMove(object sender, PointerEventArgs args)
        {
            Vector2 origin = args.Position;
            if (pointersDown.TryGetValue(args.PointerId, out var pointer))
            {
                var move = args.Position - pointer.Position;
                pointer.Position = args.Position;
                origin = pointer.Origin;

                AnalizeMovement(args.PointerId, pointer, move, args.Time);
            }

            var gesture = gesturesPool.Get();

            gesture.Init(args.PointerId, origin, args.Position, args.Time);
            gesture.GestureType = GestureType.Move;

            Publish(gesture);
        }

        private void InputPanel_PointerUp(object sender, PointerEventArgs args)
        {
            Vector2 origin = args.Position;
            if (pointersDown.TryRemove(args.PointerId, out var pointer))
            {
                origin = pointer.Origin;

                if(pointer.LockedGesture == GestureType.None)
                {
                    if( (origin - args.Position).Length() < MinDragSize )
                    {
                        var tapGesture = gesturesPool.Get();

                        tapGesture.Init(args.PointerId, origin, args.Position, args.Time);
                        tapGesture.GestureType = GestureType.Tap;

                        Publish(tapGesture);
                    }
                }

                pointerDownElementsPool.Return(pointer);
            }

            var gesture = gesturesPool.Get();

            gesture.Init(args.PointerId, origin, args.Position, args.Time);
            gesture.GestureType = GestureType.Up;

            Publish(gesture);
        }

        private void InputPanel_PointerLost(object sender, PointerEventArgs args)
        {
            Vector2 origin = args.Position;
            if (pointersDown.TryRemove(args.PointerId, out var pointer))
            {
                origin = pointer.Origin;
                pointerDownElementsPool.Return(pointer);
            }

            var gesture = gesturesPool.Get();
            gesture.Init(args.PointerId, origin, args.Position, args.Time);
            gesture.GestureType = GestureType.PointerLost;

            Publish(gesture);
        }

        private void AnalizeMovement(PointerId pointerId, PointerDownElement pointer, Vector2 move, TimeSpan time)
        {
            Vector2 drag = pointer.Position - pointer.Origin;

            if ((pointer.LockedGesture & (GestureType.HorizontalDrag | GestureType.VerticalDrag | GestureType.Hold | GestureType.HoldStart | GestureType.Down)) == GestureType.None)
            {
                if (Math.Abs(drag.X) > MinDragSize && Math.Abs(drag.X) > Math.Abs(drag.Y))
                {
                    pointer.LockedGesture = GestureType.HorizontalDrag;
                    move = drag;
                }
                else if (Math.Abs(drag.Y) > MinDragSize && Math.Abs(drag.Y) > Math.Abs(drag.X))
                {
                    pointer.LockedGesture = GestureType.VerticalDrag;
                    move = drag;
                }
                else if (drag.Length() > MinDragSize)
                {
                    pointer.LockedGesture = GestureType.FreeDrag;
                    move = drag;
                }
            }

            if ((pointer.LockedGesture & GestureType.DragGestures) != GestureType.None)
            {
                var offset = move;

                if (pointer.LockedGesture.HasFlag(GestureType.HorizontalDrag)) offset.Y = 0;
                if (pointer.LockedGesture.HasFlag(GestureType.VerticalDrag)) offset.X = 0;

                var gesture = gesturesPool.Get();
                gesture.Init(pointerId, pointer.Origin, pointer.Position, time);
                gesture.GestureType = pointer.LockedGesture & GestureType.DragGestures;
                gesture.Offset = offset;
                Publish(gesture);

                if(!pointer.LockedGesture.HasFlag(GestureType.FreeDrag))
                {
                    gesture = gesturesPool.Get();
                    gesture.Init(pointerId, pointer.Origin, pointer.Position, time);
                    gesture.GestureType = GestureType.FreeDrag;
                    gesture.Offset = move;
                    Publish(gesture);
                }
            }
        }

        private void AnalizeHolds(PointerId pointerId, PointerDownElement pointer, TimeSpan time)
        {
            Vector2 drag = pointer.Position - pointer.Origin;

            if (pointer.LockedGesture == GestureType.None && drag.Length() < MinDragSize)
            {
                TimeSpan elapsed = time - pointer.DownTime;

                if (elapsed > HoldStartTime)
                {
                    pointer.LockedGesture = GestureType.HoldStart;

                    var gesture = gesturesPool.Get();
                    gesture.Init(pointerId, pointer.Origin, pointer.Position, time);
                    gesture.GestureType = GestureType.HoldStart;

                    Publish(gesture);
                }
            }
            else if (pointer.LockedGesture == GestureType.HoldStart)
            {
                TimeSpan elapsed = time - pointer.DownTime;

                if (drag.Length() > MinDragSize)
                {
                    pointer.LockedGesture &= ~GestureType.HoldGestures;

                    var gesture = gesturesPool.Get();
                    gesture.Init(pointerId, pointer.Origin, pointer.Position, time);
                    gesture.GestureType = GestureType.HoldCancel;
                    Publish(gesture);
                    return;
                }

                if (elapsed > HoldTime)
                {
                    pointer.LockedGesture = GestureType.Hold;

                    var gesture = gesturesPool.Get();
                    gesture.Init(pointerId, pointer.Origin, pointer.Position, time);
                    gesture.GestureType = GestureType.Hold;

                    Publish(gesture);
                }
            }
        }

        public void Update(TimeSpan globalTime, TimeSpan ellapsedTime)
        {
            foreach(var id in pointersDown.Keys)
            {
                if (pointersDown.TryGetValue(id, out var pointer))
                {
                    AnalizeHolds(id, pointer, globalTime);
                }
            }

            while (gesturesToPublish.TryDequeue(out var gesture))
            {
                PublishToConsumers(gesture);

                if (gesture.PointerCapturedBy != null && gesture.GestureType != GestureType.CapturedByOther)
                {
                    gesture.Reset();
                    gesture.GestureType = GestureType.CapturedByOther;
                    PublishToConsumers(gesture);
                }
                gesturesPool.Return(gesture);
            }
        }

        private void PublishToConsumers(Gesture gesture)
        {
            for(var idx =0; idx < consumers.Count; ++idx)
            {
                consumers[idx].OnGesture(gesture);
                if (gesture.Handled) break;
            }
        }

        public void Register(IGestureConsumer consumer) => consumers.Add(consumer);
        public void Unregister(IGestureConsumer consumer) => consumers.Remove(consumer);
    }
}
