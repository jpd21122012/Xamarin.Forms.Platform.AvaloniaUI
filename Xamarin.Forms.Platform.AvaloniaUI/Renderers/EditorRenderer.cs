﻿using System.ComponentModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
//using AvaloniaScrollBarVisibility = Avalonia.Controls.ScrollBarVisibility;

namespace Xamarin.Forms.Platform.AvaloniaUI
{
    public class EditorRenderer : ViewRenderer<Editor, TextBox>
    {
        bool _fontApplied;

        protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
        {
            if (e.NewElement != null)
            {
                if (Control == null) // construct and SetNativeControl and suscribe control event
                {
                    SetNativeControl(new TextBox
                    {
                        //VerticalScrollBarVisibility = AvaloniaScrollBarVisibility.Visible,
                        TextWrapping = TextWrapping.Wrap,
                        AcceptsReturn = true
                    });
                    // TODO: 
                    //Control.LostFocus += NativeOnLostFocus; 
                    //Control.TextChanged += NativeOnTextChanged;
                }

                // Update control property 
                UpdateText();
                UpdateInputScope();
                UpdateTextColor();
                UpdateFont();
                UpdateMaxLength();
            }

            base.OnElementChanged(e);
        }


        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == Editor.TextProperty.PropertyName)
                UpdateText();
            else if (e.PropertyName == InputView.KeyboardProperty.PropertyName)
                UpdateInputScope();
            else if (e.PropertyName == Editor.TextColorProperty.PropertyName)
                UpdateTextColor();
            else if (e.PropertyName == Editor.FontAttributesProperty.PropertyName)
                UpdateFont();
            else if (e.PropertyName == Editor.FontFamilyProperty.PropertyName)
                UpdateFont();
            else if (e.PropertyName == Editor.FontSizeProperty.PropertyName)
                UpdateFont();
            else if (e.PropertyName == InputView.MaxLengthProperty.PropertyName)
                UpdateMaxLength();
        }

        //void NativeOnTextChanged(object sender, Avalonia.Controls.TextChangedEventArgs textChangedEventArgs)
        //{
        //	((IElementController)Element).SetValueFromRenderer(Editor.TextProperty, Control.Text);
        //}

        //void NativeOnLostFocus(object sender, RoutedEventArgs e)
        //{
        //	Element.SendCompleted();
        //}

        void UpdateFont()
        {
            if (Control == null)
                return;

            Editor editor = Element;

            bool editorIsDefault = editor.FontFamily == null && editor.FontSize == Device.GetNamedSize(NamedSize.Default, typeof(Editor), true) && editor.FontAttributes == FontAttributes.None;
            if (editor == null || (editorIsDefault && !_fontApplied))
                return;

            if (editorIsDefault)
            {
                //Control.ClearValue(Avalonia.Controls.Control.FontStyleProperty);
                //Control.ClearValue(Avalonia.Controls.Control.FontSizeProperty);
                //Control.ClearValue(Avalonia.Controls.Control.FontFamilyProperty);
                //Control.ClearValue(Avalonia.Controls.Control.FontWeightProperty);
                //Control.ClearValue(Avalonia.Controls.Control.FontStretchProperty);
            }
            else
                Control.ApplyFont(editor);

            _fontApplied = true;
        }

        void UpdateInputScope()
        {
            //Control.InputScope = Element.Keyboard.ToInputScope();
        }

        void UpdateText()
        {
            string newText = Element.Text ?? "";

            if (Control.Text == newText)
                return;

            Control.Text = newText;
            Control.SelectionStart = Control.Text.Length;
        }

        void UpdateTextColor()
        {
            //Control.UpdateDependencyColor(Avalonia.Controls.Control.ForegroundProperty, Element.TextColor);
        }

        void UpdateMaxLength()
        {
            //Control.MaxLength = Element.MaxLength;

            var currentControlText = Control.Text;

            if (currentControlText.Length > Element.MaxLength)
                Control.Text = currentControlText.Substring(0, Element.MaxLength);
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
                    //Control.LostFocus -= NativeOnLostFocus;
                    //Control.TextChanged -= NativeOnTextChanged;
                }
            }

            _isDisposed = true;
            base.Dispose(disposing);
        }
    }
}