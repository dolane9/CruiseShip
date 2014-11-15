using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace CruiseShip
{
    class Program
    {
        /*
        static void Main(string[] args)
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

                while((line = reader.ReadLine()) != null)
                {
                    // Add 2 entries for each bidirectional Route (A -> B and B -> A)
                    bits = line.Split(' ');
                    routes.Add( new Route(int.Parse(bits[0]), int.Parse(bits[1]), int.Parse(bits[2])));
                    //routes.Add(new Route(int.Parse(bits[1]), int.Parse(bits[0]), int.Parse(bits[2])));
                }
            }

            Dictionary<int, List<Route>> routesDict = CreateRoutesDictionary(routes);
            routesDict = SortRoutes(routesDict);
            PrintRoutesDictionary(routesDict);

            int smallestFuel = CalculateSmallestFuel(routesDict, NUM_OF_PORTS);
            //int smallestFuel = FindSmallestFuel(routesDict, NUM_OF_PORTS);
            Console.WriteLine("********Smallest Fuel needed: {0}", smallestFuel);

            // Wait for key before exiting
            Console.ReadKey();
        }
        */

        private static int CalculateSmallestFuel(Dictionary<int, List<Route>> routesDict, int numPorts)
        {
            HashSet<int> visitedPorts = new HashSet<int>();
            int smallestFuel = 0;

            while (visitedPorts.Count < numPorts)
            {
                Route temp = GetSmallestFuelRoute(routesDict);
                visitedPorts.Add(temp.PortA);
                visitedPorts.Add(temp.PortB);

                // Remove this Route
                List<Route> routes = routesDict[temp.PortA];
                routes.Remove(temp);
                routesDict[temp.PortA] = routes;

                if (visitedPorts.Count == numPorts)
                {
                    return temp.Fuel;
                }

                Console.WriteLine("Hit key to finish while loop iteration..");
                Console.ReadKey();
            }

            return smallestFuel;
        }

        private static int FindSmallestFuel(Dictionary<int, List<Route>> routesDict, int numPorts)
        {
            // Get the lowest fuel needed for the first
            Route smallestFuelRoute = GetSmallestFuelRoute(routesDict);
            int highestFuel = smallestFuelRoute.Fuel;
            int currentPort = smallestFuelRoute.PortB;
            
            // Create a List of all ports that have been visited
            List<int> visitedPorts = new List<int>();
            visitedPorts.Add(smallestFuelRoute.PortA);

            Console.WriteLine("Starting Port: {0}", visitedPorts[0]);

            // Keep a record of which Route was followed
            int lastRouteChoosen = -1;

            while (visitedPorts.Count < numPorts)
            {
                // Iterate through each Route from this port. Take the route with the smallest fuel value that has
                // destination that hasn't already been visited.
                for (int i = 0; i < routesDict[currentPort].Count; i++)
                {
                    Console.WriteLine("Current Port: {0}, Fuel of Shortest Route: {1}, Minimum Fuel so far: {2}",
                        currentPort, routesDict[currentPort][i].Fuel, highestFuel);

                    // Check if the destination port has already been visited (avoid getting stuck in a loop)
                    if (!visitedPorts.Contains(routesDict[currentPort][i].PortB) && routesDict[currentPort][i].Fuel >= highestFuel)
                    {
                        visitedPorts.Add(smallestFuelRoute.PortB);
                        highestFuel = routesDict[currentPort][i].Fuel;
                        currentPort = routesDict[currentPort][i].PortB;
                        Console.WriteLine("Traveling from {0} to {1}", routesDict[currentPort][i].PortA, routesDict[currentPort][i].PortB);
                        lastRouteChoosen = i;
                        break;
                    }
                    else
                    {
                        // Need to back track back a port
                        
                        if (visitedPorts.Count() - 1 >= 0)
                        {
                            // Remove the last added port, not going to take this route
                            visitedPorts.RemoveAt(visitedPorts.Count - 1);

                            if (visitedPorts.Count() - 1 >= 0)
                            {
                                // Set current port to the last port in the List of visited ports
                                currentPort = visitedPorts[visitedPorts.Count - 1];

                                // Set the counter to be the lastRouteChoosen (iteration will end and the loop will check the next shortest Route)
                                i = lastRouteChoosen;

                                Console.WriteLine("in else bracket");
                            }
                            else
                            {
                                // Force out of while loop, for testing
                                numPorts = -1;
                            }
                        }
                    }
                }
            }

            return highestFuel;
        }

        /// <summary>
        /// Finds and returns the smallest Fuel value in any of the Route objects in the parameter.
        /// </summary>
        /// <param name="routesDict"> A Dictionary containing the Route objects to search through. </param>
        /// <returns> The smallest Fuel value from the Route objects in the Dictionary. </returns>
        private static Route GetSmallestFuelRoute(Dictionary<int, List<Route>> routesDict)
        {
            Route smallestFuelRoute = routesDict[1][0];

            foreach(KeyValuePair<int, List<Route>> entry in routesDict)
            {
                if (entry.Value.Count() > 0 && entry.Value[0].Fuel < smallestFuelRoute.Fuel)
                {
                    smallestFuelRoute = entry.Value[0];
                }
            }

            return smallestFuelRoute;
        }

        /// <summary>
        /// Takes a Dictionary with a List of routes as the value and sorts them.
        /// </summary>
        /// <param name="routesDict"> A Dictionary containing the List of routes to be sorted. </param>
        /// <returns> Returns the Dictionary supplied with the each List of routes sorted. </returns>
        private static Dictionary<int, List<Route>> SortRoutes(Dictionary<int, List<Route>> routesDict)
        {
            Dictionary<int, List<Route>> sortedRoutes = new Dictionary<int, List<Route>>();

            // Iterate through Dictionary supplied and sort each List of routes
            foreach (KeyValuePair<int, List<Route>> e in routesDict)
            {
                // Sort List of routes and add to Dictionary
                e.Value.Sort();
                sortedRoutes.Add(e.Key, e.Value);
            }

            return sortedRoutes;
        }

        /// <summary>
        /// Creates a Dictionary based on a List of Routes. Each entry contains a starting port
        /// and a list of Routes that contain this starting port.
        /// </summary>
        /// <param name="routes"> List containing the Routes that will be in the Dictionary. </param>
        /// <returns> A Dictionary containing A List of Routes based on their starting port. </returns>
        public static Dictionary<int, List<Route>> CreateRoutesDictionary(List<Route> routes)
        {
            Dictionary<int, List<Route>> routesDict = new Dictionary<int, List<Route>>();

            // Create one entry in the Dictionary for each starting port (For each different PortA value)
            foreach (Route r in routes)
            {
                if (routesDict.ContainsKey(r.PortA))
                {
                    // An entry with this starting point exists. Add the Route to this entry.
                    List<Route> routesList = routesDict[r.PortA];// = routesDict[r.PortA].Add(r);
                    routesList.Add(r);
                    routesDict[r.PortA] = routesList;
                }
                else
                {
                    // There is no entry with this starting point. Create one.
                    routesDict.Add(r.PortA, new List<Route>() { r });
                }
            }

            return routesDict;
        }
    }
}
