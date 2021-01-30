using AliceAppraisal.Models;
using Serilog;
using System;
using System.Threading.Tasks;

namespace AliceAppraisal.Engine.Strategy {
	public abstract class BaseStrategy {
		protected readonly IExternalService externalService;
		protected readonly ILogger logger;
		private readonly IStepManager stepManager;

		protected BaseStrategy(IServiceFactory serviceFactory) {
			this.stepManager = new StepManager(this, serviceFactory);
			this.logger = serviceFactory.GetLogger();
			this.externalService = serviceFactory.GetExternalService();
		}

		public bool IsSuitableStrategy(AliceRequest request, State state)
			=> Check(request, state ?? new State());
		

		public Task<AliceResponse> Run(AliceRequest request, State state)
			=>  CreateResponse(request, state);
		


		protected virtual async Task<AliceResponse> CreateResponse(AliceRequest request, State state) {
			try {
				var simple = await Respond(request, state); // GetRequestTypes


				UpdateState(state);

				var response = AliceResponseBuilder.Create()
					.WithData(request)
					.WithState(state)
					.WithText(simple)
					.Build();

				return response;
			} finally {
				stepManager.ResetCustomStep();
			}
		}

		protected virtual void UpdateState(State state) {
			var nextStepName = stepManager.GetNextStep();
			state.SaveCurrentAndNextStep(this.GetType().FullName, nextStepName);
		}

		/// <summary>
		/// Проверка на пригодность стратегии для обработки запроса
		/// </summary>
		protected abstract bool Check(AliceRequest request, State state);
		protected abstract Task<SimpleResponse> Respond(AliceRequest request, State state);

		public abstract Task<SimpleResponse> GetMessage(AliceRequest request, State state);
		public abstract SimpleResponse GetHelp();
		public abstract SimpleResponse GetMessageForUnknown(AliceRequest request, State state);


		private BaseStrategy GetNextStep(string customNextStep = null) {
			stepManager.ChangeDefaultStepTo(customNextStep);
			return stepManager.GetNextStrategy();
		}
		/// <summary>
		/// Вспомогательный метод создания ответа от следующего шага диалога
		/// </summary>
		protected Task<SimpleResponse> CreateNextStepMessage(
			AliceRequest request, 
			State state, 
			string customNextStep = null) {
			var nextStepInstanse = GetNextStep(customNextStep);
			return nextStepInstanse.GetMessage(request, state);
		}

		/// <summary>
		/// Вспомогательный метод создания ответа от следующего шага диалога
		/// </summary>
		protected SimpleResponse CreateNextStepHelp(string customNextStep = null) {
			var nextStepInstanse = GetNextStep(customNextStep);
			return nextStepInstanse.GetHelp();
		}

		/// <summary>
		/// Вспомогательный метод создания ответа от следующего шага диалога
		/// </summary>
		protected SimpleResponse CreateNextStepMessageForUnknown(AliceRequest request, State state, string customNextStep = null) {
			var nextStepInstanse = GetNextStep(customNextStep);
			return nextStepInstanse.GetMessageForUnknown(request, state);
		}
	}
}
