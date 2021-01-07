using AliceAppraisal.Engine.Services;
using AliceAppraisal.Models;
using AliceAppraisal.Static;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AliceAppraisal.Engine.Stratagy {
	public class ModelStratagy : BaseStratagy {
		public ModelStratagy(IServiceFactory serviceFactory) : base(serviceFactory) {
		}

		protected override bool Check(AliceRequest request, State state) {
			return request.HasIntent(Intents.ModelName);
		}

		protected override async Task<SimpleResponse> Respond(AliceRequest request, State state) {
			await Task.Yield();
			var model = request.GetSlot(Intents.ModelName, Slots.Model);

			if (model.IsNullOrEmpty()) {
				return new SimpleResponse {
					Text = $"Не удалось распознать модель вашего авто, попробуйте повторить ваш запрос.",
					Buttons = new[] { "Оценить другой авто", "Выйти" }
				};
			}

			var newModelId = model.ExtractId() ?? throw new ArgumentException($"Не удалось извлечь ID модели из сущности {model}");
			state.UpdateModelId(newModelId, this);
			return textGeneratorService.CreateNextTextRequest(this);

		}
	}
}
