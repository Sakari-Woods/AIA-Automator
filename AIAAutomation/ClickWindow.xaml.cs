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
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AIAAutomation
{
    /// <summary>
    /// Interaction logic for ClickWindow.xaml
    /// </summary>
    public partial class ClickWindow : Window
    {
        MainWindow win = (MainWindow)Application.Current.MainWindow;
        public ClickWindow()
        {
            InitializeComponent();
        }

            private void clickmodal_submit(object sender, MouseButtonEventArgs e)
        {
            win.automationid = clickmodal_automationidtarget.Text;     
            ListBoxItem newlistitem = new ListBoxItem();
            newlistitem.Name = win.automationid + "click";
            SolidColorBrush back = new SolidColorBrush();
            back.Color = (Color)ColorConverter.ConvertFromString("Aquamarine");
            newlistitem.Background = back;
            newlistitem.Content = "Click  :"+win.automationid+":";
            win.TimelineBox.Items.Add(newlistitem);
            this.Close();
        }
            private void clickDown(object sender, MouseButtonEventArgs e)
        {
            Console.WriteLine("WOW");
        }
    }
}
