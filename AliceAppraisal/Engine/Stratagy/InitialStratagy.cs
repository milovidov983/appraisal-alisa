using AliceAppraisal.Engine.Services;
using AliceAppraisal.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AliceAppraisal.Engine.Stratagy {
	public class InitialStratagy : BaseStratagy {
		public InitialStratagy(IServiceFactory serviceFactory) : base(serviceFactory) {
		}

		protected override bool Check(AliceRequest request, State state) {
			return request.Request.Command.IsNullOrEmpty() && state.PrevAction.IsNullOrEmpty(); 
		}

		protected override async Task<SimpleResponse> Respond(AliceRequest request, State state) {
			await Task.Yield();
			state.SetPrevAction(this);
			return new SimpleResponse {
				Text = $"Привет я Бот обладающий навыком угадывать цену у подержанных автомобилей, " +
				$"хотите я попробую оценить стоимость вашего авто на вторичном рынке?",
				Buttons = new[] { "Да", "Нет", "Выйти" }
			};

		}
	}
}
