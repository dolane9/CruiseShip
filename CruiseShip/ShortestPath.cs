using System;
using System.Collections.Generic;
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

            //routes.Sort();
            //PrintRoutesList(routes);

            routes.Sort( (r1, r2) => r1.Fuel.CompareTo(r2.Fuel));
            PrintRoutesList(routes);

            //Console.WriteLine("Smallest Fuel-tank needed: " + CalculateSmallestFuelTank(routes, NUM_OF_PORTS));

            Console.ReadKey();
        }

        /// <summary>
        /// Takes a sorted list of Routes and returns the smallest fuel tank capacity
        /// needed to visit every Port at least once. Ports can be visited multiple times
        /// to 're-fill' the tank.
        /// </summary>
        /// <param name="routes"> A sorted list of Route objects. </param>
        /// <param name="numPorts"> The amount of unique ports that can be visited. </param>
        /// <returns> An int representing the smallest capacity the fuel tank needs to be. </returns>
        private static int CalculateSmallestFuelTank(List<Route> routes, int numPorts)
        {
            // Keep track of unique visited ports.
            HashSet<int> visitedPorts = new HashSet<int>();

            foreach (Route r in routes)
            {
                visitedPorts.Add(r.PortA);
                visitedPorts.Add(r.PortB);

                // The last port to be visited will belong to the Route that
                // contains the smallest possible fuel tank capacity.
                if (visitedPorts.Count == numPorts)
                {
                    return r.Fuel;
                }
            }

            return -1;
        }

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
    }
}
