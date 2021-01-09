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

		protected override bool Check(AliceRequest request, State state) {
			return request.HasIntent(Intents.YandexReject)
				&& (state.PrevAction.Is(typeof(GetCityStrategy)) || state.PrevAction.Is(typeof(ChangeParamStrategy)));
		}

		protected override async Task<SimpleResponse> Respond(AliceRequest request, State state) {
			await Task.Yield();
			state.SaveCurrentStep(this);
			state.Clear();
			return new SimpleResponse {
				Text = $"Всего вам хорошего, до свидания.",
				Buttons = new[] { "Выйти" }
			};

		}
	}
}
