using AliceAppraisal.Engine.Services;
using AliceAppraisal.Models;
using AliceAppraisal.Static;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AliceAppraisal.Engine.Strategy {
	public class GetEngineTypeStrategy : BaseStrategy {
		public GetEngineTypeStrategy(IServiceFactory serviceFactory) : base(serviceFactory) {
		}

		protected override bool Check(AliceRequest request, State state) {
			return request.HasIntent(Intents.EngineType);
		}

		protected override async Task<SimpleResponse> Respond(AliceRequest request, State state) {
			await Task.Yield();
			var engine = request.GetSlot(Intents.EngineType, Slots.Engine);

			if (engine.IsNullOrEmpty()) {
				return new SimpleResponse {
					Text = $"Не удалось распознать тип двигателя, попробуйте повторить ваш запрос.",
					Buttons = new[] { "Оценить другой авто", "Вернутся на шаг назад", "Выйти" }
				};
			}

			state.UpdateEngineType(engine, this);

			return textGeneratorService.CreateNextTextRequest(this);

		}
	}
}
