using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using Microsoft.Maps.MapControl.WPF;
using System.Collections;
using System.Data.Entity.Validation;
using System.Data.Entity.Infrastructure;

namespace SkogsCRM
{
    public class Controller
    {
        
        private SkogsDBEntities ctx = new SkogsDBEntities();

        

        public string AddCustomer(string socialSecurityNbr, string firstName, string surname, string employeeId)
        {
            string message = Utilities.CheckCustomerFieldsFormatting(socialSecurityNbr, firstName, surname, employeeId);
            if (message == null)
            {
                Customer c = new Customer();
                c.socialSecurityNbr = socialSecurityNbr;
                c.firstName = firstName;
                c.surname = surname;
                c.employeeId = Int32.Parse(employeeId);

                if (ctx.SalesAgent.Find(c.employeeId) != null) {
                    try
                    {
                        ctx.Customer.Add(c);
                        ctx.SaveChanges();
                        message = "Customer " + socialSecurityNbr + " added.";
                      
                    }
                    catch (DbUpdateException e)
                    {
                        message = Utilities.CheckMySqlException(e);
                    }
                }
                else
                {
                    message = "No such sales agent in the database!";
                }
            }
            return message;
        }

        public string EditCustomer(string socialSecurityNbr, string firstName, string surname, string employeeId)
        {
            string message = Utilities.CheckCustomerFieldsFormatting(socialSecurityNbr, firstName, surname, employeeId);
            if (message == null)
            {
                Customer c = ctx.Customer.Find(socialSecurityNbr);
                if (ctx.SalesAgent.Find(Int32.Parse(employeeId)) != null)
                {
                    try
                    {
                        c.firstName = firstName;
                        c.surname = surname;
                        c.employeeId = Int32.Parse(employeeId);
                        ctx.SaveChanges();
                        message = "Changes made to customer " + socialSecurityNbr + " has been saved.";
                    }
                    catch (DbUpdateException e)
                    {
                        message = Utilities.CheckMySqlException(e);
                    }
                }
                else
                {
                    message = "No such sales agent in the database!";
                }
            }
            return message;
        }

        public string AddForestEstate(string coordinates, string socialSecurityNbr)
        {
            string message = Utilities.CheckForestEstateFieldsFormatting(coordinates, socialSecurityNbr);
            if (message == null)
            {
                ForestEstate fE = new ForestEstate();
                fE.coordinates = coordinates;
                fE.socialSecurityNbr = socialSecurityNbr;

                try
                {
                    ctx.ForestEstate.Add(fE);
                    ctx.SaveChanges();
                    message = "Forest estate added.";
                }
                catch (DbUpdateException e)
                {
                    message = Utilities.CheckMySqlException(e);
                }
            }
            return message;
        }

        public string AddSalesAgent(string firstName, string surname, string employeeId, string telephoneNbr)
        {
            string message = Utilities.CheckSalesAgentFieldsFormatting(firstName, surname, employeeId, telephoneNbr);
            if (message == null)
            {
                SalesAgent sA = new SalesAgent();
                sA.firstName = firstName;
                sA.surname = surname;
                sA.telephoneNbr = telephoneNbr;
                sA.employeeId = Int32.Parse(employeeId);

                if (ctx.SalesAgent.Find(sA.employeeId) == null)
                {
                    try
                    {
                        ctx.SalesAgent.Add(sA);
                        ctx.SaveChanges();
                        message = "Sales Agent " + employeeId + " added.";
                    }
                    catch (DbUpdateException e)
                    {
                        message = Utilities.CheckMySqlException(e);
                    }
                }
                else
                {
                    message = "A sales agent with that ID already exists.";
                }
            }
            return message;
        }

        public string EditSalesAgent(string firstName, string surname, string employeeId, string telephoneNbr)
        {
            string message = Utilities.CheckSalesAgentFieldsFormatting(firstName, surname, employeeId, telephoneNbr);
            if (message == null)
            {
                SalesAgent sA = ctx.SalesAgent.Find(Int32.Parse(employeeId));
                if (ctx.SalesAgent.Find(sA.employeeId) != null)
                {
                    try
                    {
                        sA.firstName = firstName;
                        sA.surname = surname;
                        sA.telephoneNbr = telephoneNbr;
                        ctx.SaveChanges();
                        message = "Changes made to Sales Agent nbr " + employeeId + " has been saved.";
                    }
                    catch (DbUpdateException e)
                    {
                        message = Utilities.CheckMySqlException(e);
                    }
                }
                else
                {
                    message = "No such sales agent in the database!";
                }
            }
            return message;
        }

        public ArrayList DrawPolygons(string id)
        {
            Customer c = new Customer();
            c = ctx.Customer.Find(id);
            ArrayList al = new ArrayList();

            foreach (ForestEstate f in c.ForestEstate)
            {

                MapPolygon polygon = new MapPolygon();
                polygon.Fill = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.CornflowerBlue);
                polygon.Stroke = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Black);
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

            foreach (Customer c in ctx.Customer)
            {
                al.Add(c);
            }

            return al;
        }

        public ArrayList GetAllSalesAgents()
        {
            ArrayList al = new ArrayList();

            foreach (SalesAgent sa in ctx.SalesAgent)
            {
                al.Add(sa);
            }

            return al;
        }

        public void LogSQL()
        {
            ctx.Database.Log = Console.Write;
            /*
            ctx.Customer.Find("");
            ctx.Estate.Find("");
            ctx.ForestEstate.Find("");

            ctx.SaveChangesAsync().Wait();
            */
        }
        
    }//END OF CLASS

}//END OF NAMESPACE
