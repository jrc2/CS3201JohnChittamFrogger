using System.Collections;
using System.Collections.Generic;
using Windows.UI.Xaml;

namespace FroggerStarter.Model
{
    /// <summary>
    ///     Manages the players in the home spaces
    /// </summary>
    public class HomePlayerManager : IEnumerable<Player>
    {
        #region Data members

        private const int HomePlayerOffsetX = 100;

        private readonly IList<Player> homePlayers;

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="HomePlayerManager" /> class.
        /// </summary>
        public HomePlayerManager()
        {
            this.homePlayers = new List<Player> {new Player(), new Player(), new Player(), new Player(), new Player()};

            var currX = HomePlayerOffsetX;
            foreach (var player in this.homePlayers)
            {
                player.SetSpeedTo(0);
                player.Sprite.Visibility = Visibility.Collapsed;
                player.Y = 55;
                player.X = currX;
                currX += HomePlayerOffsetX;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Returns an enumerator that iterates through the homePlayers.
        /// </summary>
        /// <returns>
        ///     An enumerator that can be used to iterate through the homePlayers.
        /// </returns>
        public IEnumerator<Player> GetEnumerator()
        {
            return this.homePlayers.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion
    }
}