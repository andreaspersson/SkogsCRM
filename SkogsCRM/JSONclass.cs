using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkogsCRM
{
    class JSONclass
    {
        public string JsonStringMethod(){
        
        

        SkogsDBEntities ctx = new SkogsDBEntities();

            Coordinates xy = new Coordinates();
            Featuresz f = new Featuresz();

            ArrayList featureCollection = new ArrayList();

            string featureCollectionString = "FeatureCollection";
            string features = "Features";
            string geometryString = "Polygon";

            Customer c = new Customer();

            c = ctx.Customer.Find("851015");

            ArrayList feature = new ArrayList();

            foreach (var v in c.ForestEstate)
            {
                string cor = v.coordinates.ToString();
                ArrayList geometry = new ArrayList();
                Geometryz g = new Geometryz();
                Array corArray = cor.Split(',');

                for (int i = 0; i < corArray.Length; i++)
                {
                    //Console.WriteLine(corArray.GetValue(i));
                    ArrayList finalCoordsList = new ArrayList();
                    if (i < corArray.Length)
                    {

                        double lat;
                        double longitude;

                        string stringLat = corArray.GetValue(i).ToString();
                        string stringLongitude = corArray.GetValue(i + 1).ToString();

                        lat = double.Parse(stringLat, System.Globalization.CultureInfo.InvariantCulture);
                        longitude = double.Parse(stringLongitude, System.Globalization.CultureInfo.InvariantCulture);

                        finalCoordsList.Add(new double[] { lat, longitude
    });

                    }
                    i = i + 1;

                    foreach (double[] d in finalCoordsList)
                    {
                        ArrayList tmp = new ArrayList();
                        tmp.Add(d[1]);
                        Console.WriteLine(d[1]);
                        tmp.Add(d[0]);
                        Console.WriteLine(d[0]);
                        geometry.Add(tmp);

                    }


                }

                feature.Add(geometry);

                g.type = geometryString;
                g.coordinates = feature;



                f.type = features;
                f.geometry = g;

                featureCollection.Add(f);
                //g.Coordinates.Add(feature);


                //g.Coordinates.Add(feature);

                //g.Coordinates.Add(feature);


                //Console.WriteLine(v.coordinates.ToString()); 
            }

            

            xy.type = featureCollectionString;
            xy.features = featureCollection;
            //string JSONcoordinates;
            string JSONcoordinates = JsonConvert.SerializeObject(xy);

            return JSONcoordinates;
            
        
            //Console.WriteLine(JSONcoordinates);
        }//END OF JSONstringMethod
        
    } //END OF JSON
    public class Coordinates
    {
        private string Type;
        private ArrayList Features;

        //GETTERS & SETTERS
        public string type
        {
            get
            {
                return Type;
            }

            set
            {
                Type = value;
            }
        }

        public ArrayList features
        {
            get
            {
                return Features;
            }

            set
            {
                Features = value;
            }
        }


    }

    public class Featuresz
    {
        private string Type;
        private Geometryz Geometryz;


        //GETTERS & SETTERS
        public string type
        {
            get
            {
                return Type;
            }

            set
            {
                Type = value;
            }
        }
        public Geometryz geometry
        {
            get
            {
                return Geometryz;
            }

            set
            {
                Geometryz = value;
            }
        }

    }

    public class Geometryz
    {
        private string Type;
        private ArrayList Coordinates;

        //GETTERS & SETTERS
        public string type
        {
            get
            {
                return Type;
            }

            set
            {
                Type = value;
            }
        }
        public ArrayList coordinates
        {
            get
            {
                return Coordinates;
            }

            set
            {
                Coordinates = value;
            }
        }
    }
}//END OF NAMESPACE
