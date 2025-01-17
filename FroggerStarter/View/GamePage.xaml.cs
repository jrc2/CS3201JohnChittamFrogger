﻿using System;
using Windows.Foundation;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using FroggerStarter.Controller;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace FroggerStarter.View
{
    /// <summary>
    ///     An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class GamePage
    {
        #region Data members

        private readonly double applicationHeight = (double) Application.Current.Resources["AppHeight"];
        private readonly double applicationWidth = (double) Application.Current.Resources["AppWidth"];
        private readonly GameManager gameManager;

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="GamePage" /> class.
        /// </summary>
        public GamePage()
        {
            this.InitializeComponent();

            ApplicationView.PreferredLaunchViewSize = new Size
                {Width = this.applicationWidth, Height = this.applicationHeight};
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;
            ApplicationView.GetForCurrentView()
                           .SetPreferredMinSize(new Size(this.applicationWidth, this.applicationHeight));

            Window.Current.CoreWindow.KeyDown += this.coreWindowOnKeyDown;
            this.gameManager = new GameManager();
            this.gameManager.InitializeGame(this.canvas);
            this.setupEventListeners();
        }

        #endregion

        #region Methods

        private void setupEventListeners()
        {
            this.gameManager.PlayerLivesUpdated += this.livesOnPlayerLivesUpdated;
            this.gameManager.PlayerScoreUpdated += this.scoreOnPlayerScoreUpdated;
            this.gameManager.GameOverUpdated += this.gameOverTestOnGameOverUpdated;
            this.gameManager.TimeRemainingUpdated += this.timeRemainingOnTimeRemainingUpdated;
        }

        private void timeRemainingOnTimeRemainingUpdated(object sender, TimeRemainingEventArgs e)
        {
            this.timeRemainingTextBlock.Text = e.TimeRemaining.ToString();
        }

        private void livesOnPlayerLivesUpdated(object sender, PlayerLifeEventArgs e)
        {
            this.livesTextBlock.Text = e.Lives.ToString();
        }

        private void scoreOnPlayerScoreUpdated(object sender, PlayerScoreEventArgs e)
        {
            this.scoreTextBlock.Text = e.Score.ToString();
        }

        private void gameOverTestOnGameOverUpdated(object sender, EventArgs e)
        {
            this.gameOverTextBlock.Visibility = Visibility.Visible;
        }

        private void coreWindowOnKeyDown(CoreWindow sender, KeyEventArgs args)
        {
            switch (args.VirtualKey)
            {
                case VirtualKey.Left:
                    this.gameManager.MovePlayerLeft();
                    break;
                case VirtualKey.Right:
                    this.gameManager.MovePlayerRight();
                    break;
                case VirtualKey.Up:
                    this.gameManager.MovePlayerUp();
                    break;
                case VirtualKey.Down:
                    this.gameManager.MovePlayerDown();
                    break;
            }
        }

        #endregion
    }
}