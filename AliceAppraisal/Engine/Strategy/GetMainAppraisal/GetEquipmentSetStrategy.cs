using AliceAppraisal.Engine.Services;
using AliceAppraisal.Models;
using AliceAppraisal.Static;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AliceAppraisal.Engine.Strategy {
	public class GetEquipmentSetStrategy : BaseStrategy {
		public GetEquipmentSetStrategy(IServiceFactory serviceFactory) : base(serviceFactory) {
		}
		public override async Task<SimpleResponse> GetMessage(AliceRequest request, State state) {
			await Task.Yield();
			var randGiveWord = WordsCollection.GET_VERB.GetRand();
			return new SimpleResponse {
				Text = $"{randGiveWord} комплектацию вашего авто? " +
				$"Например Базовая, Стандартная или Максимальная",
				Buttons = new[] { "Базовая", "Стандартная", "Максимальная" }
			};
		}

		public override SimpleResponse GetMessageForUnknown(AliceRequest request, State state) {
			return new SimpleResponse {
				Text = $"Не удалось распознать тип комплектации, " +
				$"попробуйте повторить запрос или попросите у меня подсказку."
			};
		}

		public override SimpleResponse GetHelp() {
			return new SimpleResponse {
				Text = $"Для оценки автомобиля мне необходимо знать его комплектацию, существуют следующие " +
				$"типы: Базовая, Стандартная и Максимальная. " +
				$"Попробуйте произнести название приблизив микрофон ближе."
			};
		}
		protected override bool Check(AliceRequest request, State state) {
			return request.HasIntent(Intents.EquipmentType) && state.NextAction.Is(this.GetType());
		}

		protected override async Task<SimpleResponse> Respond(AliceRequest request, State state) {
			await Task.Yield();
			var value = request.GetSlot(Intents.EquipmentType, Slots.Equipment);

			if (value.IsNullOrEmpty()) {
				return GetMessageForUnknown(request, state);
			}

			state.UpdateEquipmentSet(value, this);

			var nextAction = GetNextStrategy();
			return await nextAction.GetMessage(request, state);

		}
	}
}
