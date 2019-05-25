using System.Windows;
using System.Windows.Controls;

namespace MessageServer.View
{
    public partial class ConnectSettingsWindow : Window
    {
        public ConnectSettingsWindow()
        {
            InitializeComponent();
        }

        private void ConnectButton_Click(object sender, RoutedEventArgs e)
        {
            if (System.Windows.Interop.ComponentDispatcher.IsThreadModal)
            {
                DialogResult = true;
            }
            
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            if (System.Windows.Interop.ComponentDispatcher.IsThreadModal)
            {
                DialogResult = false;
            }

            Close();
        }
    }
}
