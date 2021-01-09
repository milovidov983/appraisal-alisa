using AliceAppraisal.Engine.Services;
using AliceAppraisal.Models;
using AliceAppraisal.Static;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AliceAppraisal.Engine.Strategy {
	public class WhatCanYouDoStrategy : BaseStrategy {
		public WhatCanYouDoStrategy(IServiceFactory serviceFactory) : base(serviceFactory) {
		}

		protected override bool Check(AliceRequest request, State state) {
            return request.HasIntent(Intents.YandexHelp1);
		}

        protected override async Task<SimpleResponse> Respond(AliceRequest request, State state) {
			await Task.Yield();
			
			return new SimpleResponse {
				Text = $"Я Бот обладающий навыком угадывать цену у подержанных автомобилей.",
				Buttons = new[] { "Оценить другой авто", "Выйти" }
			};
		}
    }
}
