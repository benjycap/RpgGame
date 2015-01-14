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
using RpgGame.Geometry;

namespace RpgGame.StateMachine.Shopkeeper
{
    public class Idling : ShopkeeperBaseState
    {
        #region Field Region

        // Array to contain times at which the state should request a transition to Patrolling
        static TimeSpan[] patrollingTransitionTimes;

        #endregion

        #region Constructor Region

        public Idling(ShopkeeperFSM fsm)
            : base(fsm)
        {
            patrollingTransitionTimes = setupTransitionTimes();
        }

        private TimeSpan[] setupTransitionTimes()
        {
            TimeSpan[] times = new TimeSpan[2];
            times[0] = new TimeSpan(10, 30, 0);
            times[1] = new TimeSpan(15, 0, 0);
            return times;
        }

        #endregion

        #region State Methods

        public override void Init(Npc npc) 
        {
            Reset();

            if (fsm.FirstPass)
            {
                npc.Position = fsm.behindCounterPosition;
            }

            npc.CurrentAnimation = Direction.Left;
        }

        public override void Update(Npc npc, GameTime gameTime)
        {
            if (fsm.Time == new TimeSpan(17, 30, 0))
                fsm.ChangeState("Sleeping");

            if (patrollingTransitionTimes.Contains(fsm.Time))
                fsm.ChangeState("Patrolling");

            if (FsmManager.ChFsmList[2].IsPlaying)
                fsm.ChangeState("Babysitting");

            // If player gets within 50 pixel radius of the infrontcounterposition, send speech bubble
            // When leaving the area, set speechsent bool to false so it triggers again upon player's re-entry of radius
            if (Vector2.Distance(fsm.PlayerSprite.Position, fsm.inFrontCounterPosition) <= 50)
                SendSpeechBubble(0, npc);
            else
                speechSent[0] = false;

            // If player goes behind the counter, 
            if (CollisionDetection.DoBoxesIntersect(fsm.PlayerSprite, fsm.behindCounterArea))
                SendSpeechBubble(6, npc);
            else
                speechSent[6] = false;
        }

        #endregion

        #region Transition Methods

        protected override void InitTransitionExt(Npc npc, string transitionState)
        {
            switch (transitionState)
            {
                case "Patrolling":
                    npc.SendPath(PathProducer.GetSimplePath(npc.Position, new Vector2(128, 82), InitialDirection.Vertical));
                    return;
                case "Babysitting":
                    Reset();
                    path = MakeBabysittingTransitionPath();
                    npc.SendPath(path);
                    stopwatch.Start();
                    return;
                case "Sleeping":
                    Reset();
                    npc.SendPath(PathProducer.GetSimplePath(npc.Position, fsm.upstairsPosition, InitialDirection.Horizontal));
                    stopwatch.Start();
                    return;
            }
        }

        protected override void UpdateTransitionExt(Npc npc, GameTime gameTime, string transitionState)
        {
            switch (transitionState)
            {
                case "Patrolling":
                    npc.StepwiseMovement();

                    if (npc.PathComplete)
                        fsm.transitionComplete();

                    return;
                case "Babysitting":
                    SendSpeechBubble(3, npc, true);

                    if (stopwatch.ElapsedMilliseconds >= 2000)
                    {
                        SendSpeechBubble(4, npc, true);
                        stopwatch.Stop();
                    }

                    if (!stopwatch.IsRunning)
                        npc.StepwiseMovement();

                    if (npc.Position == fsm.chaseStartingPosition)
                    {
                        SendSpeechBubble(5, npc, true);
                        npc.CurrentAnimation = Direction.Down;
                        if (!npc.Speaking)
                            fsm.transitionComplete();
                    }
                    return;
                case "Sleeping":
                    // Send first speech bubble
                    SendSpeechBubble(1, npc, true);

                    // Move shopkeeper after first speech bubble has been displayed, until he reaches target position
                    if (stopwatch.ElapsedMilliseconds >= 2000 && npc.Position != new Vector2(fsm.upstairsPosition.X, fsm.behindCounterPosition.Y))
                        npc.StepwiseMovement();

                    // Once he reaches location, turn around and send second speech bubble
                    if (npc.Position == new Vector2(fsm.upstairsPosition.X, fsm.behindCounterPosition.Y))
                        if (stopwatch.ElapsedMilliseconds <= 5000)
                        {
                            npc.CurrentAnimation = Direction.Left;
                            npc.IsAnimating = false;
                            SendSpeechBubble(2, npc, true);
                        }
                        else
                            npc.StepwiseMovement();

                    // Send transition complete message to FSM if movement has finished
                    if (npc.PathComplete)
                    {
                        stopwatch.Stop();
                        fsm.transitionComplete();
                    }
                    return;
            }
        }

        #endregion

        #region Internal Method Region

        protected override SpeechBubble[] SetSpeech()
        {
            SpeechBubble[] speech = new SpeechBubble[7];
            speech[0] = new SpeechBubble("Greetings!", 2);
            speech[1] = new SpeechBubble("YAWN... I'm pretty tired.", 2);
            speech[2] = new SpeechBubble("Mind seeing yourself out?", 2);
            speech[3] = new SpeechBubble("What the...", 2);
            speech[4] = new SpeechBubble("Gah, not these kids again!", 2);
            speech[5] = new SpeechBubble("Would you help an old man catch this rabble?", 2);
            speech[6] = new SpeechBubble("Sir, can you please stay in front of the counter.", 2);
            return speech;
        }

        private Queue<Step> MakeBabysittingTransitionPath()
        {
            Queue<Vector2> waypoints = new Queue<Vector2>();
            Queue<InitialDirection> directions = new Queue<InitialDirection>();

            waypoints.Enqueue(fsm.behindCounterPosition);
            waypoints.Enqueue(fsm.topLeftCounterPosition);
            waypoints.Enqueue(fsm.chaseStartingPosition);

            directions.Enqueue(InitialDirection.Vertical);
            directions.Enqueue(InitialDirection.Vertical);

            return PathProducer.GetPathByWaypoint(waypoints, directions);
        }

        #endregion
    }
}
