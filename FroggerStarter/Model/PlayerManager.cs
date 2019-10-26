using System;
using System.Threading.Tasks;
using FroggerStarter.View.Sprites;

namespace FroggerStarter.Model
{
    /// <summary>
    ///     Manages the Player
    /// </summary>
    public class PlayerManager
    {
        #region Data members

        private const int BottomLaneOffset = 5;
        private const int PlayerMinX = 0;
        private const int PlayerMaxX = 600;
        private const int PlayerMinY = 55;
        private const int PlayerMaxY = 355;
        private const int AnimationDelay = 300;
        private readonly double windowHeight;
        private readonly double windowWidth;

        #endregion

        #region Properties

        /// <summary>
        ///     Gets the player.
        /// </summary>
        /// <value>
        ///     The player.
        /// </value>
        public Player Player { get; }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="PlayerManager" /> class.
        ///     Precondition: windowHeight and windowWidth greater than/= 0
        ///     Postcondition: properties initialized
        /// </summary>
        /// <param name="windowHeight">Height of the window.</param>
        /// <param name="windowWidth">Width of the window.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     windowHeight - windowHeight must be >= 0
        ///     or
        ///     windowWidth - windowWidth must be >= 0
        /// </exception>
        public PlayerManager(double windowHeight, double windowWidth)
        {
            if (windowHeight < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(windowHeight), "windowHeight must be >= 0");
            }

            if (windowWidth < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(windowWidth), "windowWidth must be >= 0");
            }

            this.windowHeight = windowHeight;
            this.windowWidth = windowWidth;

            this.Player = new Player();
            this.SetPlayerToCenterOfBottomLane();
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Kills the player.
        /// </summary>
        public async Task<bool> KillPlayer()
        {
            this.Player.SetSpeedTo(0);
            this.Player.Sprite.Content = new PlayerSprite(PlayerStages.DeadFrog);
            await Task.Delay(AnimationDelay);
            this.Player.Sprite.Content = new PlayerSprite(PlayerStages.SquishedFrog);
            await Task.Delay(AnimationDelay);
            this.Player.Sprite.Content = new PlayerSprite(PlayerStages.BlobFrog);
            await Task.Delay(AnimationDelay);
            this.Player.Sprite.Content = new PlayerSprite(PlayerStages.DeathX);
            await Task.Delay(AnimationDelay);
            this.SetPlayerToCenterOfBottomLane();
            this.Player.Sprite.Content = new PlayerSprite();
            this.Player.SetSpeedToDefault();
            return true;
        }

        /// <summary>
        ///     Sets the player to center of bottom lane.
        /// </summary>
        public void SetPlayerToCenterOfBottomLane()
        {
            this.Player.X = this.windowWidth / 2 - this.Player.Width / 2;
            this.Player.Y = this.windowHeight - this.Player.Height - BottomLaneOffset;
        }

        /// <summary>
        ///     Sets the player speed to the given value.
        ///     Postcondition this.Player.SpeedX and SpeedY == speed
        /// </summary>
        /// <param name="speed">The speed.</param>
        public void SetPlayerSpeedTo(int speed)
        {
            this.Player.SetSpeedTo(speed);
        }

        /// <summary>
        ///     Moves the Player to the left.
        ///     Precondition: none
        ///     Postcondition: Player.X = Player.X@prev - Player.Width
        /// </summary>
        public void MovePlayerLeft()
        {
            if (this.Player.X > PlayerMinX)
            {
                this.Player.MoveLeft();
            }
        }

        /// <summary>
        ///     Moves the Player to the right.
        ///     Precondition: none
        ///     Postcondition: Player.X = Player.X@prev + Player.Width
        /// </summary>
        public void MovePlayerRight()
        {
            if (this.Player.X < PlayerMaxX)
            {
                this.Player.MoveRight();
            }
        }

        /// <summary>
        ///     Moves the Player up, then checks to see if Player scored.
        ///     Precondition: none
        ///     Postcondition: Player.Y = Player.Y@prev - Player.Height
        /// </summary>
        public void MovePlayerUp()
        {
            this.Player.MoveUp();
        }

        /// <summary>
        ///     Determines whether player has scored.
        /// </summary>
        /// <returns>
        ///     <c>true</c> if player has scored; otherwise, <c>false</c>.
        /// </returns>
        public bool HasPlayerScored()
        {
            return (int) this.Player.Y == PlayerMinY;
        }

        /// <summary>
        ///     Moves the Player down.
        ///     Precondition: none
        ///     Postcondition: Player.Y = Player.Y@prev + Player.Height
        /// </summary>
        public void MovePlayerDown()
        {
            if (this.Player.Y < PlayerMaxY)
            {
                this.Player.MoveDown();
            }
        }

        #endregion
    }
}