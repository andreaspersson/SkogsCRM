using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;

namespace SkogsCRM {
    class TestClass {
        //Solution Explorer -> Högerklick på SkogsCRM -> Properties -> Ändra Startup Object till testClass
        //Ändra också Output type till Console Application om ni pallar.
        public static void Main()
        {
            /*SalesAgent sa = new SalesAgent
            {
                firstName = "Rolf",
                surname = "Sturesson",
                employeeId = 3,
                telephoneNbr = "0736348934"
            };
            */
            SkogsDBEntities ctx = new SkogsDBEntities();
            SalesAgent s1 = ctx.SalesAgent.Find(2);
            //Console.WriteLine(s1.firstName);
            //Console.WriteLine(s1.surname);
            //
            //ctx.SalesAgent.Add(sa);
            //ctx.SaveChanges();
            //Ovan fungerar för att lägga till skit i DBn
            /*
            ICollection<Customer> cCollection = s1.Customer;
            foreach (Customer c1 in cCollection)
            {
               // Console.WriteLine(c.firstName);
            }
            */


            //https://blog.oneunicorn.com/2013/05/08/ef6-sql-logging-part-1-simple-logging/


            using (var context = new SkogsDBEntities())
            {
                context.Database.Log = Console.WriteLine;

                //Console.Write(context.Database.Log);

                context.Customer.Find("*");

                /*
                var blog = context.Blogs.First(b => b.Title == "One Unicorn");

                blog.Posts.First().Title = "Green Eggs and Ham";

                blog.Posts.Add(new Post { Title = "I do not like them!" });
                */
                context.SaveChangesAsync().Wait();
            }


            

            


            

            Console.ReadLine();

        }

       

    } //END OF TESTCLASS

    
} //END OF NAMESPACE