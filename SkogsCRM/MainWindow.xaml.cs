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
            InitializeComponent();
            woodMap.Focus();
            
            SkogsDBEntities ctx = new SkogsDBEntities();
            var gridView = new GridView();
            this.listView.View = gridView;
            
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

            foreach (Customer c in controller.GetAllCustomers())
            {
                
                this.listView.Items.Add(new Customer { socialSecurityNbr = c.socialSecurityNbr, firstName = c.firstName, surname = c.surname }); ;
            }
                        
        }//END OF MAINWINDOW

        private void ListViewClick(object sender, RoutedEventArgs e)
        {
            object obj = listView.SelectedItem;
            Customer c = obj as Customer;
            string id = c.socialSecurityNbr;

            woodMap.Children.Clear();//Annars skapas polygonerna på nytt över varandra vid varje knapptryck

            foreach (MapPolygon p in controller.DrawPolygons(id))
            {
                woodMap.Children.Add(p);

            }

        }//END OF ListViewClick

        private void button_Click(object sender, RoutedEventArgs e)
        {
            //controller.AddCustomer; och så vidare!
           
        }



        private void homeButton_Click(object sender, RoutedEventArgs e)
        {
            homeGrid.Visibility = Visibility.Visible;
        }

        private void customersButton_Click(object sender, RoutedEventArgs e)
        {
            homeGrid.Visibility = Visibility.Collapsed;
        }

        
        ArrayList locations = new ArrayList();
       
        private void Map_MouseDown(object sender, System.Windows.Input.MouseEventArgs e)
        {
            e.Handled = true;
                        
            Location lul = woodMap.ViewportPointToLocation(e.GetPosition(this));
            
            locations.Add(lul);
         
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
                woodMap.Children.Clear();
                woodMap.Children.Add(polygon);
               
            }
            if (locations.Count > 5)
            {
                woodMap.Children.Clear();
                MessageBox.Show("Endast polygoner, alltså maximalt fem klick..." + "\n"  +"\n" + "Bing maps 4tehWin" + "\n"  + "\n" + "Gör om, gör rätt");
                locations.Clear();
                
            }


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
            woodMap.Children.Clear();
        }

    }//END OF MAINWINDOW
}   //END OF NAMESPACE
