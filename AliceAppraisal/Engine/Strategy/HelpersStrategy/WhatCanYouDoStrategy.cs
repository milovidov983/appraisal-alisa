﻿using AliceAppraisal.Engine.Services;
using AliceAppraisal.Models;
using AliceAppraisal.Static;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AliceAppraisal.Engine.Strategy {
	public class WhatCanYouDoStrategy : BaseStrategy {
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
		public override Task<SimpleResponse> GetMessage(AliceRequest request, State state)
			=> _defaultResponse.FromTask();
		
		public override SimpleResponse GetMessageForUnknown(AliceRequest request, State state) 
			=> _defaultResponse;
		
		public override SimpleResponse GetHelp() 
			=> _defaultResponse;
		
		protected override bool Check(AliceRequest request, State state) 
            => request.HasIntent(Intents.YandexHelp1);
		
        protected override Task<SimpleResponse> Respond(AliceRequest request, State state) 
			=> GetMessage(request, state);
		
    }
}