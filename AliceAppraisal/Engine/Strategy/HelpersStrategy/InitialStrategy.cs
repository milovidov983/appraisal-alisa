using AliceAppraisal.Engine.Services;
using AliceAppraisal.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AliceAppraisal.Engine.Strategy {
	public class InitialStrategy : BaseStrategy {

		private readonly SimpleResponse[] MessageExamples;

		public InitialStrategy(IServiceFactory serviceFactory) : base(serviceFactory) {
			MessageExamples = new SimpleResponse[] {
				new SimpleResponse {
					Text = "{0} Я бот и я умею оценивать стоимость подержанных автомобилей, " +
					$"хотите я попробую оценить стоимость какого-нибудь авто?",
					Tts = "{0} - - Я бот - и я умею оценивать стоимость подержанных автомобилей, - " +
					$"хотите я попробую оценить стоимость какого-нибудь авто?",
				},
				new SimpleResponse {
					Text = "{0} я умею предсказывать стоимость подержанных автомобилей, " +
					$"хотите узнать стоимость какого-нибудь авто?",
					Tts = "{0} - - я умею предсказывать стоимость подержанных автомобилей, - " +
					$"хотите узнать стоимость какого-нибудь авто?",
				},
			};
		}

		public override async Task<SimpleResponse> GetMessage(AliceRequest request, State state) {
			await Task.Yield();

			var greeting = WordsCollection.GET_GREETING.GetRand();
			var resp = GetRandMessage();
			string.Format(resp.Text, greeting);
			string.Format(resp.Tts, greeting);

			resp.Buttons = new[] { "Да", "Нет", "Помощь", "Выйти" };

			return resp;
		}

		private SimpleResponse GetRandMessage() {
			var rand = new Random((int)DateTime.UtcNow.Ticks);
			var index = rand.Next(0, MessageExamples.Length - 1);
			return MessageExamples[index];
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
