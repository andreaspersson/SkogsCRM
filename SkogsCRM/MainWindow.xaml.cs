using Microsoft.Maps.MapControl.WPF;
using System;
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
    }
}
