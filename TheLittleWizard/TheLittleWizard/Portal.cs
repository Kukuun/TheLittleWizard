using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace TheLittleWizard
{
    class Portal : GameObject
    {
        public bool finishedGame = false;
        Point pos;

        public Portal(Point pos, Point gridCellPos)
        {
            this.pos = pos;
            this.gridCellPos = gridCellPos;
        }

        public override void LoadContent(ContentManager content)
        {
            texture = PreLoad.Portal;
            base.LoadContent(content);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, new Rectangle(pos.X, pos.Y, 32, 32), null, Color.White, 0f, new Vector2(16, 16), SpriteEffects.None, 0);
        }
    }
}
