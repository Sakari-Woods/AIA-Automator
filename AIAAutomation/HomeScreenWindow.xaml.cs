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
    /// Interaction logic for HomeScreenWindow.xaml
    /// </summary>

    public partial class HomeScreenWindow : Window
    {
        private Window main;

        public HomeScreenWindow()
        {
            InitializeComponent();
        }
        public void setMain(Window window)
        {
            // Grabs the main window from the constructor, so we can pass it through our
            // building wizard and update values within autoEngine.
            main = window;
        }

        private void loadExisting_Click(object sender, RoutedEventArgs e)
        {

        }

        private void createNew_Click(object sender, RoutedEventArgs e)
        {
            processSelection processWindow = new processSelection();
            this.Topmost = false;
            processWindow.Show();
            
            // We pass the main window from MainWindow into processSelection.
            processWindow.setMain(main);

            this.Close();
        }
    }
}
