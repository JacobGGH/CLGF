using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Text;
using System;


namespace CommandLineGameFramework
{
    /// <summary>
    /// The base class for a scene in a game (i.e. menu, main gameplay, etc.). 
    /// Inherent from this class to create scenes for a game.
    /// </summary>
    /// <remarks>
    /// Game scene classes that inherent from this class should override the
    /// <see cref="Update()"/> and <see cref="Draw()"/> methods.
    /// </remarks>
    public class GameScene
    {
        #region Properties



        #endregion


        #region Constructors

        /// <summary>
        /// Creates a new <see cref="GameScene"/>.
        /// </summary>
        public GameScene()
        {

        }

        #endregion


        #region Functions

        /// <summary>
        /// Updates the game scene.
        /// </summary>
        public virtual void Update()
        {

        }


        /// <summary>
        /// Draws the game scene.
        /// </summary>
        public virtual void Draw()
        {

        }

        #endregion
    }
}
