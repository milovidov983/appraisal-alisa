using AliceAppraisal.Models;
using AliceAppraisal.Static;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace AliceAppraisal.Core.Engine.Strategy {
	public class ChangeCityStrategy : BaseStrategy {
		private static readonly SimpleResponse help = new SimpleResponse {
			Text = $"Изменить город оцененного авто. " +
				$"На последнем шаге с результатами оценки " +
				$"вызывается командой \"Оцени такое же авто в городе Х\", " +
				$"где Х это город в России."
		};
		private static readonly SimpleResponse unknown = new SimpleResponse {
			Text = $"Не удалось распознать указанный вами город, попробуйте повторить ваш запрос."
		};

		public ChangeCityStrategy(IServiceFactory serviceFactory) : base(serviceFactory) {
		}
		public override Task<SimpleResponse> GetMessage(AliceRequest request, State state)
			=> GetHelp().FromTask();


		public override SimpleResponse GetMessageForUnknown(AliceRequest request, State state)
			=> unknown;

		public override SimpleResponse GetHelp()
			=> help;

		protected override bool Check(AliceRequest request, State state)
			=>
			request.HasIntent(Intents.ChangeParamCity)
			&&
			(state.NextAction.Is(typeof(StartAppraisalStrategy))
			|| 
			state.PrevAction.Is(typeof(CityStrategy)))
			;

		protected override Task<SimpleResponse> Respond(AliceRequest request, State state) {
			var city = request.GetSlot(Intents.ChangeParamCity, Slots.City);

			if (city.IsNullOrEmpty()) {
				return GetMessageForUnknown(request, state).FromTask();
			} else {
				try {
					city = JsonSerializer.Deserialize<CityDto>(city)?.City;
				} catch { }
			}
			var cityRegions = CityStrategy.CityRegions;
			if (!cityRegions.TryGetValue(city.ToLowerInvariant(), out var regionId)) {
				return GetMessageForUnknown(request, state).FromTask();
			}

			state.UpdateRegion(regionId, city);

			return CreateNextStepMessage(request, state);
		}
	}
}
