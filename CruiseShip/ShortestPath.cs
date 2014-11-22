using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace CruiseShip
{
    class ShortestPath
    {
        static void Main(String[] args)
        {
            // Create the List of routes
            List<Route> routes = new List<Route>();

            // Get location of routes.txt file
            String assemblyPath = System.Reflection.Assembly.GetExecutingAssembly().CodeBase;
            String appPath = System.IO.Path.GetDirectoryName(assemblyPath);
            appPath = System.IO.Path.GetDirectoryName(appPath);
            appPath = System.IO.Path.GetDirectoryName(appPath);
            appPath = appPath.Substring(6);
            appPath += "/routes.txt";

            // Create variables for number of ports and routes
            int NUM_OF_PORTS = 0;
            int NUM_OF_ROUTES = 0;

            using (TextReader reader = File.OpenText(appPath))
            {
                string line = reader.ReadLine();
                string[] bits = line.Split(' ');

                // Get number of ports and routes
                NUM_OF_PORTS = int.Parse(bits[0]);
                NUM_OF_ROUTES = int.Parse(bits[1]);

                while ((line = reader.ReadLine()) != null)
                {
                    bits = line.Split(' ');
                    routes.Add(new Route(int.Parse(bits[0]), int.Parse(bits[1]), int.Parse(bits[2])));
                }
            }

            routes.Sort( (r1, r2) => r1.Fuel.CompareTo(r2.Fuel));
            PrintRoutesList(routes);

            JourneyManager jm = new JourneyManager(routes);
            PrintRoutesDictionary(jm.RoutesDict);

            Console.ReadKey();
        }

        #region Print Methods

        /// <summary>
        /// This method takes a List of Route objects and prints them.
        /// </summary>
        /// <param name="routes"> List of Route objects to be printed. </param>
        public static void PrintRoutesList(List<Route> routes)
        {
            foreach (Route r in routes)
            {
                Console.WriteLine(r);
            }
        }

        /// <summary>
        /// This method takes a Dictionary of integers that are connected to a List of Routes and prints them.
        /// </summary>
        /// <param name="d"> Dictionary of integers and Routes to be printed. </param>
        public static void PrintRoutesDictionary(OrderedDictionary d)
        {
            IDictionaryEnumerator e = d.GetEnumerator();
            while (e.MoveNext())
            {
                Console.WriteLine("Start: {0}", e.Key);
                PrintRoutesList(e.Value as List<Route>);
                Console.WriteLine();
            }
        }

        #endregion Print Methods
    }
}
