// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace FroggerStarter.View.Sprites
{
    /// <summary>
    ///     The PlayerSprite class
    /// </summary>
    /// <seealso cref="FroggerStarter.View.Sprites.BaseSprite" />
    /// <seealso cref="Windows.UI.Xaml.Markup.IComponentConnector" />
    /// <seealso cref="Windows.UI.Xaml.Markup.IComponentConnector2" />
    public sealed partial class PlayerSprite
    {
        #region Data members

        private readonly IList<Canvas> allPlayerStages;

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="PlayerSprite" /> class.
        ///     Postcondition: PlayerSprite initialized
        /// </summary>
        public PlayerSprite()
        {
            this.InitializeComponent();
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="PlayerSprite" /> class with a given stage.
        /// </summary>
        /// <param name="stage">The stage.</param>
        public PlayerSprite(PlayerStages stage) : this()
        {
            this.allPlayerStages = new List<Canvas>
                {this.mainFrog, this.deadFrog, this.squishedFrog, this.blob, this.deathX};

            this.collapsePlayer();

            this.allPlayerStages[(int) stage].Visibility = Visibility.Visible;
        }

        #endregion

        #region Methods

        private void collapsePlayer()
        {
            foreach (var stage in this.allPlayerStages)
            {
                stage.Visibility = Visibility.Collapsed;
            }
        }

        #endregion
    }

    /// <summary>
    ///     Player stage choices
    /// </summary>
    public enum PlayerStages
    {
        /// <summary>
        ///     The normal frog
        /// </summary>
        NormalFrog,

        /// <summary>
        ///     The dead frog
        /// </summary>
        DeadFrog,

        /// <summary>
        ///     The squished frog
        /// </summary>
        SquishedFrog,

        /// <summary>
        ///     The blob frog
        /// </summary>
        BlobFrog,

        /// <summary>
        ///     The death x
        /// </summary>
        DeathX
    }
}