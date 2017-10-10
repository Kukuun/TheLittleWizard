using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace TheLittleWizard
{
   class GameObject
    {
        // Fields
        protected float speed;
        protected float animationSpeed;
        protected Vector2 scale;
        private Vector2 position;
        protected Vector2 direction;
        private Texture2D image;
        protected Texture2D texture;
        protected Rectangle collisionRect;
        public Point gridCellPos;
        private string id;

        public string Id
        {
            get
            {
                return id;
            }
            set
            {
                id = value;
            }
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        public virtual void LoadContent(ContentManager content)
        {

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        public virtual void UnloadContent()
        {

        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public virtual void Update(GameTime gameTime)
        {

        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public virtual void Draw(SpriteBatch spriteBatch)
        {

        }
    }
}
