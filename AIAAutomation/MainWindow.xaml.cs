
﻿/* Artificial Intelligence Association
 * RPA Automation Project
 * 
 * Developed by:
 * Sakari Woods
 * Sam Herr
 * Eli Jukanovich
 * Trevor Klein Villanueva
 * 
 * 
 * 
 * Description: This program is designed to allow easy
 * automation of any Windows application by collection controls
 * by their AutomationIDs, and providing an intuitive drag-and-drop
 * framework for the user to automation their projects, regardless of
 * programming or scripting experience. Designed to be lightweight and
 * with integrated dependencies.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
//This imports all of the libraries that we need.
//There are also some .dll files that are utilized inside our project directory
//that are needed for automation.

namespace AIAAutomation{

    //Our main class, this initializes our application window.
    public partial class MainWindow  : Window{

        autoEngine ae = new autoEngine();

        public MainWindow(){

            //Initialization of main window.
            InitializeComponent();
            
            HomeScreenWindow homeScreen = new HomeScreenWindow();
            homeScreen.Show();
            homeScreen.setMain(this);
        }

        private void ActionsBox_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ListBox dragSource = null;
            ListBox parent = (ListBox)sender;
            string buffer = (GetDataFromListBox(parent, e.GetPosition(parent))).GetType().ToString();
            if(buffer == "System.Windows.Controls.Primitives.ToolBarPanel")
            {
                Console.WriteLine("failure");
                return;
            }
            dragSource = parent;
            ListBoxItem bufferitem = new ListBoxItem();
            bufferitem = (ListBoxItem)(GetDataFromListBox(parent, e.GetPosition(parent)));
            
            string stringData = bufferitem.Name;
            string dataFormat = DataFormats.UnicodeText;
            DataObject data = new DataObject(dataFormat, stringData);
            //Console.WriteLine("captured "+ bufferitem.Name);
            //If the data isn't empty, then we'll capture the data as a global element, and execute
            //our dragging copy+move action.
            //TODO: Reset details of action control once copied but after the action has been added.
            if (data != null)
            {
                DragDrop.DoDragDrop(parent, data, DragDropEffects.Copy | DragDropEffects.Move);
            }

            }
            

        private void ActionsBox_MouseMove(object sender, MouseEventArgs e)
        {
                //Console.WriteLine("dragging with "+IsDragging);
        }

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

        //This function runs when we drop an action into our Timeline window.
        private void ActionsBox_Drop(object sender, DragEventArgs e)
        {
            //Gets the data of the action we dropped so we know what we're dropping.
            string content = (string)e.Data.GetData(DataFormats.StringFormat);

            //If it's our click action, we create a clickbox item and add it to the timeline.
            if (content == "ActionEvent_Click")
            {
                StackPanel clickPanel = new StackPanel();
                TextBlock clickText = new TextBlock();
                ComboBox clickComboBox = new ComboBox();
                clickComboBox.Margin = new Thickness(5);
                //Here we want to call our populate function that grabs the automationIDs
                //and then add them as items for our selection drop-down.
                //For now here's three examples to help design formatting.
                ComboBoxItem clickComboBoxItem1 = new ComboBoxItem();
                ComboBoxItem clickComboBoxItem2 = new ComboBoxItem();
                ComboBoxItem clickComboBoxItem3 = new ComboBoxItem();
                clickComboBoxItem1.Content = "Exit_Button";
                clickComboBoxItem2.Content = "Minimize_Button";
                clickComboBoxItem2.Content = "Maximize_Button";
                clickComboBox.Items.Add(clickComboBoxItem1);
                clickComboBox.Items.Add(clickComboBoxItem2);
                clickComboBox.Items.Add(clickComboBoxItem3);

                clickText.Text = "Clicking on ";
                clickPanel.Children.Add(clickText);
                clickPanel.Children.Add(clickComboBox);
                Console.WriteLine("Generate Click Functionality");
                TimelineBox.Items.Add(clickPanel);
            }

            //If it's our type action, we create a typebox item and add it to the timeline.
            else if (content == "ActionEvent_Type")
            {
                Console.WriteLine("Generate Type Functionality");
            }
            //If it's our wait action, we create a wait item and add it to the timeline.
            else if (content=="ActionEvent_Wait")
            {
                Console.WriteLine("Generate Wait Functionality");
            }
        }

        // Function that allows importing of automation files.
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

        // Function that allows exporting of automation files.
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
        // Function that handles finding the program for automation.
        private void hookButton_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("hookButton::clicked");
        }

        // Below here is the logic to pass our creation-wizard constructors into our engine,
        // and otherwise, here is how we can tie the interface into all autoEngine functionality as well.
        private void setTarget(String name)
        {
            ae.setTarget(name, false);
        }
    }
}
