using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Text;
using System;


namespace CommandLineGameFramework
{
    /// <summary>
    /// Text which can be drawn to the console screen.
    /// </summary>
    public class Text : Drawable
    {
        #region Properties

        /// <summary>
        /// The text of this <see cref="Text"/> object.
        /// </summary>
        public string Body
        {
            get
            {
                return string.Join('\n', base.Content);
            }

            set
            {
                base.SetContent(value.Split('\n'));
            }
        }

        #endregion


        #region Constructors

        /// <summary>
        /// Creates a new <see cref="Text"/> object with no text or position.
        /// </summary>
        public Text()
        {

        }


        /// <summary>
        /// Creates a new <see cref="Text"/> object with the specified text.
        /// </summary>
        /// <param name="body">The text to load this <see cref="Text"/> object with.</param>
        /// <param name="x">(Optional | default: 0) The X coordinate of this 
        /// <see cref="Text"/> object's position.</param>
        /// <param name="y">(Optional | default: 0) The Y coordinate of this 
        /// <see cref="Text"/> object's position.</param>
        /// <remarks>
        /// You can optionally specify an X and Y position for this text object. 
        /// The default value for both is 0.
        /// </remarks>
        public Text(string body, int x = 0, int y = 0) : base(body.Split('\n'), x, y)
        {

        }

        #endregion


        #region Functions



        #endregion
    }
}
