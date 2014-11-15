using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CruiseShip
{
    class Route:IComparable<Route>
    {
        #region Fields

        int portA;
        int portB;
        int fuel;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// All args constructor.
        /// </summary>
        /// <param name="PortA"> Number of the starting port. </param>
        /// <param name="PortB"> Number of the destination port. </param>
        /// <param name="Fuel"> Fuel cost for the route. </param>
        public Route(int PortA, int PortB, int Fuel)
        {
            this.PortA = PortA;
            this.PortB = PortB;
            this.Fuel = Fuel;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// A positive integer representing the number of the starting port for this Route.
        /// </summary>
        public int PortA
        {
            get
            {
                return portA;
            }

            private set
            {
                if (value < 1)
                {
                    throw new ArgumentOutOfRangeException("Invalid value for PortA. Value must be greater than 0.");
                }

                portA = value;
            }
        }

        /// <summary>
        /// A positive integer representing the number of the destination port for this Route.
        /// </summary>
        public int PortB
        {
            get
            {
                return portB;
            }

            private set
            {
                if (value < 1)
                {
                    throw new ArgumentOutOfRangeException("Invalid value for PortB. Value must be greater than 0.");
                }

                portB = value;
            }
        }

        /// <summary>
        /// A positive integer representing the fuel cost for this Route.
        /// </summary>
        public int Fuel
        {
            get
            {
                return fuel;
            }

            private set
            {
                if (value < 1)
                {
                    throw new ArgumentOutOfRangeException("Invalid value for Fuel. Value must be greater than 0.");
                }

                fuel = value;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Overriding the ToString() method.
        /// </summary>
        /// <returns> A String containing the values in this objects properities. </returns>
        public override string ToString()
        {
            return "Route:\tPortA: " + this.PortA + " PortB: " + this.PortB + " Fuel " + this.Fuel;
        }

        /// <summary>
        /// Implementating the compareTo() method.
        /// </summary>
        /// <param name="other"> The Route object being compared to. </param>
        /// <returns> Returns less than 0 if this Route object has a smalle Fuel value. Returns 0 if the Fuel values match. Otherwise, returns greater than 0. </returns>
        public int CompareTo(Route other)
        {
            return this.Fuel.CompareTo(other.Fuel);
        }

        #endregion Methods

    }
}
