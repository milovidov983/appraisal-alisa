using AliceAppraisal.Models;
using AliceAppraisal.Static;
using System.Threading.Tasks;

namespace AliceAppraisal.Core.Engine.Strategy {
	public class ExitStrategy : BaseStrategy {
		public ExitStrategy(IServiceFactory serviceFactory) : base(serviceFactory) {
		}
		public override async Task<SimpleResponse> GetMessage(AliceRequest request, State state) {
			await Task.Yield();
			return new SimpleResponse {
				Text = "Выхожу из навыка оценки автомобилей. Хорошего дня.",
				Tts = "Выхожу из навыка оценки автомобилей - - хорошего дня"
			};
		}

		public override SimpleResponse GetMessageForUnknown(AliceRequest request, State state) 
			=> SimpleResponse.Empty;
		
		public override SimpleResponse GetHelp() 
			=> SimpleResponse.Empty;

		
		protected override bool Check(AliceRequest request, State state) 
			=> request.HasIntent(Intents.Exit) 
			|| state.NextAction.Is(typeof(StartAppraisalStrategy)) && request.HasIntent(Intents.YandexReject);
		
	
		protected override async Task<AliceResponse> CreateResponse(AliceRequest request, State state) {
			var resp = await base.CreateResponse(request, state);
			resp.Response.EndSession = true;
			return resp;
		}

		protected override Task<SimpleResponse> Respond(AliceRequest request, State state) {
			state.Clear();
			return GetMessage(request, state);
		}
	}
}
