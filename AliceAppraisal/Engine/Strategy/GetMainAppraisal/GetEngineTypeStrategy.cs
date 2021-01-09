using AliceAppraisal.Engine.Services;
using AliceAppraisal.Models;
using AliceAppraisal.Static;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AliceAppraisal.Engine.Strategy {
	public class GetEngineTypeStrategy : BaseStrategy {
		public GetEngineTypeStrategy(IServiceFactory serviceFactory) : base(serviceFactory) {
		}
		public override async Task<SimpleResponse> GetMessage(AliceRequest request, State state) {
			await Task.Yield();
			return new SimpleResponse {
				Text = $"{textGeneratorService.GetRandTakeVerb()}  тип двигателя вашего авто? Например Бензиновый, Гибрид, Дизельный или Электрический",
				Buttons = new[] { "Бензиновый", "Гибрид", "Дизельный", "Электрический", "Оценить другой авто", "Выйти" }
			};
		}

		public override SimpleResponse GetMessageForUnknown(AliceRequest request, State state) {
			return new SimpleResponse {
				Text = $"Не удалось распознать тип двигателя, попробуйте повторить ваш запрос, попробуйте повторить запрос или попросите у меня подсказку."
			};
		}

		public override SimpleResponse GetHelp() {
			return new SimpleResponse {
				Text = $"Для оценки автомобиля мне необходимо знать его тип двигателя, существуют следующие " +
				$"типы: Бензиновый, Гибрид, Дизельный, Электрический и другие. " +
				$"Попробуйте произнести название приблизив микрофон ближе."
			};
		}
		protected override bool Check(AliceRequest request, State state) {
			return request.HasIntent(Intents.EngineType) &&  state.NextAction.Is(this.GetType());
		}

		protected override async Task<SimpleResponse> Respond(AliceRequest request, State state) {
			await Task.Yield();
			var value = request.GetSlot(Intents.BodyType, Slots.Body);

			if (value.IsNullOrEmpty()) {
				return GetMessageForUnknown(request, state);
			}
			state.UpdateEngineType(value, this);
			var nextAction = GetNextStrategy();
			return await nextAction.GetMessage(request, state);
		}
	}
}
