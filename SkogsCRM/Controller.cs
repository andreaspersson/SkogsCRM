using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using Microsoft.Maps.MapControl.WPF;
using System.Collections;

namespace SkogsCRM
{
    public class Controller
    {
        //Test
        private SalesAgent sA = new SalesAgent();
        private Customer c = new Customer();
        private ForestEstate fE = new ForestEstate();
        private ValidationChecker vC = new ValidationChecker();
        private SkogsDBEntities em = new SkogsDBEntities();

        //Behöver lite säkrare checkar i ValidationChecker-klassen, men annars bör något som nedan fungera bra!
        public string AddCustomer(string socNbr, string firstName, string surname, int employeeId)
        {
            string message;
            bool ok = vC.CheckNewCustomer(socNbr, firstName, surname, employeeId);
            if (ok = true && em.Customer.Find(socNbr) == null)
            {
                c.socialSecurityNbr = socNbr;
                c.firstName = firstName;
                c.surname = surname;
                c.employeeId = employeeId;
                em.Customer.Add(c);
                em.SaveChanges();
                message = "Customer added.";
            }
            else
            {
                message = "Error";
            }
            return message;
        }
        public string AddSalesAgent(string socNbr, string firstName, string surname, int employeeId)
        {
            string message;
            bool ok = vC.CheckNewSalesAgent(firstName, surname, employeeId);
            if (ok = true && em.SalesAgent.Find(employeeId) == null)
            {
                sA.firstName = firstName;
                sA.surname = surname;
                sA.employeeId = employeeId;
                em.SalesAgent.Add(sA);
                em.SaveChanges();
                message = "Sales agent added.";
            }
            else
            {
                message = "Error";
            }
            return message;
        }
        public string AddForestEstate(string coordinates, string socNbr)
        {
            string message;
            bool ok = vC.CheckNewForestEstate(coordinates, socNbr);
            if (ok = true && em.ForestEstate.Find(coordinates) == null)
            {
                fE.coordinates = coordinates;
                fE.socialSecurityNbr = socNbr;
                em.ForestEstate.Add(fE);
                em.SaveChanges();
                message = "Forest estate added.";
            }
            else
            {
                message = "Error";
            }
            return message;
        }

        public ArrayList DrawPolygons(string id)
        {
            Customer c = new Customer();
            c = em.Customer.Find(id);
            ArrayList al = new ArrayList();


            foreach (ForestEstate f in c.ForestEstate)
            {

                MapPolygon polygon = new MapPolygon();
                polygon.Fill = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Blue);
                polygon.Stroke = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Green);
                polygon.StrokeThickness = 5;
                polygon.Opacity = 0.7;



                polygon.Locations = new LocationCollection()
                {
                    //Här läggs Locations in i loopandet nedan
                };


                string cor = f.coordinates.ToString();
                Array corArray = cor.Split(',');

                for (int i = 0; i < corArray.Length; i++)
                {
                    if (i < corArray.Length)
                    {
                        double lat;
                        double longitude;

                        string stringLat = corArray.GetValue(i).ToString();
                        string stringLongitude = corArray.GetValue(i + 1).ToString();

                        lat = double.Parse(stringLat, System.Globalization.CultureInfo.InvariantCulture);
                        longitude = double.Parse(stringLongitude, System.Globalization.CultureInfo.InvariantCulture);

                        polygon.Locations.Add(new Location(lat,longitude));
                    }
                    i = i + 1;
                }
                al.Add(polygon);
            } //END OF FOREACH
            return al;
        }

        public ArrayList GetAllCustomers()
        {
            ArrayList al = new ArrayList();

            foreach (Customer c in em.Customer)
            {
                al.Add(c);
            }

            return al;
        }

    }//END OF CLASS
}//END OF NAMESPACE
