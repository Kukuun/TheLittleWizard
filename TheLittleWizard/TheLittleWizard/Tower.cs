using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheLittleWizard
{
    class Tower : GameObject
    {
        public bool isRegularTower;
        public TowerTag towerTag;

        Point pos;

        public Tower(Point pos, bool isRegularTower, Point gridCellPos)
        {
            this.pos = pos;
            this.isRegularTower = isRegularTower;
            this.gridCellPos = gridCellPos;
        }

        public override void LoadContent(ContentManager content)
        {
            if (isRegularTower)
            {
                texture = PreLoad.RegularTower;
            }
            else
            {
                texture = PreLoad.IceTower;
            }

            base.LoadContent(content);
        }

        public override void UnloadContent()
        {

        }

        public override void Update(GameTime gameTime)
        {

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (isRegularTower)
            {
                spriteBatch.Draw(texture, new Rectangle(pos.X, pos.Y, 32, 100), null, Color.White, 0f, new Vector2(16, 100), SpriteEffects.None, 0);
            }
            else
            {
                spriteBatch.Draw(texture, new Rectangle(pos.X, pos.Y, 32, 100), null, Color.White, 0f, new Vector2(16, 100), SpriteEffects.None, 0);
            }
        }
    }
}
