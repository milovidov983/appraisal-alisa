using AliceAppraisal.Engine.Services;
using AliceAppraisal.Models;
using AliceAppraisal.Static;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AliceAppraisal.Engine.Strategy {
	public class GetHorsePowerStrategy : BaseStrategy {
		public GetHorsePowerStrategy(IServiceFactory serviceFactory) : base(serviceFactory) {
		}

		protected override bool Check(AliceRequest request, State state) {
			return request.HasIntent(Intents.DigitInput) && state.PrevAction.Is(typeof(GetDriveTypeStrategy));
		}

		protected override async Task<SimpleResponse> Respond(AliceRequest request, State state) {
			await Task.Yield();
			var horsePowerStr = request.GetSlot(Intents.DigitInput, Slots.Number);

			if (horsePowerStr.IsNullOrEmpty()) {
				return CreateFailureResponse();
			}
			if (!Int32.TryParse(horsePowerStr, out var horsePower)) {
				return CreateFailureResponse();
			}


			state.UpdateHorsePower(horsePower, this);

			return textGeneratorService.CreateNextTextRequest(this);

		}

		private static SimpleResponse CreateFailureResponse() {
			return new SimpleResponse {
				Text = $"Не удалось распознать количество лошадиных сил, попробуйте повторить ваш запрос.",
				Buttons = new[] { "Оценить другой авто", "Вернутся на шаг назад", "Выйти" }
			};
		}
	}
}
