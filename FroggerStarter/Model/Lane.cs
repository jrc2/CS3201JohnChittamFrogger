﻿using System;
using System.Collections;
using System.Collections.Generic;

namespace FroggerStarter.Model
{
    /// <summary>
    ///     Manages an individual lane
    /// </summary>
    public class Lane : IList<Vehicle>
    {
        #region Data members

        private readonly IList<Vehicle> vehicles = new List<Vehicle>();

        #endregion

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
        ///     Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"></see>.
        /// </summary>
        public int Count => this.vehicles.Count;

        /// <summary>
        ///     Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1"></see> is read-only.
        /// </summary>
        public bool IsReadOnly => this.vehicles.IsReadOnly;

        /// <summary>
        ///     Gets or sets the <see cref="Vehicle" /> at the specified index.
        /// </summary>
        /// <value>
        ///     The <see cref="Vehicle" />.
        /// </value>
        /// <param name="index">The index.</param>
        /// <returns>the <see cref="Vehicle" /> at the specified index</returns>
        public Vehicle this[int index]
        {
            get => this.vehicles[index];
            set => this.vehicles[index] = value;
        }

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
                this.vehicles.Add(new Vehicle(vehicleType));
            }
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Adds the specified vehicle.
        /// </summary>
        /// <param name="vehicle">The vehicle.</param>
        public void Add(Vehicle vehicle)
        {
            this.vehicles.Add(vehicle);
        }

        /// <summary>
        ///     Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1"></see>.
        /// </summary>
        public void Clear()
        {
            this.vehicles.Clear();
        }

        /// <summary>
        ///     Determines whether this instance contains the object.
        /// </summary>
        /// <param name="vehicle">The vehicle.</param>
        /// <returns>
        ///     <c>true</c> if [contains] [the specified vehicle]; otherwise, <c>false</c>.
        /// </returns>
        public bool Contains(Vehicle vehicle)
        {
            return this.vehicles.Contains(vehicle);
        }

        /// <summary>
        ///     Copies the elements of the <see cref="T:System.Collections.Generic.ICollection`1"></see> to an
        ///     <see cref="T:System.Array"></see>, starting at a particular <see cref="T:System.Array"></see> index.
        /// </summary>
        /// <param name="array">
        ///     The one-dimensional <see cref="T:System.Array"></see> that is the destination of the elements
        ///     copied from <see cref="T:System.Collections.Generic.ICollection`1"></see>. The <see cref="T:System.Array"></see>
        ///     must have zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        public void CopyTo(Vehicle[] array, int arrayIndex)
        {
            this.vehicles.CopyTo(array, arrayIndex);
        }

        /// <summary>
        ///     Removes the specified vehicle.
        /// </summary>
        /// <param name="vehicle">The vehicle.</param>
        /// <returns>true if item was successfully removed from this.vehicles, false otherwise</returns>
        public bool Remove(Vehicle vehicle)
        {
            return this.vehicles.Remove(vehicle);
        }

        /// <summary>
        ///     Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        ///     An enumerator that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<Vehicle> GetEnumerator()
        {
            return this.vehicles.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        /// <summary>
        ///     Indexes the of.
        /// </summary>
        /// <param name="vehicle">The vehicle.</param>
        /// <returns>The index of vehicle if found; otherwise, -1.</returns>
        public int IndexOf(Vehicle vehicle)
        {
            return this.vehicles.IndexOf(vehicle);
        }

        /// <summary>
        ///     Inserts the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="vehicle">The vehicle.</param>
        public void Insert(int index, Vehicle vehicle)
        {
            this.vehicles.Insert(index, vehicle);
        }

        /// <summary>
        ///     Removes the <see cref="T:System.Collections.Generic.IList`1"></see> item at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the item to remove.</param>
        public void RemoveAt(int index)
        {
            this.vehicles.RemoveAt(index);
        }

        #endregion
    }
}