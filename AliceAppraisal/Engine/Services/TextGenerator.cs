using AliceAppraisal.Engine.Services;
using AliceAppraisal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AliceAppraisal.Engine.Strategy {
	public class TextGenerator : ITextGeneratorService {
		private readonly IExternalService externalService;
		private readonly WordRandomizer randWords = new WordRandomizer();

		public string GetRandTakeVerb() {
			return randWords.Get();
		}

		public TextGenerator(IExternalService externalService) {
			this.externalService = externalService;
			complexResponseDataset[typeof(GetManufactureYearStrategy)] = CreateTextForGenerationStep;


			responseDataset = new Dictionary<Type, SimpleResponse> {
				[typeof(GetBodyTypeStrategy)] = new SimpleResponse {
					Text = "Какая коробка передач установлена в вашем авто? Например автомат, механика и так далее",
					Buttons = new[] { "Автомат", "Робот", "Механика", "Вариатор", "Оценить другой авто", "Выйти" }
				},

				[typeof(GetModelStrategy)] = new SimpleResponse {
					Text = $"{randWords.Get()} год выпуска вашего автомобиля?",
					Buttons = new[] { "Оценить другой авто", "Выйти" }
				},
				[typeof(GetManufactureYearStrategy)] = SimpleResponse.Empty,

				[typeof(GenerationStrategy)] = new SimpleResponse {
					Text = $"{randWords.Get()} тип кузова вашего авто? Например седан, хечбек, внедорожник и так далее",
					Buttons = new[] { "Оценить другой авто", "Выйти" }
				},
				[typeof(GearboxStrategy)] = new SimpleResponse {
					Text = $"{randWords.Get()} тип двигателя вашего авто? Например Бензиновый, Гибрид, Дизельный или Электрический",
					Buttons = new[] { "Бензиновый", "Гибрид", "Дизельный", "Электрический", "Оценить другой авто", "Выйти" }
				},
				[typeof(GetEngineTypeStrategy)] = new SimpleResponse {
					Text = $"{randWords.Get()} тип привода у вашего авто? Например Переднеприводный, Заднеприводный или Полноприводный ",
					Buttons = new[] { "Переднеприводный", "Заднеприводный", "Полноприводный", "Оценить другой авто", "Выйти" }
				},
				[typeof(GetDriveTypeStrategy)] = new SimpleResponse {
					Text = $"{randWords.Get()} примерное количество лошадиных сил в вашем авто?",
					Buttons = new[] { "Оценить другой авто", "Выйти" }
				},
				[typeof(GetHorsePowerStrategy)] = new SimpleResponse {
					Text = $"{randWords.Get()}  примерный пробег вашего авто?",
					Buttons = new[] { "Оценить другой авто", "Выйти" }
				},
				[typeof(GetRunStrategy)] = new SimpleResponse {
					Text = $"{randWords.Get()}  комплектацию вашего авто? Например Базовая, Стандартная или Максимальная",
					Buttons = new[] { "Базовая", "Стандартная", "Максимальная", "Оценить другой авто", "Выйти" }
				},
				[typeof(GetEquipmentSetStrategy)] = new SimpleResponse {
					Text = "И последнее, по умолчанию оценка будет проведена для московского региона, " +
				"если вас интересует другой регион, то введите его номер, если вас устраивает московский регион, то просто скажите Продолжить",
					Buttons = new[] { "Продолжить", "Оценить другой авто", "Выйти" }
				},
			};
		}

		private readonly Dictionary<Type, SimpleResponse> responseDataset;



		private readonly Dictionary<Type, Func<State, Task<SimpleResponse>>> complexResponseDataset
			= new Dictionary<Type, Func<State, Task<SimpleResponse>>> {
				[typeof(GetManufactureYearStrategy)] = null
			};

		public SimpleResponse CreateNextTextRequest(BaseStrategy currentStratagy) {
			if (responseDataset.TryGetValue(currentStratagy.GetType(), out var result)) {
				if (result != SimpleResponse.Empty) {
					return result;
				} else {
					throw new InvalidOperationException($"result == SimpleResponse.Empty on strategy: {currentStratagy.GetType()}");
				}
			}
			throw new NotImplementedException($"{currentStratagy.GetType()} is unknown strategy type");
		}

		public async Task<SimpleResponse> CreateNextTextRequest(BaseStrategy currentStratagy, State state) {
			if (currentStratagy is GetCityStrategy) {
				return await CreateFinalResult(state);
			}
			if (currentStratagy is GetManufactureYearStrategy) {
				var fn = complexResponseDataset[currentStratagy.GetType()];
				return await fn.Invoke(state);
			}
			throw new NotImplementedException($"currentStratagy {currentStratagy.GetType()} not have a handler");
		}

		public async Task<SimpleResponse> CreateFinalResult(State state) {
			var result = await externalService.GetAppraisalResponse(state.Request);

			if (result.Status != "success") {
				return new SimpleResponse {
					Text = "К сожалению я не смог провести оценку вашего авто. Мне не удалось найти аналогов по вашему запросу.",
					Buttons = new[] { "Оценить другой авто", "Выйти" }
				};
			}
			var countAds = result.SampleByPrices.HighPriced + result.SampleByPrices.Normal + result.SampleByPrices.LowPriced;
			return new SimpleResponse {
				Text = $"Цена вашего авто на вторичном рынке составляет {result.OneMonthPrice} р., всего было проанализировано {countAds} " +
				$"аналогичных авто, разброс цен среди них был в рамках от {result.PriceRange.Min} до {result.PriceRange.Max}. " +
				$"Хотите еще оценить автомобиль?",
				Buttons = new[] { "Оценить другой авто", "Выйти" }
			};
		}

		public async Task<SimpleResponse> CreateTextForGenerationStep(State state, bool hasScreen) {
			var modelId = state.Request.ModelId;
			var manufactureYear = state.Request.ManufactureYear;
			var fitGenerations = await externalService.GetGenerationsFor(modelId.Value, manufactureYear.Value);

			if (fitGenerations.Length > 1) {
				state.GenerationChoise = fitGenerations
					.Select((gen, index) => (Text: (index + 1).ToString(), Id: gen.Value))
					.ToDictionary(
						x => x.Text,
						x => x.Id
						);


				var genText = fitGenerations.Select((gen, index) => $"Вариант {index + 1}: {gen.Text}").ToArray();
				if (hasScreen) {
					return new SimpleResponse {
						Text = $"Выберите поколение вашего авто",
						Buttons = genText
					};
				} else {
					return new SimpleResponse {
						Text = $"Скажите какой номер варианта соответствует вашему поколению авто: {string.Join(", ", genText)}",
					};
				}

			} else {
				state.Request.GenerationId = fitGenerations.FirstOrDefault()?.Value
					?? throw new ArgumentException($"Поколение для {modelId} {manufactureYear} не найдено");

				return new SimpleResponse {
					Text = $"Какой тип кузова у вашего авто? Например седан, хэчбек и так далее."
				};
			}
		}
		

		public class AnswersForStratagy {
			public HashSet<string> PrevActions = new HashSet<string>();
			public string CommonText { get; set; }
			public string WithoutScreenText { get; set; }
			public string[] Buttons = AliceAppraisal.Static.Buttons.Base;
		}

		private static AnswersForStratagy[] answersForStratagies = new[] {
			new AnswersForStratagy {
				PrevActions = new HashSet<string>(new[]{ 
					typeof(ConfirmAppraisalStrategy).FullName,  
					typeof(AppraisalOtherStrategy).FullName 
				}),
				CommonText = "Кажется у меня нет такой марки в базе данных, попробуйте еще раз или попросите у меня подсказку.",
				
			}

		};

		public SimpleResponse CreateAnsverForUnexpectedCommand(State state) {
			return new SimpleResponse {
				Text = "Я не смог распознать вашу команду, пожалуйста попробуйте повторить, " +
				"или воспользуйтесь справочным разделом, нажав ответствующую кнопку.",
				Buttons = new[] { "Помощь", "Выйти" }
			};
		}
	}
}