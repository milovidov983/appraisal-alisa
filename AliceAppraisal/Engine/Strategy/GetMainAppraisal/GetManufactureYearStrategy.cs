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

		public override SimpleResponse GetHelp() {
			return new SimpleResponse {
				Text = $"Для оценки автомобиля мне необходимо знать его год выпуска. "+
					$"Попробуйте произнести название приблизив микрофон ближе."
			};
		}

		public override async Task<SimpleResponse> GetMessage(AliceRequest request, State state) {
			await Task.Yield();
			return new SimpleResponse {
				Text = $"{textGeneratorService.GetRandTakeVerb()} год выпуска вашего автомобиля?"
			};
		}

		public override SimpleResponse GetMessageForUnknown(AliceRequest request, State state) {
			return new SimpleResponse {
				Text = $"Не удалось распознать год выпуска вашего авто, " +
				$" попробуйте повторить ваш запрос или попросите у меня подсказку."
			};
		}

		protected override bool Check(AliceRequest request, State state) {
			return request.HasIntent(Intents.DigitInput) && state.NextAction.Is(typeof(GetManufactureYearStrategy));
		}

		protected override async Task<SimpleResponse> Respond(AliceRequest request, State state) {
			var value = request.GetSlot(Intents.DigitInput, Slots.Number);

			if (value.IsNullOrEmpty()) {
				return GetMessageForUnknown(request, state);
			}

			var isCorrectConverted = Int32.TryParse(value, out var manufactureYear);
			if (!isCorrectConverted) {
				return GetMessageForUnknown(request, state);
			}
			state.UpdateManufactureYear(manufactureYear, this);

			var nextAction = GetNextStrategy();
			return await nextAction.GetMessage(request, state);
		}
	}
}
