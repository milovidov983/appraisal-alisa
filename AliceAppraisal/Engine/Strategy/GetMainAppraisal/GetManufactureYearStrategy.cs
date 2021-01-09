using AliceAppraisal.Engine.Services;
using AliceAppraisal.Models;
using AliceAppraisal.Static;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AliceAppraisal.Engine.Strategy {
	public class GetManufactureYearStrategy : BaseStrategy {
		public GetManufactureYearStrategy(IServiceFactory serviceFactory) : base(serviceFactory) {
		}

		protected override bool Check(AliceRequest request, State state) {
			return request.HasIntent(Intents.DigitInput) && state.PrevAction.Is(typeof(GetModelStrategy));
		}

		protected override async Task<SimpleResponse> Respond(AliceRequest request, State state) {
			await Task.Yield();
			var year = request.GetSlot(Intents.DigitInput, Slots.Number);

			if (year.IsNullOrEmpty()) {
				return new SimpleResponse {
					Text = $"Не удалось распознать год выпуска вашего авто, попробуйте повторить ваш запрос.",
					Buttons = new[] { "Оценить другой авто", "Выйти" }
				};
			}

			var isCorrectConverted = Int32.TryParse(year, out var manufactureYear);
			if (!isCorrectConverted) {
				throw new ArgumentException($"Не удалось извлечь год выпуска из сущности {year}");
			}
			state.UpdateManufactureYear(manufactureYear, this);
			return await textGeneratorService.CreateNextTextRequest(this, state);
		}
	}
}
