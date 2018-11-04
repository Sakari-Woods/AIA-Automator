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
    /// Interaction logic for processSelection.xaml
    /// </summary>
    public partial class processSelection : Window
    {
        public processSelection()
        {
            InitializeComponent();
        }

        private void selectProcess_Click(object sender, RoutedEventArgs e)
        {

        }

        private void cancelProcess_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
