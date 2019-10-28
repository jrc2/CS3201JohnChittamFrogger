using System;
using System.Collections;
using System.Collections.Generic;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace FroggerStarter.Model
{
    /// <summary>
    ///     Manages an individual lane
    /// </summary>
    public class Lane : ICollection<Vehicle>
    {
        #region Data members

        private readonly IList<Vehicle> vehicles = new List<Vehicle>();
        private readonly double laneLength;
        private readonly int maxNumVehicles;
        private readonly VehicleTypes vehicleType;
        private readonly double originalSpeed;

        #endregion

        #region Properties

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

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="Lane" /> class.
        ///     Precondition: maxNumVehicles greater than/= 0; originalSpeed greater than/= 0, laneLength greater than 0
        ///     Postcondition: LaneManager properties initialized
        /// </summary>
        /// <param name="maxNumVehicles">The maximum number of vehicles.</param>
        /// <param name="vehicleType">Type of the vehicle.</param>
        /// <param name="originalSpeed">The originalSpeed.</param>
        /// <param name="direction">The direction.</param>
        /// <param name="laneLength">Width of the window.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     maxNumVehicles - maxNumVehicles must be >= 0
        ///     or
        ///     originalSpeed - originalSpeed must be > 0
        ///     or
        ///     laneLength - laneLength must be > 0
        /// </exception>
        public Lane(int maxNumVehicles, VehicleTypes vehicleType, double originalSpeed, VehicleDirections direction,
            double laneLength)
        {
            if (maxNumVehicles < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(maxNumVehicles), "maxNumVehicles must be >= 0");
            }

            if (originalSpeed < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(originalSpeed), "originalSpeed must be > 0");
            }

            if (laneLength <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(laneLength), "laneLength must be > 0");
            }

            this.maxNumVehicles = maxNumVehicles;
            this.vehicleType = vehicleType;
            this.originalSpeed = originalSpeed;
            this.Direction = direction;
            this.laneLength = laneLength;

            this.addVehiclesToRoad(direction);
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
        /// <returns>true if item was successfully removed from this.vehicles; otherwise, false.</returns>
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

        private void addVehiclesToRoad(VehicleDirections direction)
        {
            for (var i = 0; i < this.maxNumVehicles; i++)
            {
                this.vehicles.Add(new Vehicle(this.vehicleType, direction, this.originalSpeed));
                var vehicle = this.vehicles[i];
                vehicle.X = this.laneLength / this.maxNumVehicles * i - vehicle.Width;

                if (direction == VehicleDirections.Right)
                {
                    vehicle.Sprite.RenderTransformOrigin = new Point(0.5, 0.5);
                    vehicle.Sprite.RenderTransform = new ScaleTransform {ScaleX = -1};
                }
            }

            this.ResetVehicleVisibility();
        }

        /// <summary>
        ///     Sets the first vehicle in lane to visible and the rest collapsed.
        /// </summary>
        public void ResetVehicleVisibility()
        {
            for (var i = 0; i < this.vehicles.Count; i++)
            {
                var vehicle = this.vehicles[i];
                if (i > 0)
                {
                    vehicle.Sprite.Visibility = Visibility.Collapsed;
                }
            }

            this.vehicles[0].Sprite.Visibility = Visibility.Visible;
        }

        /// <summary>
        ///     Increases the speed of all vehicles.
        /// </summary>
        /// <param name="amountToAdd">The amount to add.</param>
        public void IncreaseVehicleSpeeds(double amountToAdd)
        {
            foreach (var vehicle in this.vehicles)
            {
                vehicle.Speed += amountToAdd;
            }
        }

        /// <summary>
        ///     Resets the vehicle speeds.
        /// </summary>
        public void ResetVehicleSpeeds()
        {
            foreach (var vehicle in this.vehicles)
            {
                vehicle.Speed = this.originalSpeed;
            }
        }

        /// <summary>
        ///     Moves all vehicles.
        /// </summary>
        public void MoveAllVehicles()
        {
            foreach (var vehicle in this.vehicles)
            {
                this.MoveVehicle(vehicle);
            }
        }

        /// <summary>
        ///     Moves the vehicle.
        ///     Postcondition: vehicle has moved
        /// </summary>
        /// <param name="vehicle">The vehicle to move.</param>
        public void MoveVehicle(Vehicle vehicle)
        {
            if (vehicle.Direction == VehicleDirections.Right)
            {
                this.moveVehicleRight(vehicle);
            }
            else
            {
                this.moveVehicleLeft(vehicle);
            }
        }

        private void moveVehicleLeft(Vehicle vehicle)
        {
            if (vehicleHasCrossedLeftEdge(vehicle))
            {
                this.respawnVehicle(vehicle);
            }
            else
            {
                vehicle.MoveLeft();
            }
        }

        private static bool vehicleHasCrossedLeftEdge(GameObject vehicle)
        {
            return !(vehicle.X > 0 - vehicle.Sprite.Width);
        }

        private void moveVehicleRight(Vehicle vehicle)
        {
            if (this.vehicleHasCrossedRightEdge(vehicle))
            {
                this.respawnVehicle(vehicle);
            }
            else
            {
                vehicle.MoveRight();
            }
        }

        private bool vehicleHasCrossedRightEdge(GameObject vehicle)
        {
            return !(vehicle.X < this.laneLength + vehicle.Sprite.Width);
        }

        private void respawnVehicle(Vehicle vehicle)
        {
            if (vehicle.Direction == VehicleDirections.Left)
            {
                vehicle.X = this.laneLength;
            }
            else
            {
                vehicle.X = 0 - vehicle.Sprite.Width;
            }

            if (vehicle.NeedsToBeMadeVisible)
            {
                vehicle.Sprite.Visibility = Visibility.Visible;
                vehicle.NeedsToBeMadeVisible = false;
            }
        }

        #endregion
    }
}