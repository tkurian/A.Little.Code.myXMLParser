

/* Tina Kurian
 * November 21st 2011
 * MainWindows.xaml.cs
 *      The purpose of this file is to open 
 *      any xml file and parse the data.
 *      As the program is parsing the data
 *      it will decide whether a node is a parent
 *      or child. In other words, it determines the 
 *      depth and will display a proper tree view
 *      of the xml data in the file.
 * 




/*REFERENCES: 
 * 
 *  1) http://support.microsoft.com/kb/307548 
 *
 *  2) http://www.c-sharpcorner.com/uploadfile/mahesh/openfiledialog-in-wpf/
 * 
 *  3) http://quickduck.com/blog/tag/wpf/
 */






//using statements required
using System;
using System.Collections.Generic;
using System.Linq;
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
using System.Xml;
using Microsoft.Win32; 

namespace myXMLParser
{

    /*
     * Class Name:  public partial class MainWindow : Window
     * Descsription: This class will parse an xml file and create the appropriate nodes
     */
    public partial class MainWindow : Window
    {

        //Creating a string used to hold the file name from openFileDialog
        private String fileName = null;


        /*
         *  FileName: MainWindow
         *  Description: Constructor
         *   Parameters: N/A
         *   Return: N/A  
         */
        public MainWindow()
        {
            InitializeComponent();
        }

        /*
         *  FileName: OpenAndProcessFile_Click
         *  Description: This method contains all the logic
         *          in order to parse and display a
         *          treeview of an arbitrary xml file
         *   Parameters: object sender, RoutedEventArgs e
         *   Return: void  
         */
        private void OpenAndProcessFile_Click(object sender, RoutedEventArgs e)
        {

            //create an instance of OpenFileDialog
            OpenFileDialog ofd = new OpenFileDialog();

            //Set a filter to only open xml files (although perhaps
            //this should be changed to accept other xml documents
            //which do not have a .xml extension 
            ofd.Filter = "XML documents (.xml)|*.xml";

            //Show open file dialog box. Can open will 
            //indicate whether it is possible to
            //open the file or not. See REFERENCES #2
            Nullable<bool> canOpen = ofd.ShowDialog();

            //If we can open the file than we will process/parse it!
            if (canOpen == true)
            {

                //Store the file name and get XmlTextReader to read it
                //see REFERENCES #2
                fileName = ofd.FileName;
                XmlTextReader reader = new XmlTextReader(fileName);

                //Create a TreeViewItem to keep track of parent nodes
                TreeViewItem theParent = null;

                //While we have not reached the end of the file, 
                //continue processing/parsing it.
                //see REFERENCES #1
                while (reader.Read())
                {

                    //If it is an element than determine if it is a child
                    //or parent and process it appropriately. 
                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Element:
                        {
                            var newNode = new TreeViewItem
                            {
                                Header = reader.Name
                            };

                            bool Empty = reader.IsEmptyElement;

                            //read in the name of the node
                            newNode.Header = reader.Name;

                            //If TreeViewItem created to keep track of the parent
                            //node is not null than we have a parent node which
                            //we need to add to our treeview else, we have a child
                            //node that we need to add to our treeview
                            if (theParent != null)
                            {
                                theParent.Items.Add(newNode);
                            }
                            else
                            {
                                myTreeView.Items.Add(newNode);
                            }

                            //if we have an empty elelemnt than we must set
                            //our parent node to the new node
                            if (!Empty)
                            {
                                theParent = newNode;
                            }
                            break;
                        }
                        case XmlNodeType.Text:
                        {
                            //Add the value of the parent to our tree
                            theParent.Items.Add(reader.Value);
                            break;
                        }
                        case XmlNodeType.EndElement:
                        {
                            //Recognize we have reached the end of the element
                            //and get TreeViewItem's parent item
                            if (theParent != null)
                            {
                                theParent = theParent.Parent as TreeViewItem;
                            }
                            break;
                        }
                    }
                }

            }
        }
    }
}
