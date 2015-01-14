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

namespace RpgGame.StateMachine.Child
{
    public class Enter : ChildBaseState
    {
        #region Field Region

        readonly Vector2 child0Destination = new Vector2(198, 224);
        readonly Vector2 child1Destination = new Vector2(160, 288);
        readonly Vector2 child2Destination = new Vector2(236, 288);

        #endregion

        #region Constructor Region

        public Enter(ChildFsm fsm)
            : base(fsm)
        {
        }

        #endregion

        #region Execute Methods

        // ChildBaseState methods are used for in state updates as the children have unique behaviours

        protected override void InitTransitionExt(Npc npc, string transitionState)
        {
        }

        protected override void UpdateTransitionExt(Npc npc, GameTime gameTime, string transitionState)
        {
            switch (transitionState)
            {
                case "Playing":
                    fsm.transitionComplete();
                    return;
            }
        }

        #endregion

        #region Specific Behaviour Region

        protected override void Entry0(Npc npc)
        {
            npc.SendPath(PathProducer.GetSimplePath(npc.Position, child0Destination));
            npc.Visible = true;
        }

        protected override void Entry1(Npc npc)
        {
            npc.SendPath(PathProducer.GetSimplePath(npc.Position, child1Destination));
        }

        protected override void Entry2(Npc npc)
        {
            npc.SendPath(PathProducer.GetSimplePath(npc.Position, child2Destination));
        }

        protected override void Update0(Npc npc, GameTime gameTime)
        {
            npc.StepwiseMovement();

            if (npc.PathComplete)
            {
                // Begin stopwatch
                stopwatch.Start();
                // After 2 seconds, look left
                if (stopwatch.ElapsedMilliseconds >= 2000)
                    npc.CurrentAnimation = Direction.Left;
                // After a further 2 seconds, look right
                if (stopwatch.ElapsedMilliseconds >= 4000)
                    npc.CurrentAnimation = Direction.Right;
                // At 6 seconds, face door and call out to other children
                if (stopwatch.ElapsedMilliseconds >= 6000)
                {
                    npc.CurrentAnimation = Direction.Down;
                    SendSpeechBubble(0, npc, true);
                }
                if (FsmManager.ChFsmList[2].CurrentPosition == child2Destination)
                {
                    // Change state to playing
                    fsm.ChangeState("Playing");
                }
            }
        }

        protected override void Update1(Npc npc, GameTime gameTime)
        {
            // Don't run until child0 has reached his destination
            if (FsmManager.ChFsmList[0].CurrentPosition == child0Destination)
                // Start stopwatch to keep in time with child0's actions
                stopwatch.Start();

            if (stopwatch.IsRunning)
            {
                // Once child0 has finished speaking, act
                if (stopwatch.ElapsedMilliseconds >= 8000)
                {
                    npc.Visible = true;
                    npc.StepwiseMovement();
                    // If both child1 and child2 are in position, simultaneously send speech bubbles
                    if (npc.Position == child1Destination && FsmManager.ChFsmList[2].CurrentPosition == child2Destination)
                    {
                        SendSpeechBubble(0, npc, true);
                        if (!npc.Speaking)
                            fsm.ChangeState("Playing");
                    }
                }
            }
        }

        protected override void Update2(Npc npc, GameTime gameTime)
        {
            if (FsmManager.ChFsmList[0].CurrentPosition == child0Destination)
                stopwatch.Start();
                // Child2 comes into shop 2 seconds after child 1

            if (stopwatch.IsRunning)
            {
                if (stopwatch.ElapsedMilliseconds >= 10000)
                {
                    npc.Visible = true;
                    npc.StepwiseMovement();
                    // Child2 comes in last, so we don't need to consider child1 position for simultaneous speech (child1 checks child2's instead)
                    if (npc.Position == child2Destination)
                    {
                        SendSpeechBubble(0, npc, true);
                        if (!npc.Speaking)
                            fsm.ChangeState("Playing");
                    }
                }
            }
        }
        #endregion

        #region Internal Methods

        protected override SpeechBubble[] SetSpeech()
        {
            switch (fsm.NpcID)
            {
                // Speech bubble for child 0
                case 0:
                    speech = new SpeechBubble[] { new SpeechBubble("Let's play in here!!", 2) };
                    return speech;
                // Speech bubble for childs 1 & 2
                default:
                    speech = new SpeechBubble[] { new SpeechBubble("YAY!", 2) };
                    return speech;
            }
        }

        #endregion
    }
}
