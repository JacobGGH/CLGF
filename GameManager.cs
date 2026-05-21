using System.Collections.Generic;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System;


namespace CommandLineGameFramework
{
    /// <summary>
    /// A manager which handles running a game (i.e. running the game loop, 
    /// transitioning game scenes, etc.).
    /// </summary>
    public class GameManager
    {
        #region Properties

        /// <summary>
        /// A flag which indicates:<br/>
        /// True: the game should be running.<br/> 
        /// False: the game should stop running.
        /// </summary>
        private bool Running = false;


        /// <summary>
        /// The queue of actions for the <see cref="GameManager"/> to process at the beginning 
        /// of the next game loop iteration.
        /// </summary>
        private Queue<Action> QueuedActions = new();


        /// <summary>
        /// The game's <see cref="GameScene"/> stack.
        /// </summary>
        /// <remarks>
        /// The top most <see cref="GameScene"/> in the stack is updated and drawn.
        /// </remarks>
        private Stack<GameScene> SceneStack = new();


        /// <summary>
        /// A flag which indicates:<br/>
        /// True: a game scene is queued to be entered.<br/> 
        /// False: a game scene is not queued to be entered.
        /// </summary>
        private bool EnterScene = false;


        /// <summary>
        /// A flag which indicates:<br/>
        /// True: the current game scene is queued to be exited.<br/> 
        /// False: the current game scene is not queued to be exited.
        /// </summary>
        private bool ExitScene = false;


        /// <summary>
        /// The game's stopwatch, which is used to calculate deltatime.
        /// </summary>
        private Stopwatch GameWatch = new();


        /// <summary>
        /// The FPS (frames per second) the game attempts to run at.
        /// </summary>
        public int FPS { get; private set; } = 30;


        /// <summary>
        /// The time that each frame should last, in milliseconds.
        /// </summary>
        private float TargetFrameDuration = 0;


        /// <summary>
        /// The maximum anticipated delay time of 
        /// <see cref="Thread.Sleep(int)"/>, in milliseconds.
        /// </summary>
        /// <remarks>
        /// <see cref="Thread.Sleep(int)"/> is not very precise. It can have an extra delay of
        /// anywhere from ~0.2ms to ~18ms.
        /// </remarks>
        private float MaxThreadSleepDelay = 20f;


        /// <summary>
        /// The number of milliseconds in a second.
        /// </summary>
        private const float Milliseconds = 1000f;

        #endregion


        #region Constructors

        /// <summary>
        /// Creates a new <see cref="GameManager"/>.
        /// </summary>
        internal GameManager()
        {
            //Delta time and target FPS setup.
            GameWatch.Restart();
            CalcualteTargetFrameDuration();
        }

        #endregion


        #region Functions

        /// <summary>
        /// Runs a quick test and measures the maximum delay from 
        /// <see cref="Thread.Sleep(int)"/>.
        /// </summary>
        /// <remarks>
        /// This function sets the value of <see cref="MaxThreadSleepDelay"/>.
        /// </remarks>
        private void MeasureMaxThreadSleepDelay()
        {
            //Set up variables.
            Stopwatch watch = new();
            int testCount = 50;
            int sleepTime = 10;

            //Run the test.
            float maxDelay = float.MinValue;
            for (int i = 0; i < testCount; i++)
            {
                //Measure the time taken to run Thread.Sleep().
                watch.Restart();
                Thread.Sleep(sleepTime);
                watch.Stop();

                //Record the delay.
                float delay = (float)watch.Elapsed.TotalMilliseconds - sleepTime;
                if (delay > maxDelay)
                    maxDelay = delay;
            }

            //Save the longest delay measured.
            MaxThreadSleepDelay = maxDelay;
        }


        /// <summary>
        /// Preforms the pre-game actions necessary for the game to run.
        /// </summary>
        private void PreformPreGameSetup()
        {
            //Set the console's visual settings so that the game renders correctly.
            Console.Clear();
            Console.CursorVisible = false;

            CLGF.DrawingBuffer.Load(new Text("Preforming pre-game set up..."));
            CLGF.DrawingBuffer.Draw();

            //Preform pre-game actions.
            MeasureMaxThreadSleepDelay();
        }


        /// <summary>
        /// Preforms the post-game clean up actions.
        /// </summary>
        private void PreformPostGameCleanup()
        {
            //Reset the console's visual settings.
            Console.Clear();
            Console.CursorVisible = true;
        }


