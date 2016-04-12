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
            homeGridMap.Focus();
            
            SkogsDBEntities ctx = new SkogsDBEntities();
            var gridView = new GridView();
            this.listView.View = gridView;
            var gridView1 = new GridView();
            this.listViewCustomersGrid.View = gridView1;

            gridView.Columns.Add(new GridViewColumn
            {
                Header = "Personr",
                DisplayMemberBinding = new Binding("socialSecurityNbr")
            });
            gridView.Columns.Add(new GridViewColumn
            {
                Header = "Förnamn",
                DisplayMemberBinding = new Binding("firstName")
            });
            gridView.Columns.Add(new GridViewColumn
            {
                Header = "Efternamn",
                DisplayMemberBinding = new Binding("surname")
            });
            
            //gridView end

            gridView1.Columns.Add(new GridViewColumn
            {
                Header = "Personr",
                DisplayMemberBinding = new Binding("socialSecurityNbr")
            });
            gridView1.Columns.Add(new GridViewColumn
            {
                Header = "Förnamn",
                DisplayMemberBinding = new Binding("firstName")
            });
            gridView1.Columns.Add(new GridViewColumn
            {
                Header = "Efternamn",
                DisplayMemberBinding = new Binding("surname")
            });
            gridView1.Columns.Add(new GridViewColumn
            {
                Header = "Sales agent ID",
                DisplayMemberBinding = new Binding("employeeId")
            });
            //gridView1 end

            //ListView @ Home
            listView.ItemsSource = controller.GetAllCustomers();
            //ListView @ CustomerGrid           
            listViewCustomersGrid.ItemsSource = controller.GetAllCustomers();

            foreach (Customer c in controller.GetAllCustomers())
            {
                forestEstateGridcomboBox.Items.Add(c.socialSecurityNbr + " " + c.firstName + " " + c.surname);
            }

            

            //För filtreringen med TextBox
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(listViewCustomersGrid.ItemsSource);
            

            view.Filter = CustomerFilter;
            

        }//END OF MAINWINDOW

        private bool CustomerFilter(object item)
        {
            if (String.IsNullOrEmpty(textBox_customersGridFind.Text))
                return true;
            else
                return ((item as Customer).firstName.IndexOf(textBox_customersGridFind.Text, StringComparison.OrdinalIgnoreCase) >= 0)
                    || ((item as Customer).surname.IndexOf(textBox_customersGridFind.Text, StringComparison.OrdinalIgnoreCase) >= 0)
                    || ((item as Customer).socialSecurityNbr.IndexOf(textBox_customersGridFind.Text, StringComparison.OrdinalIgnoreCase) >= 0);
                    //|| ((item as Customer).SalesAgent.employeeId.ToString().IndexOf(textBox_customersGridFind.Text, StringComparison.OrdinalIgnoreCase) >= 0);
        }

        private void SortListByTextBox(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(listViewCustomersGrid.ItemsSource).Refresh();
        }

        private void ListViewClick(object sender, RoutedEventArgs e)
        {
            object obj = listView.SelectedItem;
            Customer c = obj as Customer;
            string id = c.socialSecurityNbr;

            homeGridMap.Children.Clear();//Annars skapas polygonerna på nytt över varandra vid varje knapptryck

            foreach (MapPolygon p in controller.DrawPolygons(id))
            {
                homeGridMap.Children.Add(p);
                homeGridMap.Center = p.Locations.ElementAt(0);
                //Flyttar kartvyn till polygonen
            }

        }//END OF ListViewClick

        private void homeButton_Click(object sender, RoutedEventArgs e)
        {
            customersGrid.Visibility = Visibility.Collapsed;
            forestEstatesGrid.Visibility = Visibility.Collapsed;
            salesAgentsGrid.Visibility = Visibility.Collapsed;
            homeGrid.Visibility = Visibility.Visible;
        }

        private void customersButton_Click(object sender, RoutedEventArgs e)
        {
            homeGrid.Visibility = Visibility.Collapsed;
            forestEstatesGrid.Visibility = Visibility.Collapsed;
            salesAgentsGrid.Visibility = Visibility.Collapsed;
            customersGrid.Visibility = Visibility.Visible;
        }
        private void forestEstatesButton_Click(object sender, RoutedEventArgs e)
        {
            customersGrid.Visibility = Visibility.Collapsed;
            homeGrid.Visibility = Visibility.Collapsed;
            salesAgentsGrid.Visibility = Visibility.Collapsed;
            forestEstatesGrid.Visibility = Visibility.Visible;
        }
        private void salesAgentButton_Click(object sender, RoutedEventArgs e)
        {
            customersGrid.Visibility = Visibility.Collapsed;
            forestEstatesGrid.Visibility = Visibility.Collapsed;
            homeGrid.Visibility = Visibility.Collapsed;
            salesAgentsGrid.Visibility = Visibility.Visible;

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
            polygon.Fill = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Blue);
            polygon.Stroke = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Green);
            polygon.StrokeThickness = 5;
            polygon.Opacity = 0.7;
            polygon.Locations = new LocationCollection();

            if (locations.Count >= 3)
            {

                polygon.Locations.Clear();

                foreach (Location loc in locations)
                {
                    polygon.Locations.Add(loc);
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


            /*
            e.Handled = true;

            System.Windows.Point pt = e.GetPosition(this);
            pointC.Add(pt);

            System.Windows.Point pt1 = pointC[0];
            System.Windows.Point pt3 = pointC[1];
            System.Windows.Point pt2 = new System.Windows.Point(pt3.X, pt1.Y);
            System.Windows.Point pt4 = new System.Windows.Point(pt1.X, pt3.Y);

            Microsoft.Maps.MapControl.WPF.Location loc1 = woodMap.ViewportPointToLocation(pt1);
            Microsoft.Maps.MapControl.WPF.Location loc2 = woodMap.ViewportPointToLocation(pt2);
            Microsoft.Maps.MapControl.WPF.Location loc3 = woodMap.ViewportPointToLocation(pt3);
            Microsoft.Maps.MapControl.WPF.Location loc4 = woodMap.ViewportPointToLocation(pt4);

            MapPolygon polygon = new MapPolygon();
            polygon.Stroke = new SolidColorBrush(Colors.Red);
            polygon.StrokeThickness = 3;
            polygon.Locations = new LocationCollection()
            {
                loc1, loc2, loc3, loc4
            };

            woodMap.Children.Add(polygon);
            */
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

        private void button_customersGridClearNewCFields_Click(object sender, RoutedEventArgs e)
        {
            textBox_newCustomerSalesAgentID.Clear();
            textBox_newCustomerSocNbr.Clear();
            textBox_newCustomerFirstName.Clear();
            textBox_newCustomerSurname.Clear();
        }

        private void button_customersGridClearEditCFields_Click(object sender, RoutedEventArgs e)
        {
            textBox_editCustomerSocNbr.Clear();
            textBox_editCustomerFirstName.Clear();
            textBox_editCustomerSurname.Clear();
            textBox_editCustomerSalesAgentId.Clear();
        }

        private void MenuItem_Edit_Click(object sender, RoutedEventArgs e)
        {
            //
        }
        private void MenuItem_NewForestEstate_Click(object sender, RoutedEventArgs e)
        {
            //
        }

    }//END OF MAINWINDOW
}   //END OF NAMESPACE
