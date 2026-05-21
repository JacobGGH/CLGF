using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Text;
using System;


namespace CommandLineGameFramework
{
    /// <summary>
    /// A buffer which loads and renders <see cref="Drawable"/> objects 
    /// (i.e. <see cref="Sprite"/>, <see cref="Text"/>) the console screen.
    /// </summary>
    public class DrawingBuffer
    {
        #region Properties

        /// <summary>
        /// The buffer of characters to which <see cref="Drawable"/> objects are 
        /// loaded and which is drawn to the console screen.
        /// </summary>
        private char[] Buffer = new char[CLGF.Width * CLGF.Height];

        #endregion


        #region Constructors

        /// <summary>
        /// Creates a new <see cref="DrawingBuffer"/>.
        /// </summary>
        internal DrawingBuffer()
        {
            Clear();
        }

        #endregion


        #region Functions

        /// <summary>
        /// Loads data to the buffer.
        /// </summary>
        /// <param name="data">The data to load.</param>
        /// <param name="x">The x coordinate of the data's position.</param>
        /// <param name="y">The y coordinate of the data's position.</param>
        private void LoadToBuffer(string[] data, int x, int y)
        {
            //For each line of the data:
            for (int loopY = 0; loopY < data.Length; loopY++)
            {
                int localY = y + loopY;
                if (localY < 0 || localY >= CLGF.Height)
                    continue;

                //For each character in that line:
                for (int loopX = 0; loopX < data[loopY].Length; loopX++)
                {
                    //Skip loading the character if it's outside the console screen or is blank.
                    int localX = x + loopX;
                    if (localX < 0 || localX >= CLGF.Width)
                        continue;

                    char dataChar = data[loopY][loopX];
                    if (dataChar == ' ')
                        continue;

                    //Load that character into the buffer.
                    int targetIndex = localY * CLGF.Width + localX;
                    Buffer[targetIndex] = dataChar;
                }
            }
        }


        /// <summary>
        /// Loads a <see cref="Drawable"/> object (i.e. <see cref="Sprite"/>, 
        /// <see cref="Text"/>) to the buffer.
        /// </summary>
        /// <param name="element">The object to load.</param>
        public void Load(Drawable element)
        {
            LoadToBuffer(element.Content, element.Position.X, element.Position.Y);
        }


        /// <summary>
        /// Draws everything in the buffer to the console screen.
        /// </summary>
        public void Draw()
        {
            Console.SetCursorPosition(0, 0);
            Console.Write(new string(Buffer));

            Clear();
        }


        /// <summary>
        /// Clears the buffer.
        /// </summary>
        public void Clear()
        {
            for (int i = 0; i < Buffer.Length; i++)
                Buffer[i] = ' ';
        }

        #endregion
    }
}
