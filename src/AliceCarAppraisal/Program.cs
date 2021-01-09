using Appccelerate.StateMachine;
using Appccelerate.StateMachine.AsyncMachine;
using System;
using System.Collections.Generic;

namespace AliceCarAppraisal {
	class Program {
		static void Main(string[] args) {
			Console.WriteLine("Hello World!");
		}
	}
}

//	public class StackViewStateMachine {
//		public class Context {
//			public static Dictionary<States, IHandler> handlers = new Dictionary<States, IHandler>();
//			public string Request;
//			public string UserState;
//		}

//		public enum States {
//			GetMake,
//			GetModel,
//			ShowingPotentialMovement,
//			Moving,
//			Exploring,
//			Patrolling,
//			Fortified
//		}

//		public enum Events {
//			RequestWithMake,
//			Unselect,
//			ShowPotentialMovement,
//			ResetPotentialMovement,
//			Move,
//			RequestWithModel,
//			Patrol,
//			Fortify
//		}

//		public bool IsMakeValid(Context context) {
//			return true;
//		}

//		public void ExecuteCurrentState(Context context) {
//			//return true;
//		}

//		public void HandleInvalidRequest(Context context) {

//		}

//		private PassiveStateMachine<States, Events> Machine { get; }

//		internal StackViewStateMachine() {
//			var builder = new StateMachineDefinitionBuilder<States, Events>();

//			builder
//				.In(States.GetMake)
//				.On(Events.RequestWithMake)
//				.If<Context>(IsMakeValid)
//					.Goto(States.GetModel).Execute<Context>(ExecuteCurrentState)
//					.Otherwise().Execute<Context>(HandleInvalidRequest);


//			builder
//				.In(States.GetModel)
//				.On(Events.RequestWithModel)
//				.Goto(States.Exploring)
//				.Execute<(StackView, WorldView)>(TransitionToExploring);



//			builder
//				.WithInitialState(States.GetMake);

//			Machine = builder
//				.Build()
//				.CreatePassiveStateMachine();

//			Machine.Start();
//		}

//		private void TransitionToNormal(StackView args) {
//			args.SetStackViewState(new StackViewNormalState(args));
//		}

//		private void TransitionToSelected(StackView args) {
//			args.SetStackViewState(new StackViewSelectedState(args));
//		}

//		private void TransitionToMoving(StackView args) {
//			args.SetStackViewState(new StackViewMovingState(args));
//		}

//		private void TransitionToExploring((StackView stackView, WorldView worldView) args) {
//			args.stackView.SetStackViewState(new StackViewExploringState(args.stackView, args.worldView));
//		}

//		private void TransitionToPatrolling(StackView args) {
//			args.SetStackViewState(new StackViewPatrollingState(args));
//		}

//		private void TransitionToFortified(StackView args) {
//			args.SetStackViewState(new StackViewFortifiedState(args));
//		}

//		private void TransitionToShowingPotentialMovement(StackView args) {
//			args.SetStackViewState(new StackViewShowingPotentialMovementState(args));
//		}

//		internal void Select(StackView stackView) {
//			Machine.Fire(Events.RequestWithMake, stackView);
//		}

//		internal void Unselect(StackView stackView) {
//			Machine.Fire(Events.Unselect, stackView);
//		}

//		internal void Move(StackView stackView) {
//			Machine.Fire(Events.Move, stackView);
//		}

//		internal void Explore(StackView stackView, WorldView worldView) {
//			Machine.Fire(Events.RequestWithModel, (stackView, worldView));
//		}

//		internal void Patrol(StackView stackView) {
//			Machine.Fire(Events.Patrol, stackView);
//		}

//		internal void Fortify(StackView stackView) {
//			Machine.Fire(Events.Fortify, stackView);
//		}

//		internal void ShowPotentialMovement(StackView stackView) {
//			Machine.Fire(Events.ShowPotentialMovement, stackView);
//		}

//		internal void ResetPotentialMovement(StackView stackView) {
//			Machine.Fire(Events.ResetPotentialMovement, stackView);
//		}
//	}
