using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TheLittleWizard
{
    class GridManager
    {
        private Wizard wizard;
        private Monster monster;
        private Key regularKey;
        private Key magicKey;
        private Portal portal;

        List<Cell> placeableKeyPoints = new List<Cell>();
        Cell wizardCell;
        Cell portalCell;

        //private GraphicsDeviceManager dc;
        private Rectangle displayRectangle;

        public State myState = State.FINDREGULARTOWERKEY;
        public List<Cell> pathForWizard;

        /// <summary>
        /// Amount of rows in the grid
        /// </summary>
        private int cellRowCount;
        //Sets the cell size
        int cellSize = 32;

        /// <summary>
        /// This list contains all cells
        /// </summary>
        private List<Cell> grid;

        /// <summary>
        /// The current click type
        /// </summary>
        private CellType clickType;
        static Texture2D whitePixel;

        private static GridManager instance;

        public static GridManager Instance
        {
            get
            {
                if (instance == null)
                {
                    throw new Exception("Grid Manager not instaced!");
                }
                return instance;
            }
        }

        internal Wizard Wizard
        {
            get
            {
                return wizard;
            }
        }

        internal Portal Portal
        {
            get
            {
                return portal;
            }

            set
            {
                portal = value;
            }
        }

        public static void Create(GraphicsDeviceManager device, Rectangle rectangle)
        {
            if (instance == null)
            {
                instance = new GridManager(device, rectangle);
            }
        }

        /// <summary>
        /// Runs the needed methods for creating the grid, world and path
        /// </summary>
        /// <param name="dc"></param>
        /// <param name="displayRectangle"></param>
        private GridManager(GraphicsDeviceManager dc, Rectangle displayRectangle)
        {
            //Sets the displayRectangle
            this.displayRectangle = displayRectangle;

            //Sets the row count to then, this will create a 10 by 10 grid.
            cellRowCount = 15;
            CreateGrid();
            GenerateWorld();
            CheckPath();
        }

        /// <summary>
        /// Creates the grid
        /// </summary>
        public void CreateGrid()
        {
            //Instantiates the list of cells
            grid = new List<Cell>();            

            //Creates all the cells
            for (int x = 0; x < cellRowCount; x++)
            {
                for (int y = 0; y < cellRowCount; y++)
                {
                    grid.Add(new Cell(new Point(x, y), cellSize, x, y));
                }
            }

            wizardCell = grid.Find(x => x.gridX == 1 && x.gridY == 14);
            portalCell = grid.Find(x => x.gridX == 0 && x.gridY == 14);
            wizard = new Wizard(new Point(wizardCell.Position.X * cellSize + cellSize / 2, wizardCell.Position.Y * cellSize + cellSize / 2), new Point(1, 14));
            Portal = new Portal(new Point(portalCell.Position.X * cellSize + cellSize / 2, portalCell.Position.Y * cellSize + cellSize / 2), new Point(0, 14));
            Game1.ObjectToAdd.Add(wizard);
            Game1.ObjectToAdd.Add(portal);
        }

        /// <summary>
        /// Creats a list of neighboring cell's
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        public List<Cell> GetNeighbors(Cell cell)
        {
            List<Cell> neighbors = new List<Cell>();

            for (int x = -1; x < 2; x++)
            {
                for (int y = -1; y < 2; y++)
                {
                    if (x == 0 && y == 0)
                    {
                        continue;
                    }

                    int checkX = cell.gridX + x;
                    int checkY = cell.gridY + y;

                    foreach (Cell item in grid)
                    {
                        if (item.gridX == checkX && item.gridY == checkY)
                        {
                            //neighbors.Add(item);

                            if (Math.Abs(item.gridX - cell.gridX) + Math.Abs(item.gridY - cell.gridY) == 2)
                            {
                                if (CheckCorner(cell, item))
                                {
                                    neighbors.Add(item);
                                }
                            }
                            else
                            {
                                neighbors.Add(item);
                            }
                        }
                    }
                }
            }
            return neighbors;
        }

        /// <summary>
        /// //Checks to see if a path has been found
        /// </summary>
        public void CheckPath()
        {

            bool pathFound = false;
            foreach (Cell item in grid)
            {
                item.gCost = 0;
                item.hCost = 0;
                item.pathPart = PathParticipation.NOT;
                item.parent = null;
            }
            if (Wizard.passedForest)
            {
                int monsterX;
                int monsterY;
                foreach (Cell item in grid)
                {
                    if (item.gridX >= 4 && item.gridX <= 11 && item.gridY == 13)
                    {
                        item.myType = CellType.WALL;
                        monsterX = item.gridX;
                        monsterY = item.gridY;
                    }
                    if (item.gridX == 7 && item.gridY == 13 && !Wizard.monsterPlaced)
                    {
                        Wizard.monsterPlaced = true;
                        monster = new Monster(new Point(item.Position.X * item.CellSize + item.CellSize / 2, item.Position.Y * item.CellSize + item.CellSize / 2));
                        Game1.ObjectToAdd.Add(monster);
                    }
                }
                //Reverses passed forrest for less calculations
                Wizard.passedForest = false;
            }

            Pathfinding path = new Pathfinding();

            Cell start;
            Cell end;

            //Changes state and goal according to last goal
            if (myState == State.FINDREGULARTOWERKEY)
            {
                start = grid.Find(x => x.gridX == wizard.gridCellPos.X && x.gridY == wizard.gridCellPos.Y);
                end = grid.Find(x => x.gridX == Game1.regularKeyPosition.X && x.gridY == Game1.regularKeyPosition.Y);

                pathForWizard = path.AStar(start, end, this);
                pathFound = true;
            }
            if (myState == State.DELIEVERREGULARTOWERKEY)
            {
                start = grid.Find(x => x.gridX == wizard.gridCellPos.X && x.gridY == wizard.gridCellPos.Y);
                end = grid.Find(x => x.gridX == Game1.regularTowerPosition.X && x.gridY == Game1.regularTowerPosition.Y);

                pathForWizard = path.AStar(start, end, this);
                pathFound = true;

            }
            if (myState == State.FINDICETOWERKEY)
            {
                start = grid.Find(x => x.gridX == wizard.gridCellPos.X && x.gridY == wizard.gridCellPos.Y);
                end = grid.Find(x => x.gridX == Game1.iceKeyPosition.X && x.gridY == Game1.iceKeyPosition.Y);

                pathForWizard = path.AStar(start, end, this);
                pathFound = true;

            }
            if (myState == State.DELIEVERICETOWERKEY)
            {
                start = grid.Find(x => x.gridX == wizard.gridCellPos.X && x.gridY == wizard.gridCellPos.Y);
                end = grid.Find(x => x.gridX == Game1.iceTowerPosition.X && x.gridY == Game1.iceTowerPosition.Y);

                pathForWizard = path.AStar(start, end, this);
                pathFound = true;

            }
            if (myState == State.GOTOPORTAL)
            {
                start = grid.Find(x => x.gridX == wizard.gridCellPos.X && x.gridY == wizard.gridCellPos.Y);
                end = grid.Find(x => x.gridX == portal.gridCellPos.X && x.gridY == portal.gridCellPos.Y);

                pathForWizard = path.AStar(start, end, this);
                pathFound = true;

            }
            if (pathFound)
            {
                //Send list to wizard -> For walking purpose!
                Wizard.move = true;
            }
        }

        /// <summary>
        /// Check if a neighbor is next to a wall and stops diagonal movment  
        /// </summary>
        /// <param name="startCell"></param>
        /// <param name="corner"></param>
        /// <returns></returns>
        private bool CheckCorner(Cell startCell, Cell corner)
        {
            int x = startCell.gridX - corner.gridX;
            int y = startCell.gridY - corner.gridY;

            if (x == -1 && y == -1)
            {
                Cell cell1 = grid.Find(Cell => Cell.gridX == corner.gridX && Cell.gridY == corner.gridY - 1);
                Cell cell2 = grid.Find(Cell => Cell.gridX == corner.gridX - 1 && Cell.gridY == corner.gridY);
                if (!cell1.walkable && cell2.walkable)
                {
                    return false;
                }
            }
            else if (x == -1 && y == 1)
            {
                Cell cell1 = grid.Find(Cell => Cell.gridX == corner.gridX && Cell.gridY == corner.gridY + 1);
                Cell cell2 = grid.Find(Cell => Cell.gridX == corner.gridX - 1 && Cell.gridY == corner.gridY);
                if (!cell1.walkable && cell2.walkable)
                {
                    return false;
                }
            }
            else if (x == 1 && y == -1)
            {
                Cell cell1 = grid.Find(Cell => Cell.gridX == corner.gridX && Cell.gridY == corner.gridY - 1);
                Cell cell2 = grid.Find(Cell => Cell.gridX == corner.gridX + 1 && Cell.gridY == corner.gridY);
                if (!cell1.walkable && cell2.walkable)
                {
                    return false;
                }
            }
            else if (x == 1 && y == 1)
            {
                Cell cell1 = grid.Find(Cell => Cell.gridX == corner.gridX && Cell.gridY == corner.gridY + 1);
                Cell cell2 = grid.Find(Cell => Cell.gridX == corner.gridX + 1 && Cell.gridY == corner.gridY);
                if (!cell1.walkable && cell2.walkable)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Greate border around all cells
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="rectangle"></param>
        /// <param name="color"></param>
        /// <param name="lineWidth"></param>
        public void DrawRectangle(SpriteBatch spriteBatch, Rectangle rectangle, Color color, int lineWidth)
        {
            KeyboardState keyState = Keyboard.GetState();

            //Checks if whitePixel texture is null, if is null creates a texture
            if (whitePixel == null)
            {
                whitePixel = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
                whitePixel.SetData<Color>(new Color[] { Color.White });
            }

            //Draw a line for all sides of the cell
            if (keyState.IsKeyDown(Keys.Space))
            {
                spriteBatch.Draw(whitePixel, new Rectangle(rectangle.X, rectangle.Y, lineWidth, rectangle.Height + lineWidth), color);
                spriteBatch.Draw(whitePixel, new Rectangle(rectangle.X, rectangle.Y, rectangle.Width + lineWidth, lineWidth), color);
                spriteBatch.Draw(whitePixel, new Rectangle(rectangle.X + rectangle.Width, rectangle.Y, lineWidth, rectangle.Height + lineWidth), color);
                spriteBatch.Draw(whitePixel, new Rectangle(rectangle.X, rectangle.Y + rectangle.Height, rectangle.Width + lineWidth, lineWidth), color);
            }

            if (keyState.IsKeyDown(Keys.Enter))
            {
                ResetGame();
            }
        }


        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public void Update(GameTime gameTime)
        {
            if (Game1.regularKeyPosition == wizard.gridCellPos && myState == State.FINDREGULARTOWERKEY)
            {
                Game1.ObjectToRemove.Add(Game1.ObjectList.Find(x => x.Id == "0"));
                myState = State.DELIEVERREGULARTOWERKEY;
            }
            if (Game1.regularTowerPosition == wizard.gridCellPos && myState == State.DELIEVERREGULARTOWERKEY)
            {
                myState = State.FINDICETOWERKEY;
            }
            if (Game1.iceKeyPosition == wizard.gridCellPos && myState == State.FINDICETOWERKEY)
            {
                Game1.ObjectToRemove.Add(Game1.ObjectList.Find(x => x.Id == "1"));
                myState = State.DELIEVERICETOWERKEY;
            }
            if (Game1.iceTowerPosition == wizard.gridCellPos && myState == State.DELIEVERICETOWERKEY)
            {
                myState = State.GOTOPORTAL;
            }
            if (wizard.gridCellPos == new Point(4, 13) || wizard.gridCellPos == new Point(7, 13))
            {
                Wizard.passedForest = true;
            }
        }

        /// <summary>
        /// Use to add grass to the world, method GenerateWorld() handles the rest
        /// </summary>
        /// <param name="spriteBatch"></param>
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            foreach (Cell item in grid)
            {
                spriteBatch.Draw(item.cellTexture, new Vector2(item.gridX * item.CellSize, item.gridY * item.CellSize), Color.White);
                DrawRectangle(spriteBatch, item.BoundingRectangle, Color.White, 1);
            }
        }

        /// <summary>
        /// Creates the world
        /// </summary>
        private void GenerateWorld()
        {

            //Int's for adding to gridX or gridY
            #region Path Rows
            int row1 = 0;
            int row2 = 0;
            int row3 = 0;
            int row4 = 0;
            int row5 = 0;
            int row6 = 0;
            int row7 = 0;
            int row8 = 0;
            int row9 = 0;
            #endregion
            #region Tower
            int tower1 = 0;
            int tower2 = 0;
            #endregion 
            #region Tree Rows
            int treeRow1 = 0;
            int treeRow2 = 0;
            #endregion
            #region Keys

            #endregion
            #region Water Rows
            int waterRow1 = 0;
            int waterRow2 = 0;
            int waterRow3 = 0;
            int waterRow4 = 0;
            int waterRow5 = 0;
            int waterRow6 = 0;
            int waterRow7 = 0;
            int waterRow8 = 0;
            #endregion

            //Checks all cell's in game and adds path, water, tress ect.
            foreach (Cell item in grid)
            {

                #region Path Rows
                //row1
                if (item == grid.Find(x => x.gridX == 0 + row1 && x.gridY == 14 && row1 < 4))
                {
                    item.cellTexture = PreLoad.Ground;
                    row1++;
                }
                //row2
                if (item == grid.Find(x => x.gridX == 3 && x.gridY == 8 + row2 && row2 < 6))
                {
                    item.cellTexture = PreLoad.Ground;
                    row2++;
                }
                //row3
                if (item == grid.Find(x => x.gridX == 3 + row3 && x.gridY == 8 && row3 < 2))
                {
                    item.cellTexture = PreLoad.Ground;
                    row3++;
                }
                //row4
                if (item == grid.Find(x => x.gridX == 4 && x.gridY == 3 + row4 && row4 < 6))
                {
                    item.cellTexture = PreLoad.Ground;
                    row4++;
                }
                //row5
                if (item == grid.Find(x => x.gridX == 5 + row5 && x.gridY == 3 && row5 < 7))
                {
                    item.cellTexture = PreLoad.Ground;
                    row5++;
                }
                //row6
                if (item == grid.Find(x => x.gridX == 11 && x.gridY == 3 + row6 && row6 < 9))
                {
                    item.cellTexture = PreLoad.Ground;
                    row6++;
                }
                //row7
                if (item == grid.Find(x => x.gridX == 11 + row7 && x.gridY == 11 && row7 < 1))
                {
                    item.cellTexture = PreLoad.Ground;
                    row7++;
                }
                //row8
                if (item == grid.Find(x => x.gridX == 12 && x.gridY == 11 + row8 && row8 < 3))
                {
                    item.cellTexture = PreLoad.Ground;
                    row8++;
                }
                //row9
                if (item == grid.Find(x => x.gridX == 4 + row9 && x.gridY == 13 && row9 < 9))
                {
                    item.cellTexture = PreLoad.Ground;
                    row9++;
                }
                #endregion

                #region Tree Rows
                //treerow1
                if (item == grid.Find(x => x.gridX == 4 + treeRow1 && x.gridY == 14 && treeRow1 < 8))
                {
                    Game1.ObjectToAdd.Add(new Tree((new Point(item.Position.X * item.CellSize + item.CellSize / 2, item.Position.Y * item.CellSize + item.CellSize / 4))));
                    treeRow1++;
                    item.myType = CellType.WALL;
                }
                //treerow2
                if (item == grid.Find(x => x.gridX == 4 + treeRow2 && x.gridY == 12 && treeRow2 < 8))
                {
                    Game1.ObjectToAdd.Add(new Tree((new Point(item.Position.X * item.CellSize + item.CellSize / 2, item.Position.Y * item.CellSize + item.CellSize / 4))));
                    treeRow2++;
                    item.myType = CellType.WALL;
                }
                #endregion

                #region Towers
                // Regular tower
                if (item == grid.Find(x => x.gridX == 3 + tower1 && x.gridY == 6 && tower1 < 1))
                {
                    Game1.ObjectToAdd.Add(new Tower((new Point(item.Position.X * item.CellSize + item.CellSize / 4, item.Position.Y * item.CellSize + item.CellSize / 2)), true, new Point(item.gridX, item.gridY)));
                    tower1++;
                    Game1.regularTowerPosition = new Point(item.gridX, item.gridY);
                    item.myType = CellType.GOAL;
                }

                // Ice tower
                if (item == grid.Find(x => x.gridX == 12 + tower2 && x.gridY == 10 && tower2 < 1))
                {
                    Game1.ObjectToAdd.Add(new Tower((new Point(item.Position.X * item.CellSize + item.CellSize / 4, item.Position.Y * item.CellSize - item.CellSize / 2)), false, new Point(item.gridX, item.gridY)));
                    tower2++;
                    Game1.iceTowerPosition = new Point(item.gridX, item.gridY);
                    item.myType = CellType.GOAL;
                }
                #endregion

                #region water
                //Waterrow1
                if (item == grid.Find(x => x.gridX == 4 + waterRow1 && x.gridY == 11 && waterRow1 < 7))
                {
                    item.cellTexture = PreLoad.River;
                    waterRow1++;
                    item.myType = CellType.WALL;
                }
                //Waterrow2
                if (item == grid.Find(x => x.gridX == 4 + waterRow2 && x.gridY == 10 && waterRow2 < 7))
                {
                    item.cellTexture = PreLoad.River;
                    waterRow2++;
                    item.myType = CellType.WALL;
                }
                //Waterrow3
                if (item == grid.Find(x => x.gridX == 4 + waterRow3 && x.gridY == 9 && waterRow3 < 7))
                {
                    item.cellTexture = PreLoad.River;
                    waterRow3++;
                    item.myType = CellType.WALL;
                }
                //Waterrow4
                if (item == grid.Find(x => x.gridX == 5 + waterRow4 && x.gridY == 8 && waterRow4 < 6))
                {
                    item.cellTexture = PreLoad.River;
                    waterRow4++;
                    item.myType = CellType.WALL;
                }
                //Waterrow5
                if (item == grid.Find(x => x.gridX == 5 + waterRow4 && x.gridY == 7 && waterRow5 < 6))
                {
                    item.cellTexture = PreLoad.River;
                    waterRow5++;
                    item.myType = CellType.WALL;
                }
                //Waterrow6
                if (item == grid.Find(x => x.gridX == 5 + waterRow6 && x.gridY == 6 && waterRow6 < 6))
                {
                    item.cellTexture = PreLoad.River;
                    waterRow6++;
                    item.myType = CellType.WALL;
                }
                //Waterrow7
                if (item == grid.Find(x => x.gridX == 5 + waterRow7 && x.gridY == 5 && waterRow7 < 6))
                {
                    item.cellTexture = PreLoad.River;
                    waterRow7++;
                    item.myType = CellType.WALL;
                }

                //Waterrow8
                if (item == grid.Find(x => x.gridX == 5 + waterRow8 && x.gridY == 4 && waterRow8 < 6))
                {
                    item.cellTexture = PreLoad.River;
                    waterRow8++;
                    item.myType = CellType.WALL;
                }
                #endregion
            }
            
            GenerateKeys();
        }

        /// <summary>
        /// Method that generates the two keys and their position.
        /// </summary>
        public void GenerateKeys()
        {
            Random rndKey1 = new Random();
            Random rndKey2 = new Random();

            foreach (Cell item in grid)
            {
                if (item.myType != CellType.WALL)
                {
                    placeableKeyPoints.Add(item);
                }
            }
            // Key placing
            Random key1rnd = new Random();
            int key1RndNumber = key1rnd.Next(placeableKeyPoints.Count);
            Cell KeyCell1 = null;

            foreach (Cell item in grid)
            {
                if (placeableKeyPoints[key1RndNumber] == item)
                {
                    placeableKeyPoints.RemoveAt(key1RndNumber);
                    KeyCell1 = item;
                    break;
                }
            }

            Random key2rnd = new Random();
            int key2RndNumber = key1rnd.Next(placeableKeyPoints.Count);
            while ((placeableKeyPoints[key2RndNumber].gridX >= 4 && placeableKeyPoints[key2RndNumber].gridX <= 11 && placeableKeyPoints[key2RndNumber].gridY == 13)) //2nd key not to spawn in woods
            {
                key2RndNumber = key1rnd.Next(placeableKeyPoints.Count);
            }

            Cell KeyCell2 = null;
            foreach (Cell item in grid)
            {
                if (placeableKeyPoints[key2RndNumber] == item)
                {
                    placeableKeyPoints.RemoveAt(key2RndNumber);
                    KeyCell2 = item;
                    break;
                }
            }

            //Regular Key
            regularKey = new Key((new Point(KeyCell1.Position.X * KeyCell1.CellSize + KeyCell1.CellSize / 2, KeyCell1.Position.Y * KeyCell1.CellSize + KeyCell1.CellSize / 2)), true, new Point(KeyCell1.gridX, KeyCell1.gridY), "0");
            Game1.ObjectToAdd.Add(regularKey);
            Game1.regularKeyPosition = new Point(KeyCell1.gridX, KeyCell1.gridY);
            KeyCell1.myType = CellType.GOAL;

            // Ice tower key
            magicKey = new Key((new Point(KeyCell2.Position.X * KeyCell2.CellSize + KeyCell2.CellSize / 2, KeyCell2.Position.Y * KeyCell2.CellSize + KeyCell2.CellSize / 2)), false, new Point(KeyCell2.gridX, KeyCell2.gridY), "1");
            Game1.ObjectToAdd.Add(magicKey);
            Game1.iceKeyPosition = new Point(KeyCell2.gridX, KeyCell2.gridY);
            KeyCell2.myType = CellType.GOAL;
        }

        /// <summary>
        /// Method used to reset the game.
        /// </summary>
        public void ResetGame()
        {
            Game1.ObjectToRemove.Add(wizard); // Removes the wizard object.
            Game1.ObjectToRemove.Add(regularKey); // Removes the key for the regular tower
            Game1.ObjectToRemove.Add(magicKey); // Removes the key for the ice tower.

            if (wizard.monsterPlaced) // Checks if the monster in the forest is spawned.
            {
                wizard.monsterPlaced = false;
                Game1.ObjectToRemove.Add(monster); // Removes the monster object.
            }
            Game1.ObjectList.TrimExcess();
            Game1.ObjectToAdd.TrimExcess();
            Game1.ObjectToRemove.TrimExcess();
            wizard = new Wizard(new Point(wizardCell.Position.X * cellSize + cellSize / 2, wizardCell.Position.Y * cellSize + cellSize / 2), new Point(1, 14)); // Adds a new wizard.
            Game1.ObjectToAdd.Add(wizard); // A wizard object is added to the objectToAdd list.
            GenerateKeys(); // Generates two new keys in random cells.
            myState = State.FINDREGULARTOWERKEY; // Changes the state back to the first initiated state.
            CheckPath(); // Checks if there's a path to follow.
        }
    }
}