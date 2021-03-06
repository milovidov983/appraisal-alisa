﻿using AliceAppraisal.Core.Engine.Services;
using AliceAppraisal.Core.Models;
using AliceAppraisal.Models;
using AliceAppraisal.Static;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AliceAppraisal.Core.Engine.Strategy {
	public class ManufactureYearStrategy : BaseStrategy {
		private readonly IManufactureYearService yearService;
		public ManufactureYearStrategy(IServiceFactory serviceFactory) : base(serviceFactory) {
			this.yearService = serviceFactory.GetManufactureYearService();
		}

		public static readonly string[] Messages = new[] {
			"{0} год выпуска автомобиля, который мы оцениваем",
			"{0} в каком году был произведен автомобиль",
			"{0} год производства автомобиля",
		};

		public override async Task<SimpleResponse> GetMessage(AliceRequest request, State state) {
			await Task.Yield();
			var randGiveWord = WordsCollection.GET_VERB.GetRand();
			var randMessage = Messages.GetRand();

			var years = Array.Empty<string>();
			if (request.HasScreen()) {
				var currentYear = DateTime.UtcNow.Year;
				years = Enumerable.Range(0, 15).Select(x => (currentYear - x).ToString()).ToArray();
			}

			return new SimpleResponse {
				Text = string.Format(randMessage, randGiveWord),
				Buttons = years
			};
		}
		public override SimpleResponse GetHelp() {
			return new SimpleResponse {
				Text = $"Для оценки автомобиля мне необходимо знать его год выпуска. " +
					$"Допустимый диапазон лет от {Limits.StartProductionYear} до {Limits.EndProductionYear}" +
					$"Попробуйте произнести название приблизив микрофон ближе."
			};
		}


		public override SimpleResponse GetMessageForUnknown(AliceRequest request, State state) {
			return new SimpleResponse {
				Text = $"Не удалось распознать год выпуска вашего авто, " +
				$"попробуйте повторить ваш запрос или попросите у меня подсказку."
			};
		}

		protected override bool Check(AliceRequest request, State state) {
			return request.HasIntent(Intents.DigitInput) && state.NextAction.Is(typeof(ManufactureYearStrategy));
		}

		protected override Task<SimpleResponse> Respond(AliceRequest request, State state) {
			var value = request.GetSlot(Intents.DigitInput, Slots.Number);

			if (value.IsNullOrEmpty()) {
				return GetMessageForUnknown(request, state).FromTask();
			}

			var manufactureYear = yearService.GetYearFromUserInputOrNull(value);
			if (manufactureYear is null) {
				return GetMessageForUnknown(request, state).FromTask();
			}

			state.UpdateManufactureYear(manufactureYear.Value);

			var nextStep = GetNextStepOrDefault(state);
			return CreateNextStepMessage(request, state, nextStep);
		}


		/// Гремучее легаси
		private string GetNextStepOrDefault(State state) {
			// Если в указанный год выпуска у марки было одно поколение
			// тогда выбираем его и запрашиваем подтверждение от пользователя
			if (state.GenerationChoise.Count == 1) {
				return typeof(ConfirmGenerationStrategy).FullName;
			}
			return null;
		}

		private string Validate(int manufactureYear) {
			if (manufactureYear > DateTime.UtcNow.Year) {
				return $"Кажется указанный вами {manufactureYear} год выпуска еще наступил, попробуйте еще раз.";
			}
			if (manufactureYear < 2000) {
				return "Кажется год который вы указали выходит за нижний предел ограничения, " +
					 $"минимально возможным годом является {Limits.StartProductionYear}. " +
					 $"Попробуйте еще раз.";
			}
			return null;
		}
	}
}
