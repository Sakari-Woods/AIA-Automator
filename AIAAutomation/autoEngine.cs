using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using System.Windows.Automation;
using System.Diagnostics;
using Condition = System.Windows.Automation.Condition;

namespace AIAAutomation
{
    class autoEngine
    {
        private string targetProgram;
        private Boolean advReporting = false;

        // Replace String[] with an array version of the system controls type (system.windows.diagnogistics.controls.etc.etc?).
        public String[] Controls;
        
        // Captures process from displayed list and stores the
        // program in order to automation tasks.
        public void setTarget(String Name, Boolean advRep)
        {
            targetProgram = Name;
            advReporting = advRep;
        }

        // Below you'll find all the logic that we use to crawl through a program and collect the controls.
        //#################################################################################################

        // Function that's called that recursively searches for controls and appends each control to an array
        // stored in the autoEngine class. This makes it easy for managing controls, and enables us easy import/export
        // logic.
        private void WalkEnabledElements(AutomationElement rootElement)
        {
            // Only add Automation controls if their length is more than 2.
            // This eliminates null entries and '0' entries.
            // NOTE: We can add a checkbox for the user to toggle whether they want ALL controls, regardless if
            // they have automationids or names (this can happen especially if developers failed to label all controls).
            if(rootElement.Current.AutomationId.Length > 2)
            {
                // REWRITE: This is where we have found a control, and can save it to a list
                // to populate a drop-down or auto-complete textbox for the user to select.
                // OPTIONAL: Add a selection overlay over the running program control for the user
                // to verify that it's the control that they want.

                // We can add the value to our Controls array, NOTE: Make sure it handles resizing if we keep finding more.
                
                /*
                Button elebox = new Button();
                elebox.Content = rootElement.Current.AutomationId;
                elebox.Name = "elebox";
                elebox.Click += new System.Windows.RoutedEventHandler(elebox_Click);
                CollectorBox.Items.Add(elebox);
                */
                
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
            // DEPRECATED: This was used to call logic when the control was clicked on, but because we're
            // not adding it to a listbox anymore, this can be removed later.

            /*
            Button elebox = sender as Button;
            Element control inside of list was clicked.
            Descriptor.Text = elebox.Content.ToString();
            */
        }

        // Crawling function that grabs the process and calls the recursive searching system for controls.
        public void crawl(){
            var allProcess = Process.GetProcessesByName(targetProgram);
            
            foreach (var process in allProcess)
            {
                try
                {
                    // Automation parent root has been located, continue with execution.
                    AutomationElement rootElement = AutomationElement.FromHandle(process.MainWindowHandle);
                    // Kick off our recursive crawling to find all controls.
                    try
                    {
                        WalkEnabledElements(rootElement);
                    }
                    catch (ArgumentException)
                    {
                        // If we run into an exception, note it in the console and end."
                        Console.WriteLine("Stopping with search. Handled exception when trying to kick off recursive crawl.");
                    }
                }
                catch (ArgumentException)
                {
                    Console.WriteLine("Exception when trying to find AutmationIds.");
                }
            }
       }

    }
}
