using System;
using System.Collections;
using System.Collections.Generic;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace FroggerStarter.Model
{
    /// <summary>
    ///     Manages all lanes on the road
    /// </summary>
    public class Road : IEnumerable<Lane>
    {
        #region Data members

        private const int LaneOneLocation = 305;
        private const int LaneWidth = 50;
        private const double SpeedToAddOnRespawn = 0;
        private const double TransformOriginX = 0.5;
        private const double TransformOriginY = 0.5;
        private IEnumerable<Lane> lanes;
        private readonly double windowWidth;

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
        public IEnumerator<Lane> GetEnumerator()
        {
            return this.lanes.GetEnumerator();
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
            foreach (var lane in this.lanes)
            {
                foreach (var vehicle in lane)
                {
                    vehicle.Sprite.Visibility = Visibility.Collapsed;
                }
            }
        }

        /// <summary>
        ///     Resets the lanes to beginning of game formation.
        /// </summary>
        public void ResetLanes()
        {
            this.lanes = new List<Lane> {
                new Lane(3, VehicleTypes.Car, 2, VehicleDirections.Left),
                new Lane(2, VehicleTypes.Semi, 2.2, VehicleDirections.Right),
                new Lane(4, VehicleTypes.Car, 2.6, VehicleDirections.Left),
                new Lane(3, VehicleTypes.Semi, 2.8, VehicleDirections.Left),
                new Lane(5, VehicleTypes.Car, 3, VehicleDirections.Right)
            };

            var laneIndex = 0;
            foreach (var lane in this.lanes)
            {
                this.setVehiclesToBeginningOfLane(lane, laneIndex);

                if (lane.Direction == VehicleDirections.Right)
                {
                    rotateVehiclesInLane180Degrees(lane);
                }

                laneIndex++;
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

        private void setVehiclesToBeginningOfLane(Lane lane, int currLane)
        {
            for (var vehicleIndex = 0; vehicleIndex < lane.Count; vehicleIndex++)
            {
                var vehicle = lane[vehicleIndex];
                vehicle.Y = LaneOneLocation - LaneWidth * currLane;
                vehicle.X = this.windowWidth / lane.Count * vehicleIndex - vehicle.Width;
                if (vehicleIndex > 0)
                {
                    vehicle.Sprite.Visibility = Visibility.Collapsed;
                }
            }
        }

        /// <summary>
        ///     Moves the vehicle.
        ///     Postcondition: vehicle has moved
        /// </summary>
        /// <param name="lane">The lane the vehicle is in.</param>
        /// <param name="vehicle">The vehicle to move.</param>
        public void MoveVehicle(Lane lane, Vehicle vehicle)
        {
            if (lane.Direction == VehicleDirections.Right)
            {
                this.moveVehicleRight(lane, vehicle);
            }
            else
            {
                this.moveVehicleLeft(lane, vehicle);
            }
        }

        private void moveVehicleLeft(Lane lane, Vehicle vehicle)
        {
            if (vehicleHasCrossedLeftEdge(vehicle))
            {
                this.respawnVehicleOnRight(vehicle);
                if (lane.IndexOf(vehicle) == lane.Count - 1)
                {
                    lane.IncreaseSpeedBy(SpeedToAddOnRespawn);
                    lane.DisplayNextVehicle();
                }
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

        private void moveVehicleRight(Lane lane, Vehicle vehicle)
        {
            if (this.vehicleHasCrossedRightEdge(vehicle))
            {
                respawnVehicleOnLeft(vehicle);
                if (lane.IndexOf(vehicle) == lane.Count - 1)
                {
                    lane.IncreaseSpeedBy(SpeedToAddOnRespawn);
                    lane.DisplayNextVehicle();
                }
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
        ///     Resets the lane speeds.
        ///     Postcondition: all lane speeds in this.lanes reset
        /// </summary>
        public void ResetLaneSpeeds()
        {
            foreach (var lane in this.lanes)
            {
                lane.ResetSpeed();
            }
        }

        #endregion
    }
}