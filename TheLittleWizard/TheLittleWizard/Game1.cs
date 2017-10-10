using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;

namespace TheLittleWizard
{
    enum KeyTag { REGULARTOWERKEY, ICETOWERKEY };
    enum TowerTag { REGULARTOWER, ICETOWER };
    enum State { FINDREGULARTOWERKEY, DELIEVERREGULARTOWERKEY, FINDICETOWERKEY, DELIEVERICETOWERKEY, GOTOPORTAL }; //States for what state the Wizard is in, if he is to find Regular Key etc.
    enum CellType { START, GOAL, WALL, EMPTY }; //Walls is all non-walkable (Water/Woods/Path_In_Forest_After_Been_Passed)
    enum PathParticipation { NOT, NEIGHBOR, PATH };

    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        Texture2D image;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private static List<GameObject> objectList = new List<GameObject>();
        private static List<GameObject> objectToRemove = new List<GameObject>();
        private static List<GameObject> objectToAdd = new List<GameObject>();
        private Texture2D wizard;
        private Texture2D portal;
        private int screenWidth;
        private int screenHeight;
        public static Point regularTowerPosition, iceTowerPosition, regularKeyPosition, iceKeyPosition; //Static positions of objects, for easier reference in code
        
        /// <summary>
        /// Current objects in game
        /// </summary>
        internal static List<GameObject> ObjectList
        {
            get
            {
                return objectList;
            }

            set
            {
                objectList = value;
            }
        }
        /// <summary>
        /// Objects to remove from game, iterates AFTER all current running foreach of Objectlist has runned
        /// </summary>
        internal static List<GameObject> ObjectToRemove
        {
            get
            {
                return objectToRemove;
            }

            set
            {
                objectToRemove = value;
            }
        }
        /// <summary>
        /// Objects to add to game, iterates AFTER all current running foreach of Objectlist has runned
        /// </summary>
        internal static List<GameObject> ObjectToAdd
        {
            get
            {
                return objectToAdd;
            }

            set
            {
                objectToAdd = value;
            }
        }

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferWidth = 1024; //Fixed Width Window Size
            graphics.PreferredBackBufferHeight = 768; //Fixed Height Window Size
            graphics.ApplyChanges();
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            graphics.PreferredBackBufferHeight = 480;
            graphics.PreferredBackBufferWidth = 480;
            graphics.ApplyChanges();
            IsMouseVisible = true;

            screenHeight = GraphicsDevice.Viewport.Height;
            screenWidth = GraphicsDevice.Viewport.Width;

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            PreLoad.LoadContent(Content); //Runs Preloader's Content (Gfx for all objects to static variables to be loaded instead of loading gfx for each Object in game)

            // TODO: use this.Content to load your game content here
            GridManager.Create(graphics, new Rectangle(new Point(0, 0), new Point(screenWidth)));
            
            //Loads all objects LoadContent (Gfx stuff)
            foreach (GameObject item in objectList)
            {
                item.LoadContent(Content);
            }
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            
            //Runs through all objects Update Method
            foreach (GameObject item in objectList)
            {
                item.Update(gameTime);
            }
            GridManager.Instance.Update(gameTime); //Runs the GridManager's Update Logic (It's not a GameObject therefore not added as one)
            ListUpdate();
            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// Method to handle the add/remove of objects after Update/Draw has been runned, to avoid raceconditions
        /// </summary>
        private void ListUpdate()
        {
            lock (objectList)
            {
                objectToAdd.ForEach(item => item.LoadContent(Content)); //Makes sure all objects have runned LoadContent (If new objects which preload doesn't handle) before adding them to the ObjectList
                objectToAdd.ForEach(item => ObjectList.Add(item)); //Adds all objects from objectToAdd to ObjectList
                ObjectToRemove.ForEach(item => objectList.Remove(item)); //Removes all objects which is also found in ObjectList in the ObjectList

                objectToAdd.Clear(); //As all have been added once, clear this list
                ObjectToRemove.Clear(); //As all have been removed once, clear this list
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            GridManager.Instance.Draw(spriteBatch); //Runs the GridManager's Draw Logic (It's not a GameObject therefore not added as one)

            //Runs through all objects Draw Method
            foreach (GameObject item in objectList)
            {
                item.Draw(spriteBatch);
            }
            //spriteBatch.Draw(wizard, new Rectangle(30, 700, 32, 32), Color.White);
            spriteBatch.End();

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
