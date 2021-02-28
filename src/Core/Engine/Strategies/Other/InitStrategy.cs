using AliceAppraisal.Models;
using AliceAppraisal.Static;
using System.Threading.Tasks;

namespace AliceAppraisal.Core.Engine.Strategy {
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

		protected override bool Check(AliceRequest request, State state) 
			=>	request.Session.New
				||
				request.HasIntent(Intents.AppraisalOther)
				||
				state.NextAction.Is(typeof(StartAppraisalStrategy))
				&&
				request.HasIntent(Intents.YandexConfirm);
		

		protected override async Task<SimpleResponse> Respond(AliceRequest request, State state) {
			state.FullReset();
			return await GetMessage(request, state);
		}

		public override Task<SimpleResponse> GetMessage(AliceRequest request, State state) 
			=> CreateNextStepMessage(request, state);
		

		public override SimpleResponse GetHelp()
			=> CreateNextStepHelp();
		

		public override SimpleResponse GetMessageForUnknown(AliceRequest request, State state)
			=> CreateNextStepMessageForUnknown(request, state);

	}
}
