using AliceAppraisal.Engine.Services;
using AliceAppraisal.Helpers;
using AliceAppraisal.Models;
using AliceAppraisal.Static;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AliceAppraisal.Engine.Strategy {
	public class GetCityStrategy : BaseStrategy {

		protected List<string> Keywords { get; } = new List<string>
		{
			"продолжить"
		};
		protected bool CheckTokens(AliceRequest request) {
			return CheckTokens(
				tokens: request.Request?.Nlu?.Tokens ?? Array.Empty<string>().AsEnumerable(),
				expected: Keywords.ToArray());
		}
		private bool CheckTokens(IEnumerable<string> tokens, params string[] expected) {
			return expected.Any(expectedString => {
				var expectedTokens = expectedString.Split(" ");
				return expectedTokens.All(tokens.ContainsStartWith);
			});
		}
		public override async Task<SimpleResponse> GetMessage(AliceRequest request, State state) {
			await Task.Yield();
			return new SimpleResponse {
				Text = "По умолчанию оценка будет проведена для московского региона, " +
				"если вас устраивает московский регион, то просто скажите Продолжить. " +
				"Если вас интересует другой регион то укажите столицу этого региона.",
				Buttons = new[] { "Продолжить" }
			};
		}

		public override SimpleResponse GetMessageForUnknown(AliceRequest request, State state) {
			return new SimpleResponse {
				Text = $"Не удалось распознать регион, по умолчанию выбрана Москва."
			};
		}

		public override SimpleResponse GetHelp() {
			return new SimpleResponse {
				Text = $"Для оценки автомобиля мне необходимо знать для какого региона подбирать цену, " +
				$"по умолчанию выбрана Москва. Если вас интересует другой регион то укажите столицу этого региона."
			};
		}
		protected override bool Check(AliceRequest request, State state) {
			return (request.HasIntent(Intents.YandexConfirm) || CheckTokens(request) || request.HasIntent(Intents.CityName) ) 
				&& state.NextAction.Is(this.GetType());
		}

		private static readonly Dictionary<string, int> cityRegions;
		static GetCityStrategy() {
			cityRegions = new Dictionary<string, int>();
			foreach(var item in RegionItem.All) {
				cityRegions.TryAdd(item.CapitalName.ToLowerInvariant(), item.Code);
			}
		}

		private const string DEFAULT_CITY = "москва";

		public GetCityStrategy(IServiceFactory serviceFactory) : base(serviceFactory) {
		}

		protected override async Task<SimpleResponse> Respond(AliceRequest request, State state) {
			await Task.Yield();
			var city = request.GetSlot(Intents.CityName, Slots.City);
			if (city.IsNullOrEmpty()) {

				city = DEFAULT_CITY;
			}
			if(!cityRegions.TryGetValue(city.ToLowerInvariant(), out var regionId)) {
				regionId = cityRegions[DEFAULT_CITY];
				city = DEFAULT_CITY;
			}
			state.UpdateRegion(regionId, city, this);

			var nextAction = GetNextStrategy();
			return await nextAction.GetMessage(request, state);
		}

	}
}
