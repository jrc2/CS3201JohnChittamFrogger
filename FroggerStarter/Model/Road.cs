using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;

namespace FroggerStarter.Model
{
    /// <summary>
    ///     Manages all lanes on the road
    /// </summary>
    public class Road : IEnumerable<Vehicle>
    {
        #region Data members

        private const int LaneOneLocation = 305;
        private const int LaneHeight = 50;
        private const double SpeedToAddOnTick = 0;
        private readonly double windowWidth;
        private DispatcherTimer vehicleActionTimer;
        private readonly Random random;
        private IList<Lane> lanes;

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
            this.fillLanes();
            this.random = new Random();
            this.setupVehicleActionTimer();
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
            foreach (var lane in this.lanes)
            {
                foreach (var vehicle in lane)
                {
                    yield return vehicle;
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        private void fillLanes()
        {
            this.lanes = new List<Lane> {
                new Lane(3, VehicleTypes.Car, 2, VehicleDirections.Left, this.windowWidth),
                new Lane(2, VehicleTypes.Semi, 2.2, VehicleDirections.Right, this.windowWidth),
                new Lane(4, VehicleTypes.Car, 2.5, VehicleDirections.Left, this.windowWidth),
                new Lane(3, VehicleTypes.Semi, 2.8, VehicleDirections.Left, this.windowWidth),
                new Lane(5, VehicleTypes.Car, 3, VehicleDirections.Right, this.windowWidth)
            };

            for (var i = 0; i < this.lanes.Count; i++)
            {
                var lane = this.lanes[i];
                foreach (var vehicle in lane)
                {
                    vehicle.Y = LaneOneLocation - LaneHeight * i;
                }
            }

            this.ResetVehicleVisibility();
        }

        /// <summary>
        ///     Sets the first vehicle in each lane to visible and the rest collapsed.
        /// </summary>
        public void ResetVehicleVisibility()
        {
            foreach (var lane in this.lanes)
            {
                lane.ResetVehicleVisibility();
            }
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
            this.showRandomVehicle();
            this.increaseLaneSpeeds();
        }

        private void showRandomVehicle()
        {
            var collapsedVehicles = this.Where(vehicle => vehicle.Sprite.Visibility == Visibility.Collapsed)
                                        .ToList();
            if (collapsedVehicles.Count > 0)
            {
                var randomVehicleIndex = this.random.Next(0, collapsedVehicles.Count);
                collapsedVehicles.ElementAt(randomVehicleIndex).NeedsToBeMadeVisible = true;
            }
        }

        private void increaseLaneSpeeds()
        {
            foreach (var lane in this.lanes)
            {
                lane.IncreaseVehicleSpeeds(SpeedToAddOnTick);
            }
        }

        /// <summary>
        ///     Collapses all vehicles in every lane.
        /// </summary>
        public void CollapseAllVehicles()
        {
            foreach (var vehicle in this)
            {
                vehicle.Sprite.Visibility = Visibility.Collapsed;
            }
        }

        /// <summary>
        ///     Moves all vehicles according to their set direction and speed.
        /// </summary>
        public void MoveAllVehicles()
        {
            foreach (var lane in this.lanes)
            {
                lane.MoveAllVehicles();
            }
        }

        /// <summary>
        ///     Resets the vehicle speeds.
        ///     Postcondition: all vehicle speeds reset
        /// </summary>
        public void ResetVehicleSpeeds()
        {
            foreach (var lane in this.lanes)
            {
                lane.ResetVehicleSpeeds();
            }
        }

        /// <summary>
        ///     Starts the vehicle action timer.
        /// </summary>
        public void StartVehicleActionTimer()
        {
            this.vehicleActionTimer.Start();
        }

        /// <summary>
        ///     Stops the vehicle action timer.
        /// </summary>
        public void StopVehicleActionTimer()
        {
            this.vehicleActionTimer.Stop();
        }

        #endregion
    }
}