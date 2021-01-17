using AliceAppraisal.Engine.Services;
using AliceAppraisal.Models;
using AliceAppraisal.Static;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AliceAppraisal.Engine.Strategy {
	public class GetGenerationStrategy : BaseStrategy {
		public GetGenerationStrategy(IServiceFactory serviceFactory) : base(serviceFactory) {
		}

		protected override bool Check(AliceRequest request, State state) {
			return request.HasIntent(Intents.DigitInput) && state.NextAction.Is(this.GetType());
		}

		protected override async Task<SimpleResponse> Respond(AliceRequest request, State state) {
			var selectedGeneartion = request.GetSlot(Intents.DigitInput, Slots.Number);

			if (selectedGeneartion.IsNullOrEmpty()) {
				return GetMessageForUnknown(request, state);
			}

			var value = state.GetGenerationIdBySelected(selectedGeneartion);
			if(value is null) {
				return GetMessageForUnknown(request, state);
			}

			state.UpdateGenerationId(value.Id, value.Name, this);
			var nextAction = GetNextStrategy();
			return await nextAction.GetMessage(request, state);
		}

		public override async Task<SimpleResponse> GetMessage(AliceRequest request, State state) {
			return await CreateTextForGenerationStep(request, state);
		}

		public override SimpleResponse GetMessageForUnknown(AliceRequest request, State state) {
			var currentGens = state.GenerationChoise.Select(gen => $"Вариант {gen.Key}: {gen.Value.Name}").ToArray();
			if (request.HasScreen()) {
				var gens = state.GenerationChoise.Select(kv => $"{kv.Key}-й ({kv.Value.Name})");
				return new SimpleResponse {
					Text = $"Выберите нужный вариант поколения авто: {string.Join(" или ", gens)}?",
					Buttons = currentGens.Union(Buttons.SelectYear).ToArray()
				};
			} else {
				return new SimpleResponse {
					Text = $"Скажите какой вариант поколения ваш {string.Join("-й или ", state.GenerationChoise.Keys)}-й? " +
					$"{string.Join(", ", currentGens)}. Если среди представленных поколений нужное отсутствует, " +
					"попробуйте указать другой год выпуска вызвав команду фразой \"Изменить год выпуска\""
				};
			}
		}

		public override SimpleResponse GetHelp() {
			return new SimpleResponse {
				Text = $"Для оценки автомобиля мне необходимо знать его поколение, если в указанный год " +
				$"у автомобиля было несколько активных поколений то вам необходимо выбрать из представленных, " +
				$"назвав его номер по порядку, например \"Вариант 1\", название поколения указывать не надо."
			};
		}

		private async Task<SimpleResponse> CreateTextForGenerationStep(AliceRequest request, State state) {
			var hasScreen = request.HasScreen();
			var modelId = state.Request.ModelId;
			var manufactureYear = state.Request.ManufactureYear;

			Validate(modelId, manufactureYear);

			var findedGenerations = await externalService.GetGenerationsFor(modelId.Value, manufactureYear.Value);

			if(findedGenerations?.Any() != true) {
				throw new NotFoundExcteption(
					$"Не удалось найти поколение для модели {request.State.Session.Request.ModelEntity} " +
					$"с указанными годом выпуска {request.State.Session.Request.ManufactureYear}, " +
					$"попробуйте изменить год выпуска или модель.");
			}

			state.GenerationChoise = findedGenerations
				.Select((gen, index) => (Key: (index + 1).ToString(), Data: gen ))
				.ToDictionary(
					x => x.Key,
					x => new IdAndName { Id = x.Data.Value, Name = x.Data.Text }
					);

			if (findedGenerations.Length > 1) {
				var genText = findedGenerations.Select((gen, index) => $"Вариант {index + 1}: {gen.Text}").ToArray();
				if (hasScreen) {
					return new SimpleResponse {
						Text = $"Выберите номер варианта из представленных поколений авто, " +
						$"если среди представленных поколений нет нужного, " +
						$"попробуйте указать другой год выпуска.",
						Buttons = genText.Union(Buttons.SelectYear).Union(Buttons.Help).ToArray()
					};
				} else {
					return new SimpleResponse {
						Text = $"Скажите какой номер варианта соответствует вашему поколению авто: {string.Join(", ", genText)}. " +
						$"Если среди представленных поколений нет вашего, попробуйте указать другой год выпуска вызвав команду " +
						$"фразой \"Изменить год выпуска\"",
					};
				}

			} else {
				var nextAction = GetNextStrategy(typeof(ConfirmGenerationStrategy).FullName);
				return await nextAction.GetMessage(request, state);
			}
		}

		protected override void SetCurrentStep(State state) {
			if (state.GenerationChoise.Count > 1) {
				state.SaveCurrentStep(this);
			} else {
				state.PrevAction = this.GetType().FullName;
				state.NextAction = typeof(ConfirmGenerationStrategy).FullName;
			}
		}

		private void Validate(int? modelId, int? manufactureYear) {
			if(modelId is null) {
				throw new InvalidRequestException("Не указана модель авто, попробуйте начать с начала.");
			}
			if(manufactureYear is null) {
				throw new InvalidRequestException("Не указан год выпуска авто, попробуйте начать с начала.");
			}
		}
	}
}
