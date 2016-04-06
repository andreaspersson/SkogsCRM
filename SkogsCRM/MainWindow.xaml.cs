using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Maps.MapControl.WPF;
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
       // string coordinates = "{lat: 55.708038, lng: 13.16853}, {lat: 55.70707, lng: 13.170247}, {lat: 55.706998, lng: 13.168015}, {lat: 55.707627, lng: 13.167973}, {lat: 55.708038, lng: 13.16853}";
        private Controller controller = new Controller();
        public MainWindow()
        {
            InitializeComponent();

            map.Focus();


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

            foreach (Customer c in ctx.Customer)
            {
                
                this.listView.Items.Add(new Customer { socialSecurityNbr = c.socialSecurityNbr, firstName = c.firstName, surname = c.surname }); ;
            }

            //dataGrid.ItemsSource = ctx.Customer.ToList();
            
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            //controller.AddCustomer; och så vidare!
           
        }

        private void button_Click_1(object sender, RoutedEventArgs e)
        {
           
        }
    }
}
