using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using RpgGame.NpcClasses;
using RpgGame.NpcClasses.Actions.Movement;
using RpgGame.NpcClasses.Actions.Speech;
using RpgGame.StateMachine;
using RpgGame.SpriteClasses;

namespace RpgGame.StateMachine.Thief
{

    public class Caught : ThiefBaseState
    {
        #region Constructor Region

        public Caught(ThiefFSM fsm)
            : base(fsm)
        {
        }

        #endregion

        #region State Methods

        // Action to take upon arrival into the state
        public override void Init(Npc npc)
        {
            Reset();

            npc.Speed = 4f;
            if (Math.Abs(npc.Position.X - fsm.PlayerSprite.Position.X) > Math.Abs(npc.Position.Y - fsm.PlayerSprite.Position.Y))
            {
                npc.CurrentAnimation = ((npc.Position.X - fsm.PlayerSprite.Position.X) > 0) ? Direction.Left : Direction.Right;
            }
            else
            {
                npc.CurrentAnimation = ((npc.Position.Y - fsm.PlayerSprite.Position.Y) > 0) ? Direction.Up : Direction.Down;
            }

            SendSpeechBubble(0, npc);

            npc.SendPath(PathProducer.GetSimplePath(npc.Position, fsm.atDoorPosition));
        }

        // Action to take with each update cycle
        public override void Update(Npc npc, GameTime gameTime)
        {
            if (!npc.Speaking)
                npc.StepwiseMovement();

            if (npc.Position == fsm.atDoorPosition)
                fsm.ChangeState("Outside");
        }

        #endregion

        #region Transition Methods

        protected override void InitTransitionExt(Npc npc, string transitionState)
        {
            switch (transitionState)
            {
                case "Outside":
                    return;
            }
        }

        protected override void UpdateTransitionExt(Npc npc, GameTime gameTime, string transitionState)
        {
            switch (transitionState)
            {
                case "Outside":
                    fsm.transitionComplete();
                    return;
            }
        }

        #endregion

        #region Internal Method Region

        protected override SpeechBubble[] SetSpeech()
        {
            SpeechBubble[] speech = new SpeechBubble[1];
            speech[0] = new SpeechBubble("GAHH!", 2);
            return speech;
        }

        #endregion
    }
}
