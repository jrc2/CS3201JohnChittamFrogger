using System;
using System.Collections;
using System.Collections.Generic;
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
        private const double SpeedToAddOnRespawn = 0.2;
        private const double TransformOriginX = 0.5;
        private const double TransformOriginY = 0.5;
        private const int VehicleRotationAngle = 180;
        private readonly RotateTransform vehicleRotateTransform;
        private readonly IEnumerable<Lane> lanes;
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

            this.lanes = new List<Lane> {
                new Lane(2, VehicleTypes.Car, 1, VehicleDirections.Left),
                new Lane(3, VehicleTypes.Semi, 1.2, VehicleDirections.Right),
                new Lane(3, VehicleTypes.Car, 1.6, VehicleDirections.Left),
                new Lane(2, VehicleTypes.Semi, 1.8, VehicleDirections.Left),
                new Lane(3, VehicleTypes.Car, 2, VehicleDirections.Right)
            };

            var currLane = 0;
            foreach (var lane in this.lanes) //TODO refactor
            {
                for (var currVehicle = 0; currVehicle < lane.Count; currVehicle++)
                {
                    var vehicle = lane[currVehicle];
                    vehicle.Y = LaneOneLocation - LaneWidth * currLane;
                    vehicle.X = this.windowWidth / lane.Count * currVehicle;
                }

                currLane++;
            }

            this.vehicleRotateTransform = new RotateTransform {
                Angle = VehicleRotationAngle
            };
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

        private void moveVehicleLeft(Lane lane, GameObject vehicle)
        {
            if (vehicleHasCrossedLeftEdge(vehicle))
            {
                this.respawnVehicleOnRight(vehicle);
                lane.IncreaseSpeedBy(SpeedToAddOnRespawn);
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

        private void moveVehicleRight(Lane lane, GameObject vehicle)
        {
            if (this.vehicleHasCrossedRightEdge(vehicle))
            {
                respawnVehicleOnLeft(vehicle);
                lane.IncreaseSpeedBy(SpeedToAddOnRespawn);
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