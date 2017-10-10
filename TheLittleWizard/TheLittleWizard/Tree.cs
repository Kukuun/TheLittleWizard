using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TheLittleWizard
{
    class Tree : GameObject
    {
        Point pos;

        public Tree(Point pos)
        {
            this.pos = pos;
        }

        public override void LoadContent(ContentManager content)
        {
            texture = PreLoad.Tree;
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
            spriteBatch.Draw(texture, new Rectangle(pos.X, pos.Y, 32, 48), null, Color.White, 0f, new Vector2(16, 24), SpriteEffects.None, 0);
        }
    }
}
