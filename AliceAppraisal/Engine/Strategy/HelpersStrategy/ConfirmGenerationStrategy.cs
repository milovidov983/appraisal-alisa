﻿using AliceAppraisal.Engine.Services;
using AliceAppraisal.Models;
using AliceAppraisal.Static;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AliceAppraisal.Engine.Strategy {
	public class ConfirmGenerationStrategy : BaseStrategy {
		public ConfirmGenerationStrategy(IServiceFactory serviceFactory) : base(serviceFactory) {
		}

		public override SimpleResponse GetHelp() {
			return new SimpleResponse {
				Text = $"Вам необходимо ответить подходит-ли найденное поколение для вашего авто"
			};
		}

		public override async Task<SimpleResponse> GetMessage(AliceRequest request, State state) {
			await Task.Yield();
			var (_, Name) = state.GenerationChoise.Values.First();

			return new SimpleResponse {
				Text = $"Я выбрал поколение {Name} для вашего авто, скажите это правильное поколение?",
				Buttons = new[] { "Да", "Нет" }
			};
		}

		public override SimpleResponse GetMessageForUnknown(AliceRequest request, State state) {
			return new SimpleResponse {
				Text = $"Мне не удалось понять это поколение соответствует вашему или нет?"
			};
		}

		protected override bool Check(AliceRequest request, State state) {
			return (request.HasIntent(Intents.YandexConfirm) || request.HasIntent(Intents.YandexReject))
				&& state.NextAction.Is(typeof(ConfirmGenerationStrategy));
		}

		protected override async Task<SimpleResponse> Respond(AliceRequest request, State state) {
			await Task.Yield();
			if (request.HasIntent(Intents.YandexConfirm)) {
				var (Id, Name) = state.GenerationChoise.Values.First();
				state.UpdateGenerationId(Id, Name, this);

				var nextAction = GetNextStrategy(typeof(GetBodyTypeStrategy).FullName);
				state.NextAction = typeof(GetBodyTypeStrategy).FullName;
				return await nextAction.GetMessage(request, state);
			} else {
				var nextAction = GetNextStrategy(typeof(GetManufactureYearStrategy).FullName);
				state.NextAction = typeof(GetManufactureYearStrategy).FullName;
				return await nextAction.GetMessage(request, state);
			}
		}
	}
}
