using AliceAppraisal.Core.Models;
using AliceAppraisal.Helpers;
using AliceAppraisal.Models;
using AliceAppraisal.Static;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace AliceAppraisal.Core.Engine.Strategy {
	public class CityStrategy : BaseStrategy {

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

			var choice = request.HasScreen()
				? "Выберите"
				: "Назовите";

			return new SimpleResponse {
				Text = $"{choice} город для оценки {state.Request.MakeEntity.ExtractName().CapitalizeFirst()} " +
				$"{state.Request.GenerationValue} {state.Request.ManufactureYear} г.в." +
				$"Если не выбирать город то по умолчанию оценка будет произведена в Московском регионе, продолжить?",
				Buttons = new[] { "Продолжить" }.Union(Buttons.GetCityButtons()).Union( new[] { "Указать другой город"  }).ToArray()
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
				$"по умолчанию выбрана Москва. Если вас интересует другой регион то укажите столицу этого региона.",
				Buttons = new[] { "Продолжить", "Указать другой город" }
			};
		}

		protected override bool Check(AliceRequest request, State state) {
			return (
				request.HasIntent(Intents.YandexConfirm) 
				|| 
				CheckTokens(request) 
				|| 
				request.HasIntent(Intents.CityName) 
				) 
				&& 
				state.NextAction.Is(this.GetType());
		}

		public static Dictionary<string, int> CityRegions { get; }
		static CityStrategy() {
			CityRegions = new Dictionary<string, int>();
			foreach(var item in RegionItem.All) {
				CityRegions.TryAdd(item.CapitalName.ToLowerInvariant(), item.Code);
			}
		}

		private const string DEFAULT_CITY = "москва";

		public CityStrategy(IServiceFactory serviceFactory) : base(serviceFactory) {
		}

		protected override Task<SimpleResponse> Respond(AliceRequest request, State state) {
			var city = request.GetSlot(Intents.CityName, Slots.City);
			if (city.IsNullOrEmpty()) {
				city = DEFAULT_CITY;
			} else {
				try {
					city = JsonSerializer.Deserialize<CityDto>(city)?.City;
				} catch { }
			}

			if(!CityRegions.TryGetValue(city?.ToLowerInvariant() ?? DEFAULT_CITY, out var regionId)) {
				regionId = CityRegions[DEFAULT_CITY];
				city = DEFAULT_CITY;
			}
			state.UpdateRegion(regionId, city);

			return CreateNextStepMessage(request, state);
		}



	}
}
