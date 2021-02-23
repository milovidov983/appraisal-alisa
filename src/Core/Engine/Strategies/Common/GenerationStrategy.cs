using AliceAppraisal.Models;
using AliceAppraisal.Static;
using System.Linq;
using System.Threading.Tasks;

namespace AliceAppraisal.Core.Engine.Strategy {
	public class GenerationStrategy : BaseStrategy {
		private readonly IAppraisalProvider appraisalService;

		public GenerationStrategy(IServiceFactory serviceFactory) : base(serviceFactory) {
			this.appraisalService = serviceFactory.GetDataProvider();
		}

		protected override bool Check(AliceRequest request, State state)
			=> state.NextAction.Is(this.GetType()) &&
			(request.HasIntent(Intents.DigitInput));
			
		

		protected override Task<SimpleResponse> Respond(AliceRequest request, State state) {
			var selectedGeneartion = request.GetSlot(Intents.DigitInput, Slots.Number);

			if (selectedGeneartion.IsNullOrEmpty()) {
				return GetMessageForUnknown(request, state).FromTask();
			}

			var value = state.GetGenerationIdBySelected(selectedGeneartion);
			if(value is null) {
				return GetMessageForUnknown(request, state).FromTask();
			}

			state.UpdateGenerationId(value.Id, value.Name);
			
			return CreateNextStepMessage(request, state);
		}

		public override async Task<SimpleResponse> GetMessage(AliceRequest request, State state)
			=> await CreateTextForGenerationStep(request, state);
		

		public override SimpleResponse GetMessageForUnknown(AliceRequest request, State state) {
			var currentGens = state.GenerationChoise.Select(gen => $"Вариант {gen.Key}: {gen.Value.Name}").ToArray();
			if (request.HasScreen()) {
				var gens = state.GenerationChoise.Select(kv => $"{kv.Key}-й ({kv.Value.Name})");
				return new SimpleResponse {
					Text = $"Выберите нужный вариант поколения авто: {string.Join(" или ", gens)}? " +
					$"Важно сказать именно номер варианта если вы говорите голосом, " +
					$"а не выбираете среди представленных на экране." +
					$"Если среди представленных поколений нужное отсутствует, " +
					"попробуйте указать другой год выпуска вызвав команду \"Изменить год выпуска\"",
					Buttons = currentGens.Union(Buttons.SelectYear).ToArray()
				};
			} else {
				return new SimpleResponse {
					Text = $"Скажите номер варианта поколения {string.Join("-й или ", state.GenerationChoise.Keys)}-й? " +
					$"{string.Join(", ", currentGens)}. Важно сказать именно номер варианта. " +
					$"Если среди представленных поколений нужное отсутствует, " +
					"попробуйте указать другой год выпуска вызвав команду фразой \"Изменить год выпуска\""
				};
			}
		}

		public override SimpleResponse GetHelp() 
			=> new SimpleResponse {
				Text = $"Для оценки автомобиля мне необходимо знать его поколение, если в указанный год " +
				$"у автомобиля было несколько активных поколений то вам необходимо выбрать из представленных, " +
				$"назвав его номер по порядку, например \"Вариант 1\", название поколения указывать не надо. " +
				$"Для вывода вариантов скажите мне повторить последний шаг.",
				Buttons = Buttons.Replay
			};
		
		private async Task<SimpleResponse> CreateTextForGenerationStep(AliceRequest request, State state) {
			var hasScreen = request.HasScreen();
			var modelId = state.Request.ModelId;
			var manufactureYear = state.Request.ManufactureYear;

			Validate(modelId, manufactureYear);

			var findedGenerations = await appraisalService.GetGenerationsFor(modelId.Value, manufactureYear.Value);

			if(findedGenerations?.Any() != true) {
				return new SimpleResponse {
					Text = $"Не удалось найти поколение для модели {request.State.Session.Request.ModelEntity} " +
					$"с указанными годом выпуска {request.State.Session.Request.ManufactureYear}, " +
					$"возможно необходимо указать более полное название модели, " +
					$"Либо в указанный вами год такого поколения не выпускали, " +
					$"попробуйте изменить год выпуска или модель.",
					Buttons = Buttons.SelectYear
				};
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
						Buttons = genText.Union(Buttons.SelectYear).ToArray()
					};
				} else {
					return new SimpleResponse {
						Text = $"Скажите какой номер варианта соответствует вашему поколению авто: {string.Join(", ", genText)}. " +
						$"Если среди представленных поколений нет вашего, попробуйте указать другой год выпуска вызвав команду " +
						$"фразой \"Изменить год выпуска\"",
					};
				}
			} else {
				// Случай когда у модели было одно поколение и надо его просто подтвердить
				var customNextStep = typeof(ConfirmGenerationStrategy).FullName;
				return await CreateNextStepMessage(request, state, customNextStep);
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
