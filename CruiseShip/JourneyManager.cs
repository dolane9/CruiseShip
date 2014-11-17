using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CruiseShip
{
    class JourneyManager
    {
        #region Fields

        // List of Route objects. Each Route is avaible in this journey.
        private List<Route> routes;

        // A HashSet of ints. Each int represents a port number avaible in this journey.
        private HashSet<int> uniquePorts;

        // The number of unique ports in this journey.
        private int numUniquePorts;

        // Dictionary: Key: A port number, Value: List of Route objects with a matching starting port and the key.
        private Dictionary<int, List<Route>> routesDict;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// All args constructor.
        /// </summary>
        /// <param name="routes"> A List containing Route objects. </param>
        public JourneyManager(List<Route> routes)
        {
            this.Routes = routes;
            this.uniquePorts = this.GetUniquePorts();
            this.numUniquePorts = uniquePorts.Count();
            this.routesDict = this.CreateRoutesDictionary();
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// A List of Routes available to a ship in this particular journey.
        /// </summary>
        public List<Route> Routes
        {
            get
            {
                return this.routes;
            }
            private set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("Invalid value for Routes. Value must not be null.");
                }

                value = this.routes;
            }
        }

        /// <summary>
        /// A HashSet of ints. Each in represents a port number avaible to this Journey.
        /// </summary>
        public HashSet<int> UniquePorts
        {
            get
            {
                return this.uniquePorts;
            }
            private set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("Invalid value for UniquePorts. Value must not be null.");
                }

                value = this.uniquePorts;
            }
        }

        /// <summary>
        /// A Dictionary of Routes available to a ship in this particular journey. Key: Port number, Value: Routes that start with this port.
        /// </summary>
        public Dictionary<int, List<Route>> RoutesDict
        {
            get
            {
                return this.routesDict;
            }
            private set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("Invalid value for RoutesDict. Value must not be null.");
                }

                value = this.routesDict;
            }
        }

        /// <summary>
        /// The amount of unique ports in this journey.
        /// </summary>
        public int NumUniquePorts
        {
            get
            {
                return this.numUniquePorts;
            }
            private set
            {
                if (value < 1)
                {
                    throw new ArgumentOutOfRangeException("Invalid value for NumUniquePorts. Value must be at least 1.");
                }

                value = this.numUniquePorts;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Returns the smallest fuel tank capacity needed to visit every Port at
        /// least once. Ports can be visited multiple times to 're-fill' the tank.
        /// </summary>
        /// <returns> An int representing the smallest capacity the fuel tank needs to be.
        /// Returns -1 if no valid value could be found. </returns>
        public int CalculateSmallestFuelTankNeeded()
        {
            // Keep track of unique visited ports.
            HashSet<int> visitedPorts = new HashSet<int>();

            foreach (Route r in this.Routes)
            {
                visitedPorts.Add(r.PortA);
                visitedPorts.Add(r.PortB);

                // The last port to be visited will belong to the Route that
                // contains the smallest possible fuel tank capacity.
                if (visitedPorts.Count == this.numUniquePorts)
                {
                    return r.Fuel;
                }
            }

            return -1;
        }

        /// <summary>
        /// Sorts the internal List of Routes based on the PortA value in each Route object.
        /// </summary>
        public void SortListByPortA()
        {
            this.Routes.Sort((r1, r2) => r1.PortA.CompareTo(r2.PortA));
        }

        /// <summary>
        /// Sorts the internal List of Routes based on the PortB value in each Route object.
        /// </summary>
        public void SortListByPortB()
        {
            this.Routes.Sort((r1, r2) => r1.PortB.CompareTo(r2.PortB));
        }

        /// <summary>
        /// Sorts the internal List of Routes based on the Fuel value in each Route object.
        /// </summary>
        public void SortListByFuel()
        {
            this.Routes.Sort((r1, r2) => r1.Fuel.CompareTo(r2.Fuel));
        }

        /// <summary>
        /// Sorts the Dictionary data Field based on the amount of Routes
        /// in the Value on each entry.
        /// </summary>
        public void SortDictByAmoutOfRoutesInValue()
        {
            // Consider changing the Dictionary to an OrderedDictionary
        }
        
        /// <summary>
        /// Prints the String returned from the toString() method to standard output.
        /// </summary>
        public void PrintToString()
        {
            Console.WriteLine(this.ToString());
        }

        /// <summary>
        /// Overriding the ToString() method.
        /// </summary>
        /// <returns> A String containing the values in each Route object contained within the List. </returns>
        public override string ToString()
        {
            string str = typeof(JourneyManager).Name;
            foreach (Route r in this.Routes)
            {
                str += "\n" + r.ToString();
            }
            return str;
        }

        /// <summary>
        /// Takes a List of Route objects and returns a HashSet of ints
        /// for each port number in the List of Route objects.
        /// </summary>
        /// <param name="routes"> The List of Route objects. </param>
        /// <returns> A HashSet of ints containing the each port number. </returns>
        private HashSet<int> GetUniquePorts()
        {
            HashSet<int> uniquePorts = new HashSet<int>();

            foreach (Route r in this.Routes)
            {
                uniquePorts.Add(r.PortA);
                uniquePorts.Add(r.PortB);
            }

            return uniquePorts;
        }

        /// <summary>
        /// Creates a Dictionary based on a List of Routes. Each entry contains a
        /// starting port and a list of Routes that contain this starting port.
        /// </summary>
        /// <returns> A Dictionary containing A List of Routes based on their starting port. </returns>
        private Dictionary<int, List<Route>> CreateRoutesDictionary()
        {
            Dictionary<int, List<Route>> routesDict = new Dictionary<int, List<Route>>();

            // Create one entry in the Dictionary for each starting port (For each different PortA value)
            foreach (Route r in this.Routes)
            {
                if (routesDict.ContainsKey(r.PortA))
                {
                    // An entry with this starting point exists. Add the Route to this entry.
                    List<Route> routesList = routesDict[r.PortA];
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

        /// <summary>
        /// This method will add a Route to the List of Routes (if it does not
        /// already exist) and will NOT update the other data fields.
        /// </summary>
        /// <param name="r"> The Route object to be added to this journey. </param>
        /// <returns> True if the Route was added. Otherwise, false. </returns>
        public bool AddRouteWithoutUpdate(Route r)
        {
            return this.AddRoute(r, false);
        }

        /// <summary>
        /// This method will add the Route supplied to the List of Route
        /// objects and update the other data Fields accordingly.
        /// </summary>
        /// <param name="r"> The Route object to be added to this journey. </param>
        /// <param name="update"> If true, call the UpdateJourneyStats() method. Default value is true. </param>
        /// <returns> True if the Route was added. Otherwise, false. </returns>
        public bool AddRoute(Route r, bool update = true)
        {
            // If the Route does not already exist then add the Route to List of Routes
            if (!this.Routes.Contains(r))
            {
                this.Routes.Add(r);

                // If the bool supplied is true then update the remaining data Fields
                if (update)
                {
                    this.UpdateJourneyStats(r);
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// This method updates the fields in this class used to keep information about the Route objects.
        /// </summary>
        /// <param name="route"> A Route object representing the Route that has been added to the List of Routes. </param>
        private void UpdateJourneyStats(Route route)
        {
            // Add the two ports to the HashSet of ports.
            this.UniquePorts.Add(route.PortA);
            this.UniquePorts.Add(route.PortB);

            // Update the number of unique ports (may stay the same)
            this.NumUniquePorts = this.UniquePorts.Count();

            // Add the route to the relevant Dictionary entry or create a new one if needed
            this.AddRouteToDictionary(route);
        }

        /// <summary>
        /// This method adds the Route supplied as an argument into the Dictionary in this class.
        /// </summary>
        /// <param name="r"> The Route to be added to the Dictionary. </param>
        private void AddRouteToDictionary(Route r)
        {
            // Check if the Dictionary contains a key with this Routes' starting port
            if (this.RoutesDict.ContainsKey(r.PortA))
            {
                // Dictionary contains this key, add route to the existing entry
                List<Route> tempRoutes = this.RoutesDict[r.PortA];
                tempRoutes.Add(r);
                this.RoutesDict[r.PortA] = tempRoutes;
            }
            else
            {
                // Dictionary doesn't contain this key, add an entry for it
                this.RoutesDict.Add(r.PortA, new List<Route>() { r });
            }
        }

        #endregion Methods
    }
}
