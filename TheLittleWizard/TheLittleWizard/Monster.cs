using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheLittleWizard
{
    class Monster : GameObject
    {
        Point pos;

        public Monster(Point pos)
        {
            this.pos = pos;
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        public override void LoadContent(ContentManager content)
        {
            texture = PreLoad.Enemy;
            base.LoadContent(content);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        public override void UnloadContent()
        {

        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {

        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, new Rectangle(pos.X, pos.Y, 32, 32), null, Color.White, 0f, new Vector2(16, 16), SpriteEffects.None, 0);
        }
    }
}