using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using System.Windows.Automation;
using System.Diagnostics;
using Condition = System.Windows.Automation.Condition;

namespace AIAAutomation{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window{
        private object data;

        public MainWindow(){
            //Initialization of main window.
            InitializeComponent();
        }

        //Actions when typing into the hook process window.
        private void textBox_TextChanged(object sender, TextChangedEventArgs e){
            //text has changed inside.
        }
        private void WalkEnabledElements(AutomationElement rootElement)
        {
            //Only add Automation controls if their length is more than 2.
            //This eliminates null entries and '0' entries.
            if(rootElement.Current.AutomationId.Length > 2)
            {
                Button elebox = new Button();
                elebox.Content = rootElement.Current.AutomationId;
                elebox.Name = "elebox";
                elebox.Click += new System.Windows.RoutedEventHandler(elebox_Click);
                CollectorBox.Items.Add(elebox);
                
            }

            //Conditions of which we use to limit our crawl results.
            //We will only find controls that are enabled and are labeled as controls.
            Condition condition1 = new PropertyCondition(AutomationElement.IsControlElementProperty, true);
            Condition condition2 = new PropertyCondition(AutomationElement.IsEnabledProperty, true);
            TreeWalker walker = new TreeWalker(new AndCondition(condition1, condition2));
            AutomationElement elementNode = walker.GetFirstChild(rootElement);

            //If the element node isn't null, we then crawl through the children.
            while (elementNode != null)
            {
                WalkEnabledElements(elementNode);
                elementNode = walker.GetNextSibling(elementNode);
            }
        }
        
        //Action to perform when clicking on the elebox.
        void elebox_Click(object sender, EventArgs e)
        {
            Button elebox = sender as Button;
            //Element control inside of list was clicked.
            //Descriptor.Text = elebox.Content.ToString();
        }

        //Hook button has been clicked, so fetch the entered process ID.
        private void TestClick(object sender, RoutedEventArgs e){
            Console.WriteLine("Fetching "+processInput.Text);
            var allProcess = Process.GetProcessesByName(processInput.Text);
            foreach (var process in allProcess)
            {
                try
                {
                    //Automation parent root has been located, continue with execution.
                    AutomationElement rootElement = AutomationElement.FromHandle(process.MainWindowHandle);
                    //Kick off our recursive crawling to find all controls.
                    try
                    {
                        WalkEnabledElements(rootElement);
                    }
                    catch (ArgumentException)
                    {
                        //If we run into an exception, note it in the console and end."
                        Console.WriteLine("Stopping with search. Handled exception when trying to kick off recursive crawl.");
                    }
                }
                catch (ArgumentException)
                {
                    Console.WriteLine("Unhandled exception when trying to find AutmationIds.");
                }
            }
       }
        //Actions to perform when clicking on the ActionsBox.
        //TODO: Remove this call, or make it expand the action clicked in order to f ill out details.
        private void ActionsBox_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ListBox dragSource = null;
            ListBox parent = (ListBox)sender;
            dragSource = parent;
            data = GetDataFromListBox(dragSource, e.GetPosition(parent));
            Console.WriteLine("captured "+data);

            //If the data isn't empty, then we'll capture the data as a global element, and execute
            //our dragging copy+move action.
            //TODO: Reset details of action control once copied but after the action has been added.
            if (data != null)
            {
                DragDrop.DoDragDrop(parent, data, DragDropEffects.Copy | DragDropEffects.Move);
            }
        }

        #region GetDataFromListBox(ListBox,Point)
        private static object GetDataFromListBox(ListBox source, Point point)
        {
            UIElement element = source.InputHitTest(point) as UIElement;
            if(element != null)
            {
                object data = DependencyProperty.UnsetValue;
                while(data == DependencyProperty.UnsetValue)
                {
                    data = source.ItemContainerGenerator.ItemFromContainer(element);
                    if(data == DependencyProperty.UnsetValue)
                    {
                        element = (VisualTreeHelper.GetParent(element) as UIElement);
                    }
                    if(element == source)
                    {
                        return null;
                    }
                }
                if(data != DependencyProperty.UnsetValue)
                {
                    return data;
                }
            }
            return null;
        }
        #endregion
        private void ActionsBox_Drop(object sender, DragEventArgs e)
        {
            Console.WriteLine("released " + data);
            ListBoxItem newlistitem = new ListBoxItem();
            object newdata = new object();
            
            newlistitem.Content = newdata;
            //ActionsBox.Items.Remove(data);
            TimelineBox.Items.Add(newlistitem);
        }
        private void menu_Import(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.Filter = "Automation files (*.automation)|*.automation|All files (*.*)|*.*";
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                // Open document 
                string filename = dlg.FileName;
                ListBoxItem loadedfile = new ListBoxItem();
                loadedfile.Content = filename;
                ActionsBox.Items.Add(loadedfile);
            }
        }
        private void menu_Export(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.Filter = "Automation files (*.automation)|*.automation|All files (*.*)|*.*";
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                // Save document 
                string filename = dlg.FileName;
                ListBoxItem loadedfile = new ListBoxItem();
                string[] stringcontents = new string[100];

                stringcontents[0] = "hello";
                stringcontents[1] = "dog";
                File.WriteAllLines(filename,stringcontents);
                ActionsBox.Items.Add(loadedfile);
            }
        }
        private void ActionsBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Selection change for actionsbox.
        }
    }
}