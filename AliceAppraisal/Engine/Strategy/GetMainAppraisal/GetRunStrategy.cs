using AliceAppraisal.Engine.Services;
using AliceAppraisal.Models;
using AliceAppraisal.Static;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AliceAppraisal.Engine.Strategy {
	public class GetRunStrategy : BaseStrategy {
		public GetRunStrategy(IServiceFactory serviceFactory) : base(serviceFactory) {
		}

		protected override bool Check(AliceRequest request, State state) {
			return request.HasIntent(Intents.DigitInput) && state.PrevAction.Is(typeof(GetHorsePowerStrategy));
		}

		protected override async Task<SimpleResponse> Respond(AliceRequest request, State state) {
			await Task.Yield();
			var runStr = request.GetSlot(Intents.DigitInput, Slots.Number);

			if (runStr.IsNullOrEmpty()) {
				return CreateFailureResponse();
			}
			if (!Int32.TryParse(runStr, out var run)) {
				return CreateFailureResponse();
			}

			state.UpdateRun(run, this);
			return textGeneratorService.CreateNextTextRequest(this);

		}

		private static SimpleResponse CreateFailureResponse() {
			return new SimpleResponse {
				Text = $"Не удалось распознать указанный вами пробег, попробуйте повторить ваш запрос.",
				Buttons = new[] { "Оценить другой авто", "Вернутся на шаг назад", "Выйти" }
			};
		}
	}
}
