using System;
using FroggerStarter.View.Sprites;

namespace FroggerStarter.Model
{
    /// <summary>
    ///     The vehicle class
    /// </summary>
    /// <seealso cref="FroggerStarter.Model.GameObject" />
    public class Vehicle : GameObject
    {
        #region Data members

        private readonly double originalSpeed;

        #endregion

        #region Properties

        /// <summary>
        ///     Gets the direction.
        /// </summary>
        /// <value>
        ///     The direction.
        /// </value>
        public VehicleDirections Direction { get; }

        /// <summary>
        ///     Gets or sets the speed.
        /// </summary>
        /// <value>
        ///     The speed.
        /// </value>
        public double Speed
        {
            get => SpeedX;
            set => SpeedX = value;
        }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="Vehicle" /> class.
        ///     Precondition: Speed greater than/= 0
        /// </summary>
        /// <param name="vehicleType">Type of the vehicle.</param>
        /// <param name="direction">The direction.</param>
        /// <param name="speed">The speed.</param>
        /// <exception cref="ArgumentOutOfRangeException">speed - speed must be at least 0</exception>
        public Vehicle(VehicleTypes vehicleType, VehicleDirections direction, double speed)
        {
            if (speed < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(speed), "speed must be at least 0");
            }

            if (vehicleType == VehicleTypes.Car)
            {
                Sprite = new CarSprite();
            }
            else
            {
                Sprite = new SemiSprite();
            }

            this.Direction = direction;
            this.originalSpeed = speed;
            this.Speed = speed;
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Resets the speed.
        /// </summary>
        public void ResetSpeed()
        {
            this.Speed = this.originalSpeed;
        }

        #endregion
    }
}