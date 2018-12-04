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
    /// Interaction logic for optionsWindow.xaml
    /// </summary>
    public partial class optionsWindow : Window
    {
        public Window main;

        public optionsWindow()
        {
            InitializeComponent();
        }

        public void setMain(Window window)
        {
            Console.WriteLine("options recieved main");
            main = window;
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Clicked on next");
            Console.WriteLine("Advanced Reporting is " + advReport.IsChecked);
            if (advReport.IsChecked == true)
            {
                autoEngine.advReporting = true;
            }
            else
            {
                autoEngine.advReporting = false;
            }

            //Console.WriteLine("main has "+main.Content);
            // System.Windows.Controls.Grid
            // Re-enable visibility on editorGrid.
            this.Close();
        }
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Clicked on back");
            processSelection processWindow = new processSelection();
            processWindow.setMain(main);
            processWindow.ShowDialog();
            this.Close();
        }

    }
}
