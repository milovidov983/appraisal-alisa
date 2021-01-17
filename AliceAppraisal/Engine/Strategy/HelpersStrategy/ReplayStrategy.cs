using AliceAppraisal.Models;
using AliceAppraisal.Static;
using System.Threading.Tasks;

namespace AliceAppraisal.Engine.Strategy {
	public class ReplayStrategy : BaseWithoutChangeStepStrategy {
		public ReplayStrategy(IServiceFactory serviceFactory) : base(serviceFactory) {
		}
		public override async Task<SimpleResponse> GetMessage(AliceRequest request, State state) {
			var strategyFactory = serviceFactory.StrategyFactory;
			var stratagy = strategyFactory.GetStrategy(state.NextAction);
			return await stratagy.GetMessage(request, state);
		}

		public override SimpleResponse GetMessageForUnknown(AliceRequest request, State state) {
			return SimpleResponse.Empty;
		}

		public override SimpleResponse GetHelp() {
			return SimpleResponse.Empty;
		}

		protected override bool Check(AliceRequest request, State state) {
            return request.HasIntent(Intents.YandexRepeat);
		}

		protected override async Task<SimpleResponse> Respond(AliceRequest request, State state) {
			return await GetMessage(request, state);
		}
    }
}
