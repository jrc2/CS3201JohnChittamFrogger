using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace FroggerStarter.Model
{
    /// <summary>
    ///     Manages all lanes on the road
    /// </summary>
    public class Road : IList<Vehicle>
    {
        #region Data members

        private const int LaneOneLocation = 305;
        private const int LaneWidth = 50;
        private const double SpeedToAddOnTick = 0;
        private const double TransformOriginX = 0.5;
        private const double TransformOriginY = 0.5;

        private DispatcherTimer vehicleActionTimer;
        private readonly Random random;
        private readonly IList<Vehicle> vehicles = new List<Vehicle>();
        private readonly double windowWidth;

        #endregion

        #region Properties

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
        ///     Initializes a new instance of the <see cref="Model.Road" /> class.
        ///     Precondition: windowHeight and windowWidth greater than/= 0
        ///     Postcondition: lanes properties and data members initialized
        /// </summary>
        /// <param name="windowHeight">Height of the window.</param>
        /// <param name="windowWidth">Width of the window.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     windowHeight - windowHeight must be >= 0
        ///     or
        ///     windowWidth - windowWidth must be >= 0
        /// </exception>
        public Road(double windowHeight, double windowWidth)
        {
            if (windowHeight < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(windowHeight), "windowHeight must be >= 0");
            }

            if (windowWidth < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(windowWidth), "windowWidth must be >= 0");
            }

            this.windowWidth = windowWidth;
            this.ResetLanes();
            this.random = new Random();
            this.setupVehicleActionTimer();
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Finds the index of the given vehicle in this.vehicles
        /// </summary>
        /// <param name="vehicle">The vehicle.</param>
        /// <returns>The index of the vehicle if found in vehicles; otherwise, -1.</returns>
        public int IndexOf(Vehicle vehicle)
        {
            return this.vehicles.IndexOf(vehicle);
        }

        /// <summary>
        ///     Inserts the specified vehicle at the given index.
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
        /// <returns>
        ///     true if item was successfully removed from vehicles; otherwise, false.
        ///     This method also returns false if item is not found in the vehicles.
        /// </returns>
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

        private void setupVehicleActionTimer()
        {
            this.vehicleActionTimer = new DispatcherTimer();
            this.vehicleActionTimer.Tick += this.vehicleActionTimerOnTick;
            this.vehicleActionTimer.Interval = new TimeSpan(0, 0, 2);
            this.vehicleActionTimer.Start();
        }

        private void vehicleActionTimerOnTick(object sender, object e)
        {
            var collapsedVehicles = this.vehicles.Where(vehicle => vehicle.Sprite.Visibility == Visibility.Collapsed)
                                        .ToList();
            if (collapsedVehicles.Count > 0)
            {
                var randomVehicleIndex = this.random.Next(0, collapsedVehicles.Count);
                collapsedVehicles.ElementAt(randomVehicleIndex).Sprite.Visibility = Visibility.Visible;
            }

            foreach (var vehicle in this.vehicles)
            {
                vehicle.Speed += SpeedToAddOnTick;
            }
        }

        /// <summary>
        ///     Collapses all vehicles in every lane.
        /// </summary>
        public void CollapseAllVehicles()
        {
            foreach (var vehicle in this.vehicles)
            {
                vehicle.Sprite.Visibility = Visibility.Collapsed;
            }
        }

        /// <summary>
        ///     Resets the lanes to beginning of game formation.
        /// </summary>
        public void ResetLanes()
        {
            var lanes = new List<Lane> {
                new Lane(3, VehicleTypes.Car, 2, VehicleDirections.Left, this.windowWidth),
                new Lane(2, VehicleTypes.Semi, 2.2, VehicleDirections.Right, this.windowWidth),
                new Lane(4, VehicleTypes.Car, 2.5, VehicleDirections.Left, this.windowWidth),
                new Lane(3, VehicleTypes.Semi, 2.8, VehicleDirections.Left, this.windowWidth),
                new Lane(5, VehicleTypes.Car, 3, VehicleDirections.Right, this.windowWidth)
            };

            for (var i = 0; i < lanes.Count; i++)
            {
                var lane = lanes[i];
                this.addVehicleToRoad(lane, i);
            }
        }

        private void addVehicleToRoad(Lane lane, int laneIndex)
        {
            foreach (var vehicle in lane)
            {
                vehicle.Y = LaneOneLocation - LaneWidth * laneIndex;
            }

            if (lane.Direction == VehicleDirections.Right)
            {
                rotateVehiclesInLane180Degrees(lane);
            }

            foreach (var vehicle in lane)
            {
                this.vehicles.Add(vehicle);
            }
        }

        private static void rotateVehiclesInLane180Degrees(Lane lane)
        {
            foreach (var vehicle in lane)
            {
                vehicle.Sprite.RenderTransformOrigin = new Point(TransformOriginX, TransformOriginY);
                vehicle.Sprite.RenderTransform = new ScaleTransform {ScaleX = -1};
            }
        }

        /// <summary>
        ///     Moves all vehicles according to their set direction and speed.
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
            return !(vehicle.X < this.windowWidth + vehicle.Sprite.Width);
        }

        private void respawnVehicle(Vehicle vehicle)
        {
            if (vehicle.Direction == VehicleDirections.Left)
            {
                vehicle.X = this.windowWidth;
            }
            else
            {
                vehicle.X = 0 - vehicle.Sprite.Width;
            }
        }

        /// <summary>
        ///     Resets the vehicle speeds.
        ///     Postcondition: all vehicle speeds reset
        /// </summary>
        public void ResetVehicleSpeeds()
        {
            foreach (var vehicle in this.vehicles)
            {
                vehicle.ResetSpeed();
            }
        }

        #endregion
    }
}