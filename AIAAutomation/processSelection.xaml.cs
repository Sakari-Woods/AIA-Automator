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
using Microsoft.Win32;
using System.Windows.Automation;
using System.Diagnostics;
using Condition = System.Windows.Automation.Condition;

namespace AIAAutomation
{
    /// <summary>
    /// Interaction logic for processSelection.xaml
    /// </summary>
    public partial class processSelection : Window
    {
        // String is made public so we can reference it from the MainWindow.
        private Window main;
        private string clickedName;

        public processSelection()
        {
            InitializeComponent();
            processIndex();
        }

        public void setMain(Window window)
        {
            main = window;
        }

        private void processIndex()
        {
            Console.WriteLine("Indexing processes.");
            var allProcess = Process.GetProcesses();
            String[] processBuffer = new String[allProcess.Length];
            int a = 0;
            // Move processes to a list. We do this so we can sort a->z our processes.
            // If we want a different sorting method, we can adjust it here.
            foreach (var process in allProcess)
            {
                processBuffer[a] = process.ProcessName;
                a++;
            }
            Array.Sort<String>(processBuffer);
            // Now add our list items of processes from the list.
            for(int i = 0; i < allProcess.Length-1; i++)
            {
                ListBoxItem currentprocess = new ListBoxItem();
                currentprocess.Content = processBuffer[i];
                currentprocess.PreviewMouseLeftButtonDown += new MouseButtonEventHandler(chosenProgram);
                processList.Items.Add(currentprocess);   
            }
        }
        private void chosenProgram(object sender, RoutedEventArgs e)
        {
            // Grabs the name of the object and removes system information so all we
            // have is the name that we want.
            clickedName = sender.ToString();
            while (clickedName.Substring(0,1) != ":")
            {
                clickedName = clickedName.Substring(1);
            }
            clickedName = clickedName.Substring(2);
            Console.WriteLine("Chose " + clickedName);
        }

        private void selectProcess_Click(object sender, RoutedEventArgs e)
        {
            if(clickedName != null)
            {
                Console.WriteLine("Found " + clickedName + ", proceeding");
                optionsWindow options = new optionsWindow();
                options.ShowDialog();
                options.setMain(main);
                // Pass our values, and move into our options dialogue window.
               // optionsWindow.setMain(main);
                // Record the found program and send it to our editor.
                // Call options wizard screen here.
                this.Close();
            }
            else
            {
                Console.WriteLine("Please select a process.");
            }
        }

        private void cancelProcess_Click(object sender, RoutedEventArgs e)
        {
            clickedName = null;
            // Create HomeScreenWindow again because maybe the user accidentally clicked Cancel,
            // or wanted to load an existing script.
            HomeScreenWindow homeScreen = new HomeScreenWindow();
            homeScreen.Show();
            // Sets the MainWindow to be the main of HomeScreenWindow, important because if we don't do this
            // we'll lose track of our updating value hand-off and won't be able to call our autoEngine.
            homeScreen.setMain(main);
            this.Close();
        }
    }
}
