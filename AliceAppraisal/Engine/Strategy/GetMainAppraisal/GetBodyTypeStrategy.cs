using AliceAppraisal.Engine.Services;
using AliceAppraisal.Models;
using AliceAppraisal.Static;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AliceAppraisal.Engine.Strategy {
	public class GetBodyTypeStrategy : BaseStrategy {
		public GetBodyTypeStrategy(IServiceFactory serviceFactory) : base(serviceFactory) {
		}


		public override async Task<SimpleResponse> GetMessage(AliceRequest request, State state) {
			await Task.Yield();
			return new SimpleResponse {
				Text = $"Какой тип кузова у вашего авто? Например седан, хэчбек и так далее.",
				Buttons = Buttons.BodyTypesBtn
			};
		}

		public override SimpleResponse GetMessageForUnknown(AliceRequest request, State state) {
			return new SimpleResponse {
				Text = $"Не удалось распознать тип кузова вашего авто," +
				$" попробуйте повторить запрос или попросите у меня подсказку.",
				Buttons = Buttons.BodyTypesBtn
			};
		}

		public override SimpleResponse GetHelp() {
			return new SimpleResponse {
				Text = $"Для оценки автомобиля мне необходимо знать его тип кузова, существуют следующие " +
				$"типы кузовов: седан, хечбек, внедорожник, универсал, купе, лифтбек и другие. " +
				$"Попробуйте произнести название приблизив микрофон ближе.",
				Buttons = Buttons.BodyTypesBtn
			};
		}

		protected override bool Check(AliceRequest request, State state) {
			return request.HasIntent(Intents.BodyType) &&  state.NextAction.Is(this.GetType());
		}

		protected override async Task<SimpleResponse> Respond(AliceRequest request, State state) {
			await Task.Yield();
			var value = request.GetSlot(Intents.BodyType, Slots.Body);

			if (value.IsNullOrEmpty()) {
				return GetMessageForUnknown(request, state);
			}

			state.UpdateBodyType(value, this);

			var nextAction = GetNextStrategy();
			return await nextAction.GetMessage(request, state);
		}
	}
}
