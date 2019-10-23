using FroggerStarter.View.Sprites;

namespace FroggerStarter.Model
{
    /// <summary>
    ///     The vehicle class
    /// </summary>
    /// <seealso cref="FroggerStarter.Model.GameObject" />
    public class Vehicle : GameObject
    {
        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="Vehicle" /> class.
        /// </summary>
        /// <param name="vehicleType">Type of the vehicle.</param>
        public Vehicle(VehicleTypes vehicleType)
        {
            if (vehicleType == VehicleTypes.Car)
            {
                Sprite = new CarSprite();
            }
            else
            {
                Sprite = new SemiSprite();
            }
        }

        #endregion
    }
}