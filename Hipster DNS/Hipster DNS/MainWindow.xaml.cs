using Hipster_DNS.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Hipster_DNS
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        bool adaptersListOpen;
        List<string> adapters;

        public MainWindow()
        {
            InitializeComponent();

            (this.FindResource("ShowAdaptersList") as Storyboard).Completed += delegate { adaptersListOpen = true; };
            (this.FindResource("HideAdaptersList") as Storyboard).Completed += delegate { adaptersListOpen = false; };

            adapters = NetshHandler.GetAdapters();
            SelectedAdapterTextBlock.Text = adapters[0];
            foreach (string adapter in adapters)
            {
                AdpatersStackPanel.Children.Add(new AdapterItem() {AdapterName = adapter, setAdapter = SetAdapter});
            }
            
        }

        private void SetAdapter(string adapterName)
        {
            SelectedAdapterTextBlock.Text = adapterName;
            (this.FindResource("HideAdaptersList") as Storyboard).Begin();
        }

        private void CloseButton_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void DragGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();

        }

        private void DragGrid(object sender, MouseButtonEventArgs e)
        {

        }

        private void AdapterGrid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (!adaptersListOpen) (this.FindResource("ShowAdaptersList") as Storyboard).Begin();
            else (this.FindResource("HideAdaptersList") as Storyboard).Begin();
        }

        private void ApplyButton_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            NetshHandler.SetDns(1, SelectedAdapterTextBlock.Text, ip11.Text + "." + ip12.Text + "." + ip13.Text + "." + ip14.Text);
            NetshHandler.SetDns(2, SelectedAdapterTextBlock.Text, ip21.Text + "." + ip22.Text + "." + ip23.Text + "." + ip24.Text);
        
        }

        private void ResetButton_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            NetshHandler.ResetDNS(SelectedAdapterTextBlock.Text);
        }
		
		/*IPs events*/

        private void ip11_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (ip11.Text.Length >= 3) { ip12.Focus(); }
            if (ip11.Text.Length > 3)
            {
                ip11.Text = ip11.Text.Substring(0, 3);
            }
        }

      

        
    }
}
