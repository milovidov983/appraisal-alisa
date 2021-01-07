using AliceAppraisal.Engine.Services;
using AliceAppraisal.Models;
using AliceAppraisal.Static;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AliceAppraisal.Engine.Stratagy {
	public class MakeStratagy : BaseStratagy {
		public MakeStratagy(IServiceFactory serviceFactory) : base(serviceFactory) {
		}

		protected override bool Check(AliceRequest request, State state) {
			return request.HasIntent(Intents.MakeName);
		}

		protected override async Task<SimpleResponse> Respond(AliceRequest request, State state) {
			await Task.Yield();
			var make = request.GetSlot(Intents.MakeName, Slots.Make);

			if (make.IsNullOrEmpty()) {
				return new SimpleResponse {
					Text = $"Не удалось распознать марку вашего авто, попробуйте повторить ваш запрос.",
					Buttons = new[] { "Оценить другой авто", "Выйти" }
				};
			}

			var newMakeId = make.ExtractId() ?? throw new ArgumentException($"Не удалось извлечь ID марки из сущности {make}");
			state.UpdateMakeId(newMakeId, this);
			return textGeneratorService.CreateNextTextRequest(this);
		}

	}
}
