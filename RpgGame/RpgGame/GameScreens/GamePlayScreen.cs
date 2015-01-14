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
using RpgGame.ClockClasses;
using RpgGame.NpcClasses;
using RpgGame.NpcClasses.NpcTypes;
using RpgGame.NpcClasses.Actions.Movement;
using RpgGame.NpcClasses.Actions.Speech;
using RpgGame.Geometry;
using RpgGame.StateMachine;
using RpgGame.StateMachine.BaseClasses;
using RpgGame.StateMachine.Shopkeeper;
using RpgGame.StateMachine.Customer;
using RpgGame.StateMachine.Child;
using RpgGame.StateMachine.Thief;

using RpgGame.Components;
using RpgGame.HUD;

namespace RpgGame.GameScreens
{
    public class GamePlayScreen : BaseGameState
    {
        #region Field Region
        
        Random random;
        // Engine is used to define tile height and width. Its static properties are referenced in the TileMap class.
        Engine engine = new Engine(32, 32);
        // World instance holds data for all the levels in the game
        World world;
        TileMap map;
        Player player;

        Clock clock;
        Display clockDisplay;


        // NPCs and FSMs
        NPCManager npcManager;

        ShopkeeperNpc shopkeeper;
        ShopkeeperFSM shopkeeperFsm;

        CustomerNpc customer;
        CustomerFSM customerFsm;

        ChildNpc child0, child1, child2;
        ChildFsm childFsm0, childFsm1, childFsm2;

        ThiefNpc thief;
        ThiefFSM thiefFsm;

        #endregion

        #region Animations Region

        AnimationDictionary customerAnimation, shopkeeperAnimation, childAnimation;
        Texture2D customerSpriteSheet, shopkeeperSpriteSheet, childSpriteSheet;

        #endregion

        #region Constructor Region

        public GamePlayScreen(Game game, GameStateManager manager)
            : base(game, manager)
        {
        }

        #endregion

        #region XNA Method Region

        public override void Initialize()
        {
            new AnimatedSprite(gameRef);

            random = new Random();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            player = new Player(gameRef);
            gameRef.Services.AddService(typeof(Player), player);

            new FsmInitialiser(gameRef, player.PlayerSprite);

            world = new World();
            gameRef.Services.AddService(typeof(World), world);

            // Get Services
            // Clock
            clock = (Clock)Game.Services.GetService(typeof(Clock));
            clockDisplay = new Display(clock, menuFont);
            clock.SetTime(8, 30);
            // NPC Manager
            npcManager = (NPCManager)Game.Services.GetService(typeof(NPCManager));

            // Load Npcs
            LoadNpcAnimations();

            makeDefaultMap();

            MakeShopMap();
            MakeShopkeeperAndCustomerNpcs();
            MakeChildNpcs();

            world.CurrentLevel = 1;
            player.SetPlayerLocation(100, 100);
        }

        public override void Update(GameTime gameTime)
        {
            player.Update(gameTime, world);
            npcManager.Update(gameTime);
            ChangeClockSpeed();
            clockDisplay.Update(gameTime);

            // Run FSMs
            FsmManager.Execute(gameTime);

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            // Draw the components which move with player input (i.e. influenced by player.Camera.Transformation)
            gameRef.spriteBatch.Begin(
                SpriteSortMode.Deferred,
                BlendState.AlphaBlend,
                SamplerState.PointClamp,
                null,
                null,
                null,
                player.Camera.Transformation);
            // Draw map layers
            world.Draw(gameRef.spriteBatch, player.Camera);
            // Draw player sprite
            player.Draw(gameTime, gameRef.spriteBatch);
            // Draw NPC sprites
            npcManager.Draw(gameTime, gameRef.spriteBatch, player.Camera);
            gameRef.spriteBatch.End();

            // Draw the batch of components which are static relative to the player (HUD components)
            gameRef.spriteBatch.Begin();
            // Draw clock
            clockDisplay.Draw(gameTime, gameRef.spriteBatch, gameRef.ScreenRectangle);
            gameRef.spriteBatch.End();

            base.Draw(gameTime);
        }

        #endregion

        #region NPC region

