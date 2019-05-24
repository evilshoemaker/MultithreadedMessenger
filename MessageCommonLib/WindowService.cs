using System;
using System.Windows;
using System.Windows.Controls;

namespace MessageCommonLib
{
    public class WindowService
    {
        public static void ShowUserControl(Type viewType, object dataContext, bool isModal, Action<bool?> closeAction)
        {
            Control view = null;

            var constructor = viewType.GetConstructor(new Type[0]);
            if (constructor != null)
            {
                view = constructor.Invoke(new object[0]) as UserControl;
            }

            if (view != null)
            {
                view.DataContext = dataContext;

                Window window = new Window();
                window.SizeToContent = SizeToContent.WidthAndHeight;
                window.Owner = Application.Current.MainWindow;
                window.WindowStartupLocation = WindowStartupLocation.CenterOwner;

                Grid rootGrid = new Grid();
                rootGrid.Children.Add(view);

                window.Content = rootGrid;
                window.Closed += (s, e) => closeAction(window.DialogResult);

                if (isModal)
                {
                    window.ShowDialog();
                }
                else
                {
                    window.Show();
                }
            }
        }

        public static void Show(Type windowType, object dataContext, bool isModal, Action<bool?> closeAction)
        {
            Window window = null;

            var constructor = windowType.GetConstructor(new Type[0]);
            if (constructor != null)
            {
                window = constructor.Invoke(new object[0]) as Window;
            }

            if (window != null)
            {
                window.DataContext = dataContext;
                window.Owner = Application.Current.MainWindow;

                window.Closed += (s, e) => closeAction(window.DialogResult);

                if (isModal)
                {
                    window.ShowDialog();
                }
                else
                {
                    window.Show();
                }
            }
        }
    }
}
