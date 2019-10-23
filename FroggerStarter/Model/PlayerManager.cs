using System;
using Windows.UI.Xaml.Controls;

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

        #endregion

        #region Properties

        /// <summary>
        ///     Gets the player canvas.
        /// </summary>
        /// <value>
        ///     The player canvas.
        /// </value>
        public Canvas PlayerCanvas { get; }

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

            this.PlayerCanvas = new Canvas {
                Height = windowHeight,
                Width = windowWidth
            };

            this.Player = new Player();
            this.placePlayer();
        }

        #endregion

        #region Methods

        private void placePlayer()
        {
            this.PlayerCanvas.Children.Add(this.Player.Sprite);
            this.SetPlayerToCenterOfBottomLane();
        }

        /// <summary>
        ///     Sets the player to center of bottom lane.
        ///     Postcondition: player is centered at bottom of screen
        /// </summary>
        public void SetPlayerToCenterOfBottomLane()
        {
            this.Player.X = this.PlayerCanvas.Width / 2 - this.Player.Width / 2;
            this.Player.Y = this.PlayerCanvas.Height - this.Player.Height - BottomLaneOffset;
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
            if (this.Player.Y > PlayerMinY)
            {
                this.Player.MoveUp();
            }
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