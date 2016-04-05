using Newtonsoft.Json;
using System;
using System.Collections;
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

            SkogsDBEntities ctx = new SkogsDBEntities();
            SalesAgent s1 = ctx.SalesAgent.Find(2);
            Console.WriteLine(s1.firstName);
            Console.WriteLine(s1.surname);
            //
            //ctx.SalesAgent.Add(sa);
            //ctx.SaveChanges();
            //Ovan fungerar för att lägga till skit i DBn

            ICollection<Customer> cCollection = s1.Customer;
            foreach (Customer c in cCollection)
            {
               // Console.WriteLine(c.firstName);
            }


            Coordinates xy = new Coordinates();
            Coordinates.Featuresz f = new Coordinates.Featuresz();
            Coordinates.Featuresz.Geometryz g = new Coordinates.Featuresz.Geometryz();
            ArrayList featureCollection = new ArrayList();
            string featureCollectionString = "FeatureCollection";
            string features = "Features";
            string geometry = "Polygon";

            xy.Type = featureCollectionString;
            f.Type = features;
            g.Type = geometry;

            //Nullpointer
            //g.Coordinates.Add(something);
            
            f.Geometry = g;
            featureCollection.Add(f);
            xy.Features = featureCollection;

            string JSONcoordinates = JsonConvert.SerializeObject(xy);

            Console.WriteLine(JSONcoordinates);

            Console.ReadLine();

        }

       

    } //END OF TESTCLASS

    
    public class Coordinates
    {
        private string type;
        private ArrayList features;

        //GETTERS & SETTERS
        public string Type
        {
            get
            {
                return type;
            }

            set
            {
                type = value;
            }
        }

        public ArrayList Features
        {
            get
            {
                return features;
            }

            set
            {
                features = value;
            }
        }

        public class Featuresz
        {
            private string type;
            private Geometryz geometryz;


            //GETTERS & SETTERS
            public string Type
            {
                get
                {
                    return type;
                }

                set
                {
                    type = value;
                }
            }
            public Geometryz Geometry
            {
                get
                {
                    return geometryz;
                }

                set
                {
                    geometryz = value;
                }
            }

            public class Geometryz
            {
                private string type;
                private ArrayList coordinates;

                //GETTERS & SETTERS
                public string Type
                {
                    get
                    {
                        return type;
                    }

                    set
                    {
                        type = value;
                    }
                }
                public ArrayList Coordinates
                {
                    get
                    {
                        return coordinates;
                    }

                    set
                    {
                        coordinates = value;
                    }
                }
            }
        }
    }

} //END OF NAMESPACE