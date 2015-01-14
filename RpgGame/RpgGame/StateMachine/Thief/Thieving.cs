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

    public class Thieving : ThiefBaseState
    {
        #region Constructor Region

        public Thieving(ThiefFSM fsm)
            : base(fsm)
        {
        }

        #endregion

        #region State Methods

        // Action to take upon arrival into the state
        public override void Init(Npc npc)
        {
            Reset();

            npc.SendPath(PathProducer.GetSimplePath(npc.Position, fsm.shelfPosition, InitialDirection.Vertical));
        }

        // Action to take with each update cycle
        public override void Update(Npc npc, GameTime gameTime)
        {
            npc.StepwiseMovement();

            if (npc.Position == fsm.shelfPosition)
            {
                npc.CurrentAnimation = Direction.Up;
                SendSpeechBubble(0, npc);

                if (!npc.Speaking)
                    npc.SendPath(PathProducer.GetSimplePath(npc.Position, fsm.atDoorPosition));
            }

            if (npc.Position == fsm.atDoorPosition)
                fsm.ChangeState("Outside");

            if (Vector2.Distance(npc.Position, fsm.PlayerSprite.Position) < 64)
                fsm.ChangeState("Caught");
        }

        #endregion

        #region Transition Methods

        protected override void InitTransitionExt(Npc npc, string transitionState)
        {
            switch (transitionState)
            {
                case "Outside":
                    return;
                case "Caught":
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
                case "Caught":
                    fsm.transitionComplete();
                    return;
            }
        }

        #endregion

        #region Internal Method Region

        protected override SpeechBubble[] SetSpeech()
        {
            SpeechBubble[] speech = new SpeechBubble[1];
            speech[0] = new SpeechBubble("Jackpot... heh heh heh.", 2);
            return speech;
        }

        #endregion
    }
}
