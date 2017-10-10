using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace TheLittleWizard
{
    class Wizard : GameObject
    {
        public bool hasRegularTowerKey = false, hasIceTowerKey = false; //If he has picked up the keys
        public Point pos; //His Graphical position
        public bool move = false; //Makes sure he doesn't move / checks the path list, until he has recieved a new complete path.
        public bool passedForest = false; //When he has passed forest and checks for a new path, this will be true (Though it will be set to false again, but the path in forest is set to Wall)
        public bool monsterPlaced = false; //Checks if monster has been placed in forest for visualizing passed forest.

        int counter = 0; //Counter for movement
        int limit = 1; //If counter hits limit it will move
        float countDuration = 0.5f; //Sets the time before count
        float currentTime = 0f; //currentTime


        public Wizard(Point pos, Point gridCellPos)
        {
            this.pos = pos;
            this.gridCellPos = gridCellPos;
        }

        public override void LoadContent(ContentManager content)
        {
            texture = PreLoad.Wizard;
            base.LoadContent(content);
        }

        public override void UnloadContent()
        {

        }

        public override void Update(GameTime gameTime)
        {
            //If there is a full path ready, this is set to move
            if (move)
            {
                currentTime += (float)gameTime.ElapsedGameTime.TotalSeconds; //Adds the time from last update to currenttime (The elapsed time from last update)

                if (currentTime >= countDuration) //If the total count of time from last updates added together exceeds countDuration
                {
                    counter++;
                    currentTime -= countDuration; //Almost resets the currentTime
                }

                if (counter >= limit) //If counter hits limit move the wizard, and remove the old path slot
                {
                    counter = 0; //Counter reset

                    gridCellPos.X = GridManager.Instance.pathForWizard[0].gridX; //Moves wizard's info on Gridcell, used for reference about cells
                    gridCellPos.Y = GridManager.Instance.pathForWizard[0].gridY; //Moves wizard's info on Gridcell, used for reference about cells
                    
                    //Moves the Wizards graphical position to the cell he is moving to
                    pos = new Point(GridManager.Instance.pathForWizard[0].Position.X * GridManager.Instance.pathForWizard[0].CellSize + GridManager.Instance.pathForWizard[0].CellSize / 2, GridManager.Instance.pathForWizard[0].Position.Y * GridManager.Instance.pathForWizard[0].CellSize + GridManager.Instance.pathForWizard[0].CellSize / 2);
             
                    //Removes the path he just moved to
                    GridManager.Instance.pathForWizard.RemoveAt(0);

                    bool isEmpty = !GridManager.Instance.pathForWizard.Any(); //if the path the wizard is following, is now empty (return true)

                    if (isEmpty) //If the list was empty
                    {
                        move = false; //Disables his move to make sure he doesn't move/checks for incomplete lists.
                        GridManager.Instance.CheckPath(); //Starts a new iteration of CheckPath
                    }
                }
            }
        }
        
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, new Rectangle(pos.X, pos.Y, 32, 32), null, Color.White, 0f,new Vector2(16,16), SpriteEffects.None,0);
        }
    }
}
