using Microsoft.Maps.MapControl.WPF;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SkogsCRM
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Controller controller = new Controller();
        public MainWindow()
        {
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            InitializeComponent();
            homeGrid.Visibility = Visibility.Visible;
            customersGrid.Visibility = Visibility.Collapsed;
            forestEstatesGrid.Visibility = Visibility.Collapsed;
            salesAgentsGrid.Visibility = Visibility.Collapsed;
            Style styleActive = FindResource("ActiveMenuButtonStyle") as Style;
            homeButton.Style = styleActive;
            homeGridMap.Focus();
            
            SkogsDBEntities ctx = new SkogsDBEntities();
            ctx.Database.Log = Console.Write;
            var gridViewCustomers = new GridView();
            var gridViewSalesAgents = new GridView();
            this.listView.View = gridViewCustomers;
            this.listViewSalesAgentGrid.View = gridViewSalesAgents;

            //Customer table
            gridViewCustomers.Columns.Add(new GridViewColumn
            {
                Header = "Personr",
                DisplayMemberBinding = new Binding("socialSecurityNbr")
            });
            gridViewCustomers.Columns.Add(new GridViewColumn
            {
                Header = "Förnamn",
                DisplayMemberBinding = new Binding("firstName")
            });
            gridViewCustomers.Columns.Add(new GridViewColumn
            {
                Header = "Efternamn",
                DisplayMemberBinding = new Binding("surname")
            });
            gridViewCustomers.Columns.Add(new GridViewColumn
            {
                Header = "Nbr of forest estates",
                DisplayMemberBinding = new Binding("ForestEstate.Count")
            });
            gridViewCustomers.Columns.Add(new GridViewColumn
            {
                Header = "Assigned agent's ID",
                DisplayMemberBinding = new Binding("employeeId")
            });

            //Sales agent table
            gridViewSalesAgents.Columns.Add(new GridViewColumn
            {
                Header = "Employee ID",
                DisplayMemberBinding = new Binding("employeeId")
            });
            gridViewSalesAgents.Columns.Add(new GridViewColumn
            {
                Header = "First name",
                DisplayMemberBinding = new Binding("firstName")
            });
            gridViewSalesAgents.Columns.Add(new GridViewColumn
            {
                Header = "Surname",
                DisplayMemberBinding = new Binding("surname")
            });
            gridViewSalesAgents.Columns.Add(new GridViewColumn
            {
                Header = "Telephone Nbr",
                DisplayMemberBinding = new Binding("telephoneNbr")
            });

            //ListView @ Home
            listView.ItemsSource = controller.GetAllCustomers();
            //ListView @ Sales Agents
            listViewSalesAgentGrid.ItemsSource = controller.GetAllSalesAgents();

            //För filtreringen med TextBox
            CollectionView viewCustomers = CollectionViewSource.GetDefaultView(listView.ItemsSource) as CollectionView;
            CollectionView viewSalesAgents = CollectionViewSource.GetDefaultView(listViewSalesAgentGrid.ItemsSource) as CollectionView;

            viewCustomers.Filter = CustomerFilter;
            viewSalesAgents.Filter = SalesAgentFilter;
            
            controller.LogSQL();

        }//END OF MAINWINDOW
        private bool CustomerFilter(object item)
        {
            if (String.IsNullOrEmpty(textBox_customersListViewFind.Text))
                return true;
            else
                return ((item as Customer).firstName.IndexOf(textBox_customersListViewFind.Text, StringComparison.OrdinalIgnoreCase) >= 0)
                    || ((item as Customer).surname.IndexOf(textBox_customersListViewFind.Text, StringComparison.OrdinalIgnoreCase) >= 0)
                    || ((item as Customer).socialSecurityNbr.IndexOf(textBox_customersListViewFind.Text, StringComparison.OrdinalIgnoreCase) >= 0);
        }

        private void SortListByCustomerTextBox(object sender, TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(listView.ItemsSource).Refresh();
        }

        private bool SalesAgentFilter(object item)
        {
            if (String.IsNullOrEmpty(textBox_salesAgentsGridFind.Text))
                return true;
            else
                return ((item as SalesAgent).employeeId.ToString().IndexOf(textBox_salesAgentsGridFind.Text, StringComparison.OrdinalIgnoreCase) >= 0)
                    || ((item as SalesAgent).firstName.IndexOf(textBox_salesAgentsGridFind.Text, StringComparison.OrdinalIgnoreCase) >= 0)
                    || ((item as SalesAgent).surname.IndexOf(textBox_salesAgentsGridFind.Text, StringComparison.OrdinalIgnoreCase) >= 0)
                    || ((item as SalesAgent).telephoneNbr.IndexOf(textBox_salesAgentsGridFind.Text, StringComparison.OrdinalIgnoreCase) >= 0);
        }

        private void SortListBySalesAgentTextBox(object sender, TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(listViewSalesAgentGrid.ItemsSource).Refresh();
        }

        private void ListViewClick(object sender, RoutedEventArgs e)
        {
            object obj = listView.SelectedItem;
            Customer c = obj as Customer;
            string id = c.socialSecurityNbr;

            homeGridMap.Children.Clear();//Annars skapas polygonerna på nytt över varandra vid varje knapptryck
            forestEstatesGridMap.Children.Clear();

            foreach (MapPolygon p in controller.DrawPolygons(id))
            {
                if (homeGrid.Visibility == Visibility.Visible)
                {
                    homeGridMap.Children.Add(p);
                    homeGridMap.Center = p.Locations.ElementAt(0); //Flyttar kartvyn till polygonen
                }
                if (forestEstatesGrid.Visibility == Visibility.Visible)
                {
                    forestEstatesGridMap.Children.Add(p);
                    forestEstatesGridMap.Center = p.Locations.ElementAt(0); //Flyttar kartvyn till polygonen
                }
            }

        }//END OF ListViewClick

        private void homeButton_Click(object sender, RoutedEventArgs e)
        {
            customersGrid.Visibility = Visibility.Collapsed;
            forestEstatesGrid.Visibility = Visibility.Collapsed;
            salesAgentsGrid.Visibility = Visibility.Collapsed;
            homeGrid.Visibility = Visibility.Visible;
            customerTableGrid.Visibility = Visibility.Visible;
            Style styleActive = FindResource("ActiveMenuButtonStyle") as Style;
            Style styleInActive = FindResource("MenuButtonStyle") as Style;
            customersButton.Style = styleInActive;
            forestEstatesButton.Style = styleInActive;
            salesAgentButton.Style = styleInActive;
            homeButton.Style = styleActive;
            homeGridMap.Children.Clear();
            locations.Clear();
            label_response.Content = "Response: ";
        }

        private void customersButton_Click(object sender, RoutedEventArgs e)
        {
            homeGrid.Visibility = Visibility.Collapsed;
            forestEstatesGrid.Visibility = Visibility.Collapsed;
            salesAgentsGrid.Visibility = Visibility.Collapsed;
            customersGrid.Visibility = Visibility.Visible;
            customerTableGrid.Visibility = Visibility.Visible;
            Style styleActive = FindResource("ActiveMenuButtonStyle") as Style;
            Style styleInActive = FindResource("MenuButtonStyle") as Style;
            homeButton.Style = styleInActive;
            forestEstatesButton.Style = styleInActive;
            salesAgentButton.Style = styleInActive;
            customersButton.Style = styleActive;
            label_response.Content = "Response: ";
        }
        private void forestEstatesButton_Click(object sender, RoutedEventArgs e)
        {
            customersGrid.Visibility = Visibility.Collapsed;
            homeGrid.Visibility = Visibility.Collapsed;
            salesAgentsGrid.Visibility = Visibility.Collapsed;
            forestEstatesGrid.Visibility = Visibility.Visible;
            customerTableGrid.Visibility = Visibility.Visible;
            Style styleActive = FindResource("ActiveMenuButtonStyle") as Style;
            Style styleInActive = FindResource("MenuButtonStyle") as Style;
            homeButton.Style = styleInActive;
            customersButton.Style = styleInActive;
            salesAgentButton.Style = styleInActive;
            forestEstatesButton.Style = styleActive;
            forestEstatesGridMap.Children.Clear();
            locations.Clear();
            label_response.Content = "Response: ";
        }
        private void salesAgentButton_Click(object sender, RoutedEventArgs e)
        {
            customersGrid.Visibility = Visibility.Collapsed;
            forestEstatesGrid.Visibility = Visibility.Collapsed;
            homeGrid.Visibility = Visibility.Collapsed;
            customerTableGrid.Visibility = Visibility.Collapsed;
            salesAgentsGrid.Visibility = Visibility.Visible;
            Style styleActive = FindResource("ActiveMenuButtonStyle") as Style;
            Style styleInActive = FindResource("MenuButtonStyle") as Style;
            homeButton.Style = styleInActive;
            customersButton.Style = styleInActive;
            forestEstatesButton.Style = styleInActive;
            salesAgentButton.Style = styleActive;
            label_response.Content = "Response: ";
        }

        ArrayList locations = new ArrayList();
        private void Map_MouseDown(object sender, System.Windows.Input.MouseEventArgs e)
        {
            e.Handled = true;

            if (homeGrid.Visibility == Visibility.Visible)
            {
                Location loc = homeGridMap.ViewportPointToLocation(e.GetPosition(homeGridMap));
                locations.Add(loc);
            }
            if (forestEstatesGrid.Visibility == Visibility.Visible)
            {
                Location loc = forestEstatesGridMap.ViewportPointToLocation(e.GetPosition(forestEstatesGridMap));
                locations.Add(loc);
            }
         }
        
        private void Map_MouseUp(object sender, System.Windows.Input.MouseEventArgs e)
        {
            MapPolygon polygon = new MapPolygon();
            polygon.Fill = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.CornflowerBlue);
            polygon.Stroke = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Black);
            polygon.StrokeThickness = 5;
            polygon.Opacity = 0.7;
            polygon.Locations = new LocationCollection();

            if (locations.Count >= 3)
            {

                polygon.Locations.Clear();

                foreach (Location loc in locations)
                {
                    polygon.Locations.Add(loc);
                    //coordinates += loc.Latitude.ToString().Replace(',', '.') + "," + loc.Longitude.ToString().Replace(',', '.') + ",";
                    //Console.WriteLine(coordinates);

                }

                if (homeGrid.Visibility == Visibility.Visible)
                {
                    homeGridMap.Children.Clear();
                    forestEstatesGridMap.Children.Clear();
                    homeGridMap.Children.Add(polygon);

                }
                if (forestEstatesGrid.Visibility == Visibility.Visible)
                {
                    forestEstatesGridMap.Children.Clear();
                    homeGridMap.Children.Clear();
                    forestEstatesGridMap.Children.Add(polygon);
                }
                
             }

            //if (locations.Count > 5)
            //{
            //    homeGridMap.Children.Clear();
            //    forestEstatesGridMap.Children.Clear();
            //    MessageBox.Show("Endast polygoner, alltså maximalt fem klick..." + "\n"  +  + "\n" + "Försök igen!");
            //    locations.Clear();
            //    
            //}
            
        }

        private void ManualMapReset(object sender, RoutedEventArgs e)
        {
            locations.Clear();
            
            if (homeGrid.Visibility == Visibility.Visible)
            {
                homeGridMap.Children.Clear();
            }
            if (forestEstatesGrid.Visibility == Visibility.Visible)
            {
                forestEstatesGridMap.Children.Clear();
            }
        }

        private void MapAerialView(object sender, RoutedEventArgs e)
        {
            if (homeGrid.Visibility == Visibility.Visible)
            {
                homeGridMap.Mode = new AerialMode();
            }
            else
            {
                forestEstatesGridMap.Mode = new AerialMode();
            }
        }

        private void MapRoadView(object sender, RoutedEventArgs e)
        {
            if (homeGrid.Visibility == Visibility.Visible)
            {
                homeGridMap.Mode = new RoadMode();
            }
            else
            {
                forestEstatesGridMap.Mode = new RoadMode();
            }
        }

        private void button_customersGridClearNewCFields_Click(object sender, RoutedEventArgs e)
        {
            textBox_newCustomerSalesAgentID.Clear();
            textBox_newCustomerSocNbr.Clear();
            textBox_newCustomerFirstName.Clear();
            textBox_newCustomerSurname.Clear();
        }

        private void button_customersGridClearEditCFields_Click(object sender, RoutedEventArgs e)
        {
            textBox_editCustomerSSN.Clear();
            textBox_editCustomerFirstName.Clear();
            textBox_editCustomerSurname.Clear();
            textBox_editCustomerSalesAgentId.Clear();
        }
        private void button_forestEstateGridClearSSNField_Click(object sender, RoutedEventArgs e)
        {
            textBox_addForestEstateSSN.Clear();
        }

        private void button_salesAgentGridClearAddFields_Click(object sender, RoutedEventArgs e)
        {
            textBox_newSalesAgentFirstName.Clear();
            textBox_newSalesAgentSurname.Clear();
            textBox_addNewSalesAgentID.Clear();
            textBox_addNewSalesAgentTelephoneNbr.Clear();
        }

        private void button_salesAgentGridClearEditFields(object sender, RoutedEventArgs e)
        {
            textBox_editSalesAgentFirstName.Clear();
            textBox_editSalesAgentSurname.Clear();
            textBox_editSalesAgentId.Clear();
            textBox_editSalesAgentTelephoneNbr.Clear();
        }

        private void MenuItem_Edit_Click(object sender, RoutedEventArgs e)
        {
            if (listView.SelectedItem != null)
            {
                Customer c = listView.SelectedItem as Customer;
                textBox_editCustomerSSN.Text = c.socialSecurityNbr;
                textBox_editCustomerFirstName.Text = c.firstName;
                textBox_editCustomerSurname.Text = c.surname;
                textBox_editCustomerSalesAgentId.Text = c.employeeId.ToString();
                customersButton_Click(sender, e);
            }
        }
        private void MenuItem_NewForestEstate_Click(object sender, RoutedEventArgs e)
        {
            if (listView.SelectedItem != null)
            {
                Customer c = listView.SelectedItem as Customer;
                textBox_addForestEstateSSN.Text = c.socialSecurityNbr;
                forestEstatesButton_Click(sender, e);
            }
        }
        private void MenuItem_Edit_SalesAgent_Click(object sender, RoutedEventArgs e)
        {
            if (listViewSalesAgentGrid.SelectedItem != null)
            {
                SalesAgent sa = listViewSalesAgentGrid.SelectedItem as SalesAgent;
                textBox_editSalesAgentFirstName.Text = sa.firstName;
                textBox_editSalesAgentSurname.Text = sa.surname;
                textBox_editSalesAgentTelephoneNbr.Text = sa.telephoneNbr;
                textBox_editSalesAgentId.Text = sa.employeeId.ToString();
            }
        }

        private void button_customersGridAddCustomer_Click(object sender, RoutedEventArgs e)
        {
            string socialSecurityNbr = textBox_newCustomerSocNbr.Text;
            string firstName = textBox_newCustomerFirstName.Text;
            string surname = textBox_newCustomerSurname.Text;
            string employeeId = textBox_newCustomerSalesAgentID.Text;
            string message = controller.AddCustomer(socialSecurityNbr, firstName, surname, employeeId);
            label_response.Content = "Response: " + message;
            listView.ItemsSource = controller.GetAllCustomers();
        }

        private void button_forestEstateGridAddForestEstate_Click(object sender, RoutedEventArgs e)
        {
            string socialSecurityNbr = textBox_addForestEstateSSN.Text;
            string coordinates = null;
            if (locations != null)
            {
                string tempCoords = null;
                foreach (Location loc in locations)
                {
                    tempCoords += loc.Latitude.ToString().Replace(',', '.') + "," + loc.Longitude.ToString().Replace(',', '.') + ",";
                }
                if (tempCoords != null)
                {
                    coordinates = tempCoords.Remove(tempCoords.Length - 1);
                }
            }
            string message = controller.AddForestEstate(coordinates, socialSecurityNbr);
            label_response.Content = "Response: " + message;
            listView.ItemsSource = controller.GetAllCustomers();
        }

        private void button_customersGridEditCustomer_Click(object sender, RoutedEventArgs e)
        {
            string socialSecurityNbr = textBox_editCustomerSSN.Text;
            string firstName = textBox_editCustomerFirstName.Text;
            string surname = textBox_editCustomerSurname.Text;
            string employeeId = textBox_editCustomerSalesAgentId.Text;
            string message = controller.EditCustomer(socialSecurityNbr, firstName, surname, employeeId);
            label_response.Content = "Response: " + message;
            listView.ItemsSource = controller.GetAllCustomers();
        }

        private void button_salesAgentGridAddSalesAgent_Click(object sender, RoutedEventArgs e)
        {
            string firstName = textBox_newSalesAgentFirstName.Text;
            string surname = textBox_newSalesAgentSurname.Text;
            string employeeId = textBox_addNewSalesAgentID.Text;
            string telephoneNbr = textBox_addNewSalesAgentTelephoneNbr.Text;
            string message = controller.AddSalesAgent(firstName, surname, employeeId, telephoneNbr);
            label_response.Content = "Response: " + message;
            listViewSalesAgentGrid.ItemsSource = controller.GetAllSalesAgents();
        }

        private void button_salesAgentGridEditSalesAgent_Click(object sender, RoutedEventArgs e)
        {
            string firstName = textBox_editSalesAgentFirstName.Text;
            string surname = textBox_editSalesAgentSurname.Text;
            string employeeId = textBox_editSalesAgentId.Text;
            string telephoneNbr = textBox_editSalesAgentTelephoneNbr.Text;
            string message = controller.EditSalesAgent(firstName, surname, employeeId, telephoneNbr);
            label_response.Content = "Response: " + message;
            listViewSalesAgentGrid.ItemsSource = controller.GetAllSalesAgents();
        }
    }//END OF MAINWINDOW
}//END OF NAMESPACE
