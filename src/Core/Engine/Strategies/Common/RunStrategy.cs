using AliceAppraisal.Core.Engine.Services;
using AliceAppraisal.Models;
using AliceAppraisal.Static;
using System;
using System.Threading.Tasks;

namespace AliceAppraisal.Core.Engine.Strategy {
	public class RunStrategy : BaseStrategy {
		public RunStrategy(IServiceFactory serviceFactory) : base(serviceFactory) {
		}
		public override async Task<SimpleResponse> GetMessage(AliceRequest request, State state) {
			await Task.Yield();
			var randGiveWord = WordsCollection.GET_VERB.GetRand();
			return new SimpleResponse {
				Text = $"{randGiveWord} примерный пробег вашего авто",
			};
		}

		public override SimpleResponse GetMessageForUnknown(AliceRequest request, State state)
			=> new SimpleResponse {
				Text = $"Не удалось распознать указанный вами пробег, " +
				$"попробуйте повторить запрос или попросите у меня подсказку."
			};


		public override SimpleResponse GetHelp()
			=> new SimpleResponse {
				Text = $"Для оценки автомобиля мне необходимо знать его пробег, пробег необходимо указывать в километрах. " +
				$"Попробуйте произнести название приблизив микрофон ближе."
			};


		protected override bool Check(AliceRequest request, State state)
			=> request.HasIntent(Intents.DigitInput) && state.NextAction.Is(this.GetType());


		protected override Task<SimpleResponse> Respond(AliceRequest request, State state) {
			var runStr = request.GetSlot(Intents.DigitInput, Slots.Number);
			if (runStr.IsNullOrEmpty()) {
				return GetMessageForUnknown(request, state).FromTask();
			}
			if (!Int32.TryParse(runStr, out var run)) {
				return GetMessageForUnknown(request, state).FromTask();
			}

			state.UpdateRun(run);

			return CreateNextStepMessage(request, state);
		}
	}
}
