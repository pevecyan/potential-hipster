using Hipster_DNS.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
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

        bool settingsOpen;

        public MainWindow()
        {
            InitializeComponent();

            if (!new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator))
            {
                (this.FindResource("ShowNotificationGrid") as Storyboard).Begin();
                //this.Close();
            }

            (this.FindResource("ShowAdaptersList") as Storyboard).Completed += delegate { adaptersListOpen = true; };
            (this.FindResource("HideAdaptersList") as Storyboard).Completed += delegate { adaptersListOpen = false; };
            (this.FindResource("ShowSettings") as Storyboard).Completed += delegate { settingsOpen = true; };
            (this.FindResource("HideSettings") as Storyboard).Completed += delegate { settingsOpen = false; };

            adapters = NetshHandler.GetAdapters();
            SelectedAdapterTextBlock.Text = adapters[0];
            foreach (string adapter in adapters)
            {
                AdpatersStackPanel.Children.Add(new AdapterItem() {AdapterName = adapter, setAdapter = SetAdapter});
            }

            if ((string)Properties.Settings.Default["Adapter"] == "/")
            {
                Properties.Settings.Default["Adapter"] = SelectedAdapterTextBlock.Text;
                Properties.Settings.Default.Save();
            }
            else
            {
                SelectedAdapterTextBlock.Text = (string)Properties.Settings.Default["Adapter"];
            }

            ip11.Text = ((string)Properties.Settings.Default["DNS1"]).Split('.')[0];
            ip12.Text = ((string)Properties.Settings.Default["DNS1"]).Split('.')[1];
            ip13.Text = ((string)Properties.Settings.Default["DNS1"]).Split('.')[2];
            ip14.Text = ((string)Properties.Settings.Default["DNS1"]).Split('.')[3];

            ip21.Text = ((string)Properties.Settings.Default["DNS2"]).Split('.')[0];
            ip24.Text = ((string)Properties.Settings.Default["DNS2"]).Split('.')[3];
            ip22.Text = ((string)Properties.Settings.Default["DNS2"]).Split('.')[1];
            ip23.Text = ((string)Properties.Settings.Default["DNS2"]).Split('.')[2];
            
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


        private void ipInput_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            TextBox input = sender as TextBox;

            if (input.Text.Length >= 3)
            {
                if (sender == ip11) { ip12.Focus(); }
                if (sender == ip12) { ip13.Focus(); }
                if (sender == ip13) { ip14.Focus(); }
                if (sender == ip14) { ip21.Focus(); }

                if (sender == ip22) { ip23.Focus(); }
                if (sender == ip21) { ip22.Focus(); }
                if (sender == ip23) { ip24.Focus(); }
                
            }
            if (input.Text.Length > 3){input.Text = input.Text.Substring(0, 3);}
            int number;
            if (input.Text.Length > 0)
            {
                if(int.TryParse(input.Text, out number))
                {
                    if (number > 255)
                    {
                        input.Text = "255";
                    }
                }
                else
                {
                    input.Text = "0";
                }
            }
        }

        private void ipInput_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox input = sender as TextBox;
            input.SelectAll();
        }

        private void SettingsButtonGrid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (settingsOpen) (this.FindResource("HideSettings") as Storyboard).Begin();
            else (this.FindResource("ShowSettings") as Storyboard).Begin();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Properties.Settings.Default["DNS1"] = ip11.Text + "." + ip12.Text + "." + ip13.Text + "." + ip14.Text;
            Properties.Settings.Default["DNS2"] = ip21.Text + "." + ip22.Text + "." + ip23.Text + "." + ip24.Text;
            Properties.Settings.Default["Adapter"] = SelectedAdapterTextBlock.Text;
            
            Properties.Settings.Default.Save();
        }
    }
}
