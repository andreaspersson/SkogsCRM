using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkogsCRM {
    class TestClass {
        //Solution Explorer -> Högerklick på SkogsCRM -> Properties -> Ändra Startup Object till testClass
        //Ändra också Output type till Console Application om ni pallar.
        public static void Main()
        {
            SalesAgent sa = new SalesAgent
            {
                firstName = "Rolf",
                surname = "Sturesson",
                employeeId = 3,
                telephoneNbr = "0736348934"
            };

            SkogsDBEntities lol = new SkogsDBEntities();
            SalesAgent s1 = lol.SalesAgent.Find(2);
            Console.WriteLine(s1.firstName);
            Console.WriteLine(s1.surname);
            //
            //lol.SalesAgent.Add(sa);
            //lol.SaveChanges();
            //Ovan fungerar för att lägga till skit i DBn

            ICollection<Customer> cCollection = s1.Customer;
            foreach (Customer c in cCollection)
            {
                Console.WriteLine(c.firstName);
            }
            Console.ReadLine();

        }
    }
}