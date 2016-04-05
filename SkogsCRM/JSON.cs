using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkogsCRM
{
    class JSON
    {

        public static void JsonStringMethod(){

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

                g.Type = geometryString;
                g.Coordinates = feature;



                f.Type = features;
                f.Geometry = g;

                featureCollection.Add(f);
                //g.Coordinates.Add(feature);


                //g.Coordinates.Add(feature);

                //g.Coordinates.Add(feature);


                //Console.WriteLine(v.coordinates.ToString()); 
            }
            xy.Type = featureCollectionString;
            xy.Features = featureCollection;
            string JSONcoordinates = JsonConvert.SerializeObject(xy);
            Console.WriteLine(JSONcoordinates);
        }



    } //END OF JSON
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
}//END OF NAMESPACE
