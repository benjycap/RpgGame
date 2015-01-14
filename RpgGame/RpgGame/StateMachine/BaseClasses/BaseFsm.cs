using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

using RpgGame.SpriteClasses;
using RpgGame.NpcClasses;
using RpgGame.NpcClasses.Actions.Speech;
using RpgGame.ClockClasses;

namespace RpgGame.StateMachine.BaseClasses
{
    public abstract class BaseFsm
    {
        #region Fields

        // Game Ref
        static Game gameRef;
        // In game clock reference for use by states 
        static Clock clock;
        // Field for access to the game's conversations
        static List<Conversation> conversations;
        // Player reference in order to identify player location
        static AnimatedSprite player;

        // Reference to NPC that the FSM is paired with
        protected Npc npc;

        protected TransitionDictionary transitions;
        protected StateDictionary stateDictionary;
        protected StateInstanceDictionary stateInstances;

        List<string> validStates;

        // Reference to current state to be used in execution
        protected BaseState currentState;
        protected string currentStateName;
        // Nominal identity of the next state to be used in execution
        protected string transitionState;

        #endregion

        #region Flags

        // True if the state machine currently carrying out a transition
        protected bool inTransition;
        // False if the state machine needs to execute the init method of a transition or state
        protected bool doneInit;

        #endregion

        #region Public Properties

        public List<string> ValidStates
        {
            get
            {
                if (validStates == null)
                {
                    validStates = new List<string>();
                    foreach (string item in stateDictionary.Keys)
                        validStates.Add(item);
                }
                return validStates;
            }
        }

        public TransitionDictionary Transitions
        {
            get { return transitions; }
        }

        // True if this is the very first state executed by the state machine
        // This is used to initilise properties of the NPC such as position
        public bool FirstPass { get; protected set; }

        // Give states access to the in game clock
        public TimeSpan Time { get { return clock.Time; } }

        // Access to the game's list of conversations
        public List<Conversation> Conversations { get { return conversations; } }

        // Return player sprite
        public AnimatedSprite PlayerSprite { get { return player; } }

        // Return current state as string
        public string CurrentState { get { return currentStateName; } }

        // Return next state as string
        public string TransitionState { get { return transitionState; } }

        // Return true if in transition
        public bool InTransition { get { return inTransition; } }

        #endregion

        #region Constructor Region

        // This constructor which passes the Game type parameter must be ran before any derived Fsm class is created
        public BaseFsm(Game game, AnimatedSprite player)
        {
            // Hold reference to game
            gameRef = game;
            // Load clock from game services
            clock = (Clock)game.Services.GetService(typeof(Clock));
            // Load conversations from game services
            conversations = (List<Conversation>)game.Services.GetService(typeof(List<Conversation>));
            // Player reference
            BaseFsm.player = player;
        }

        public BaseFsm(Npc npc)
        {
            if (clock == null)
                throw new NullReferenceException("BaseFsm has not been initialised with appropriate constructor");

            this.npc = npc;

            // Transitions are imported from TransitionData.xml
            transitions = XMLImporter.GetTransitions(npc);
            // State dictionary is created as per abstract method ConstructStateDictionary
            stateDictionary = ConstructStateDictionary();
            // Instances of the appropriate states are created and populated via the StateList constructor
            stateInstances = new StateInstanceDictionary(stateDictionary, this);

            // Initialise flags
            inTransition = false;
            doneInit = false;
            FirstPass = true;

            InitialiseCurrentState();
        }

        // Every derived FSM must implement the following method which returns a state dictionary for
        // assignment to the stateDictionary field
        protected abstract StateDictionary ConstructStateDictionary();

        // Sets the current state based on certain conditions
        protected abstract void InitialiseCurrentState();

        #endregion

        #region Public Method Region

        // Execution engine behind the state machine
        public void Execute(GameTime gameTime)
        {
            // If Entry flag is set to false, carry out entry method
            if (!doneInit)
            {
                // Use transitions method if inTransition is true, else use the regular update method
                if (inTransition)
                    currentState.InitTransition(npc, transitionState);
                else
                    currentState.Init(npc);

                doneInit = true;
                FirstPass = false;
            }

            // Run appropriate update method
            if (inTransition)
                currentState.UpdateTransition(npc, gameTime, transitionState);
            else
                currentState.Update(npc, gameTime);
        }

        // Method to be called by the current state to request a state change according to passed parameter
        public void ChangeState(string newState)
        {
            if (!stateInstances.Keys.Contains(newState))
                throw new ArgumentException("The newState parameter must match a state's key as designated in the StateInstanceDictionary");
            if (!transitions[currentStateName].Contains(newState))
                throw new ArgumentException("The newState parameter must be legal according to the TransitionDictionary");

            // Stop speech bubble in progress
            npc.AbortSpeechBubble();
            // Set entry flag to false so entry method is run
            doneInit = false;
            // Set transition flag to true so transition methods are run
            inTransition = true;

            transitionState = newState;
        }

        // Method called by a state to request movement of execution from transition to state
        public void transitionComplete()
        {
            // Set transition flag to false
            inTransition = false;
            // Set doneInit flag to false
            doneInit = false;

            // Update currentState field to represent the new state we have transitioned to
            currentState = stateInstances[transitionState];
            currentStateName = transitionState;
        }

        #endregion

    }
}
