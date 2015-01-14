using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace RpgGame
{

    public class GameStateManager : GameComponent
    {

        #region Event Region

        public event EventHandler OnStateChange;

        #endregion

        #region Fields and Properties

        Stack<GameState> gameStates = new Stack<GameState>();

        // DrawableGameComponent are drawn in ascending order. The component with the lowest DrawOrder
        // property is drawn first.
        const int startDrawOrder = 5000;
        const int drawOrderInc = 100;
        int drawOrder;

        // Return the state currently on top of the stack
        public GameState CurrentState
        {
            get { return gameStates.Peek(); }
        }

        #endregion

        #region Constructor Region

        public GameStateManager(Game game)
            : base(game)
        {
            drawOrder = startDrawOrder;
        }

        #endregion

        #region XNA Method Region

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        #endregion

        #region Methods Region

        // Method to pop a screen off the top of the stack (e.g. close the options menu)
        public void PopState()
        {
            if (gameStates.Count > 0)
            {
                RemoveState();
                drawOrder -= drawOrderInc;

                // Call event handler code (unless event is not subscribed to)
                if (OnStateChange != null)
                    OnStateChange(this, null);
            }
        }

        private void RemoveState()
        {
            GameState State = gameStates.Peek();
            OnStateChange -= State.StateChange;
            Game.Components.Remove(State);
            gameStates.Pop();
        }

        // Push a screen onto the top of event stack (e.g. opening options menu)
        public void PushState(GameState newState)
        {
            drawOrder += drawOrderInc;
            newState.DrawOrder = drawOrder;

            AddState(newState);

            if (OnStateChange != null)
                OnStateChange(this, null);
        }

        private void AddState(GameState newState)
        {
            gameStates.Push(newState);
            Game.Components.Add(newState);
            OnStateChange += newState.StateChange;
        }

        // Refresh to a new stack (removes all other states)
        public void ChangeState(GameState newState)
        {
            while (gameStates.Count > 0)
                RemoveState();

            newState.DrawOrder = startDrawOrder;
            drawOrder = startDrawOrder;

            AddState(newState);

            if (OnStateChange != null)
                OnStateChange(this, null);
        }
        
        #endregion

    }
}
