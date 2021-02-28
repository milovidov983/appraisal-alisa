using AliceAppraisal.Models;
using AliceAppraisal.Static;
using System.Threading.Tasks;

namespace AliceAppraisal.Core.Engine.Strategy {
	public class HelpStrategy : BaseStrategy {
		public HelpStrategy(IServiceFactory serviceFactory) : base(serviceFactory) {
		}
		public override Task<SimpleResponse> GetMessage(AliceRequest request, State state) 
			=> CreateNextStepHelp(state.NextAction).FromTask();
		
		public override SimpleResponse GetMessageForUnknown(AliceRequest request, State state) 
			=> SimpleResponse.Empty;
		
		public override SimpleResponse GetHelp() 
			=> SimpleResponse.Empty;
		
		protected override bool Check(AliceRequest request, State state) 
			=> request.HasIntent(Intents.YandexHelp2);

		protected override Task<SimpleResponse> Respond(AliceRequest request, State state) 
			=> GetMessage(request, state);
		
	}
}