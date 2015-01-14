using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using RpgGame.TileEngine;
using RpgGame.Geometry;


namespace RpgGame.SpriteClasses
{
    public enum Direction { Down, Left, Right, Up }

    public class AnimatedSprite
    {
        #region Field Region

        static Game gameRef;
        // Access to world for blocked node collision avoidence
        static World world;

        Dictionary<Direction, Animation> animations;
        Direction currentAnimation;
        bool isAnimating;
        bool visible;

        protected Texture2D texture;
        Vector2 position;
        Vector2 velocity;
        Vector2 acceleration;
        float speed = 2.0f;

        #endregion

        #region Proprty Region

        public Direction CurrentAnimation
        {
            get { return currentAnimation; }
            set { currentAnimation = value; }
        }

        public bool IsAnimating
        {
            get { return isAnimating; }
            set { isAnimating = value; }
        }

        public int Width
        {
            get { return animations[currentAnimation].FrameWidth; }
        }

        public int Height
        {
            get { return animations[currentAnimation].FrameHeight; }
        }

        public float Speed
        {
            get { return speed;}
            set { speed = MathHelper.Clamp(value, 1.0f, 16.0f); }
        }

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public Vector2 Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }

        public Vector2 Acceleration
        {
            get { return acceleration; }
            set { acceleration = value; }
        }

        public bool Visible
        {
            get { return visible; }
            set { visible = value; }
        }

        protected World World
        {
            get
            {
                if(world == null)
                    world = (World)gameRef.Services.GetService(typeof(World));
                return world;
            }
        }


        #endregion

        #region Constructor Region

        public AnimatedSprite(Texture2D sprite, Dictionary<Direction, Animation> animation)
        {
            if (gameRef == null)
                throw new NullReferenceException("AnimatedSprite not correctly initialised with Game field");

            texture = sprite;
            visible = true;
            animations = new Dictionary<Direction, Animation>();

            foreach (Direction key in animation.Keys)
                animations.Add(key, (Animation)animation[key].Clone());
        }

        public AnimatedSprite(Game game)
        {
            gameRef = game;
        }

        #endregion

        #region Method Region

        public virtual void Update(GameTime gameTime)
        {
            if (isAnimating)
                animations[currentAnimation].Update(gameTime);
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch, Camera camera)
        {
            if (visible)
                spriteBatch.Draw(
                    texture,
                    position,
                    animations[currentAnimation].CurrentFrameRect,
                    Color.White);
        }

        public void LockToMap()
        {
            position.X = MathHelper.Clamp(position.X, 0, TileMap.WidthInPixels - Width);
            position.Y = MathHelper.Clamp(position.Y, 0, TileMap.HeightInPixels - Height);
        }

        // Method used to keep a sprites position within the integer set
        public void Round()
        {
            position.X = RoundToNearestInt.Round(position.X);
            position.Y = RoundToNearestInt.Round(position.Y);
        }

        #endregion
    }
}
