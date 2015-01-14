using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using RpgGame.NpcClasses;
using RpgGame.NpcClasses.NpcTypes;
using RpgGame.NpcClasses.Actions.Movement;
using RpgGame.NpcClasses.Actions.Speech;
using RpgGame.StateMachine;
using RpgGame.SpriteClasses;

namespace RpgGame.StateMachine.Child
{
    public class Playing : ChildBaseState
    {
        #region Field Region

        Random random;

        bool allCaught;
        bool timeout;

        #endregion

        #region Constructor Region

        public Playing(ChildFsm fsm)
            : base(fsm)
        {
            random = new Random((fsm.NpcID + 1) * DateTime.Now.Millisecond);
            allCaught = false;
            timeout = true;
        }

        #endregion

        #region State Methods

        public override void Init(Npc npc)
        {
            fsm.IsCaught = false;
        }

        public override void Update(Npc npc, GameTime gameTime)
        {
            // If time reaches 3.30pm, leave store without being caught by player
            if (fsm.Time == new TimeSpan(15, 30, 0))
                fsm.ChangeState("Caught");

            // Behaviour if player and child are within 64 pixels distance
            if (Vector2.Distance(npc.Position, fsm.PlayerSprite.Position) <= 64)
            {
                // Abort current stepwise path
                npc.AbortPath();
                // Flee from player
                npc.Flee(fsm.PlayerSprite);
                // Ensure npc remains within confines of shop floor
                if (npc.Position.X > 508) npc.Position = new Vector2(508, npc.Position.Y);
                if (npc.Position.Y < 82) npc.Position = new Vector2(npc.Position.X, 82);

                // If player catches child
                if (npc.Colliding)
                {
                    timeout = false;
                    fsm.ChangeState("Caught");
                }
            }
            // Behaviour if shopkeeper and child are within 56 pixels distance
            else if (Vector2.Distance(npc.Position, FsmManager.SFsm.Shopkeeper.Position) <= 56)
            {                
                // Abort current stepwise path
                npc.AbortPath();
                // Flee from shopkeeper
                npc.Flee(FsmManager.SFsm.Shopkeeper);
                // Ensure npc remains within confines of shop floor
                if (npc.Position.X > 508) npc.Position = new Vector2(508, npc.Position.Y);
                if (npc.Position.Y < 82) npc.Position = new Vector2(npc.Position.X, 82);
            }
            // Random wandering behaviour
            else
            {
                if (npc.PathComplete)
                    npc.SendPath(PathProducer.GetSimplePath(npc.Position, RandomFloorLocation()));
                npc.StepwiseMovement();
            }
        }

        #endregion

        #region Transition Methods

        protected override void InitTransitionExt(Npc npc, string transitionState)
        {
            switch (transitionState)
            {
                case "Caught":
                    Reset();
                    fsm.IsCaught = true;
                    return;
            }
        }

        protected override void UpdateTransitionExt(Npc npc, GameTime gameTime, string transitionState)
        {
            switch (transitionState)
            {
                case "Caught":
                    if (!timeout)
                        SendSpeechBubble(0, npc, true);

                    foreach (ChildFsm chFsm in FsmManager.ChFsmList)
                    {
                        allCaught = true;

                        if (!chFsm.IsCaught || npc.Speaking)
                        {
                            allCaught = false;
                            break;
                        }
                    }

                    if (allCaught)
                        fsm.transitionComplete();

                    return;
            }
        }

        #endregion

        private Vector2 RandomFloorLocation()
        {
            int randomx, randomy;
            randomx = random.Next(0, 508);
            randomy = random.Next(82, 416);

            Vector2 location = new Vector2(randomx, randomy);
            return location;
        }


        #region Internal Methods

        protected override SpeechBubble[] SetSpeech()
        {
            switch (fsm.NpcID)
            {
                // Speech bubble for child 0
                case 0:
                    speech = new SpeechBubble[] { new SpeechBubble("No, you caught me!", 2) };
                    return speech;
                // Speech bubble for child1
                case 1:
                    speech = new SpeechBubble[] { new SpeechBubble("Aww you're no fun.", 2) };
                    return speech;
                // Speech bubble for child2
                case 2:
                    speech = new SpeechBubble[] { new SpeechBubble(":(", 2) };
                    return speech;
                default:
                    return null;
            }
        }

        protected override void Reset()
        {
            base.Reset();

            allCaught = false;
        }

        #endregion
    }
}
