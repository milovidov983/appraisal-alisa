using AliceAppraisal.Engine.Services;
using AliceAppraisal.Models;
using AliceAppraisal.Static;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AliceAppraisal.Engine.Stratagy {
	public class RejectStratagy : BaseStratagy {
		public RejectStratagy(IServiceFactory serviceFactory) : base(serviceFactory) {
		}

		protected override bool Check(AliceRequest request, State state) {
			return request.HasIntent(Intents.YandexReject)
				&& (state.PrevAction.Is(typeof(CityStratagy)) || state.PrevAction.Is(typeof(ChangeParamStratagy)));
		}

		protected override async Task<SimpleResponse> Respond(AliceRequest request, State state) {
			await Task.Yield();
			state.SetPrevAction(this);
			state.Clear();
			return new SimpleResponse {
				Text = $"Всего вам хорошего, до свидания.",
				Buttons = new[] { "Выйти" }
			};

		}
	}
}
