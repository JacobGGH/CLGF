using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Text;
using System;


namespace CommandLineGameFramework
{
    /// <summary>
    /// A framework for programming games that run in the command prompt/command line.
    /// </summary>
    public static class CLGF
    {
        #region Properties

        /// <summary>
        /// The manager responsible for running the game.
        /// </summary>
        public static GameManager GameManager { get; private set; }


        /// <summary>
        /// The buffer <see cref="CLGF"/> uses to draw to the console screen.
        /// </summary>
        /// <remarks>
        /// Currently <see cref="CLGF"/> supports drawing <see cref="Sprite"/> and 
        /// <see cref="Text"/> objects to the console screen.
        /// </remarks>
        public static DrawingBuffer DrawingBuffer { get; private set; }


        /// <summary>
        /// The amount of time since the last frame, in milliseconds.
        /// </summary>
        public static float DeltaTime { get; internal set; }


        /* "Console.BufferWidth" and "Console.BufferHeight" are cached as properties 
         * because getting them every frame would be time consuming and cause the game 
         * to lag. */
        /// <summary>
        /// The width of the console screen.
        /// </summary>
        /// <remarks>
        /// This value is acutally <see cref="Console.BufferWidth"/>. But because
        /// objects cannot be drawn outside of that buffer, you can 
        /// think of it as the actual width.
        /// </remarks>
        public static int Width { get; private set; } = Console.BufferWidth;


        /// <summary>
        /// The height of the console screen.
        /// </summary>
        /// <remarks>
        /// This value is acutally <see cref="Console.BufferHeight"/>. But because
        /// objects cannot be drawn outside of that buffer, you can 
        /// think of it as the actual height.
        /// </remarks>
        public static int Height { get; private set; } = Console.BufferHeight;

        #endregion


        #region Constructors

        /// <summary>
        /// The static constructor for <see cref="CLGF"/>.
        /// </summary>
        static CLGF()
        {
            /* The order of initialization here is important as some properties
             * may rely on others. */
            DeltaTime = 0.0f;
            Width = Console.BufferWidth;
            Height = Console.BufferHeight;

            //"DrawingBuffer" relies on "Width" and "Height".
            DrawingBuffer = new();
            GameManager = new();
        }

        #endregion
    }
}
