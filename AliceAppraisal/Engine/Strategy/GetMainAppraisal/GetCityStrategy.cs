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

		protected override bool Check(AliceRequest request, State state) {
			return CheckTokens(request) || request.HasIntent(Intents.CityName);
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
			}
			state.UpdateRegion(regionId, this);

			return await textGeneratorService.CreateNextTextRequest(this, state);

		}
	}
}
