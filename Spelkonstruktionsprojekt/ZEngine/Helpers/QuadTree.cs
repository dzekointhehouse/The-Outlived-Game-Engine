using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Microsoft.Xna.Framework;
using Spelkonstruktionsprojekt.ZEngine.Managers;
using ZEngine.Components;

namespace ZEngine.Helpers
{
    public class QuadTree
    {
        private const int MAX_ELEMENTS = 4;

        public static QuadNode CreateTree(IEnumerable<uint> entities, Rectangle bounds)
        {
            var tree = CreateNode(null, bounds);
            foreach (var entity in entities)
            {
                Insert(ref tree, entity);
            }
            return tree;
        }

        public static IEnumerable<Tuple<uint, QuadNode>> MovingEntities(QuadNode tree)
        {
            var stack = new Stack<QuadNode>();
            stack.Push(tree);
            while (stack.Count > 0)
            {
                var current = stack.Pop();
                for (var i = 0; i < current.PermanentMovingEntities.Count; i++)
                {
                    yield return new Tuple<uint, QuadNode>(current.PermanentMovingEntities[i].Item1, current);
                }
                if (current.ChildNodes != null)
                {
                    for (var i = 0; i < current.ChildNodes.Length; i++)
                    {
                        stack.Push(current.ChildNodes[i]);
                    }
                }
            }
        }

        public static IEnumerable<uint> StillEntities(uint movingEntityId, QuadNode root)
        {
            var stack = new Stack<QuadNode>();
            stack.Push(root);
            while (stack.Count > 0)
            {
                var current = stack.Pop();
                for (var i = 0; i < current.PermanentStillEntities.Count; i++)
                {
                    yield return current.PermanentStillEntities[i].Item1;
                }
                for (var i = 0; i < current.PermanentMovingEntities.Count; i++)
                {
                    if (movingEntityId == current.PermanentMovingEntities[i].Item1) continue;
                    yield return current.PermanentMovingEntities[i].Item1;
                }
                if (current.ChildNodes != null)
                {
                    for (var i = 0; i < current.ChildNodes.Length; i++)
                    {
                        stack.Push(current.ChildNodes[i]);
                    }
                }
            }
            var parent = root.Parent;
            while (parent != null)
            {
                for (var i = 0; i < parent.PermanentStillEntities.Count; i++)
                {
                    yield return parent.PermanentStillEntities[i].Item1;
                }
                for (var i = 0; i < parent.PermanentMovingEntities.Count; i++)
                {
                    yield return parent.PermanentMovingEntities[i].Item1;
                }
                parent = parent.Parent;
            }
        }

        public static void Clear(QuadNode tree)
        {
            var stack = new Stack<QuadNode>();
            stack.Push(tree);
            while (stack.Count > 0)
            {
                var current = stack.Pop();
                current.PermanentMovingEntities.Clear();
                current.PermanentStillEntities.Clear();
                current.TempMovingEntitiesCount = 0;
                current.TempStillEntitiesCount = 0;
                if (current.ChildNodes == null) ;
                for (var i = 0; i < current.ChildNodes.Length; i++)
                {
                    stack.Push(current.ChildNodes[i]);
                }
            }
        }

        public static QuadNode CreateNode(QuadNode parent, Rectangle bounds)
        {
            var node = new QuadNode
            {
                Parent = parent,
                Bounds = bounds
            };
            return node;
        }

