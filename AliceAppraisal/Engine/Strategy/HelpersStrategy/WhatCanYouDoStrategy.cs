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
		public override async Task<SimpleResponse> GetMessage(AliceRequest request, State state) {
			await Task.Yield();
			return new SimpleResponse {
				Text = $"Я Бот обладающий навыком угадывать цену у подержанных автомобилей. " +
				$"Что бы запустить оценку скажите Начать оценку."
			};
		}

		public override SimpleResponse GetMessageForUnknown(AliceRequest request, State state) {
			return new SimpleResponse {
				Text = $"Я Бот обладающий навыком угадывать цену у подержанных автомобилей. " +
				$"Что бы запустить оценку скажите Начать оценку."
			};
		}
		public override SimpleResponse GetHelp() {
			return new SimpleResponse {
				Text = $"Я Бот обладающий навыком угадывать цену у подержанных автомобилей. " +
				$"Что бы запустить оценку скажите Начать оценку."
			};
		}
		protected override bool Check(AliceRequest request, State state) {
            return request.HasIntent(Intents.YandexHelp1);
		}

        protected override async Task<SimpleResponse> Respond(AliceRequest request, State state) {
			return await GetMessage(request, state);
		}
    }
}
