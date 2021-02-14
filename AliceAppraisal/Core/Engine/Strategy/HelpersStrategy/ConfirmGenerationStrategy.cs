using AliceAppraisal.Engine.Services;
using AliceAppraisal.Models;
using AliceAppraisal.Static;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AliceAppraisal.Engine.Strategy {
	public class ConfirmGenerationStrategy : BaseStrategy {
		public ConfirmGenerationStrategy(IServiceFactory serviceFactory) : base(serviceFactory) {
		}

		public override SimpleResponse GetHelp() {
			return new SimpleResponse {
				Text = $"Вам необходимо ответить подходит-ли найденное поколение для вашего авто.",
				Buttons = Buttons.YesNo
			};
		}

		public override Task<SimpleResponse> GetMessage(AliceRequest request, State state) {
			var value = state.GenerationChoise.Values.First();

			return Task.FromResult(
				new SimpleResponse {
					Text = $"Я выбрал поколение {value.Name} для вашего авто, скажите это правильное поколение?",
					Buttons = Buttons.YesNo
				}
			);
		}

		public override SimpleResponse GetMessageForUnknown(AliceRequest request, State state) 
			=> new SimpleResponse {
				Text = $"Мне не удалось понять это поколение соответствует вашему или нет?",
				Buttons = Buttons.YesNo
			};

		protected override bool Check(AliceRequest request, State state) 
			=> (request.HasIntent(Intents.YandexConfirm) || request.HasIntent(Intents.YandexReject))
				&& state.NextAction.Is(typeof(ConfirmGenerationStrategy));
		

		protected override Task<SimpleResponse> Respond(AliceRequest request, State state) {
			string customNextStep = null;
			if (request.HasIntent(Intents.YandexConfirm)) {
				var value = state.GenerationChoise.Values.FirstOrDefault()
					?? throw new ArgumentException("Ожидалось что есть одно выбранное поколение.");

				state.UpdateGenerationId(value.Id, value.Name);
			} else {
				customNextStep = typeof(GetManufactureYearStrategy).FullName;
			}
			return CreateNextStepMessage(request, state, customNextStep);
		}
	}
}