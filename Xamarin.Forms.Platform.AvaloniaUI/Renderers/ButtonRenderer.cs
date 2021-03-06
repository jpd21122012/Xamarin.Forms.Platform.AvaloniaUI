﻿using System;
using System.ComponentModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using AButton = Avalonia.Controls.Button;
using AImage = Avalonia.Controls.Image;
using AThickness = Avalonia.Thickness;

namespace Xamarin.Forms.Platform.AvaloniaUI
{
	public class ButtonRenderer : ViewRenderer<Button, AButton>
	{
		bool _fontApplied;

		protected override void OnElementChanged(ElementChangedEventArgs<Button> e)
		{
			if (e.NewElement != null)
			{
				if (Control == null) // construct and SetNativeControl and suscribe control event
				{
					SetNativeControl(new AButton());
					Control.Click += HandleButtonClick;
				}

				UpdateContent();

				if (Element.TextColor != Color.Default)
					UpdateTextColor();

				if (Element.BorderColor != Color.Default)
					UpdateBorderColor();

				if (Element.BorderWidth != 0)
					UpdateBorderWidth();

				UpdateFont();
			}

			base.OnElementChanged(e);
		}

		protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);

			if (e.PropertyName == Button.TextProperty.PropertyName || e.PropertyName == Button.ImageProperty.PropertyName)
				UpdateContent();
			else if (e.PropertyName == Button.TextColorProperty.PropertyName)
				UpdateTextColor();
			else if (e.PropertyName == Button.FontProperty.PropertyName)
				UpdateFont();
			else if (e.PropertyName == Button.BorderColorProperty.PropertyName)
				UpdateBorderColor();
			else if (e.PropertyName == Button.BorderWidthProperty.PropertyName)
				UpdateBorderWidth();
		}

		void HandleButtonClick(object sender, RoutedEventArgs e)
		{
			IButtonController buttonView = Element as IButtonController;
			if (buttonView != null)
				buttonView.SendClicked();
		}
		
		void UpdateBorderColor()
		{
			Control.UpdateDependencyColor(AButton.BorderBrushProperty, Element.BorderColor);
		}

		void UpdateBorderWidth()
		{
			Control.BorderThickness = Element.BorderWidth <= 0d ? new AThickness(1) : new AThickness(Element.BorderWidth);
		}

		void UpdateContent()
		{
			var text = Element.Text;
			var elementImage = Element.Image;

			// No image, just the text
			if (elementImage == null)
			{
				Control.Content = text;
				return;
			}

			var image = new AImage
			{
				//Source = new BitmapImage(new Uri("/" + elementImage.File, UriKind.Relative)),
				Width = 30,
				Height = 30,
				VerticalAlignment = VerticalAlignment.Center,
				HorizontalAlignment = HorizontalAlignment.Center
			};

			// No text, just the image
			if (string.IsNullOrEmpty(text))
			{
				Control.Content = image;
				return;
			}

			// Both image and text, so we need to build a container for them
			var layout = Element.ContentLayout;
			var container = new StackPanel();
			var textBlock = new TextBlock
			{
				Text = text,
				VerticalAlignment = VerticalAlignment.Center,
				HorizontalAlignment = HorizontalAlignment.Center
			};

			var spacing = layout.Spacing;

			container.HorizontalAlignment = HorizontalAlignment.Center;
			container.VerticalAlignment = VerticalAlignment.Center;

			switch (layout.Position)
			{
				case Button.ButtonContentLayout.ImagePosition.Top:
					container.Orientation = Orientation.Vertical;
					image.Margin = new AThickness(0, 0, 0, spacing);
					break;
				case Button.ButtonContentLayout.ImagePosition.Bottom:
					container.Orientation = Orientation.Vertical;
					image.Margin = new AThickness(0, spacing, 0, 0);
					break;
				case Button.ButtonContentLayout.ImagePosition.Right:
					container.Orientation = Orientation.Horizontal;
					image.Margin = new AThickness(spacing, 0, 0, 0);
					break;
				default:
					// Defaults to image on the left
					container.Orientation = Orientation.Horizontal;
					image.Margin = new AThickness(0, 0, spacing, 0);
					break;
			}

			container.Children.Add(image);
			container.Children.Add(textBlock);

			Control.Content = container;
		}

		void UpdateFont()
		{
			if (Control == null || Element == null)
				return;

			if (Element.Font == Font.Default && !_fontApplied)
				return;

			Font fontToApply = Element.Font == Font.Default ? Font.SystemFontOfSize(NamedSize.Medium) : Element.Font;

			Control.ApplyFont(fontToApply);
			_fontApplied = true;
		}

		void UpdateTextColor()
		{
			Control.UpdateDependencyColor(AButton.ForegroundProperty, Element.TextColor);
		}

		bool _isDisposed;

		protected override void Dispose(bool disposing)
		{
			if (_isDisposed)
				return;

			if (disposing)
			{
				if (Control != null)
				{
					Control.Click -= HandleButtonClick;
				}
			}

			_isDisposed = true;
			base.Dispose(disposing);
		}
	}
}