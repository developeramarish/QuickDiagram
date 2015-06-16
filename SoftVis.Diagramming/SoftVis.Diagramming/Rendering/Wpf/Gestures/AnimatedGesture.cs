﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;

namespace Codartis.SoftVis.Rendering.Wpf.Gestures
{
    /// <summary>
    /// Smooths out the scale and transform changes during zoom calculation.
    /// </summary>
    internal class AnimatedGesture : Animatable, IGesture
    {
        public event ScaleChangedEventHandler ScaleChanged;
        public event TranslateChangedEventHandler TranslateChanged;

        private readonly IGesture _gesture;
        private readonly Duration _animationDuration;
        private readonly EasingFunctionBase _easingFunction = new QuadraticEase() { EasingMode = EasingMode.EaseOut };

        public AnimatedGesture(IGesture gesture, TimeSpan animationTimeSpan)
        {
            _gesture = gesture;
            _gesture.ScaleChanged += OnScaleChanged;
            _gesture.TranslateChanged += OnTranslateChanged;
            _animationDuration = new Duration(animationTimeSpan);
        }

        public static readonly DependencyProperty ScaleProperty = DependencyProperty.Register("Scale", 
            typeof(double), typeof(AnimatedGesture), new PropertyMetadata(OnScalePropertyChanged));

        public static readonly DependencyProperty TranslateProperty = DependencyProperty.Register("Translate", 
            typeof(Vector), typeof(AnimatedGesture), new PropertyMetadata(OnTranslatePropertyChanged));

        public IGestureTarget Target
        {
            get { return _gesture.Target; }
        }

        private void OnScaleChanged(object sender, ScaleChangedEventArgs args)
        {
            var animation = new DoubleAnimation(_gesture.Target.Scale, args.NewScale, _animationDuration)
            {
                EasingFunction = _easingFunction
            };
            BeginAnimation(ScaleProperty, animation);
        }

        private void OnTranslateChanged(object sender, TranslateChangedEventArgs args)
        {
            var animation = new VectorAnimation(_gesture.Target.Translate, args.NewTranslate, _animationDuration)
            {
                EasingFunction = _easingFunction
            };
            BeginAnimation(TranslateProperty, animation);
        }

        private static void OnScalePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var a = d as AnimatedGesture;
            a.OnScaleChanged((double)e.NewValue);
        }

        private static void OnTranslatePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var a = d as AnimatedGesture;
            a.OnTranslateChanged((Vector)e.NewValue);
        }

        private void OnScaleChanged(double scale)
        {
            if (ScaleChanged != null)
                ScaleChanged(this, new ScaleChangedEventArgs(scale));
        }

        private void OnTranslateChanged(Vector translate)
        {
            if (TranslateChanged != null)
                TranslateChanged(this, new TranslateChangedEventArgs(translate));
        }

        protected override Freezable CreateInstanceCore()
        {
            throw new NotImplementedException();
        }
    }
}
