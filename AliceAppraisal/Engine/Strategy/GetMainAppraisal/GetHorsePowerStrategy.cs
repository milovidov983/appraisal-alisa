using AliceAppraisal.Engine.Services;
using AliceAppraisal.Models;
using AliceAppraisal.Static;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AliceAppraisal.Engine.Strategy {
	public class GetHorsePowerStrategy : BaseStrategy {
		public GetHorsePowerStrategy(IServiceFactory serviceFactory) : base(serviceFactory) {
		}

		public override async Task<SimpleResponse> GetMessage(AliceRequest request, State state) {
			await Task.Yield();
			return new SimpleResponse {
				Text = $"{textGeneratorService.GetRandTakeVerb()} примерное количество лошадиных сил в вашем авто?",
			};
		}

		public override SimpleResponse GetMessageForUnknown(AliceRequest request, State state) {
			return new SimpleResponse {
				Text = $"Не удалось распознать количество лошадиных сил, " +
				$"попробуйте повторить запрос или попросите у меня подсказку."
			};
		}

		public override SimpleResponse GetHelp() {
			return new SimpleResponse {
				Text = $"Для оценки автомобиля мне необходимо знать мощность двигателя, мощность необходимо указывать в лошадиных силах. " +
				$"Попробуйте произнести название приблизив микрофон ближе."
			};
		}
		protected override bool Check(AliceRequest request, State state) {
			return request.HasIntent(Intents.DigitInput) && state.NextAction.Is(this.GetType());
		}

		protected override async Task<SimpleResponse> Respond(AliceRequest request, State state) {
			await Task.Yield();
			var horsePowerStr = request.GetSlot(Intents.DigitInput, Slots.Number);

			if (horsePowerStr.IsNullOrEmpty()) {
				return GetMessageForUnknown(request, state);
			}
			if (!Int32.TryParse(horsePowerStr, out var horsePower)) {
				return GetMessageForUnknown(request, state);
			}


			state.UpdateHorsePower(horsePower, this);

			var nextAction = GetNextStrategy();
			return await nextAction.GetMessage(request, state);
		}
	}
}
