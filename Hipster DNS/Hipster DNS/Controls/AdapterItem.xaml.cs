using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Hipster_DNS
{
	/// <summary>
	/// Interaction logic for AdapterItem.xaml
	/// </summary>
	public partial class AdapterItem : UserControl
	{
        public Action<string> setAdapter;
        public string AdapterName { set { AdapterNameTextBlock.Text = value; } }

		public AdapterItem()
		{
			this.InitializeComponent();
		}

        private void LayoutRoot_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            setAdapter(AdapterNameTextBlock.Text);
        }
        
	}
}