using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheLittleWizard
{
    class Pathfinding
    {
        /// <summary>
        /// G, H and F cost is received from the cell. 
        /// </summary>
        //public int gCost;
        //public int hCost;
        //public int fCost;
        //public GridManager grid;

        /// <summary>
        /// The AStar algorithm.
        /// 
        /// </summary>
        /// <param name="cellStart">Cell to start.</param>
        /// <param name="cellGoal">Cell to end at.</param>
        /// <param name="grid"></param>
        /// <returns></returns>
        public List<Cell> AStar(Cell cellStart, Cell cellGoal, GridManager grid)
        {
            List<Cell> closedSet = new List<Cell>();
            List<Cell> openSet = new List<Cell>();
            openSet.Add(cellStart);

            while (openSet.Count > 0)
            {
                Cell currentCell = openSet[0];

                for (int i = 1; i < openSet.Count; i++)
                {
                    if (openSet[i].fCost < currentCell.fCost || openSet[i].fCost == currentCell.fCost && openSet[i].hCost < currentCell.hCost)
                    {
                        currentCell = openSet[i];
                    }
                }
                openSet.Remove(currentCell);
                closedSet.Add(currentCell);

                if (currentCell == cellGoal)
                {
                    return RetracePath(cellStart, cellGoal);
                }

                List<Cell> myNeighbors = new List<Cell>(grid.GetNeighbors(currentCell));

                // Makes sure that the character can't move inbetween a diagonal wall.
                // It finds a vertical or horizontal way instead.
                foreach (Cell neighbor in myNeighbors)
                {
                    int newMovementCostToNeighbor = 0;
                    Point tempNeighborPosition;

                    // Left Top cell check.
                    if (currentCell.Position.X - 1 == neighbor.Position.X && currentCell.Position.Y - 1 == neighbor.Position.Y) 
                    {
                        newMovementCostToNeighbor = currentCell.gCost + 14; 
                        tempNeighborPosition = neighbor.Position; 
                        tempNeighborPosition.X += 1; 
                        CellType tempType = myNeighbors.Find(c => c.Position == tempNeighborPosition).myType; 

                        if (tempType == CellType.WALL)
                        {
                            continue;
                        }

                        tempNeighborPosition = neighbor.Position; 
                        tempNeighborPosition.Y += 1; 
                        tempType = myNeighbors.Find(c => c.Position == tempNeighborPosition).myType; 

                        if (tempType == CellType.WALL)
                        {
                            continue;
                        }
                    }

                    // Left Bottom cell check.
                    else if (currentCell.Position.X - 1 == neighbor.Position.X && currentCell.Position.Y + 1 == neighbor.Position.Y)
                    {
                        newMovementCostToNeighbor = currentCell.gCost + 14;
                        tempNeighborPosition = neighbor.Position;
                        tempNeighborPosition.X += 1;
                        CellType tempType = myNeighbors.Find(c => c.Position == tempNeighborPosition).myType;

                        if (tempType == CellType.WALL)
                        {
                            continue;
                        }

                        tempNeighborPosition = neighbor.Position;
                        tempNeighborPosition.Y -= 1;
                        tempType = myNeighbors.Find(c => c.Position == tempNeighborPosition).myType;

                        if (tempType == CellType.WALL)
                        {
                            continue;
                        }
                    }

                    // Right Top cell check.
                    else if (currentCell.Position.X + 1 == neighbor.Position.X && currentCell.Position.Y - 1 == neighbor.Position.Y)
                    {
                        newMovementCostToNeighbor = currentCell.gCost + 14;
                        tempNeighborPosition = neighbor.Position;
                        tempNeighborPosition.X -= 1;
                        CellType tempType = myNeighbors.Find(c => c.Position == tempNeighborPosition).myType;

                        if (tempType == CellType.WALL)
                        {
                            continue;
                        }

                        tempNeighborPosition = neighbor.Position;
                        tempNeighborPosition.Y += 1;
                        tempType = myNeighbors.Find(c => c.Position == tempNeighborPosition).myType;

                        if (tempType == CellType.WALL)
                        {
                            continue;
                        }
                    }

                    // Right Bottom cell check.
                    else if (currentCell.Position.X + 1 == neighbor.Position.X && currentCell.Position.Y + 1 == neighbor.Position.Y)
                    {
                        newMovementCostToNeighbor = currentCell.gCost + 14;
                        tempNeighborPosition = neighbor.Position;
                        tempNeighborPosition.X -= 1;
                        CellType tempType = myNeighbors.Find(c => c.Position == tempNeighborPosition).myType;

                        if (tempType == CellType.WALL)
                        {
                            continue;
                        }

                        tempNeighborPosition = neighbor.Position;
                        tempNeighborPosition.Y -= 1;
                        tempType = myNeighbors.Find(c => c.Position == tempNeighborPosition).myType;

                        if (tempType == CellType.WALL)
                        {
                            continue;
                        }
                    }

                    // Straight
                    else
                    {
                        newMovementCostToNeighbor = currentCell.gCost + 10;
                    }

                    if (neighbor.myType == CellType.WALL || closedSet.Contains(neighbor))
                    {
                        continue;
                    }

                    neighbor.pathPart = PathParticipation.NEIGHBOR;

                    if (newMovementCostToNeighbor < neighbor.gCost || !openSet.Contains(neighbor))
                    {
                        if (neighbor.parent == null)
                        {
                            neighbor.gCost = newMovementCostToNeighbor;
                            neighbor.hCost = GetDistance(neighbor, cellGoal);
                            neighbor.parent = currentCell;
                        }

                        if (!openSet.Contains(neighbor) && !closedSet.Contains(neighbor))
                        {
                            openSet.Add(neighbor);
                        }
                    }
                }
            }
            //If everything goes wrong...
            List<Cell> failReturn = new List<Cell>();
            failReturn.Add(cellStart);
            return failReturn;
        }

        /// <summary>
        /// Used to backtrack from the destination cell to the start cell.
        /// </summary>
        /// <param name="cellStart">Start cell</param>
        /// <param name="cellGoal">Destination cell</param>
        /// <returns></returns>
        public List<Cell> RetracePath(Cell cellStart, Cell cellGoal)
        {
            List<Cell> path = new List<Cell>();
            Cell currentCell = cellGoal;

            while (currentCell != cellStart)
            {
                path.Add(currentCell);
                currentCell.pathPart = PathParticipation.PATH;
                currentCell = currentCell.parent;
            }
            cellStart.pathPart = PathParticipation.PATH;
            path.Add(cellStart);
            path.Reverse();

            foreach (Cell item in path)
            {
                item.isPath = true;
            }
            return path;
        }

        /// <summary>
        /// Used to calculate the h cost between a neighbor and the cell we wish to get to.
        /// </summary>
        /// <param name="cellA">Neighbor cell</param>
        /// <param name="cellB">Destination cell</param>
        /// <returns></returns>
        public int GetDistance(Cell cellA, Cell cellB)
        {
            int dstX = Math.Abs(cellA.gridX - cellB.gridX);
            int dstY = Math.Abs(cellA.gridY - cellB.gridY);

            if (dstX > dstY)
            {
                return 20 * dstY + 10 * (dstX - dstY);
            }
            return 20 * dstX + 10 * (dstY - dstX);
        }
    }
}
