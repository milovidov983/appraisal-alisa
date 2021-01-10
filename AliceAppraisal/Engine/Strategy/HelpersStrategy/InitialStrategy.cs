using AliceAppraisal.Engine.Services;
using AliceAppraisal.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AliceAppraisal.Engine.Strategy {
	public class InitialStrategy : BaseStrategy {
		public InitialStrategy(IServiceFactory serviceFactory) : base(serviceFactory) {
		}
		public override async Task<SimpleResponse> GetMessage(AliceRequest request, State state) {
			await Task.Yield();

			return new SimpleResponse {
				Text = $"Привет я Бот обладающий навыком угадывать цену у подержанных автомобилей, " +
				$"хотите я попробую оценить стоимость вашего авто на вторичном рынке?",
				Buttons = new[] { "Да", "Нет", "Помощь", "Выйти" }
			};
		}

		public override SimpleResponse GetMessageForUnknown(AliceRequest request, State state) {
			return new SimpleResponse {
				Text = $"Хотите я попробую оценить стоимость вашего авто на вторичном рынке?",
				Buttons = new[] { "Да", "Нет", "Помощь", "Выйти" }
			};
		}
		public override SimpleResponse GetHelp() {
			return new SimpleResponse {
				Text = $"Привет я Бот обладающий навыком угадывать цену у подержанных автомобилей, " +
							$"хотите я попробую оценить стоимость вашего авто на вторичном рынке?",
				Buttons = new[] { "Да", "Нет", "Помощь", "Выйти" }
			};

		}
		protected override bool Check(AliceRequest request, State state) {
			return request.Request.Command.IsNullOrEmpty() && state.PrevAction.IsNullOrEmpty()
				|| state.NextAction.Is(this.GetType()); 
		}

		protected override async Task<SimpleResponse> Respond(AliceRequest request, State state) {
			return await GetMessage(request, state);
		}
	}
}
