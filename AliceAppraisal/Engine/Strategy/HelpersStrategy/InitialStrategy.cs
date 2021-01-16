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
				Text = $"Привет! Я бот и я умею оценивать стоимость подержанных автомобилей, " +
				$"хотите я попробую оценить стоимость какого-нибудь авто?",
				Tts = $"Привет! - - Я бот - и я умею оценивать стоимость подержанных автомобилей, - " +
				$"хотите я попробую оценить стоимость какого-нибудь авто?",
				Buttons = new[] { "Да", "Нет", "Помощь", "Выйти" }
			};
		}

		public override SimpleResponse GetMessageForUnknown(AliceRequest request, State state) {
			// Текущая команда не может быть активирована
			return SimpleResponse.Empty;
		}
		public override SimpleResponse GetHelp() {
			// Текущая команда не может быть активирована
			return SimpleResponse.Empty;
		}

		protected override bool Check(AliceRequest request, State state) {
			return request.Session.New;
		}

		protected override async Task<SimpleResponse> Respond(AliceRequest request, State state) {
			return await GetMessage(request, state);
		}
	}
}
