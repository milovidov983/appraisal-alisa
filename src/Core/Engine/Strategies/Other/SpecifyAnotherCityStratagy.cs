using AliceAppraisal.Models;
using AliceAppraisal.Static;
using System;
using System.Threading.Tasks;

namespace AliceAppraisal.Core.Engine.Strategy {
	public class SpecifyAnotherCityStratagy : BaseStrategy {
		private static readonly SimpleResponse help = new SimpleResponse {
			Text = $"Изменить город оцененного авто. Корректными считаются города столицы своих регионов."
		};
		private static readonly SimpleResponse unknown = new SimpleResponse {
			Text = $"Не удалось распознать указанный вами город, попробуйте повторить ваш запрос."
		};

		public SpecifyAnotherCityStratagy(IServiceFactory serviceFactory) : base(serviceFactory) {
		}
		public override Task<SimpleResponse> GetMessage(AliceRequest request, State state)
			=> (new SimpleResponse { Text = "Укажите столицу региона для которого необходимо оценить авто" }).FromTask();


		public override SimpleResponse GetMessageForUnknown(AliceRequest request, State state)
			=> unknown;

		public override SimpleResponse GetHelp()
			=> help;

		protected override bool Check(AliceRequest request, State state)
			=>
			request.HasIntent(Intents.SpecifyAnotherCity)
			&&
			state.NextAction.Is(typeof(CityStrategy));

		protected override Task<SimpleResponse> Respond(AliceRequest request, State state) {
			return GetMessage(request, state);
		}
	}
}
