using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheLittleWizard
{
    class Key : GameObject
    {
        bool isRegularTowerKey;
        Point pos;

        /// <summary>
        /// Constructor used to define position, type of key, cell position and id.
        /// </summary>
        /// <param name="pos">Key position</param>
        /// <param name="isRegularTowerKey">If it belong to a certain tower</param>
        /// <param name="gridCellPos">Cell position inside the grid</param>
        /// <param name="id">Key identity</param>
        public Key(Point pos, bool isRegularTowerKey, Point gridCellPos, string id)
        {
            this.pos = pos;
            this.isRegularTowerKey = isRegularTowerKey;
            this.gridCellPos = gridCellPos;
            Id = id;
        }
        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        public override void LoadContent(ContentManager content)
        {
            if (isRegularTowerKey)
            {
                texture = PreLoad.RegularTowerKey;
            }
            else
            {
                texture = PreLoad.IceTowerKey;
            }

            base.LoadContent(content);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (isRegularTowerKey)
            {
                spriteBatch.Draw(texture, new Rectangle(pos.X, pos.Y, 32, 32), null, Color.White, 0f, new Vector2(10, 10), SpriteEffects.None, 0);
            }
            else
            {
                spriteBatch.Draw(texture, new Rectangle(pos.X, pos.Y, 32, 32), null, Color.White, 0f, new Vector2(10, 10), SpriteEffects.None, 0);
            }
        }
    }
}
