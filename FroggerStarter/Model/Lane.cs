using System;
using System.Collections;
using System.Collections.Generic;
using Windows.UI.Xaml;

namespace FroggerStarter.Model
{
    /// <summary>
    ///     Manages an individual lane
    /// </summary>
    public class Lane : IEnumerable<Vehicle>
    {
        #region Data members

        private readonly IList<Vehicle> vehicles = new List<Vehicle>();

        #endregion

        #region Properties

        /// <summary>
        ///     Gets the direction of all vehicles in the lane
        /// </summary>
        /// <value>
        ///     The direction of all vehicles in lane.
        /// </value>
        public VehicleDirections Direction { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Lane" /> class.
        /// Precondition: maxNumVehicles greater than/= 0; speed greater than/= 0
        /// Postcondition: LaneManager properties initialized
        /// </summary>
        /// <param name="maxNumVehicles">The number vehicles.</param>
        /// <param name="vehicleType">Type of vehicles.</param>
        /// <param name="speed">The Speed of all vehicles in lane.</param>
        /// <param name="direction">The Direction of all vehicles in lane.</param>
        /// <param name="windowWidth">Width of the window.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// maxNumVehicles - maxNumVehicles must be >= 0
        /// or
        /// speed - speed must be > 0
        /// </exception>
        public Lane(int maxNumVehicles, VehicleTypes vehicleType, double speed, VehicleDirections direction, double windowWidth)
        {
            if (maxNumVehicles < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(maxNumVehicles), "maxNumVehicles must be >= 0");
            }

            if (speed < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(speed), "speed must be > 0");
            }

            this.Direction = direction;

            for (var i = 0; i < maxNumVehicles; i++)
            {
                this.vehicles.Add(new Vehicle(vehicleType, direction, speed));
                var vehicle = this.vehicles[i];
                vehicle.X = windowWidth / maxNumVehicles * i - vehicle.Width;
                if (i > 0)
                {
                    vehicle.Sprite.Visibility = Visibility.Collapsed;
                }
            }
        }

        #endregion

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// An enumerator that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<Vehicle> GetEnumerator()
        {
            return this.vehicles.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}