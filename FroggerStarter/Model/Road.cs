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
        private const double SpeedToAddOnRespawn = 0;
        private const double TransformOriginX = 0.5;
        private const double TransformOriginY = 0.5;
        private IList<Vehicle> vehicles = new List<Vehicle>();
        private readonly double windowWidth;

        public int Count => this.vehicles.Count;

        public bool IsReadOnly => this.vehicles.IsReadOnly;

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
        }

        #endregion

        #region Methods

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
        public void ResetLanes() //TODO refactor
        {
            var lanes = new List<Lane> {
                new Lane(3, VehicleTypes.Car, 2, VehicleDirections.Left),
                new Lane(2, VehicleTypes.Semi, 2.2, VehicleDirections.Right),
                new Lane(4, VehicleTypes.Car, 2.6, VehicleDirections.Left),
                new Lane(3, VehicleTypes.Semi, 2.8, VehicleDirections.Left),
                new Lane(5, VehicleTypes.Car, 3, VehicleDirections.Right)
            };

            var laneIndex = 0;
            foreach (var lane in lanes)
            {
                this.setVehiclesToBeginningOfLane(lane, laneIndex);

                if (lane.Direction == VehicleDirections.Right)
                {
                    rotateVehiclesInLane180Degrees(lane);
                }

                laneIndex++;
                foreach (var vehicle in lane)
                {
                    this.vehicles.Add(vehicle);
                }
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

        private void setVehiclesToBeginningOfLane(Lane lane, int laneIndex)
        {
            for (var vehicleIndex = 0; vehicleIndex < lane.Count; vehicleIndex++)
            {
                var vehicle = lane[vehicleIndex];
                vehicle.Y = LaneOneLocation - LaneWidth * laneIndex;
                vehicle.X = this.windowWidth / lane.Count * vehicleIndex - vehicle.Width;
                if (vehicleIndex > 0)
                {
                    vehicle.Sprite.Visibility = Visibility.Collapsed;
                }
            }
        }

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
        /// <param name="lane">The lane the vehicle is in.</param>
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
                this.respawnVehicleOnRight(vehicle); //TODO need code to add next vehicle to lane....possibly a timer on lane
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

        private void respawnVehicleOnRight(GameObject vehicle)
        {
            vehicle.X = this.windowWidth;
        }

        private void moveVehicleRight(Vehicle vehicle)
        {
            if (this.vehicleHasCrossedRightEdge(vehicle))
            {
                respawnVehicleOnLeft(vehicle);
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

        private static void respawnVehicleOnLeft(GameObject vehicle)
        {
            vehicle.X = 0 - vehicle.Sprite.Width;
        }

        /// <summary>
        ///     Resets the vehicle speeds.
        ///     Postcondition: all lane speeds in this.lanes reset
        /// </summary>
        public void ResetVehicleSpeeds()
        {
            foreach (var vehicle in this.vehicles)
            {
                vehicle.ResetSpeed();
            }
        }

        public int IndexOf(Vehicle vehicle)
        {
            return this.vehicles.IndexOf(vehicle);
        }

        public void Insert(int index, Vehicle vehicle)
        {
            this.vehicles.Insert(index, vehicle);
        }

        public void RemoveAt(int index)
        {
            this.vehicles.RemoveAt(index);
        }

        public void Add(Vehicle vehicle)
        {
            this.vehicles.Add(vehicle);
        }

        public void Clear()
        {
            this.vehicles.Clear();
        }

        public bool Contains(Vehicle vehicle)
        {
            return this.vehicles.Contains(vehicle);
        }

        public void CopyTo(Vehicle[] array, int arrayIndex)
        {
            this.vehicles.CopyTo(array, arrayIndex);
        }

        public bool Remove(Vehicle vehicle)
        {
            return this.vehicles.Remove(vehicle);
        }

        #endregion
    }
}