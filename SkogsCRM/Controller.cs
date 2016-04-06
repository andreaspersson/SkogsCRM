using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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


    }
}
