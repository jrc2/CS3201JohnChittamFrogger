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
            var allFrogStages = new List<Canvas>
                {this.mainFrog, this.deadFrog, this.squishedFrog, this.blob, this.deathX};
            foreach (var state in allFrogStages)
            {
                state.Visibility = Visibility.Collapsed;
            }

            allFrogStages[(int) stage].Visibility = Visibility.Visible;
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