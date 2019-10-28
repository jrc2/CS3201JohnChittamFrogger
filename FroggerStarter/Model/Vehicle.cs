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
        #region Properties

        /// <summary>
        ///     Gets the direction.
        /// </summary>
        /// <value>
        ///     The direction.
        /// </value>
        public VehicleDirections Direction { get; }

        /// <summary>
        ///     Gets or sets a value indicating whether [needs to be visible].
        /// </summary>
        /// <value>
        ///     <c>true</c> if [needs to be visible]; otherwise, <c>false</c>.
        /// </value>
        public bool NeedsToBeMadeVisible { get; set; }

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
            this.Speed = speed;
            this.NeedsToBeMadeVisible = false;
        }

        #endregion
    }
}