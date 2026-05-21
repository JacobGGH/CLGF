using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Text;
using System;


namespace CommandLineGameFramework
{
    /// <summary>
    /// A 2-dimensional position (X, Y).
    /// </summary>
    /// <remarks>
    /// In <see cref="CLGF"/>: 0, 0 is the top left of the screen.
    /// </remarks>
    public class Position
    {
        #region Properties

        /// <summary>
        /// The X coordinate of the position.
        /// </summary>
        public int X { get; set; }


        /// <summary>
        /// The Y coordinate of the position.
        /// </summary>
        public int Y { get; set; }

        #endregion


        #region Constructors

        /// <summary>
        /// Creates a new <see cref="Position"/>.
        /// </summary>
        /// <param name="x">(Optional | default: 0) The X coordinate to initialize the 
        /// position with.</param>
        /// <param name="y">(Optional | default: 0) The Y coordinate to initialize the 
        /// position with.</param>
        /// <remarks>
        /// You can optionally specify an X and Y coordinate for this position. The default
        /// value for both is 0.
        /// </remarks>
        public Position(int x = 0, int y = 0)
        {
            this.X = x;
            this.Y = y;
        }

        #endregion


        #region Functions



        #endregion
    }
}
