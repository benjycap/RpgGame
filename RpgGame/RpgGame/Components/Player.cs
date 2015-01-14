using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using RpgGame;
using RpgGame.TileEngine;
using RpgGame.SpriteClasses;
using RpgGame.NpcClasses;
using RpgGame.Geometry;

namespace RpgGame.Components
{
    // Player class encapsulates an instance of Camera
    public class Player
    {
        # region Field Region

        Camera camera;
        Game1 gameRef;
        AnimatedSprite sprite;
        NPCManager npcManager;
        Vector2 NpcCollisionMarker;
        Vector2 MapCollisionMarker;

        #endregion

        #region Property Region

        public Camera Camera
        {
            get { return camera; }
            set { camera = value; }
        }

        public AnimatedSprite PlayerSprite { get { return sprite; } }

        #endregion

        #region Constructor Region

        public Player(Game game)
        {
            gameRef = (Game1)game;
            camera = new Camera(gameRef.ScreenRectangle);
            
            npcManager = (NPCManager)game.Services.GetService(typeof(NPCManager));

            loadPlayerSprite();
        }

        #endregion

        #region XNA Method Region

        public void Update(GameTime gameTime, World world)
        {
            sprite.Update(gameTime);

            Zoom();
            IsRunning();
            PlayerMovement();
            Camera_Mode();
            NpcCollisionDetection();
            MapCollisionDetection(world);

            camera.Update(gameTime);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            sprite.Draw(gameTime, spriteBatch, camera);
        }

        #endregion

        #region Update Methods Region

        private void Zoom()
        {
            if (InputHandler.KeyReleased(Keys.PageUp))
            {
                camera.ZoomIn();
                if (camera.CameraMode == CameraMode.Follow)
                    camera.LockToSprite(sprite);
            }

            else if (InputHandler.KeyReleased(Keys.PageDown))
            {
                camera.ZoomOut();
                if (camera.CameraMode == CameraMode.Follow)
                    camera.LockToSprite(sprite);
            }
        }

        private void IsRunning()
        {
            // Player Running
            if (InputHandler.KeyDown(Keys.LeftShift))
            {
                sprite.Speed = 4.0f;
            }
            if (InputHandler.KeyReleased(Keys.LeftShift))
            {
                sprite.Speed = 2.0f;
            }
        }

        private void PlayerMovement()
        {
            // Player Movement
            Vector2 motion = new Vector2();

            // Vertical movement
            if (InputHandler.KeyDown(Keys.W))
            {
                sprite.CurrentAnimation = Direction.Up;
                motion.Y = -1;
            }
            else if (InputHandler.KeyDown(Keys.S))
            {
                sprite.CurrentAnimation = Direction.Down;
                motion.Y = 1;
            }

            // Horizontal movement
            if (InputHandler.KeyDown(Keys.A))
            {
                sprite.CurrentAnimation = Direction.Left;
                motion.X = -1;
            }
            else if (InputHandler.KeyDown(Keys.D))
            {
                sprite.CurrentAnimation = Direction.Right;
                motion.X = 1;
            }

            // Motion logic
            if (motion != Vector2.Zero)
            {
                sprite.IsAnimating = true;
                motion.Normalize();

                sprite.Position += motion * sprite.Speed;
                sprite.LockToMap();

                if (camera.CameraMode == CameraMode.Follow)
                    camera.LockToSprite(sprite);
            }
            // Disable animating when no input from player
            else
            {
                sprite.IsAnimating = false;
            }
        }

        private void Camera_Mode()
        {
            if (InputHandler.KeyReleased(Keys.F))
            {
                camera.ToggleCameraMode();
                if (camera.CameraMode == CameraMode.Follow)
                    camera.LockToSprite(sprite);
            }

            if (camera.CameraMode != CameraMode.Follow)
            {
                if (InputHandler.KeyReleased(Keys.C))
                {
                    camera.LockToSprite(sprite);
                }
            }
        }

        // Collision detection method brute forces through each NPC
        private void NpcCollisionDetection()
        {
            foreach (Npc npc in npcManager.NPCs)
            {
                // If npc is invisible, no collision avoidence necessary
                if (npc.Visible)
                {
                    // If player connects with an NPC
                    if (CollisionDetection.DoBoxesIntersect(sprite, npc))
                    {
                        // Position resets to position from last update
                        sprite.Position = NpcCollisionMarker;
                    }
                }
            }
            // Update new position
            NpcCollisionMarker = sprite.Position;
        }

        // Uses the CurrentCollision property of the world class to carry out collision logic on inanimate objects
        private void MapCollisionDetection(World world) 
        {
            List<Rectangle> collisionZones = world.CurrentCollision.CollisionZones;
            foreach (Rectangle zone in collisionZones)
            {
                if (CollisionDetection.DoBoxesIntersect(sprite, zone))
                {
                    // Position resets to position from last update
                    sprite.Position = MapCollisionMarker;
                }
            }
            // Update new position
            MapCollisionMarker = sprite.Position;
        }
    
        #endregion

        #region Load Content Methods

        private void loadPlayerSprite()
        {
            Texture2D spriteSheet = gameRef.Content.Load<Texture2D>(
               @"PlayerSprites\MaleFighter");

            Dictionary<Direction, Animation> animations = new Dictionary<Direction, Animation>();

            Animation animation = new Animation(3, 32, 32, 0, 0);
            animations.Add(Direction.Down, animation);

            animation = new Animation(3, 32, 32, 0, 32);
            animations.Add(Direction.Left, animation);

            animation = new Animation(3, 32, 32, 0, 64);
            animations.Add(Direction.Right, animation);

            animation = new Animation(3, 32, 32, 0, 96);
            animations.Add(Direction.Up, animation);

            sprite = new AnimatedSprite(spriteSheet, animations);
        }

        #endregion

        #region Sprite Manipulation Methods

        public void SetPlayerLocation(float xPos, float yPos)
        {
            sprite.Position = new Vector2(xPos, yPos);
        }

        #endregion
    }
}