        /// <summary>
        /// Processes all of the game's queued actions.
        /// </summary>
        private void ProcessQueuedActions()
        {
            if (QueuedActions.Count == 0)
                return;

            while (QueuedActions.Count > 0)
                QueuedActions.Dequeue().Invoke();
        }


        /// <summary>
        /// Sleeps the game until the end of the current frame.
        /// </summary>
        private void SleepUntilEndOfFrame()
        {
            /* "Thread.Sleep()" is only used if the amout of time left to sleep is longer
             * than the maximum amout of delay expected from it. This prevents the game from
             * hogging the CPU while also allowing it to run smoothly at its target FPS.
             * See "MaxThreadSleepDelay" for more details. */
            while (GameWatch.Elapsed.TotalMilliseconds < TargetFrameDuration)
            {
                double sleepTime = TargetFrameDuration - GameWatch.Elapsed.TotalMilliseconds;
                if (sleepTime > MaxThreadSleepDelay)
                    Thread.Sleep((int)(sleepTime - MaxThreadSleepDelay));

                else
                    Thread.SpinWait(10);
            }
        }


        /// <summary>
        /// Starts the game (begins the game loop).
        /// </summary>
        /// <param name="startingGameScene">The game scene with which the game starts.</param>
        public void Run(GameScene startingGameScene)
        {
            //Preform pre-game set up.
            PreformPreGameSetup();

            //Enter the starting game scene.
            EnterNewScene(startingGameScene);

            //Begin the game loop.
            Running = true;
            while (Running)
            {
                //Calcualte delta time.
                CLGF.DeltaTime = (float)GameWatch.Elapsed.TotalMilliseconds;
                GameWatch.Restart();

                //Process any queued actions (i.e. changing game scenes, updating FPS, etc.).
                ProcessQueuedActions();

                //Update and render the game's next frame.
                if (SceneStack.Count > 0)
                {
                    SceneStack.Peek().Update();
                    SceneStack.Peek().Draw();
                }

                else
                {
                    throw new Exception("There are no more game scenes to run, " +
                        "but the game is still running.");
                }

                //Cap the game to the target FPS.
                SleepUntilEndOfFrame();
            }

            //Preform post-game clean up.
            PreformPostGameCleanup();
        }


        /// <summary>
        /// Exits the game (stops the game loop).
        /// </summary>
        /// <remarks>
        /// Take any pre-exit actions (i.e. saving progress) before 
        /// calling this method.
        /// </remarks>
        public void Exit()
        {
            Running = false;
        }


        /// <summary>
        /// Changes the game's current scene.
        /// </summary>
        /// <param name="scene">The game scene to enter.</param>
        public void EnterNewScene(GameScene scene)
        {
            if (!EnterScene && !ExitScene)
            {
                EnterScene = true;

                QueuedActions.Enqueue(() =>
                {
                    SceneStack.Push(scene);
                    EnterScene = false;
                });
            }
        }


        /// <summary>
        /// Exits the current game scene.
        /// </summary>
        /// <remarks>
        /// The current game scene will not be exited if the result would be
        /// that there are no more game scenes left to run (in other words:
        /// if the game manager's scene stack would become empty). There must be
        /// at least one game scene for the game to run at all times. If the game
        /// should exited, use <see cref="Exit()"/>.
        /// </remarks>
        public void ExitCurrentScene()
        {
            if (!ExitScene && !EnterScene && SceneStack.Count > 1)
            {
                ExitScene = true;

                QueuedActions.Enqueue(() =>
                {
                    SceneStack.Pop();
                    ExitScene = false;
                });
            }
        }


        /// <summary>
        /// Calcualtes and updates <see cref="TargetFrameDuration"/> so that the game 
        /// runs at its set FPS.
        /// </summary>
        private void CalcualteTargetFrameDuration()
        {
            TargetFrameDuration = Milliseconds / FPS;
        }


        /// <summary>
        /// Updates the game to run at a new FPS.
        /// </summary>
        /// <param name="newFPS">The new FPS rate.</param>
        public void UpdateFPS(int newFPS)
        {
            QueuedActions.Enqueue(() =>
            {
                FPS = newFPS;
                CalcualteTargetFrameDuration();
            });
        }

        #endregion
    }
}
