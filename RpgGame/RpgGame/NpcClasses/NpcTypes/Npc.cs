using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RpgGame.SpriteClasses;
using RpgGame.TileEngine;
using RpgGame.Geometry;
using RpgGame.NpcClasses.Actions.Movement;
using RpgGame.NpcClasses.Actions.Speech;
using RpgGame.StateMachine;

namespace RpgGame.NpcClasses
{
    public class Npc : AnimatedSprite
    {
        #region Field Region

        // Moving flag
        protected bool moving;
        // Current target destination 
        protected Vector2 destination;
        // Current path being follows
        protected Queue<Step> path;
        // Current step in the queue in progress
        protected int currentStep;
        // Flag to prevent new commands being received by NPC if it is already in action
        protected bool pathComplete;
        // Marker for use in collision prevention with player
        protected Vector2 collisionMarker;
        // Fields used in the Flee and Chase methods
        protected Vector2 velocity;
        protected bool colliding;
        Rectangle adjustedChaser = new Rectangle(0,0,0,0);
        
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

        public bool Colliding
        {
            get { return colliding; }
        }

        #endregion

        #region Constructor

        public Npc(Texture2D sprite, Dictionary<Direction, Animation> animation)
            : base(sprite, animation)
        {
            moving = false;
            pathComplete = true;
            path = new Queue<Step>();
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

        #region Stepwise Movement Region

        // The following method allows the NPC to follow a path in the form of a discrete set of stepwise movements
        public void StepwiseMovement()
        {
            // Iterate through the queue
            if (path.Count != 0)
            {
                Step step = path.Peek();

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
        
        #endregion

        #region Vectorwise Movement

        // Method has npc flee from chaser using steering behaviour
        public void Flee(AnimatedSprite chaser)
        {
            collisionMarker = Position;

            Vector2 diff = Position - chaser.Position;
            diff.Normalize();
            velocity = diff * Speed;

            Position += velocity;
            Round();

            // Set the Npc's animation direction
            if (Math.Abs(diff.X) > Math.Abs(diff.Y))
            {
                if (diff.X > 0)
                    CurrentAnimation = Direction.Right;
                else if (diff.X < 0)
                    CurrentAnimation = Direction.Left;
            }
            else
            {
                if (diff.Y > 0)
                    CurrentAnimation = Direction.Down;
                else if (diff.Y < 0)
                    CurrentAnimation = Direction.Up;
            }

            // If the target is fleeing, in order for collision to flag as true we need to create a new rectangle 
            // representing the chaser who's sides are larger than original
            // This is to overcome the inbuilt collision prevention already in place for the player (Player.NpcCollisionDetection())

            adjustedChaser = new Rectangle(
                (int)Math.Round(chaser.Position.X - 5),
                (int)Math.Round(chaser.Position.Y - 5), 
                chaser.Width + 10, 
                chaser.Height + 10);

            // If collision, move set colliding flag to true
            if (CollisionDetection.DoBoxesIntersect(this, adjustedChaser))
                colliding = true;
            else
                colliding = false;

            LockToMap();
        }

        // Algorithm normalises direction towards the target to be chased and moves towards it at rate Npc Speed
        public void Chase(AnimatedSprite target, bool slowOnApproach = false)
        {
            // Marker used for collision detection
            collisionMarker = Position;

            Vector2 diff = target.Position - Position;

            if (slowOnApproach)
            {
                // If Npc is further than 96 pixels away, increase his speed to 4, else reduce it to 1
                if (EntityDistance.GetDistance(this, target) > 96)
                    Speed = 4f;
                else
                    Speed = 1f;
            }

            // Normalise vector to a unit direction vector
            diff.Normalize();

            // Set the Npc's animation direction
            if (Math.Abs(diff.X) > Math.Abs(diff.Y))
            {
                if (diff.X > 0)
                    CurrentAnimation = Direction.Right;
                else if (diff.X < 0)
                    CurrentAnimation = Direction.Left;
            }
            else
            {
                if (diff.Y > 0)
                    CurrentAnimation = Direction.Down;
                else if (diff.Y < 0)
                    CurrentAnimation = Direction.Up;
            }

            // Move the Npc
            Position += diff * Speed;
            // Keep Npc within the integer plane
            Round();

            // If collision, move Npc back, and disable animation
            if (CollisionDetection.DoBoxesIntersect(this, target))
            {
                Position = collisionMarker;
                IsAnimating = false;
                colliding = true;
            }
            else
            {
                colliding = false;
                IsAnimating = true;
            }
        }

        #endregion

        #region State/Transition enablers

        public virtual string GetNpcType() { return "Npc"; } 

        // A speech bubble is activated as soon as it is sent to the npc
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

        // Allows States to send pathing information to the NPC
        public void SendPath(Queue<Step> path)
        {
            AbortPath();
            // Path used by Npc should be a deep copy, because the queue is popped as it is processed
            this.path = new Queue<Step>(path);
            pathComplete = false;
            // Set currentStep to 0
            currentStep = 0;
        }

        public void AbortPath()
        {
            if (!(path.Count == 0))
            {
                path.Clear();
                pathComplete = true;
                moving = false;
            }
        }

        #endregion
    }
}
