using System;
using System.Collections.Generic;

namespace FroggerStarter.Model
{
    /// <summary>
    ///     Manages an individual lane
    /// </summary>
    public class Lane
    {
        #region Properties

        /// <summary>
        ///     Gets and sets the speed of all vehicles in lane
        /// </summary>
        /// <value>
        ///     The speed of all vehicles in lane.
        /// </value>
        public double Speed { get; set; }

        /// <summary>
        ///     Gets the original speed of all vehicles in lane.
        /// </summary>
        /// <value>
        ///     The original speed of all vehicles in lane.
        /// </value>
        public double OriginalSpeed { get; }

        /// <summary>
        ///     Gets the direction of all vehicles in the lane
        /// </summary>
        /// <value>
        ///     The direction of all vehicles in lane.
        /// </value>
        public VehicleDirections Direction { get; }

        /// <summary>
        ///     Gets the vehicles.
        /// </summary>
        /// <value>
        ///     The vehicles.
        /// </value>
        public IList<Vehicle> Vehicles { get; } = new List<Vehicle>();

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="Lane" /> class.
        ///     Precondition: numVehicles greater than/= 0; speed greater than/= 0
        ///     Postcondition: LaneManager properties initialized
        /// </summary>
        /// <param name="numVehicles">The number vehicles.</param>
        /// <param name="vehicleType">Type of vehicles.</param>
        /// <param name="speed">The Speed of all vehicles in lane.</param>
        /// <param name="direction">The Direction of all vehicles in lane.</param>
        public Lane(int numVehicles, VehicleTypes vehicleType, double speed, VehicleDirections direction)
        {
            if (numVehicles < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(numVehicles), "numVehicles must be >= 0");
            }

            if (speed < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(speed), "speed must be > 0");
            }

            this.OriginalSpeed = speed;
            this.Speed = speed;
            this.Direction = direction;

            for (var i = 0; i < numVehicles; i++)
            {
                this.Vehicles.Add(new Vehicle(vehicleType));
            }
        }

        #endregion
    }
}