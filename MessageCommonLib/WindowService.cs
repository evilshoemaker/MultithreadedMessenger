using System;
using System.Windows;
using System.Windows.Controls;

namespace MessageCommonLib
{
    public class WindowService
    {
        public static void Show(Type viewType, object dataContext, bool isModal, Action<bool?> closeAction)
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
    }
}
