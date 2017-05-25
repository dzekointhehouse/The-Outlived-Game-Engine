using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Spelkonstruktionsprojekt
{
    public class Node
    {
        // X coordinate
        // Y coordinate
        // F is the total cost
        // G is the cost to make a move to a node
        // H is the estimated heuristic cost.
        public int X;
        public int Y;
        public int F = 0;
        public int G;
        public int H;
        public Node Parent;

        public Node(int x, int y)
        {
            X = x;
            Y = y;
        }
    }

    class Astar
    {
        static int ComputeHScore(int x, int y, int targetX, int targetY)
        {
            return Math.Abs(targetX - x) + Math.Abs(targetY - y);
        }

        static List<Node> GetWalkableAdjacentSquares(int x, int y, int[,] map)
        {
            List<Node> Walkables = new List<Node>(4);
            var proposedLocations = new List<Node>()
            {
                new Node (x, y - 1),
                new Node (x, y + 1),
                new Node (x - 1, y),
                new Node (x + 1, y),
            };

            if (x <= 28 && y >= 28)
            {
                if (map[proposedLocations[0].Y, proposedLocations[0].X] == 28)
                    Walkables.Add(proposedLocations[0]);
                if (map[proposedLocations[1].Y, proposedLocations[1].X] == 28)
                    Walkables.Add(proposedLocations[1]);
                if (map[proposedLocations[2].Y, proposedLocations[2].X] == 28)
                    Walkables.Add(proposedLocations[2]);
                if (map[proposedLocations[3].Y, proposedLocations[3].X] == 28)
                    Walkables.Add(proposedLocations[3]);
            }
            return proposedLocations;
            // Returning the list with available adjacent moves
            //return proposedLocations.Where(
            //    node => map[node.Y, node.X] == 4).ToList();
        }

        public static Node Search(int[,] map, Node start, Node goal)
        {
            Node current = null;
            var openList = new List<Node>();
            var closedList = new List<Node>();
            int g = 0;


            // start by adding the original position to the open list
            openList.Add(start);

            while (openList.Count > 0)
            {
                // algorithm's logic goes here

                var lowest = openList.Min(node => node.F);
                current = openList.First(node => node.F == lowest);

                // add the current square to the closed list
                closedList.Add(current);

                // remove it from the open list
                openList.Remove(current);

                // if we added the destination to the closed list, we've found a path
                if (closedList.FirstOrDefault(node => node.X == goal.X && node.Y == goal.Y) != null)
                    break;

                var adjacentSquares = GetWalkableAdjacentSquares(current.X, current.Y, map);
                g++;

                foreach (var adjacentSquare in adjacentSquares)
                {
                    // if this adjacent square is already in the closed list, ignore it
                    if (closedList.FirstOrDefault(node => node.X == adjacentSquare.X
                                                          && node.Y == adjacentSquare.Y) != null)
                        continue;

                    // if it's not in the open list...
                    if (openList.FirstOrDefault(l => l.X == adjacentSquare.X
                                                     && l.Y == adjacentSquare.Y) == null)
                    {
                        // compute its score, set the parent
                        adjacentSquare.G = g;
                        adjacentSquare.H = ComputeHScore(adjacentSquare.X,
                            adjacentSquare.Y, goal.X, goal.Y);
                        adjacentSquare.F = adjacentSquare.G + adjacentSquare.H;
                        adjacentSquare.Parent = current;

                        // and add it to the open list
                        openList.Insert(0, adjacentSquare);
                    }
                    else
                    {
                        // test if using the current G score makes the adjacent square's F score
                        // lower, if yes update the parent because it means it's a better path
                        if (g + adjacentSquare.H < adjacentSquare.F)
                        {
                            adjacentSquare.G = g;
                            adjacentSquare.F = adjacentSquare.G + adjacentSquare.H;
                            adjacentSquare.Parent = current;
                        }
                    }
                }
            }
            return closedList[0];
        }
    }
}