        public static void Split(QuadNode node)
        {
            var subWidth = (int) (node.Bounds.Width * 0.5);
            var subHeight = (int) (node.Bounds.Height * 0.5);
            var topLeft = new Rectangle(node.Bounds.Left, node.Bounds.Top, subWidth, subHeight);
            var topRight = new Rectangle(node.Bounds.Right, node.Bounds.Top, subWidth, subHeight);
            var bottomLeft = new Rectangle(node.Bounds.Left, node.Bounds.Bottom, subWidth, subHeight);
            var bottomRight = new Rectangle(node.Bounds.Left, node.Bounds.Bottom, subWidth, subHeight);

            //Create four child nodes
            node.ChildNodes = new[]
            {
                CreateNode(node, topLeft),
                CreateNode(node, topRight),
                CreateNode(node, bottomLeft),
                CreateNode(node, bottomRight)
            };

            //Place moving entities into child nodes where they fit
            //Or keep them if they only fit in the current node
            for (var i = 0; i < node.TempMovingEntitiesCount; i++)
            {
                InsertHelper2(ref node, node.TempMovingEntities[i]);
                node.TempMovingEntities[i] = null;
            }
            node.TempMovingEntitiesCount = 0;

            //Place still entities into child nodes where they fit
            //Or keep them if they only fit in the current node
            for (var i = 0; i < node.TempStillEntitiesCount; i++)
            {
                InsertHelper2(ref node, node.TempStillEntities[i]);
                node.TempStillEntities[i] = null;
            }
            node.TempStillEntitiesCount = 0;
        }

        public static void Insert(ref QuadNode parent, uint entityId)
        {
            var positionComponent = ComponentManager.Instance.GetEntityComponentOrDefault<PositionComponent>(entityId);
            var dimensionsComponent =
                ComponentManager.Instance.GetEntityComponentOrDefault<DimensionsComponent>(entityId);
            InsertHelper(ref parent,
                new Tuple<uint, Rectangle>(
                    entityId,
                    new Rectangle(
                        (int) positionComponent.Position.X,
                        (int) positionComponent.Position.Y,
                        dimensionsComponent.Width,
                        dimensionsComponent.Height)));
        }

        public static void InsertHelper(ref QuadNode node, Tuple<uint, Rectangle> entity)
        {
            //Return if the node does not fit in this quad node
            if (!node.Bounds.Contains(entity.Item2)) return;

            //Fits no more entites, try to split entities into smaller quad nodes
            if (node.TempMovingEntitiesCount >= MAX_ELEMENTS || node.TempStillEntitiesCount >= MAX_ELEMENTS)
            {
                Split(node);
            }

            //Has no child nodes and fits more entities
            if (node.ChildNodes == null)
            {
                if (ComponentManager.Instance.EntityHasComponent<MoveComponent>(entity.Item1))
                {
                    node.TempMovingEntities[node.TempMovingEntitiesCount++] = entity;
                }
                else
                {
                    if (node.PermanentStillEntities.Count > 1000)
                    {
                        Debug.WriteLine("");
                    }
                    node.TempStillEntities[node.TempStillEntitiesCount++] = entity;
                }
            }
            else
            {
                InsertHelper2(ref node, entity);
            }
        }

        public static void InsertHelper2(ref QuadNode parent, Tuple<uint, Rectangle> entity)
        {
            //If node has children and if the entity fits in any child, put it in that child node
            for (var childNodeIndex = 0; childNodeIndex < parent.ChildNodes.Length; childNodeIndex++)
            {
                if (parent.ChildNodes[childNodeIndex].Bounds.Contains(entity.Item2))
                {
                    InsertHelper(ref parent.ChildNodes[childNodeIndex], entity);
                    return;
                }
            }

            //If entity too large too fit in any child node
            if (ComponentManager.Instance.EntityHasComponent<MoveComponent>(entity.Item1))
            {
                parent.PermanentMovingEntities.Add(entity);
            }
            else
            {
                if (parent.PermanentStillEntities.Count > 1000)
                {
                    Debug.WriteLine("");
                }
                parent.PermanentStillEntities.Add(entity);
            }
        }
    }

    public class QuadNode
    {
        public QuadNode Parent;
        public List<Tuple<uint, Rectangle>> PermanentMovingEntities = new List<Tuple<uint, Rectangle>>();
        public List<Tuple<uint, Rectangle>> PermanentStillEntities = new List<Tuple<uint, Rectangle>>();
        public Tuple<uint, Rectangle>[] TempStillEntities = new Tuple<uint, Rectangle>[4];
        public int TempStillEntitiesCount = 0;
        public Tuple<uint, Rectangle>[] TempMovingEntities = new Tuple<uint, Rectangle>[4];
        public int TempMovingEntitiesCount = 0;
        public Rectangle Bounds;
        public QuadNode[] ChildNodes;
    }
}