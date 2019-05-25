using System;
using System.Windows;
using System.Windows.Media;

namespace MessageCommonLib
{
    public class IconDependencyClass : DependencyObject
    {
        #region IsCheckedOnDataProperty

        public static readonly DependencyProperty DataForPathProperty;

        public static void SetDataForPath(DependencyObject DepObject, string value)
        {
            DepObject.SetValue(DataForPathProperty, value);
        }

        public static string GetDataForPath(DependencyObject DepObject)
        {
            return (string)DepObject.GetValue(DataForPathProperty);
        }

        public static readonly DependencyProperty ColorOneProperty;

        public static void SetColorOne(DependencyObject DepObject, Brush value)
        {
            DepObject.SetValue(ColorOneProperty, value);
        }

        public static Brush GetColorOne(DependencyObject DepObject)
        {
            return (Brush)DepObject.GetValue(ColorOneProperty);
        }

        public static readonly DependencyProperty ColorTwoProperty;

        public static void SetColorTwo(DependencyObject DepObject, Brush value)
        {
            DepObject.SetValue(ColorTwoProperty, value);
        }

        public static Brush GetColorTwo(DependencyObject DepObject)
        {
            return (Brush)DepObject.GetValue(ColorTwoProperty);
        }

        #endregion

        static IconDependencyClass()
        {
            PropertyMetadata MyPropertyMetadata = new PropertyMetadata(String.Empty);

            DataForPathProperty = DependencyProperty.RegisterAttached("DataForPath",
                                                                typeof(string),
                                                                typeof(IconDependencyClass),
                                                                MyPropertyMetadata);

            PropertyMetadata BrushPropertyMetadata = new PropertyMetadata(new SolidColorBrush());

            ColorOneProperty = DependencyProperty.RegisterAttached("ColorOne",
                                                                typeof(Brush),
                                                                typeof(IconDependencyClass),
                                                                BrushPropertyMetadata);

            ColorTwoProperty = DependencyProperty.RegisterAttached("ColorTwo",
                                                                typeof(Brush),
                                                                typeof(IconDependencyClass),
                                                                BrushPropertyMetadata);
        }
    }
}
