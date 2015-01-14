using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using RpgGame.SpriteClasses;

namespace RpgGame.NpcClasses.Actions.Movement
{
    public enum InitialDirection { Horizontal, Vertical }
    /// <summary>
    /// Class facilitates the creation of a paths
    /// </summary>
    public static class PathProducer
    {
        // GetSimplePath takes two positions, origin and destination, and returns a path made of 2 waypoints
        // Output will traverse the shorter side of the path first, then the longer side
        public static Queue<Step> GetSimplePath(Vector2 origin, Vector2 destination)
        {
            Queue<Step> path = new Queue<Step>();

            float x = origin.X - destination.X;
            float y = origin.Y - destination.Y;
            
            Direction xDirection = (x >= 0) ? Direction.Left : Direction.Right;
            Direction yDirection = (y >= 0) ? Direction.Up : Direction.Down;

            if (Math.Abs(x) >= Math.Abs(y))
            {
                path.Enqueue(new Step((int)Math.Floor(y), yDirection));
                path.Enqueue(new Step((int)Math.Floor(x), xDirection));
            }
            else
            {
                path.Enqueue(new Step((int)Math.Floor(x), xDirection));
                path.Enqueue(new Step((int)Math.Floor(y), yDirection));
            }

            return path;
        }

        // OVerloaded version of GetSimplePath gives output that will traverse the side first as chosen by direction parameter
        public static Queue<Step> GetSimplePath(Vector2 origin, Vector2 destination, InitialDirection direction)
        {
            Queue<Step> path = new Queue<Step>();

            float x = origin.X - destination.X;
            float y = origin.Y - destination.Y;

            Direction xDirection = (x >= 0) ? Direction.Left : Direction.Right;
            Direction yDirection = (y >= 0) ? Direction.Up : Direction.Down;

            if (direction == InitialDirection.Vertical)
            {
                path.Enqueue(new Step((int)Math.Floor(y), yDirection));
                path.Enqueue(new Step((int)Math.Floor(x), xDirection));
            }
            else
            {
                path.Enqueue(new Step((int)Math.Floor(x), xDirection));
                path.Enqueue(new Step((int)Math.Floor(y), yDirection));
            }

            return path;
        }

        // Generalises GetSimplePath into a method that can handle more than one "destination" - i.e. waypoints.
        // An NPC who follows the returned path will travel to each waypoint
        public static Queue<Step> GetPathByWaypoint(Queue<Vector2> waypoints, Queue<InitialDirection> directions)
        {
            if (waypoints.Count != directions.Count + 1)
                throw new ArgumentException("Size of directions queue must be exactly 1 smaller than size of waypoints queue");

            Queue<Step> path = new Queue<Step>();
            Vector2 currentWaypoint = waypoints.Dequeue();
            Vector2 nextWaypoint;
            InitialDirection direction;

            // Iterate through every waypoint in the queue and add two StepwiseMovement commands as appropriate depending on initalDirection
            for (int i = 0; i <= waypoints.Count; i++)
            {
                nextWaypoint = waypoints.Dequeue();
                direction = directions.Dequeue();

                float x = currentWaypoint.X - nextWaypoint.X;
                float y = currentWaypoint.Y - nextWaypoint.Y;

                Direction xDirection = (x >= 0) ? Direction.Left : Direction.Right;
                Direction yDirection = (y >= 0) ? Direction.Up : Direction.Down;

                if (direction == InitialDirection.Vertical)
                {
                    path.Enqueue(new Step((int)Math.Floor(y), yDirection));
                    path.Enqueue(new Step((int)Math.Floor(x), xDirection));
                }
                else
                {
                    path.Enqueue(new Step((int)Math.Floor(x), xDirection));
                    path.Enqueue(new Step((int)Math.Floor(y), yDirection));
                }

                currentWaypoint = nextWaypoint;
            }

            return path;
        }

    }
}
