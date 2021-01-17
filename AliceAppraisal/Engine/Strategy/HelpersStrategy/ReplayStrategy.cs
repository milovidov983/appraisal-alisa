using AliceAppraisal.Models;
using AliceAppraisal.Static;
using System.Threading.Tasks;

namespace AliceAppraisal.Engine.Strategy {
	public class ReplayStrategy : BaseStrategy {
		public ReplayStrategy(IServiceFactory serviceFactory) : base(serviceFactory) { }
		public override Task<SimpleResponse> GetMessage(AliceRequest request, State state) 
			=> CreateNextStepMessage(request, state, state.NextAction);
		
		public override SimpleResponse GetMessageForUnknown(AliceRequest request, State state) 
			=> SimpleResponse.Empty;

		public override SimpleResponse GetHelp() 
			=> SimpleResponse.Empty;

		protected override bool Check(AliceRequest request, State state) 
			=> request.HasIntent(Intents.YandexRepeat);

		protected override Task<SimpleResponse> Respond(AliceRequest request, State state) 
			=> GetMessage(request, state);
	}
}