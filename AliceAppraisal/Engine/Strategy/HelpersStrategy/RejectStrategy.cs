using AliceAppraisal.Engine.Services;
using AliceAppraisal.Models;
using AliceAppraisal.Static;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AliceAppraisal.Engine.Strategy {
	public class RejectStratagy : BaseStrategy {
		public RejectStratagy(IServiceFactory serviceFactory) : base(serviceFactory) {
		}
		public override async Task<SimpleResponse> GetMessage(AliceRequest request, State state) {
			await Task.Yield();
			return new SimpleResponse {
				Text = $"Всего вам хорошего, до свидания.",
				Buttons = new[] { "Выйти" }
			};
		}

		public override SimpleResponse GetMessageForUnknown(AliceRequest request, State state) => GetHelp();

		public override SimpleResponse GetHelp()
			=> new SimpleResponse {
				Text = $"Для выхода скажите \"Выйти\".",
				Buttons = new[] { "Выйти" }
			};

		protected override bool Check(AliceRequest request, State state) 
			=> request.HasIntent(Intents.YandexReject);
		

		protected override async Task<SimpleResponse> Respond(AliceRequest request, State state) {
			state.Clear();
			return await GetMessage(request, state);
		}
	}
}
