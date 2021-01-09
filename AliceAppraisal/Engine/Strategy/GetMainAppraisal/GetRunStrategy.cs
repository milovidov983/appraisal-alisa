using AliceAppraisal.Engine.Services;
using AliceAppraisal.Models;
using AliceAppraisal.Static;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AliceAppraisal.Engine.Strategy {
	public class GetRunStrategy : BaseStrategy {
		public GetRunStrategy(IServiceFactory serviceFactory) : base(serviceFactory) {
		}
		public override async Task<SimpleResponse> GetMessage(AliceRequest request, State state) {
			await Task.Yield();
			return new SimpleResponse {
				Text = $"{textGeneratorService.GetRandTakeVerb()}  примерный пробег вашего авто?",
			};
		}

		public override SimpleResponse GetMessageForUnknown(AliceRequest request, State state) {
			return new SimpleResponse {
				Text = $"Не удалось распознать указанный вами пробег, " +
				$"попробуйте повторить запрос или попросите у меня подсказку."
			};
		}

		public override SimpleResponse GetHelp() {
			return new SimpleResponse {
				Text = $"Для оценки автомобиля мне необходимо знать его пробег, пробег необходимо указывать в километрах. " +
				$"Попробуйте произнести название приблизив микрофон ближе."
			};
		}
		protected override bool Check(AliceRequest request, State state) {
			return request.HasIntent(Intents.DigitInput) && state.NextAction.Is(this.GetType());
		}

		protected override async Task<SimpleResponse> Respond(AliceRequest request, State state) {
			await Task.Yield();
			var runStr = request.GetSlot(Intents.DigitInput, Slots.Number);

			if (runStr.IsNullOrEmpty()) {
				return GetMessageForUnknown(request, state);
			}
			if (!Int32.TryParse(runStr, out var run)) {
				return GetMessageForUnknown(request, state);
			}

			state.UpdateRun(run, this);
			var nextAction = GetNextStrategy();
			return await nextAction.GetMessage(request, state);

		}


	}
}
