﻿using AliceAppraisal.Core.Engine.Services;
using AliceAppraisal.Models;
using AliceAppraisal.Static;
using System;
using System.Threading.Tasks;
using AliceAppraisal.Core.Models;

namespace AliceAppraisal.Core.Engine.Strategy {
	public class HorsePowerStrategy : BaseStrategy {
		public HorsePowerStrategy(IServiceFactory serviceFactory) : base(serviceFactory) {
		}

		public override async Task<SimpleResponse> GetMessage(AliceRequest request, State state) {
			await Task.Yield();
			var randGiveWord = WordsCollection.GET_VERB.GetRand();
			return new SimpleResponse {
				Text = $"{randGiveWord} примерное количество лошадиных сил в вашем авто",
			};
		}

		public override SimpleResponse GetMessageForUnknown(AliceRequest request, State state) {
			return new SimpleResponse {
				Text = $"Не удалось распознать количество лошадиных сил, " +
				$"попробуйте повторить запрос или попросите у меня подсказку."
			};
		}

		public override SimpleResponse GetHelp()
			=> new SimpleResponse {
				Text = $"Для оценки автомобиля мне необходимо знать мощность двигателя, " +
				$"мощность необходимо указывать в лошадиных силах. " +
				$"Попробуйте произнести количество приблизив микрофон ближе."
			};
		
		protected override bool Check(AliceRequest request, State state) 
			=> request.HasIntent(Intents.DigitInput) && state.NextAction.Is(this.GetType());
		

		protected override Task<SimpleResponse> Respond(AliceRequest request, State state) {
			var horsePowerStr = request.GetSlot(Intents.DigitInput, Slots.Number);

			if (horsePowerStr.IsNullOrEmpty()) {
				return GetMessageForUnknown(request, state).FromTask();
			}
			if (!Int32.TryParse(horsePowerStr, out var horsePower)) {
				return GetMessageForUnknown(request, state).FromTask();
			}

			state.UpdateHorsePower(horsePower);

			
			return CreateNextStepMessage(request, state);
		}
	}
}
