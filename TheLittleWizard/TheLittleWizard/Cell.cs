using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TheLittleWizard.CellType;

namespace TheLittleWizard
{
    class Cell
    {
        public bool walkable;
        /// <summary>
        /// Variables for the g and h cost.
        /// </summary>
        public int gCost;
        public int hCost;

        /// <summary>
        /// The X and Y coordinates for a cell.
        /// </summary>
        public int gridX;
        public int gridY;
        public Cell parent;
        public bool isPath = false;
        GraphicsDevice graphics;
        static Texture2D whitePixel;
        /// <summary>
        /// Used to load Grass texture.
        /// 
        /// </summary>
        public Texture2D cellTexture;

        /// <summary>
        /// Used to load Ground texture.
        /// </summary>
        public Texture2D cellTextureGround;

        /// <summary>
        /// The grid position of the cell
        /// </summary>
        private Point position;

        /// <summary>
        /// The size of the cell
        /// </summary>
        private int cellSize;

        /// <summary>
        /// The cell's sprite
        /// </summary>
        private Texture2D image;

        /// <summary>
        /// Sets the celltype to empty as default
        /// </summary>
        public CellType myType = EMPTY;

        public PathParticipation pathPart = PathParticipation.NOT;

        public int fCost { get { return gCost + hCost; } }

        public int CellSize
        {
            get
            {
                return cellSize;
            }
        }

        public Point Position
        {
            get
            {
                return position;
            }

            set
            {
                position = value;
            }
        }

        /// <summary>
        /// The bounding rectangle of the cell
        /// </summary>
        public Rectangle BoundingRectangle
        {
            get
            {
                return new Rectangle(Position.X * CellSize, Position.Y * CellSize, CellSize, CellSize);
            }
        }

        /// <summary>
        /// The cell's constructor
        /// </summary>
        /// <param name="position">The cell's grid position</param>
        /// <param name="size">The cell's size</param>
        public Cell(Point position, int size, int gridX, int gridY)
        {
            this.walkable = true;

            //Sets the position
            this.Position = position;

            //Sets the cell size
            this.cellSize = size;

            this.gridX = gridX;
            this.gridY = gridY;

            cellTextureGround = PreLoad.Ground;
            cellTexture = PreLoad.Grass;
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        public virtual void LoadContent()
        {
            whitePixel = new Texture2D(graphics, 1, 1);
            whitePixel.SetData(new[] { Color.White });
        }

        /// <summary>
        /// Renders the cell
        /// </summary>
        /// <param name="dc">The graphic context</param>
        public void Render(GraphicsDeviceManager dc)
        {
            Texture2D cellRect = new Texture2D(graphics, 32, 32);

            Color[] data = new Color[32 * 32];

            for (int i = 0; i < data.Length; i++)
            {
                data[i] = Color.Black;
            }
            cellRect.SetData(data);

            Vector2 coor = new Vector2(32, 32);
        }
    }
}
