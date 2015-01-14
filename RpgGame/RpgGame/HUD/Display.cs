using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using RpgGame.ClockClasses;
using RpgGame.StateMachine;

namespace RpgGame.HUD
{
    // Heads Up Display component that displays the current in game time.
    public class Display
    {
        #region Field Region

        Clock clock;
        SpriteFont font;
        bool showInfo;

        #endregion

        #region Constructor

        public Display(Clock clock, SpriteFont font)
        {
            this.clock = clock;
            this.font = font;
            this.showInfo = false;
        }

        #endregion

        #region XNA Methods

        public void Update(GameTime gameTime)
        {
            if (InputHandler.KeyReleased(Keys.I))
                showInfo = !showInfo;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Rectangle screenDimensions)
        {
            spriteBatch.DrawString(
                font,
                clock.Time.ToString("hh\\:mm"),
                new Vector2(screenDimensions.Width - 90, 0),
                Color.White);

            if (showInfo)
            {
                spriteBatch.DrawString(
                    font,
                    "Game time ratio: " + clock.Speed,
                    new Vector2(screenDimensions.Width - 280, 30),
                    Color.White);

                spriteBatch.DrawString(
                    font,
                    "Press + to Increment",
                    new Vector2(screenDimensions.Width - 300, 60),
                    Color.White);

                spriteBatch.DrawString(
                    font,
                    "Press - to Decrement",
                    new Vector2(screenDimensions.Width - 300, 90),
                    Color.White);

                spriteBatch.DrawString(
                    font,
                    "WASD to move. Shift to run.",
                    new Vector2(50, screenDimensions.Height - 50),
                    Color.White);

                PrintStateInfo(gameTime, spriteBatch, screenDimensions);
            }
            else
            {
                string s = "Press I for more information";

                spriteBatch.DrawString(
                    font,
                    s,
                    new Vector2(screenDimensions.Width - font.MeasureString(s).X - 5, screenDimensions.Height - font.MeasureString(s).Y),
                    Color.White);
            }

        }

        #endregion

        #region Private methods

        private void PrintStateInfo(GameTime gameTime, SpriteBatch spriteBatch, Rectangle screenDimensions)
        {
            string shopkeeperInfo, customerInfo, child0Info, child1Info, child2Info;

            shopkeeperInfo = "Shopkeeper: " + (!FsmManager.SFsm.InTransition ? FsmManager.SFsm.CurrentState : FsmManager.SFsm.CurrentState + " -> " + FsmManager.SFsm.TransitionState);
            customerInfo = "Customer: " + (!FsmManager.CFsm.InTransition ? FsmManager.CFsm.CurrentState : FsmManager.CFsm.CurrentState + " -> " + FsmManager.CFsm.TransitionState);
            child0Info = "Child0: " + (!FsmManager.ChFsmList[0].InTransition ? FsmManager.ChFsmList[0].CurrentState : FsmManager.ChFsmList[0].CurrentState + " -> " + FsmManager.ChFsmList[0].TransitionState);
            child1Info = "Child1: " + (!FsmManager.ChFsmList[1].InTransition ? FsmManager.ChFsmList[1].CurrentState : FsmManager.ChFsmList[1].CurrentState + " -> " + FsmManager.ChFsmList[1].TransitionState);
            child2Info = "Child2: " + (!FsmManager.ChFsmList[2].InTransition ? FsmManager.ChFsmList[2].CurrentState : FsmManager.ChFsmList[2].CurrentState + " -> " + FsmManager.ChFsmList[2].TransitionState);

            float textYLocation;

            textYLocation = screenDimensions.Height - font.MeasureString(child2Info).Y;
            spriteBatch.DrawString(
                font,
                child2Info,
                new Vector2(screenDimensions.Width - font.MeasureString(child2Info).X, textYLocation),
                Color.White);

            textYLocation -= font.MeasureString(child1Info).Y;
            spriteBatch.DrawString(
                font,
                child1Info,
                new Vector2(screenDimensions.Width - font.MeasureString(child1Info).X, textYLocation),
                Color.White);

            textYLocation -= font.MeasureString(child0Info).Y;
            spriteBatch.DrawString(
                font,
                child0Info,
                new Vector2(screenDimensions.Width - font.MeasureString(child0Info).X, textYLocation),
                Color.White);

            textYLocation -= font.MeasureString(customerInfo).Y;
            spriteBatch.DrawString(
                font,
                customerInfo,
                new Vector2(screenDimensions.Width - font.MeasureString(customerInfo).X, textYLocation),
                Color.White);

            textYLocation -= font.MeasureString(shopkeeperInfo).Y;
            spriteBatch.DrawString(
                font,
                shopkeeperInfo,
                new Vector2(screenDimensions.Width - font.MeasureString(shopkeeperInfo).X, textYLocation),
                Color.White);
        }

        #endregion
    }
}
