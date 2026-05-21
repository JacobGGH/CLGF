using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Text;
using System;


namespace CommandLineGameFramework
{
    /// <summary>
    /// The base class for an object that can be drawn to the console screen
    /// (i.e. <see cref="Sprite"/>, <see cref="Text"/>, etc.).
    /// </summary>
    public class Drawable
    {
        #region Properties

        /// <summary>
        /// The drawable content of this object.
        /// </summary>
        /// <remarks>
        /// Examples of content:<br/>
        /// A <see cref="Sprite"/>'s texture is its content.<br/>
        /// A <see cref="Text"/>'s body (text) is its content.
        /// </remarks>
        internal string[] Content = [];


        /// <summary>
        /// The width of this object's content.
        /// </summary>
        public int Width { get; private set; }


        /// <summary>
        /// The height of this object's content.
        /// </summary>
        public int Height { get; private set; }


        /// <summary>
        /// The y coordinate of the content's top edge.
        /// </summary>
        public int Top
        {
            get { return Position.Y; }
        }


        /// <summary>
        /// The y coordinate of the content's bottom edge.
        /// </summary>
        public int Bottom
        {
            get { return Position.Y + Height; }
        }


        /// <summary>
        /// The x coordinate of the content's left edge.
        /// </summary>
        public int Left
        {
            get { return Position.X; }
        }


        /// <summary>
        /// The x coordinate of the content's right edge.
        /// </summary>
        public int Right
        {
            get { return Position.X + Width; }
        }


        /// <summary>
        /// The position of this object.
        /// </summary>
        public Position Position { get; set; } = new();

        #endregion


        #region Constructors

        /// <summary>
        /// Creates a new <see cref="Drawable"/> object without content or a position.
        /// </summary>
        internal Drawable()
        {

        }


        /// <summary>
        /// Creates a new <see cref="Drawable"/> object with the specified content.
        /// </summary>
        /// <param name="content">The content to load the drawable object with.</param>
        /// <param name="x">(Optional | default: 0) The X coordinate of the 
        /// drawable object's position.</param>
        /// <param name="y">(Optional | default: 0) The Y coordinate of the 
        /// drawable object's position.</param>
        /// <remarks>
        /// You can optionally specify an X and Y position for this object. The default
        /// value for both is 0.
        /// </remarks>
        internal Drawable(string[] content, int x = 0, int y = 0)
        {
            SetContent(content);
            this.Position = new Position(x, y);
        }

        #endregion


        #region Functions

        /// <summary>
        /// Sets the content of this object.
        /// </summary>
        /// <param name="content">The content to set.</param>
        internal void SetContent(string[] content)
        {
            //Set the drawable object's content.
            Content = content;

            //Re-calculate the content's width and height.
            Width = CalculateContentWidth();
            Height = content.Length;
        }


        /// <summary>
        /// Calcualtes the width of this object's content.
        /// </summary>
        /// <returns>The width of this object's content.</returns>
        private int CalculateContentWidth()
        {
            int width = 0;
            foreach (string textureLine in Content)
            {
                if (textureLine.Length > width)
                    width = textureLine.Length;
            }

            return width;
        }


        /// <summary>
        /// Calculates the coordinate relative to the parnet that would center 
        /// the child on the parent.
        /// </summary>
        /// <param name="parentSize">The size of the parent (width or height).</param>
        /// <param name="childSize">The size of the child (width or height).</param>
        /// <returns>The coordinate relative to the parnet that would center 
        /// the child on the parent.</returns>
        private int CalculateCenter(int parentSize, int childSize)
        {
            return (parentSize / 2) - (childSize / 2);
        }


        /// <summary>
        /// Centers this object relative to a specified width on the X axis.
        /// </summary>
        /// <param name="parentWidth">The width to center this object on.</param>
        public void CenterX(int parentWidth)
        {
            Position.X = CalculateCenter(parentWidth, Width);
        }


        /// <summary>
        /// Centers this object relative to a specified 
        /// <see cref="Drawable"/> object on the X axis.
        /// </summary>
        /// <param name="parent">The <see cref="Drawable"/> 
        /// object to center this object on.</param>
        public void CenterX(Drawable parent)
        {
            Position.X = parent.Position.X + CalculateCenter(parent.Width, Width);
        }


        /// <summary>
        /// Centers this object relative to a specified height on the Y axis.
        /// </summary>
        /// <param name="parentHeight">The height to center this object on.</param>
        public void CenterY(int parentHeight)
        {
            Position.Y = CalculateCenter(parentHeight, Height);
        }


        /// <summary>
        /// Centers this object relative to a specified 
        /// <see cref="Drawable"/> object on the Y axis.
        /// </summary>
        /// <param name="parent">The <see cref="Drawable"/> 
        /// object to center this object on.</param>
        public void CenterY(Drawable parent)
        {
            Position.Y = parent.Position.Y + CalculateCenter(parent.Height, Height);
        }


        /// <summary>
        /// Centers this object relative to a specified width and height.
        /// </summary>
        /// <param name="parentWidth">The width to center this object on.</param>
        /// <param name="parentHeight">The height to center this object on.</param>
        public void Center(int parentWidth, int parentHeight)
        {
            CenterX(parentWidth);
            CenterY(parentHeight);
        }


        /// <summary>
        /// Centers this object relative to a specified <see cref="Drawable"/> object.
        /// </summary>
        /// <param name="parent">The <see cref="Drawable"/> 
        /// object to center this object on.</param>
        public void Center(Drawable parent)
        {
            CenterX(parent);
            CenterY(parent);
        }

        #endregion
    }
}
