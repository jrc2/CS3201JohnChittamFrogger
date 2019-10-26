using System;
using System.Security.Cryptography.X509Certificates;
using FroggerStarter.View.Sprites;

namespace FroggerStarter.Model
{
    /// <summary>
    ///     Defines the frog model
    /// </summary>
    /// <seealso cref="FroggerStarter.Model.GameObject" />
    public class Player : GameObject
    {
        #region Data members

        private const int SpeedXDirection = 50;
        private const int SpeedYDirection = 50;

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="Player" /> class.
        ///     Postcondition: Player initialized
        /// </summary>
        public Player()
        {
            Sprite = new PlayerSprite();
            SetSpeed(SpeedXDirection, SpeedYDirection);
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Sets the speed to given value.
        ///     Precondition: speed greater than/= 0
        ///     Postcondition: this.SpeedX and this.SpeedY == 0
        /// </summary>
        /// <param name="speed">The speed.</param>
        public void SetSpeedTo(int speed)
        {
            if (speed < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(speed), "speed much be >= 0");
            }

            SetSpeed(speed, speed);
        }

        #endregion
    }
}