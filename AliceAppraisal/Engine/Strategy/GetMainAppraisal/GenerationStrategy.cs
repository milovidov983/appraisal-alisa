using AliceAppraisal.Engine.Services;
using AliceAppraisal.Models;
using AliceAppraisal.Static;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AliceAppraisal.Engine.Strategy {
	public class GenerationStrategy : BaseStrategy {
		public GenerationStrategy(IServiceFactory serviceFactory) : base(serviceFactory) {
		}

		protected override bool Check(AliceRequest request, State state) {
			return request.HasIntent(Intents.DigitInput) && state.PrevAction.Is(typeof(GetManufactureYearStrategy));
		}

		protected override async Task<SimpleResponse> Respond(AliceRequest request, State state) {
			await Task.Yield();
			var selectedGeneartion = request.GetSlot(Intents.DigitInput, Slots.Number);

			if (selectedGeneartion.IsNullOrEmpty()) {
				return CreateFailureResponse();
			}

			var newGenerationId = state.GetGenerationIdBySelected(selectedGeneartion);
			if(newGenerationId is null) {
				return CreateFailureResponse();
			}
				
			state.UpdateGenerationId(newGenerationId.Value, this);
			return textGeneratorService.CreateNextTextRequest(this);
		}

		private static SimpleResponse CreateFailureResponse() {
			return new SimpleResponse {
				Text = $"Не удалось распознать ваш ответ с поколением авто, попробуйте повторить ваш запрос.",
				Buttons = new[] { "Оценить другой авто", "Выйти" }
			};
		}
	}
}
