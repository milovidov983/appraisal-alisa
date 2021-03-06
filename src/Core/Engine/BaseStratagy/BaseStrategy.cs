using AliceAppraisal.Core.Models;
using AliceAppraisal.Models;
using Serilog;
using System.Threading.Tasks;

namespace AliceAppraisal.Core.Engine {
	public abstract class BaseStrategy {
		protected readonly ILogger logger;

		private readonly IStepService stepService;

		protected BaseStrategy(IServiceFactory serviceFactory) {
			this.stepService = serviceFactory.GetStepService();
			this.logger = serviceFactory.GetLoggerFactory().GetLogger();
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
				stepService.ResetCustomStep();
			}
		}

		protected virtual void UpdateState(State state) {
			var nextStepName = stepService.GetNextStep(this);
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
			stepService.ChangeDefaultStepTo(customNextStep);
			return stepService.GetNextStrategy(this);
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

		protected SimpleResponse CreateNextStepHelp(string customNextStep = null) {
			var nextStepInstanse = GetNextStep(customNextStep);
			return nextStepInstanse.GetHelp();
		}

		protected SimpleResponse CreateNextStepMessageForUnknown(AliceRequest request, State state, string customNextStep = null) {
			var nextStepInstanse = GetNextStep(customNextStep);
			return nextStepInstanse.GetMessageForUnknown(request, state);
		}
	}
}
