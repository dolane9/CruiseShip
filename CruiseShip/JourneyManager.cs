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
        List<Route> Routes
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
        HashSet<int> UniquePorts
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
        Dictionary<int, List<Route>> RoutesDict
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

        #endregion Methods
    }
}
