using AliceAppraisal.Engine.Services;
using AliceAppraisal.Models;
using AliceAppraisal.Static;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AliceAppraisal.Engine.Stratagy {
	public class BodyTypeStratagy : BaseStratagy {
		public BodyTypeStratagy(IServiceFactory serviceFactory) : base(serviceFactory) {
		}

		protected override bool Check(AliceRequest request, State state) {
			return request.HasIntent(Intents.BodyType);
		}

		protected override async Task<SimpleResponse> Respond(AliceRequest request, State state) {
			await Task.Yield();
			var body = request.GetSlot(Intents.BodyType, Slots.Body);

			if (body.IsNullOrEmpty()) {
				return new SimpleResponse {
					Text = $"Не удалось распознать тип кузова, попробуйте повторить ваш запрос.",
					Buttons = new[] { "Оценить другой авто", "Выйти" }
				};
			}

			state.UpdateBodyType(body, this);

			return textGeneratorService.CreateNextTextRequest(this);

		}
	}
}
