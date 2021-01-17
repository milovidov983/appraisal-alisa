using AliceAppraisal.Models;
using AliceAppraisal.Static;
using System.Threading.Tasks;

namespace AliceAppraisal.Engine.Strategy {
	/// <summary>
	/// Первая стратегия.
	/// Либо
	/// Обрабатывает реплики:
	/// ((Оценить другой авто)|(Начать с начала)|(начать оценку))
	/// Либо обрабатывает согласие на начало новой оценки после уже совершенной
	/// </summary>
	public class InitStrategy : BaseStrategy {
		public InitStrategy(IServiceFactory serviceFactory) : base(serviceFactory) {
		}

		protected override bool Check(AliceRequest request, State state) {
			return
				request.Session.New
				||
				request.HasIntent(Intents.AppraisalOther)
				||
				state.NextAction.Is(typeof(StartAppraisalStrategy))
				&&
				request.HasIntent(Intents.YandexConfirm);
		}

        protected override async Task<SimpleResponse> Respond(AliceRequest request, State state) {
			state.Clear();
			return await GetMessage(request, state);
		}

		public override async Task<SimpleResponse> GetMessage(AliceRequest request, State state) {
			var nextAction = GetNextStrategy();
			return await nextAction.GetMessage(request, state);
		}

		public override SimpleResponse GetHelp() {
			var nextAction = GetNextStrategy();
			return nextAction.GetHelp();
		}

		public override SimpleResponse GetMessageForUnknown(AliceRequest request, State state) {
			var nextAction = GetNextStrategy();
			return nextAction.GetMessageForUnknown(request, state);
		}
	}
}
