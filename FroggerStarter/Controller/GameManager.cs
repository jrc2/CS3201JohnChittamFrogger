using System;
using System.Drawing;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using FroggerStarter.Model;

namespace FroggerStarter.Controller
{
    /// <summary>
    ///     Manages all aspects of the game play including moving the player,
    ///     the Vehicles as well as lives and score.
    /// </summary>
    public class GameManager
    {
        #region Data members

        private const int MaxScore = 3;
        private int lives = 3;
        private int score;
        private Canvas gameCanvas;
        private Road roadManager;
        private PlayerManager playerManager;
        private DispatcherTimer timer;

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="GameManager" /> class.
        /// </summary>
        /// <param name="backgroundHeight">Height of the background.</param>
        /// <param name="backgroundWidth">Width of the background.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     backgroundHeight &lt;= 0
        ///     or
        ///     backgroundWidth &lt;= 0
        /// </exception>
        public GameManager(double backgroundHeight, double backgroundWidth)
        {
            if (backgroundHeight <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(backgroundHeight));
            }

            if (backgroundWidth <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(backgroundWidth));
            }

            this.setupGameTimer();
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Occurs when [player lives updated].
        /// </summary>
        public event EventHandler<PlayerLifeEventArgs> PlayerLivesUpdated;

        /// <summary>
        ///     Occurs when [player score updated].
        /// </summary>
        public event EventHandler<PlayerScoreEventArgs> PlayerScoreUpdated;

        /// <summary>
        ///     Occurs when [game over updated].
        /// </summary>
        public event EventHandler<EventArgs> GameOverUpdated;

        private void setupGameTimer()
        {
            this.timer = new DispatcherTimer();
            this.timer.Tick += this.timerOnTick;
            this.timer.Interval = new TimeSpan(0, 0, 0, 0, 15);
            this.timer.Start();
        }

        /// <summary>
        ///     Initializes the game working with appropriate classes to play frog
        ///     and vehicle on game screen.
        ///     Precondition: gamePage != null
        ///     Postcondition: Game is initialized and ready for play.
        /// </summary>
        /// <param name="gamePage">The game page.</param>
        /// <exception cref="ArgumentNullException">gameCanvas</exception>
        public void InitializeGame(Canvas gamePage)
        {
            this.gameCanvas = gamePage ?? throw new ArgumentNullException(nameof(gamePage));
            this.roadManager = new Road(this.gameCanvas.Height, this.gameCanvas.Width);
            this.playerManager = new PlayerManager(this.gameCanvas.Height, this.gameCanvas.Width);
//            this.gameCanvas.Children.Add(this.roadManager.CreateRoad());
            foreach (var lane in this.roadManager)
            {
                foreach (var vehicle in lane)
                {
                    this.gameCanvas.Children.Add(vehicle.Sprite);
                }
            }
            this.gameCanvas.Children.Add(this.playerManager.PlayerCanvas);
        }

        private void timerOnTick(object sender, object e)
        {
            foreach (var lane in this.roadManager)
            {
                this.moveVehiclesInLane(lane);
            }

            if (this.score == MaxScore || this.lives == 0)
            {
                this.onGameOver();
            }
        }

        private void moveVehiclesInLane(Lane lane)
        {
            foreach (var vehicle in lane)
            {
                this.roadManager.MoveVehicle(lane, vehicle);
                this.checkForCollision(vehicle);
            }
        }

        private void checkForCollision(GameObject vehicle)
        {
            var vehicleRectangle = createRectangleForSprite(vehicle.Sprite);
            var playerRectangle = createRectangleForSprite(this.playerManager.Player.Sprite);

            if (vehicleRectangle.IntersectsWith(playerRectangle))
            {
                this.playerManager.SetPlayerToCenterOfBottomLane();
                this.lives--;
                this.onPlayerLivesUpdated();
                this.roadManager.ResetLaneSpeeds();
            }
        }

        private static RectangleF createRectangleForSprite(FrameworkElement sprite)
        {
            var spriteRectangle = new RectangleF {
                X = (float) Canvas.GetLeft(sprite),
                Y = (float) Canvas.GetTop(sprite),
                Width = (float) sprite.Width,
                Height = (float) sprite.Height
            };

            return spriteRectangle;
        }

        private void onPlayerLivesUpdated()
        {
            var data = new PlayerLifeEventArgs {Lives = this.lives};
            this.PlayerLivesUpdated?.Invoke(this, data);
        }

        private void onPlayerScoreUpdated()
        {
            var data = new PlayerScoreEventArgs {Score = this.score};
            this.PlayerScoreUpdated?.Invoke(this, data);
        }

        private void onGameOver()
        {
            this.timer.Stop();
            this.playerManager.SetPlayerSpeedTo(0);
            this.GameOverUpdated?.Invoke(this, null);
        }

        /// <summary>
        ///     Moves the player to the left.
        ///     Precondition: none
        ///     Postcondition: player.X = player.X@prev - player.Width
        /// </summary>
        public void MovePlayerLeft()
        {
            this.playerManager.MovePlayerLeft();
        }

        /// <summary>
        ///     Moves the player to the right.
        ///     Precondition: none
        ///     Postcondition: player.X = player.X@prev + player.Width
        /// </summary>
        public void MovePlayerRight()
        {
            this.playerManager.MovePlayerRight();
        }

        /// <summary>
        ///     Moves the player up, then checks to see if player scored.
        ///     Precondition: none
        ///     Postcondition: player.Y = player.Y@prev - player.Height
        /// </summary>
        public void MovePlayerUp()
        {
            this.playerManager.MovePlayerUp();
            this.checkIfPlayerScored();
        }

        private void checkIfPlayerScored()
        {
            if (!this.playerManager.HasPlayerScored())
            {
                return;
            }

            this.score++;
            this.onPlayerScoreUpdated();
            this.playerManager.SetPlayerToCenterOfBottomLane();
        }

        /// <summary>
        ///     Moves the player down.
        ///     Precondition: none
        ///     Postcondition: player.Y = player.Y@prev + player.Height
        /// </summary>
        public void MovePlayerDown()
        {
            this.playerManager.MovePlayerDown();
        }

        #endregion
    }

    /// <summary>
    ///     Event args for when player scores
    /// </summary>
    /// <seealso cref="System.EventArgs" />
    public class PlayerScoreEventArgs : EventArgs
    {
        #region Properties

        /// <summary>
        ///     Gets or sets the score.
        /// </summary>
        /// <value>
        ///     The score.
        /// </value>
        public int Score { get; set; }

        #endregion
    }

    /// <summary>
    ///     Event args for when player loses a life
    /// </summary>
    /// <seealso cref="System.EventArgs" />
    public class PlayerLifeEventArgs : EventArgs
    {
        #region Properties

        /// <summary>
        ///     Gets or sets the number of lives.
        /// </summary>
        /// <value>
        ///     The number of lives.
        /// </value>
        public int Lives { get; set; }

        #endregion
    }
}