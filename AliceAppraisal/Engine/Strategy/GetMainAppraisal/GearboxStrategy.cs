using AliceAppraisal.Engine.Services;
using AliceAppraisal.Models;
using AliceAppraisal.Static;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AliceAppraisal.Engine.Strategy {
	public class GearboxStrategy : BaseStrategy {
		public GearboxStrategy(IServiceFactory serviceFactory) : base(serviceFactory) {
		}

		public override async Task<SimpleResponse> GetMessage(AliceRequest request, State state) {
			await Task.Yield();
			return new SimpleResponse {
				Text = "Какая коробка передач установлена в вашем авто? Например автомат, механика и так далее",
				Buttons = new[] { "Автомат", "Робот", "Механика", "Вариатор" }
			};
		}

		public override SimpleResponse GetMessageForUnknown(AliceRequest request, State state) {
			return new SimpleResponse {
				Text = $"Не удалось распознать тип коробки передач, попробуйте повторить запрос или попросите у меня подсказку."
			};
		}

		public override SimpleResponse GetHelp() {
			return new SimpleResponse {
				Text = $"Для оценки автомобиля мне необходимо знать его тип коробки передач, " +
				$"существуют следующие типы коробок: Автомат, Робот, Механика, Вариатор. " +
				$"Попробуйте произнести название приблизив микрофон ближе."
			};
		}
		protected override bool Check(AliceRequest request, State state) {
			return request.HasIntent(Intents.GearboxType) && state.NextAction.Is(typeof(GearboxStrategy));
		}

		protected override async Task<SimpleResponse> Respond(AliceRequest request, State state) {
			await Task.Yield();
			var value = request.GetSlot(Intents.BodyType, Slots.Body);

			if (value.IsNullOrEmpty()) {
				return GetMessageForUnknown(request, state);
			}

			state.UpdateGearbox(value, this);


			var nextAction = GetNextStrategy();
			return await nextAction.GetMessage(request, state);


		}
	}
}
