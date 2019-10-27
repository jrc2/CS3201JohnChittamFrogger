using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
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

        private const int OriginalTimeRemaining = 20;
        private int lives = 4;
        private int score;
        private int timeRemaining = OriginalTimeRemaining;
        private Canvas gameCanvas;
        private Road roadManager;
        private PlayerManager playerManager;
        private HomePlayerManager homePlayerManager;
        private DispatcherTimer mainGameTimer;
        private DispatcherTimer lifeTimer;

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="GameManager" /> class.
        ///     Postconition: GameManager initialized
        /// </summary>
        public GameManager()
        {
            this.setupGameTimer();
            this.setupLifeTimer();
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

        /// <summary>
        ///     Occurs when [time remaining updated].
        /// </summary>
        public event EventHandler<TimeRemainingEventArgs> TimeRemainingUpdated;

        private void setupGameTimer()
        {
            this.mainGameTimer = new DispatcherTimer();
            this.mainGameTimer.Tick += this.mainGameTimerOnTick;
            this.mainGameTimer.Interval = new TimeSpan(0, 0, 0, 0, 15);
            this.mainGameTimer.Start();
        }

        private void setupLifeTimer()
        {
            this.lifeTimer = new DispatcherTimer();
            this.lifeTimer.Tick += this.lifeTimerOnTick;
            this.lifeTimer.Interval = new TimeSpan(0, 0, 1);
            this.lifeTimer.Start();
        }

        private async void lifeTimerOnTick(object sender, object e)
        {
            this.timeRemaining--;
            this.onTimeRemainingUpdated();
            if (this.timeRemaining == 0)
            {
                await this.killPlayer();
                this.resetTimer();
            }
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
            this.homePlayerManager = new HomePlayerManager();
            this.addVehiclesToCanvas();
            this.addHomeFrogsToCanvas();
            this.gameCanvas.Children.Add(this.playerManager.Player.Sprite);
        }

        private void addHomeFrogsToCanvas()
        {
            foreach (var player in this.homePlayerManager)
            {
                this.gameCanvas.Children.Add(player.Sprite);
            }
        }

        private void addVehiclesToCanvas()
        {
            foreach (var vehicle in this.roadManager)
            {
                this.gameCanvas.Children.Add(vehicle.Sprite);
            }
        }

        private void mainGameTimerOnTick(object sender, object e)
        {
            this.roadManager.MoveAllVehicles();
            foreach (var vehicle in this.roadManager)
            {
                if (vehicle.Sprite.Visibility == Visibility.Visible)
                {
                    this.checkForCollision(vehicle);
                }
            }

            if (this.lives == 0)
            {
                this.onGameOver();
            }
        }

        private async void checkForCollision(GameObject vehicle)
        {
            var vehicleRectangle = createRectangleForSprite(vehicle.Sprite);
            var playerRectangle = createRectangleForSprite(this.playerManager.Player.Sprite);

            if (vehicleRectangle.IntersectsWith(playerRectangle))
            {
                await this.killPlayer();
            }
        }

        private async Task killPlayer()
        {
            this.mainGameTimer.Stop();
            this.lifeTimer.Stop();
            await this.playerManager.KillPlayer();
            this.roadManager.CollapseAllVehicles();
            this.lives--;
            this.onPlayerLivesUpdated();
            this.mainGameTimer.Start();
            this.lifeTimer.Start();
            this.resetLanes();
            this.roadManager.ResetVehicleSpeeds();
        }

        private void resetLanes()
        {
            this.roadManager.ResetLanes();
            foreach (var vehicle in this.roadManager)
            {
                this.gameCanvas.Children.Remove(vehicle.Sprite);
                this.gameCanvas.Children.Add(vehicle.Sprite);
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
            this.resetTimer();
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
            this.mainGameTimer.Stop();
            this.lifeTimer.Stop();
            this.playerManager.SetPlayerSpeedTo(0);
            this.GameOverUpdated?.Invoke(this, null);
        }

        private void onTimeRemainingUpdated()
        {
            var data = new TimeRemainingEventArgs {TimeRemaining = this.timeRemaining};
            this.TimeRemainingUpdated?.Invoke(this, data);
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
            var mainPlayerRectangle = createRectangleForSprite(this.playerManager.Player.Sprite);

            if (this.playerManager.PlayerAtPlayerMinY())
            {
                if (!this.isPlayerInHomeLanding(mainPlayerRectangle))
                {
                    this.MovePlayerDown();
                }
            }
        }

        private bool isPlayerInHomeLanding(RectangleF mainPlayerRectangle)
        {
            foreach (var collapsedHomePlayer in this.getCollapsedHomePlayers())
            {
                var homePlayerRectangle = createRectangleForSprite(collapsedHomePlayer.Sprite);
                if (mainPlayerRectangle.IntersectsWith(homePlayerRectangle))
                {
                    this.processPlayerInHomeLanding(collapsedHomePlayer);
                    return true;
                }
            }

            return false;
        }

        private void processPlayerInHomeLanding(Player collapsedHomePlayer)
        {
            this.score += this.timeRemaining;
            this.onPlayerScoreUpdated();
            collapsedHomePlayer.Sprite.Visibility = Visibility.Visible;
            if (this.getCollapsedHomePlayers().Count == 0)
            {
                this.onGameOver();
            }
            else
            {
                this.playerManager.SetPlayerToCenterOfBottomLane();
            }

            this.resetTimer();
        }

        private void resetTimer()
        {
            this.timeRemaining = OriginalTimeRemaining;
            this.onTimeRemainingUpdated();
        }

        private ICollection<Player> getCollapsedHomePlayers()
        {
            var collapsedHomePlayers = new Collection<Player>();

            foreach (var homePlayer in this.homePlayerManager.Where(homePlayer =>
                homePlayer.Sprite.Visibility == Visibility.Collapsed))
            {
                collapsedHomePlayers.Add(homePlayer);
            }

            return collapsedHomePlayers;
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

    /// <summary>
    ///     Event args for when timer updated
    /// </summary>
    /// <seealso cref="System.EventArgs" />
    public class TimeRemainingEventArgs : EventArgs
    {
        #region Properties

        /// <summary>
        ///     Gets or sets the time remaining.
        /// </summary>
        /// <value>
        ///     The time remaining.
        /// </value>
        public int TimeRemaining { get; set; }

        #endregion
    }
}