using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RpgLibrary.SpriteClasses;
using RpgLibrary.TileEngine;
using RpgLibrary.Geometry;
using RpgLibrary.NPCs.Actions.Movement;
using RpgLibrary.NPCs.Actions.Speech;
using RpgLibrary.StateMachine;

namespace RpgLibrary.NPCs
{
    public enum NpcType { Shopkeeper, Customer, Tig }

    public class NPC : AnimatedSprite
    {
        #region Standard Movement Field Region

        // Moving flag
        protected bool moving;
        // Current target destination 
        protected Vector2 destination;
        // Current path being follows
        protected Queue<StepwiseMovement> path;
        // Current step in the queue in progress
        protected int currentStep;
        // Flag to prevent new commands being received by NPC if it is already in action
        protected bool pathComplete;
        // Marker for use in collision prevention with player
        protected Vector2 CollisionMarker;
        
        #endregion

        #region Speech Field Region

        // Speech Info
        protected SpeechBubble speech;
        
        #endregion

        #region Property Region

        public bool PathComplete
        {
            get { return pathComplete; }
        }

        public bool Moving
        {
            get { return moving; }
        }

        public bool Speaking
        {
            get { return (speech != null) ? speech.IsActive : false; }
        }

        public int CurrentStep
        {
            get { return currentStep; }
        }

        #endregion

        #region Constructor

        public NPC(Texture2D sprite, Dictionary<Direction, Animation> animation)
            : base(sprite, animation)
        {
            moving = false;
            pathComplete = true;
            path = new Queue<StepwiseMovement>();
        }

        #endregion

        #region XNA Methods Region

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // Update speech bubbles
            if (speech != null)
                speech.Update(gameTime);

            // Failsafe to stop animation if path is empty
            if (path.Count == 0)
                IsAnimating = false;
        }


        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, Camera camera)
        {
            base.Draw(gameTime, spriteBatch, camera);

            // Draw speech bubbles
            if (speech != null)
                speech.Draw(gameTime, spriteBatch, this);
        }

        #endregion

        #region Method Region

        // Allows States to send pathing information to the NPC
        public void SendPath(Queue<StepwiseMovement> path)
        {
            this.path = new Queue<StepwiseMovement>(path);
            pathComplete = false;
            // Set currentStep to 0
            currentStep = 0;
        }

        // The following method allows the NPC to follow a path in the form of a discrete set of stepwise movements
        public void StepwiseMovement()
        {
            // Iterate through the queue
            if (path.Count != 0)
            {
                StepwiseMovement step = path.Peek();

                // If moving flag is false (i.e. step not currently in progress) initialise as follows
                if (!moving)
                {
                    // Set flag to true
                    moving = true;
                    // Start animating NPC
                    IsAnimating = true;

                    // Fix intended destination
                    destination = Position;
                    switch (step.Direction)
                    {
                        case Direction.Right:
                            destination.X = Position.X + step.Distance;
                            break;
                        case Direction.Left:
                            destination.X = Position.X - step.Distance;
                            break;
                        case Direction.Down:
                            destination.Y = Position.Y + step.Distance;
                            break;
                        case Direction.Up:
                            destination.Y = Position.Y - step.Distance;
                            break;
                    }
                }
                
                // Whilst NPC has not reached this step's destination
                if (Position != destination)
                {
                    // Move in target direction
                    Move(step.Direction);

                    // If the NPC's destination "unreachable" due to being a distance away smaller than the size of Speed, we need a method to push it the last fraction
                    if (((step.Direction == Direction.Up || step.Direction == Direction.Down) && Math.Abs(Position.Y - destination.Y) < Speed)
                    || ((step.Direction == Direction.Left || step.Direction == Direction.Right) && Math.Abs(Position.X - destination.X) < Speed))
                        MoveFraction(step.Direction);

                    // If NPC tries to walk past edge of map, skip to next instruction to avoid getting stuck
                    if ((step.Direction == Direction.Left && Position.X == 0)
                    || (step.Direction == Direction.Right && Position.X == TileMap.WidthInPixels - Width)
                    || (step.Direction == Direction.Up && Position.Y == 0)
                    || (step.Direction == Direction.Down && Position.Y == TileMap.HeightInPixels - Height))
                    {
                        path.Dequeue();
                        moving = false;
                    }
                }

                // Once current step is completed, incrememnt step counter, dequeue the step and set moving flag to false
                else
                {
                    currentStep += 1;
                    path.Dequeue();
                    moving = false;
                }
            }

            // Once queue is empty, stop NPC animating and set appropriate flags
            else
            {
                IsAnimating = false;
                moving = false;
                pathComplete = true;
            }
        }
   
        private void Move(Direction direction)
        {
            Vector2 motion = Vector2.Zero;

            switch(direction)
            {
                case Direction.Up:
                    CurrentAnimation = Direction.Up;
                    motion.Y = -1;
                    break;
                case Direction.Down:
                    CurrentAnimation = Direction.Down;
                    motion.Y = 1;
                    break;
                case Direction.Left:
                    CurrentAnimation = Direction.Left;
                    motion.X = -1;
                    break;
                case Direction.Right:
                    CurrentAnimation = Direction.Right;
                    motion.X = 1;
                    break;
            }

            // Motion logic
            if (motion != Vector2.Zero)
            {
                Position += motion * Speed;
                LockToMap();
            }
        }

        private void MoveFraction(Direction direction)
        {
            Vector2 motion = Vector2.Zero;

            float yDiff = 0;
            float xDiff = 0;

            if (direction == Direction.Up || direction == Direction.Down)
                yDiff = Math.Abs(destination.Y - Position.Y);
            else
                xDiff = Math.Abs(destination.X - Position.X);

            switch (direction)
            {
                case Direction.Up:
                    CurrentAnimation = Direction.Up;
                    motion.Y = -yDiff;
                    break;
                case Direction.Down:
                    CurrentAnimation = Direction.Down;
                    motion.Y = yDiff;
                    break;
                case Direction.Left:
                    CurrentAnimation = Direction.Left;
                    motion.X = -xDiff;
                    break;
                case Direction.Right:
                    CurrentAnimation = Direction.Right;
                    motion.X = xDiff;
                    break;
            }

            Position += motion;
            LockToMap();

            currentStep += 1;
            path.Dequeue();
            moving = false;
            if (path.Count == 0)
                pathComplete = true;
        }

        public void PushSpeechBubble(SpeechBubble speechBubble)
        {
            speech = speechBubble;
            speech.IsActive = true;
        }

        public void AbortSpeechBubble()
        {
            if (speech != null)
                speech.IsActive = false;
        }

        #endregion
    }
}
