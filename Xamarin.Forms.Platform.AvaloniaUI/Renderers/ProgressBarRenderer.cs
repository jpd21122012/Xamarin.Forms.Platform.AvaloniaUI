﻿using System.ComponentModel;
using Avalonia;
using Xamarin.Forms.Internals;
using AProgressBar = Avalonia.Controls.ProgressBar;

namespace Xamarin.Forms.Platform.AvaloniaUI
{
	public class ProgressBarRenderer : ViewRenderer<ProgressBar, AProgressBar>
	{
		protected override void OnElementChanged(ElementChangedEventArgs<ProgressBar> e)
		{
			if (e.NewElement != null)
			{
				if (Control == null) // construct and SetNativeControl and suscribe control event
				{
					SetNativeControl(new AProgressBar { Minimum = 0, Maximum = 1 });
                    // TODO: 
                    //Control.ValueChanged += HandleValueChanged;
                }

                // Update control property 
                UpdateProgress();
				UpdateProgressColor();
			}

			base.OnElementChanged(e);
		}

		protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);

			if (e.PropertyName == ProgressBar.ProgressProperty.PropertyName)
				UpdateProgress();
			else if (e.PropertyName == ProgressBar.ProgressColorProperty.PropertyName)
				UpdateProgressColor();
		}

		void UpdateProgressColor()
		{
            // TODO: 
            //Control.UpdateDependencyColor(WProgressBar.ForegroundProperty, Element.ProgressColor.IsDefault ? Color.DeepSkyBlue : Element.ProgressColor);
        }

        void UpdateProgress()
		{
			Control.Value = Element.Progress;
		}

		//void HandleValueChanged(object sender, RoutedPropertyChangedEventArgs<double> routedPropertyChangedEventArgs)
		//{
		//	((IVisualElementController)Element)?.InvalidateMeasure(InvalidationTrigger.MeasureChanged);
		//}

		bool _isDisposed;

		protected override void Dispose(bool disposing)
		{
			if (_isDisposed)
				return;

			if (disposing)
			{
				if (Control != null)
				{
					//Control.ValueChanged -= HandleValueChanged;
				}
			}

			_isDisposed = true;
			base.Dispose(disposing);
		}
	}
}