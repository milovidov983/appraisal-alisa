using AliceAppraisal.Engine.Services;
using AliceAppraisal.Models;
using AliceAppraisal.Static;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AliceAppraisal.Engine.Strategy {
	public class ChangeParamStrategy : BaseStrategy {
		public ChangeParamStrategy(IServiceFactory serviceFactory) : base(serviceFactory) {
		}

		protected override bool Check(AliceRequest request, State state) {
			return request.HasIntent(Intents.ChangeParamRun) 
				&& ( state.PrevAction.Is(typeof(GetCityStrategy)) || state.PrevAction.Is(typeof(ChangeParamStrategy)));
		}

		protected override async Task<SimpleResponse> Respond(AliceRequest request, State state) {
			await Task.Yield();
			var runStr = request.GetSlot(Intents.ChangeParamRun, Slots.Run);

			if (runStr.IsNullOrEmpty()) {
				return CreateFailureResponse();
			}
			if (!Int32.TryParse(runStr, out var run)) {
				return CreateFailureResponse();
			}

			state.UpdateRun(run, this);

			return await textGeneratorService.CreateFinalResult(state);

		}

		private static SimpleResponse CreateFailureResponse() {
			return new SimpleResponse {
				Text = $"Не удалось распознать указанный вами пробег, попробуйте повторить ваш запрос.",
				Buttons = new[] { "Оценить другой авто", "Вернутся на шаг назад", "Выйти" }
			};
		}
	}
}