        private void LoadNpcAnimations()
        {
            customerSpriteSheet = gameRef.Content.Load<Texture2D>(@"PlayerSprites\malerogue");

            customerAnimation = new AnimationDictionary();
            Animation animation = new Animation(3, 32, 32, 0, 0);
            customerAnimation.Add(Direction.Down, animation);
            animation = new Animation(3, 32, 32, 0, 32);
            customerAnimation.Add(Direction.Left, animation);
            animation = new Animation(3, 32, 32, 0, 64);
            customerAnimation.Add(Direction.Right, animation);
            animation = new Animation(3, 32, 32, 0, 96);
            customerAnimation.Add(Direction.Up, animation);

            shopkeeperSpriteSheet = gameRef.Content.Load<Texture2D>(@"PlayerSprites\malewizard");

            shopkeeperAnimation = new AnimationDictionary();
            animation = new Animation(3, 32, 32, 0, 0);
            shopkeeperAnimation.Add(Direction.Down, animation);
            animation = new Animation(3, 32, 32, 0, 32);
            shopkeeperAnimation.Add(Direction.Left, animation);
            animation = new Animation(3, 32, 32, 0, 64);
            shopkeeperAnimation.Add(Direction.Right, animation);
            animation = new Animation(3, 32, 32, 0, 96);
            shopkeeperAnimation.Add(Direction.Up, animation);

            childSpriteSheet = gameRef.Content.Load<Texture2D>(@"PlayerSprites\femalepriest");

            childAnimation = new AnimationDictionary();
            animation = new Animation(3, 32, 32, 0, 0);
            childAnimation.Add(Direction.Down, animation);
            animation = new Animation(3, 32, 32, 0, 32);
            childAnimation.Add(Direction.Left, animation);
            animation = new Animation(3, 32, 32, 0, 64);
            childAnimation.Add(Direction.Right, animation);
            animation = new Animation(3, 32, 32, 0, 96);
            childAnimation.Add(Direction.Up, animation);
        }

        private void MakeShopkeeperAndCustomerNpcs()
        {
            shopkeeper = new ShopkeeperNpc(shopkeeperSpriteSheet, shopkeeperAnimation);
            customer = new CustomerNpc(customerSpriteSheet, customerAnimation);
            thief = new ThiefNpc(customerSpriteSheet, customerAnimation.Clone());

            npcManager.AddNPC(shopkeeper);
            npcManager.AddNPC(customer);
            npcManager.AddNPC(thief);

            customerFsm = new CustomerFSM(customer);
            shopkeeperFsm = new ShopkeeperFSM(shopkeeper);
            thiefFsm = new ThiefFSM(thief);

            FsmManager.AddShopkeeperFsm(shopkeeperFsm);
            FsmManager.AddCustomerFsm(customerFsm);
            FsmManager.AddThiefFsm(thiefFsm);
        }

        private void MakeChildNpcs()
        {
            child0 = new ChildNpc(childSpriteSheet, childAnimation.Clone());
            child1 = new ChildNpc(childSpriteSheet, childAnimation.Clone());
            child2 = new ChildNpc(childSpriteSheet, childAnimation.Clone());

            npcManager.AddNPC(child0);
            npcManager.AddNPC(child1);
            npcManager.AddNPC(child2);

            childFsm0 = new ChildFsm(child0);
            childFsm1 = new ChildFsm(child1);
            childFsm2 = new ChildFsm(child2);

            FsmManager.AddChildFsm(childFsm0);
            FsmManager.AddChildFsm(childFsm1);
            FsmManager.AddChildFsm(childFsm2);
        }

        #endregion

        #region Maps Region

        private void makeDefaultMap()
        {
            Texture2D tilesetTexture = Game.Content.Load<Texture2D>(@"Tilesets\tileset1");
            Tileset tileset1 = new Tileset(tilesetTexture, 8, 8, 32, 32);

            tilesetTexture = Game.Content.Load<Texture2D>(@"Tilesets\shelves");
            Tileset tileset2 = new Tileset(tilesetTexture, 8, 8, 32, 32);

            List<Tileset> tilesets = new List<Tileset>();
            tilesets.Add(tileset1);
            tilesets.Add(tileset2);

            MapLayer layer = new MapLayer(100, 100);

            for (int y = 0; y < layer.Height; y++)
            {
                for (int x = 0; x < layer.Width; x++)
                {
                    Tile tile = new Tile(0, 0);
                    layer.SetTile(x, y, tile);
                }
            }

            MapLayer splatter = new MapLayer(100, 100);

            for (int i = 0; i < 100; i++)
            {
                int x = random.Next(0, 100);
                int y = random.Next(0, 100);
                int index = random.Next(2, 14);

                Tile tile = new Tile(index, 0);
                splatter.SetTile(x, y, tile);
            }

            splatter.SetTile(1, 0, new Tile(0, 1));
            splatter.SetTile(2, 0, new Tile(2, 1));
            splatter.SetTile(3, 0, new Tile(0, 1));

            List<MapLayer> mapLayers = new List<MapLayer>();

            mapLayers.Add(layer);
            mapLayers.Add(splatter);

            map = new TileMap(tilesets, mapLayers);

            world.PushLevel(map, new CollisionMap());
        }

