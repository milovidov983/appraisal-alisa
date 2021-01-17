using AliceAppraisal.Engine.Services;
using AliceAppraisal.Models;
using AliceAppraisal.Static;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AliceAppraisal.Engine.Strategy {
	public class WhatCanYouDoStrategy : BaseWithoutChangeStepStrategy {
		private const string MESSAGE = "Я могу угадывать цену у подержанных автомобилей. " +
				"Этап оценки состоит из нескольких простых шагов, " +
				"я спрошу у вас некоторые характеристики авто " +
				"и по ним попытаюсь найти аналоги и вывести объективную цену " +
				"на вторичном рынке в указанном вами регионе. " +
				"Что бы запустить оценку скажите \"Начать оценку\"";
		private static readonly SimpleResponse _defaultResponse = new SimpleResponse {
			Text = MESSAGE
		};

		public WhatCanYouDoStrategy(IServiceFactory serviceFactory) : base(serviceFactory) {
		}
		public override async Task<SimpleResponse> GetMessage(AliceRequest request, State state) {
			await Task.Yield();
			return _defaultResponse;
		}

		public override SimpleResponse GetMessageForUnknown(AliceRequest request, State state) {
			return _defaultResponse;
		}
		public override SimpleResponse GetHelp() {
			return _defaultResponse;
		}
		protected override bool Check(AliceRequest request, State state) {
            return request.HasIntent(Intents.YandexHelp1);
		}

        protected override async Task<SimpleResponse> Respond(AliceRequest request, State state) {
			return await GetMessage(request, state);
		}
    }
}
