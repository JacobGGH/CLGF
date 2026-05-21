using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Text;
using System;


namespace CommandLineGameFramework
{
    /// <summary>
    /// A game sprite composed of ASCII art which can be drawn to the console screen.
    /// </summary>
    public class Sprite : Drawable
    {
        #region Properties

        /// <summary>
        /// The lines of characters which compose the ASCII art.
        /// </summary>
        public string[] Texture
        {
            get
            {
                return base.Content;
            }

            set
            {
                base.SetContent(value);
            }
        }

        #endregion


        #region Constructors

        /// <summary>
        /// Creates a new <see cref="Sprite"/> without a texture or position.
        /// </summary>
        public Sprite() : base()
        {

        }


        /// <summary>
        /// Creates a new <see cref="Sprite"/> with the specified texture.
        /// </summary>
        /// <param name="texture">The texture to load the sprite with.</param>
        /// <param name="x">(Optional | default: 0) The X coordinate of the 
        /// sprite's position.</param>
        /// <param name="y">(Optional | default: 0) The Y coordinate of the 
        /// sprite's position.</param>
        /// <returns>A new <see cref="Sprite"/> with the specified texture.</returns>
        /// <remarks>
        /// You can optionally specify an X and Y position for this Sprite. The default
        /// value for both is 0.
        /// </remarks>
        public static Sprite FromString(string texture, int x = 0, int y = 0)
        {
            return new Sprite(texture.Split('\n'), x, y);
        }


        /// <summary>
        /// Creates a new <see cref="Sprite"/> with the specified texture.
        /// </summary>
        /// <param name="texture">The texture to load the sprite with.</param>
        /// <param name="x">(Optional | default: 0) The X coordinate of the 
        /// sprite's position.</param>
        /// <param name="y">(Optional | default: 0) The Y coordinate of the 
        /// sprite's position.</param>
        /// <remarks>
        /// You can optionally specify an X and Y position for this sprite. The default
        /// value for both is 0.
        /// </remarks>
        public Sprite(string[] texture, int x = 0, int y = 0) : base(texture, x, y)
        {

        }


        /// <summary>
        /// Creates a new <see cref="Sprite"/> loaded with a texture read from 
        /// an ASCII art .txt file.
        /// </summary>
        /// <param name="texturePath">The path of the ASCII art .txt file from 
        /// which the texture will be loaded.</param>
        /// <param name="x">(Optional | default: 0) The X coordinate of the 
        /// sprite's position.</param>
        /// <param name="y">(Optional | default: 0) The Y coordinate of the 
        /// sprite's position.</param>
        /// <remarks>
        /// You can optionally specify an X and Y position for this sprite. The default
        /// value for both is 0.
        /// </remarks>
        public Sprite(string texturePath, int x = 0, int y = 0)
        {
            //Load the sprite's texture.
            string[] texture = File.ReadAllLines(texturePath);
            this.Texture = texture;

            //Set the sprite's position.
            this.Position = new Position(x, y);
        }


        #endregion


        #region Functions

        /// <summary>
        /// Checks if a specified <see cref="Sprite"/> is colliding with this sprite.
        /// </summary>
        /// <param name="otherSprite">The sprite to check collision with.</param>
        /// <returns>
        /// True: the sprites are colliding.<br/>
        /// False: the sprite are not colliding.
        /// </returns>
        public bool Collide(Sprite otherSprite)
        {
            //Do an initial AABB collision check.
            bool AABBCollide =
                Left < otherSprite.Right &&
                Right > otherSprite.Left &&
                Top < otherSprite.Bottom &&
                Bottom > otherSprite.Top;

            if (!AABBCollide)
                return false;

            //Do a "pixel perfect" collision check if there's an AABB collision.
            //For each line in this sprite's texture:
            for (int y = 0; y < Height; y++)
            {
                string textureLine = Texture[y];
                //For each character in that line:
                for (int x = 0; x < textureLine.Length; x++)
                {
                    //Only check non-blank characters that are inside the other sprite.
                    char textureChar = textureLine[x];
                    if (textureChar == ' ')
                        continue;

                    int charX = Position.X + x;
                    int charY = Position.Y + y;
                    if (charX < otherSprite.Left || charX > otherSprite.Right ||
                        charY < otherSprite.Top || charY > otherSprite.Bottom)
                        continue;

                    //For each line in the other sprite's texture:
                    for (int otherY = 0; otherY < otherSprite.Height; otherY++)
                    {
                        string otherTextureLine = otherSprite.Texture[otherY];
                        //For each character in that line:
                        for (int otherX = 0; otherX < otherTextureLine.Length; otherX++)
                        {
                            /* If that character is not blank and intersents the current 
                             * character that is being looked at from this sprite: there is
                             * a collision. */
                            char otherTextureChar = otherTextureLine[otherX];
                            if (otherTextureChar == ' ')
                                continue;

                            int otherCharX = otherSprite.Position.X + otherX;
                            int otherCharY = otherSprite.Position.Y + otherY;
                            if (charX == otherCharX && charY == otherCharY)
                                return true;
                        }
                    }
                }
            }

            return false;
        }

        #endregion
    }
}