        private void MakeShopMap()
        {
            Texture2D tilesetTexture = Game.Content.Load<Texture2D>(@"Tilesets\cleartile");
            Tileset clear = new Tileset(tilesetTexture, 1, 1, 32, 32);
            tilesetTexture = Game.Content.Load<Texture2D>(@"Tilesets\floors");
            Tileset floor = new Tileset(tilesetTexture, 8, 7, 32, 32);
            tilesetTexture = Game.Content.Load<Texture2D>(@"Tilesets\shelves");
            Tileset shelves = new Tileset(tilesetTexture, 16, 16, 32, 32);
            tilesetTexture = Game.Content.Load<Texture2D>(@"Tilesets\table");
            Tileset table = new Tileset(tilesetTexture, 1, 3, 32, 32);
            tilesetTexture = Game.Content.Load<Texture2D>(@"Tilesets\wallsanddoor");
            Tileset wallsAndDoor = new Tileset(tilesetTexture, 6, 2, 32, 32);

            List<Tileset> tilesets = new List<Tileset>();
            tilesets.Add(clear);
            tilesets.Add(floor);
            tilesets.Add(shelves);
            tilesets.Add(table);
            tilesets.Add(wallsAndDoor);

            MapLayer floorLayer = new MapLayer(25, 14);
            MapLayer wallLayer = new MapLayer(25, 14);
            MapLayer layer = new MapLayer(25, 14);

            // Floor
            for (int y = 0; y < floorLayer.Height; y++)
            {
                for (int x = 0; x < floorLayer.Width; x++)
                {
                    floorLayer.SetTile(x, y, 12, 1);
                }
            }

            // Walls
            // Top left walls
            wallLayer.SetTile(0, 0, 0, 4);
            wallLayer.SetTile(0, 1, 6, 4);
            // Top walls
            for (int x = 1; x < floorLayer.Width - 1; x++)
            {
                wallLayer.SetTile(x, 0, 1, 4);
                wallLayer.SetTile(x, 1, 7, 4);
            }
            // Top right walls
            wallLayer.SetTile(24, 0, 2, 4);
            wallLayer.SetTile(24, 1, 8, 4);
            // Side walls
            for (int y = 2; y < floorLayer.Height; y++)
            {
                wallLayer.SetTile(0, y, 3, 4);
                wallLayer.SetTile(0, y, 9, 4);
                wallLayer.SetTile(24, y, 4, 4);
                wallLayer.SetTile(24, y, 10, 4);
            }
            // Door
            wallLayer.SetTile(20, 0, 5, 4);
            wallLayer.SetTile(20, 1, 11, 4);

            // Shelves
            for (int i = 1; i <= 8; i++)
            {
                layer.SetTile(i, 1, i + 103, 2);
                layer.SetTile(i, 2, i + 111, 2);
            }

            // Table
            for (int i = 5; i <= 11; i++)
            {
                layer.SetTile(18, i, 1, 3);
            }
            layer.SetTile(18, 4, 0, 3);
            layer.SetTile(18, 12, 2, 3);


            List<MapLayer> mapLayers = new List<MapLayer>();

            mapLayers.Add(floorLayer);
            mapLayers.Add(wallLayer);
            mapLayers.Add(layer);


            map = new TileMap(tilesets, mapLayers);

            List<Rectangle> cols = new List<Rectangle>();
            cols.Add(new Rectangle(0, 0, 640, 64));
            cols.Add(new Rectangle(34, 64, 256, 16));
            cols.Add(new Rectangle(576, 128, 23, 283));

            world.PushLevel(map, new CollisionMap(cols));
        }

        #endregion

        #region Time Manipulation Methods

        private void ChangeClockSpeed()
        {
            if (InputHandler.KeyDown(Keys.Add))
                clock.IncrementSpeed();
            if (InputHandler.KeyDown(Keys.Subtract))
                clock.DecrementSpeed();
            if (InputHandler.KeyDown(Keys.Multiply))
                clock.AddMinute();
        }

        #endregion
    }
}
