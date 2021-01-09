using AliceAppraisal.Engine.Services;
using AliceAppraisal.Models;
using AliceAppraisal.Static;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AliceAppraisal.Engine.Strategy {
	public class GearboxStrategy : BaseStrategy {
		public GearboxStrategy(IServiceFactory serviceFactory) : base(serviceFactory) {
		}

		protected override bool Check(AliceRequest request, State state) {
			return request.HasIntent(Intents.GearboxType);
		}

		protected override async Task<SimpleResponse> Respond(AliceRequest request, State state) {
			await Task.Yield();
			var gearbox = request.GetSlot(Intents.GearboxType, Slots.Gearbox);

			if (gearbox.IsNullOrEmpty()) {
				return new SimpleResponse {
					Text = $"Не удалось распознать тип коробки передач, попробуйте повторить ваш запрос.",
					Buttons = new[] { "Оценить другой авто", "Вернутся на шаг назад", "Выйти" }
				};
			}

			state.UpdateGearbox(gearbox, this);

			return textGeneratorService.CreateNextTextRequest(this);

		}
	}
}
