﻿using AliceAppraisal.Engine.Services;
using AliceAppraisal.Models;
using AliceAppraisal.Static;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AliceAppraisal.Engine.Strategy {
	public class ConfirmAppraisalStrategy : BaseStrategy {
		public ConfirmAppraisalStrategy(IServiceFactory serviceFactory) : base(serviceFactory) {
		}

		protected override bool Check(AliceRequest request, State state) {
			return request.HasIntent(Intents.YandexConfirm) && state.PrevAction.Is(typeof(InitialStrategy));
		}

		protected override async Task<SimpleResponse> Respond(AliceRequest request, State state) {
			await Task.Yield();
			state.SaveCurrentStep(this);

			return new SimpleResponse {
				Text = $"Отлично! Что бы я мог сделать максимально " +
				$"точную оценку мне необходимо услышать от вас, " +
				$"несколько ответов, это не займет много времени. " +
				$"Скажите какой марки ваше авто?",
				Buttons = new[] { "Выйти" }
			};

		}
	}
}