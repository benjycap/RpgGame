using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace RpgGame.ClockClasses
{
    public class Clock : GameComponent
    {
        #region Field Region

        // Field to represent in game time
        TimeSpan time;
        // In game hours per real time hour
        int gameTimeRatio;
        // Keeps track of elapsed game time in order to update clock
        TimeSpan timeElapsed;
        // Designates how often we should update our clock
        TimeSpan updateFrequency;

        #endregion

        #region Property Region

        // Expose in game time
        public TimeSpan Time
        {
            get { return time; }
        }

        // Expose speed
        public int Speed
        {
            get { return gameTimeRatio; }
        }

        #endregion

        #region Constructor Region

        public Clock(Game game, int ratio)
            : base(game)
        {
            gameTimeRatio = ratio;

            // We don't want a value slower than real time
            if (gameTimeRatio < 1)
                gameTimeRatio = 1;

            updateFrequency = TimeSpan.FromMinutes(1 / (double)gameTimeRatio);
            timeElapsed = TimeSpan.Zero;
        }

        #endregion

        #region XNA Method Region

        public override void Initialize()
        {
            initialiseTime();

            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            updateTime(gameTime);

            base.Update(gameTime);
        }

        #endregion

        #region Method Region

        // Logic to initialise in game time
        private void initialiseTime()
        {
            TimeSpan realTime = DateTime.Now.TimeOfDay;

            // We lose a certain level of granality due to integer divison of seconds here
            // Not important as in game clock will use only hours & minutes
            int minutesCarriedOver = (realTime.Seconds * gameTimeRatio) / 60;
            int minutes = ((realTime.Minutes * gameTimeRatio) + minutesCarriedOver) % 60;
            
            int hoursCarriedOver = (realTime.Minutes * gameTimeRatio) / 60;
            int hours = ((realTime.Hours * gameTimeRatio) + hoursCarriedOver) % 24;
            
            time = new TimeSpan(hours, minutes, 0);
        }

        // Logic to update in game time 
        private void updateTime(GameTime gameTime)
        {
            // Increment timeElapsed by the amount of time that has passed since last update
            timeElapsed += gameTime.ElapsedGameTime;

            if (timeElapsed >= updateFrequency)
            {
                // Reset condition necessary here or we tick over to 1 day
                if (time.Hours == 23 && time.Minutes == 59)
                    time = TimeSpan.Zero;
                else
                    time = new TimeSpan(time.Hours, time.Minutes + 1, 0);

                timeElapsed = TimeSpan.Zero;
            }
        }

        #endregion

        #region Time Manipulation Methods

        public void IncrementSpeed()
        {
            if (gameTimeRatio < 120)
            {
                gameTimeRatio += 1;
                updateFrequency = TimeSpan.FromMinutes(1 / (double)gameTimeRatio);
            }
        }

        public void DecrementSpeed()
        {
            if (gameTimeRatio > 1)
            {
                gameTimeRatio -= 1;
                updateFrequency = TimeSpan.FromMinutes(1 / (double)gameTimeRatio);
            }
        }

        public void SetTime(int hours, int minutes)
        {
            time = new TimeSpan(hours, minutes, 0);
        }

        public void AddMinute()
        {
            time = new TimeSpan(time.Hours, time.Minutes + 1, 0);
        }

        #endregion
    }

}