using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MessageCommonLib.Behaviours
{
    public static class HostAddressOnlyBehaviour
    {
        public static readonly DependencyProperty IsEnabledProperty =
                DependencyProperty.RegisterAttached("IsEnabled", typeof(bool),
                typeof(HostAddressOnlyBehaviour), new UIPropertyMetadata(false, OnValueChanged));

        public static bool GetIsEnabled(Control o) { return (bool)o.GetValue(IsEnabledProperty); }

        public static void SetIsEnabled(Control o, bool value) { o.SetValue(IsEnabledProperty, value); }

        private static void OnValueChanged(DependencyObject dependencyObject,
                DependencyPropertyChangedEventArgs e)
        {
            var uiElement = dependencyObject as Control;
            if (uiElement == null) return;
            if (e.NewValue is bool && (bool)e.NewValue)
            {
                uiElement.PreviewTextInput += OnTextInput;
                uiElement.PreviewKeyDown += OnPreviewKeyDown;
                DataObject.AddPastingHandler(uiElement, OnPaste);
            }

            else
            {
                uiElement.PreviewTextInput -= OnTextInput;
                uiElement.PreviewKeyDown -= OnPreviewKeyDown;
                DataObject.RemovePastingHandler(uiElement, OnPaste);
            }
        }

        private static void OnTextInput(object sender, TextCompositionEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null)
            {
                if (!IsValueValid(textBox.Text + e.Text))
                {
                    e.Handled = true;
                }
            }
        }

        private static bool IsValueValid(string value)
        {
            return Regex.IsMatch(value, @"^(?:[0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])(?:\.(?:[0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])?){0,3}$");
        }

        private static void OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space) e.Handled = true;
        }

        private static void OnPaste(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(DataFormats.Text))
            {
                var text = Convert.ToString(e.DataObject.GetData(DataFormats.Text)).Trim();
                if (!IsValueValid(text))
                {
                    e.CancelCommand();
                }
            }
            else
            {
                e.CancelCommand();
            }
        }
    }
}
