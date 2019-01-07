﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;

namespace Xamarin.Forms.Platform.AvaloniaUI
{
    public class FormsPanel : Panel
    {
        IElementController ElementController => Element as IElementController;

        public Layout Element { get; private set; }

        public FormsPanel(Layout element)
        {
            Element = element;
        }

        protected override Avalonia.Size MeasureOverride(Avalonia.Size availableSize)
        {
            if (Element == null || availableSize.Width * availableSize.Height == 0)
                return new Avalonia.Size(0, 0);

            Element.IsInNativeLayout = true;

            for (var i = 0; i < ElementController.LogicalChildren.Count; i++)
            {
                var child = ElementController.LogicalChildren[i] as VisualElement;
                if (child == null)
                    continue;

                IVisualElementRenderer renderer = Platform.GetOrCreateRenderer(child);
                if (renderer == null)
                    continue;

                Control control = renderer.GetNativeElement();
                if (control.Bounds.Width != child.Width || control.Bounds.Height != child.Height)
                {
                    double width = child.Width <= -1 ? Bounds.Width : child.Width;
                    double height = child.Height <= -1 ? Bounds.Height : child.Height;
                    control.Measure(new Avalonia.Size(width, height));
                }
            }

            Avalonia.Size result;
            if (double.IsInfinity(availableSize.Width) || double.IsPositiveInfinity(availableSize.Height))
            {
                Size request = Element.Measure(availableSize.Width, availableSize.Height, MeasureFlags.IncludeMargins).Request;
                result = new Avalonia.Size(request.Width, request.Height);
            }
            else
            {
                result = availableSize;
            }
            Element.IsInNativeLayout = false;

            if (Double.IsPositiveInfinity(result.Height))
                result.WithHeight(0.0);
            if (Double.IsPositiveInfinity(result.Width))
                result.WithWidth(0.0);

            return result;
        }

        protected override Avalonia.Size ArrangeOverride(Avalonia.Size finalSize)
        {
            if (Element == null)
                return finalSize;

            Element.IsInNativeLayout = true;

            for (var i = 0; i < ElementController.LogicalChildren.Count; i++)
            {
                var child = ElementController.LogicalChildren[i] as VisualElement;
                if (child == null)
                    continue;

                IVisualElementRenderer renderer = Platform.GetRenderer(child);
                if (renderer == null)
                    continue;

                Rectangle bounds = child.Bounds;
                Control control = renderer.GetNativeElement();
                Rect childFinal = new Rect(bounds.X, bounds.Y, Math.Max(0, bounds.Width), Math.Max(0, bounds.Height));
                control.Arrange(childFinal);
            }

            Element.IsInNativeLayout = false;

            return finalSize;
        }
    }
}
