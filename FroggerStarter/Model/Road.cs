using System;
using System.Collections;
using System.Collections.Generic;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;
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
        private Canvas road;

        private readonly IEnumerable<Lane> lanes;

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

            this.lanes = new List<Lane> {
                new Lane(2, VehicleTypes.Car, 1, VehicleDirections.Left),
                new Lane(3, VehicleTypes.Semi, 1.2, VehicleDirections.Right),
                new Lane(3, VehicleTypes.Car, 1.6, VehicleDirections.Left),
                new Lane(2, VehicleTypes.Semi, 1.8, VehicleDirections.Left),
                new Lane(3, VehicleTypes.Car, 2, VehicleDirections.Right)
            };

            this.vehicleRotateTransform = new RotateTransform {
                Angle = VehicleRotationAngle
            };

            this.road = new Canvas {
                Height = windowHeight,
                Width = windowWidth
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
        ///     Creates the road.
        ///     Postcondition: this.road is completed
        /// </summary>
        /// <returns>Completed road</returns>
        public Canvas CreateRoad()
        {
            var distanceFromTop = LaneOneLocation;

            foreach (var lane in this.lanes)
            {
                this.road = this.addLaneToRoad(lane, this.road, distanceFromTop);

                distanceFromTop -= LaneWidth;
            }

            return this.road;
        }

        private Canvas addLaneToRoad(Lane lane, Canvas canvas, int distanceFromTop)
        {
            for (var i = 0; i < lane.Count; i++)
            {
                canvas = this.addVehicleToLane(lane, i, distanceFromTop, canvas);
            }

            return canvas;
        }

        private Canvas addVehicleToLane(Lane currentLane, int vehicleIndex, int distanceFromTop, Canvas canvas)
        {
            var vehicle = currentLane[vehicleIndex];
            if (currentLane.Direction == VehicleDirections.Right)
            {
                vehicle.Sprite.RenderTransformOrigin = new Point(TransformOriginX, TransformOriginY);
                vehicle.Sprite.RenderTransform = this.vehicleRotateTransform;
            }

            canvas.Children.Add(vehicle.Sprite);
            Canvas.SetTop(vehicle.Sprite, distanceFromTop);
            Canvas.SetLeft(vehicle.Sprite, this.road.Width / currentLane.Count * vehicleIndex);

            return canvas;
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
                lane.Speed += SpeedToAddOnRespawn;
            }
            else
            {
                Canvas.SetLeft(vehicle.Sprite, Canvas.GetLeft(vehicle.Sprite) - lane.Speed);
            }
        }

        private static bool vehicleHasCrossedLeftEdge(GameObject vehicle)
        {
            return !(Canvas.GetLeft(vehicle.Sprite) > 0 - vehicle.Sprite.Width);
        }

        private void respawnVehicleOnRight(GameObject vehicle)
        {
            Canvas.SetLeft(vehicle.Sprite, this.road.Width);
        }

        private void moveVehicleRight(Lane lane, GameObject vehicle)
        {
            if (this.vehicleHasCrossedRightEdge(vehicle))
            {
                respawnVehicleOnLeft(vehicle);
                lane.Speed += SpeedToAddOnRespawn;
            }
            else
            {
                Canvas.SetLeft(vehicle.Sprite, Canvas.GetLeft(vehicle.Sprite) + lane.Speed);
            }
        }

        private bool vehicleHasCrossedRightEdge(GameObject vehicle)
        {
            return !(Canvas.GetLeft(vehicle.Sprite) < this.road.Width + vehicle.Sprite.Width);
        }

        private static void respawnVehicleOnLeft(GameObject vehicle)
        {
            Canvas.SetLeft(vehicle.Sprite, 0 - vehicle.Sprite.Width);
        }

        /// <summary>
        ///     Resets the lane speeds.
        ///     Postcondition: all lane speeds in this.lanes reset
        /// </summary>
        public void ResetLaneSpeeds()
        {
            foreach (var lane in this.lanes)
            {
                lane.Speed = lane.OriginalSpeed;
            }
        }

        #endregion
    }
}