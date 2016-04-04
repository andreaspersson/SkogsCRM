using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkogsCRM {
    class TestClass {
        public static void Main()
        {
            SalesAgent sa = new SalesAgent
            {
                firstName = "Rolf",
                surname = "Sturesson",
                employeeId = 2,
                telephoneNbr = "0736348934"
            };

            SkogsDBEntities lol = new SkogsDBEntities();
            SalesAgent s1 = lol.SalesAgent.Find(2);
            Console.WriteLine(s1.firstName);
            Console.WriteLine(s1.surname);
            Console.ReadLine();
        }
    }
}