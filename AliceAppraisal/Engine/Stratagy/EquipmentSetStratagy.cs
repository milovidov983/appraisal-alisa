using AliceAppraisal.Engine.Services;
using AliceAppraisal.Models;
using AliceAppraisal.Static;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AliceAppraisal.Engine.Stratagy {
	public class EquipmentSetStratagy : BaseStratagy {
		public EquipmentSetStratagy(IServiceFactory serviceFactory) : base(serviceFactory) {
		}

		protected override bool Check(AliceRequest request, State state) {
			return request.HasIntent(Intents.EquipmentType);
		}

		protected override async Task<SimpleResponse> Respond(AliceRequest request, State state) {
			await Task.Yield();
			var equipment = request.GetSlot(Intents.EquipmentType, Slots.Equipment);

			if (equipment.IsNullOrEmpty()) {
				return new SimpleResponse {
					Text = $"Не удалось распознать тип комплектации, попробуйте повторить ваш запрос.",
					Buttons = new[] { "Оценить другой авто", "Вернутся на шаг назад", "Выйти" }
				};
			}

			state.UpdateEquipmentSet(equipment, this);

			return textGeneratorService.CreateNextTextRequest(this);

		}
	}
}
