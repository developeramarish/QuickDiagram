﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace Codartis.SoftVis.Rendering.Wpf.Gestures
{
    /// <summary>
    /// Calculates translate changes when panning and zooming with keys.
    /// </summary>
    internal class KeyboardPanAndZoomGesture : PanAndZoomGestureBase
    {
        private const int _acceleration = 2;
        private const int _deceleration = 4;
        private const int _maxSpeed = 30;
        private const int _minSpeed = -_maxSpeed;

        private readonly DispatcherTimer _timer;

        private int _zoomSpeed = 0;
        private int _horizontalSpeed = 0;
        private int _verticalSpeed = 0;
        private bool[] _isKeyDown = new bool[(int)GestureKeys.Max + 1];

        public KeyboardPanAndZoomGesture(IGestureTarget gestureTarget)
            : base(gestureTarget)
        {
            Target.PreviewKeyDown += OnKeyDown;
            Target.PreviewKeyUp += OnKeyUp;

            _timer = CreateTimer(1000 / 25, OnTimerTick);
            _timer.Start();
        }

        private DispatcherTimer CreateTimer(int intervalMillisec, EventHandler tickHandler)
        {
            var timer = new DispatcherTimer();
            timer.Tick += tickHandler;
            timer.Interval = new TimeSpan(0, 0, 0, 0, intervalMillisec);
            return timer;
        }

        private void OnTimerTick(object sender, EventArgs e)
        {
            _zoomSpeed = CalculateSpeed(_zoomSpeed, GestureKeys.ZoomIn, GestureKeys.ZoomOut);
            _verticalSpeed = CalculateSpeed(_verticalSpeed, GestureKeys.Up, GestureKeys.Down);
            _horizontalSpeed = CalculateSpeed(_horizontalSpeed, GestureKeys.Left, GestureKeys.Right);

            if (_zoomSpeed != 0)
            {
                var zoomDirection = _zoomSpeed > 0 ? ZoomDirection.In : ZoomDirection.Out;
                var limitReached = ZoomBy(GetTargetCenterPoint(), zoomDirection, Math.Abs(_zoomSpeed));
                if (limitReached)
                    _zoomSpeed = 0;
            }

            if (_horizontalSpeed != 0 || _verticalSpeed != 0)
            {
                Translate(new Vector(_horizontalSpeed * Target.Scale, _verticalSpeed * Target.Scale));
            }
        }

        private int CalculateSpeed(int speed, GestureKeys positiveDirection, GestureKeys negativeDirection)
        {
            if (_isKeyDown[(int)positiveDirection] && !(_isKeyDown[(int)negativeDirection]) && speed >= 0)
            {
                speed += _acceleration;
            }
            else if (_isKeyDown[(int)negativeDirection] && !(_isKeyDown[(int)positiveDirection]) && speed <= 0)
            {
                speed -= _acceleration;
            }
            else if (speed < 0)
            {
                speed += Math.Min(Math.Abs(speed), _deceleration);
            }
            else if (speed > 0)
            {
                speed -= Math.Min(Math.Abs(speed), _deceleration);
            }

            speed = Math.Min(_maxSpeed, speed);
            speed = Math.Max(_minSpeed, speed);

            return speed;
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            SetKeyDownState(e.Key, true);
            e.Handled = true;
        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            SetKeyDownState(e.Key, false);
            e.Handled = true;
        }

        private void SetKeyDownState(Key key, bool newState)
        {
            switch (key)
            {
                case (Key.Up):
                    _isKeyDown[(int)GestureKeys.Up] = newState;
                    break;
                case (Key.Down):
                    _isKeyDown[(int)GestureKeys.Down] = newState;
                    break;
                case (Key.Left):
                    _isKeyDown[(int)GestureKeys.Left] = newState;
                    break;
                case (Key.Right):
                    _isKeyDown[(int)GestureKeys.Right] = newState;
                    break;
                case (Key.W):
                    _isKeyDown[(int)GestureKeys.ZoomIn] = newState;
                    break;
                case (Key.S):
                    _isKeyDown[(int)GestureKeys.ZoomOut] = newState;
                    break;
                default:
                    break;
            }
        }

        private enum GestureKeys
        {
            Up = 0,
            Down,
            Left,
            Right,
            ZoomIn,
            ZoomOut,
            Max = ZoomOut
        }
    }
}
