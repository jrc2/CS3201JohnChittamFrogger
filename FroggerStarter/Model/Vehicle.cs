using FroggerStarter.View.Sprites;

namespace FroggerStarter.Model
{
    /// <summary>
    ///     The vehicle class
    /// </summary>
    /// <seealso cref="FroggerStarter.Model.GameObject" />
    public class Vehicle : GameObject
    {

        private double originalSpeed;

        public VehicleDirections Direction { get; }
        public double Speed { get => this.SpeedX; set => this.SpeedX = value; }

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="Vehicle" /> class.
        /// </summary>
        /// <param name="vehicleType">Type of the vehicle.</param>
        public Vehicle(VehicleTypes vehicleType, VehicleDirections direction, double speed)
        {
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

        public void ResetSpeed()
        {
            this.Speed = this.originalSpeed;
        }

        #endregion
    }
}