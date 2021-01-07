using AliceAppraisal.Engine.Services;
using AliceAppraisal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AliceAppraisal.Engine.Stratagy {
	public class TextGenerator : ITextGeneratorService {
		private readonly IExternalService externalService;
		private readonly WordRandomizer randWords = new WordRandomizer();

		public TextGenerator(IExternalService externalService) {
			this.externalService = externalService;
			complexResponseDataset[typeof(ManufactureYearStratagy)] = CreateTextForGenerationStep;


			responseDataset = new Dictionary<Type, SimpleResponse> {
				[typeof(BodyTypeStratagy)] = new SimpleResponse {
					Text = "Какая коробка передач установлена в вашем авто? Например автомат, механика и так далее",
					Buttons = new[] { "Автомат", "Робот", "Механика", "Вариатор", "Оценить другой авто", "Выйти" }
				},
				[typeof(MakeStratagy)] = new SimpleResponse {
					Text = $"{randWords.Get()} пожалуйста модель вашего автомобиля?",
					Buttons = new[] { "Оценить другой авто", "Выйти" }
				},
				[typeof(ModelStratagy)] = new SimpleResponse {
					Text = $"{randWords.Get()} год выпуска вашего автомобиля?",
					Buttons = new[] { "Оценить другой авто", "Выйти" }
				},
				[typeof(ManufactureYearStratagy)] = SimpleResponse.Empty,

				[typeof(GenerationStratagy)] = new SimpleResponse {
					Text = $"{randWords.Get()} тип кузова вашего авто? Например седан, хечбек, внедорожник и так далее",
					Buttons = new[] { "Оценить другой авто", "Выйти" }
				},
				[typeof(GearboxStratagy)] = new SimpleResponse {
					Text = $"{randWords.Get()} тип двигателя вашего авто? Например Бензиновый, Гибрид, Дизельный или Электрический",
					Buttons = new[] { "Бензиновый", "Гибрид", "Дизельный", "Электрический", "Оценить другой авто", "Выйти" }
				},
				[typeof(EngineTypeStratagy)] = new SimpleResponse {
					Text = $"{randWords.Get()} тип привода у вашего авто? Например Переднеприводный, Заднеприводный или Полноприводный ",
					Buttons = new[] { "Переднеприводный", "Заднеприводный", "Полноприводный", "Оценить другой авто", "Выйти" }
				},
				[typeof(DriveTypeStratagy)] = new SimpleResponse {
					Text = $"{randWords.Get()} примерное количество лошадиных сил в вашем авто?",
					Buttons = new[] { "Оценить другой авто", "Выйти" }
				},
				[typeof(HorsePowerStratagy)] = new SimpleResponse {
					Text = $"{randWords.Get()}  примерный пробег вашего авто?",
					Buttons = new[] { "Оценить другой авто", "Выйти" }
				},
				[typeof(RunStratagy)] = new SimpleResponse {
					Text = $"{randWords.Get()}  комплектацию вашего авто? Например Базовая, Стандартная или Максимальная",
					Buttons = new[] { "Базовая", "Стандартная", "Максимальная", "Оценить другой авто", "Выйти" }
				},
				[typeof(EquipmentSetStratagy)] = new SimpleResponse {
					Text = "И последнее, по умолчанию оценка будет проведена для московского региона, " +
				"если вас интересует другой регион, то введите его номер, если вас устраивает московский регион, то просто скажите Продолжить",
					Buttons = new[] { "Продолжить", "Оценить другой авто", "Выйти" }
				},
			};
		}

		private readonly Dictionary<Type, SimpleResponse> responseDataset;



		private readonly Dictionary<Type, Func<State, Task<SimpleResponse>>> complexResponseDataset
			= new Dictionary<Type, Func<State, Task<SimpleResponse>>> {
				[typeof(ManufactureYearStratagy)] = null
			};

		public SimpleResponse CreateNextTextRequest(BaseStratagy currentStratagy) {
			if (responseDataset.TryGetValue(currentStratagy.GetType(), out var result)) {
				if (result != SimpleResponse.Empty) {
					return result;
				} else {
					throw new InvalidOperationException($"result == SimpleResponse.Empty on strategy: {currentStratagy.GetType()}");
				}
			}
			throw new NotImplementedException($"{currentStratagy.GetType()} is unknown strategy type");
		}

		public async Task<SimpleResponse> CreateNextTextRequest(BaseStratagy currentStratagy, State state) {
			if (currentStratagy is CityStratagy) {
				return await CreateFinalResult(state);
			}
			if (currentStratagy is ManufactureYearStratagy) {
				var fn = complexResponseDataset[currentStratagy.GetType()];
				return await fn.Invoke(state);
			}
			throw new NotImplementedException($"currentStratagy {currentStratagy.GetType()} not have a handler");
		}

		public async Task<SimpleResponse> CreateFinalResult(State state) {
			var result = await externalService.GetAppraisalResponse(state.AppraisalRequest);

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

		public async Task<SimpleResponse> CreateTextForGenerationStep(State state) {
			var modelId = state.AppraisalRequest.ModelId;
			var manufactureYear = state.AppraisalRequest.ManufactureYear;
			var fitGenerations = await externalService.GetGenerationsFor(modelId.Value, manufactureYear.Value);

			if (fitGenerations.Length > 1) {
				state.GenerationChose = fitGenerations
					.Select((gen, index) => (Text: (index + 1).ToString(), Id: gen.Value))
					.ToDictionary(
						x => x.Text,
						x => x.Id
						);

				return new SimpleResponse {
					Text = $"Выберите поколение вашего авто",
					Buttons = fitGenerations.Select((gen, index) => $"Вариант {index + 1}: {gen.Text}").ToArray()
				};
			} else {
				state.AppraisalRequest.GenerationId = fitGenerations.FirstOrDefault()?.Value
					?? throw new ArgumentException($"Поколение для  {modelId} {manufactureYear} не найдено");

				return new SimpleResponse {
					Text = $"Какой тип кузова у вашего авто? Например седан, хэчбек и так далее."
				};
			}
		}
		


		public SimpleResponse CreateAnsverForUnexpectedCommand() {
			return new SimpleResponse {
				Text = "Я не смог распознать вашу команду, пожалуйста попробуйте повторить, " +
				"или воспользуйтесь справочным разделом, нажав ответствующую кнопку.",
				Buttons = new[] { "Помощь", "Выйти" }
			};
		}
	}
}