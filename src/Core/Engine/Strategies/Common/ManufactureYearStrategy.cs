using AliceAppraisal.Core.Engine.Services;
using AliceAppraisal.Models;
using AliceAppraisal.Static;
using System;
using System.Threading.Tasks;

namespace AliceAppraisal.Core.Engine.Strategy {
	public class ManufactureYearStrategy : BaseStrategy {
		public ManufactureYearStrategy(IServiceFactory serviceFactory) : base(serviceFactory) {
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

			return new SimpleResponse {
				Text = string.Format(randMessage, randGiveWord)
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

			var isCorrectConverted = Int32.TryParse(value, out var manufactureYear);
			if (!isCorrectConverted) {
				return GetMessageForUnknown(request, state).FromTask();
			}
			string error = Validate(manufactureYear);
			if(error != null){
				throw new InvalidRequestException(error);
			}
			state.UpdateManufactureYear(manufactureYear);

			var nextStep = GetNextStepOrDefault(state);
			return CreateNextStepMessage(request, state, nextStep);
		}


		/// Гремучее легаси
		private string GetNextStepOrDefault(State state) {
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
