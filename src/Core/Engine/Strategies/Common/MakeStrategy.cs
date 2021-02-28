using AliceAppraisal.Models;
using AliceAppraisal.Static;
using System;
using System.Threading.Tasks;

namespace AliceAppraisal.Core.Engine.Strategy {
	public class MakeStrategy : BaseStrategy {
		private readonly IAppraisalProvider dataService;
		public MakeStrategy(IServiceFactory serviceFactory) : base(serviceFactory) {
			this.dataService = serviceFactory.GetDataProvider();
		}

		protected override bool Check(AliceRequest request, State state) 
			=> (
				request.HasIntent(Intents.MakeName)
				&& 
				(
					state.NextAction.Is(typeof(MakeStrategy)) 
					|| 
					state.NextAction.Is(typeof(InitStrategy))
				)
			);
		

		protected override Task<SimpleResponse> Respond(AliceRequest request, State state) {
			var value = request.GetSlot(Intents.MakeName, Slots.Make);

			if (value.IsNullOrEmpty()) {
				return GetMessageForUnknown(request, state).FromTask();
			}

			var makeId = value.ExtractId()
				?? throw new ArgumentException($"Не удалось извлечь ID марки из сущности {value}");

			state.UpdateMake(makeId, value);

			
			return CreateNextStepMessage(request, state);
		}


		public static readonly string[] Messages = new[] {
			$"Назовите марку авто, который вы хотите оценить.",
			$"Назовите марку автомобиля, который вы хотите оценить.",
			$"Скажите мне пожалуйста название марки автомобиля, который вы хотите оценить.",
			$"Скажите название марки автомобиля, который вы хотите оценить.",
			$"Укажите марку авто, которое вы хотите оценить"
		};

		public override async Task<SimpleResponse> GetMessage(AliceRequest request, State state) {
			var makes = request.HasScreen() 
				? await dataService.GetPupularMakes() 
				: Array.Empty<string>();

			var additionalText = request.HasScreen()
				? " Или выберите из наиболее популярных марок."
				: "";

			return new SimpleResponse {
				Text = Messages.GetRand() + additionalText,
				Buttons = makes
			};
		}

		public override SimpleResponse GetMessageForUnknown(AliceRequest request, State state)
			=> new SimpleResponse {
				Text = GetMessageForUnknownText()
			};
		
		
		public static string GetMessageForUnknownText() {
			return $"Что бы оценить авто мне надо знать его марку, " +
				$"пожалуйста назовите только марку Вашего авто или попросите у меня подсказку.";
		}

		public override SimpleResponse GetHelp() 
			=> new SimpleResponse {
				Text = GetHelpText()
			};

		public static string GetHelpText() {
			return $"Для оценки автомобиля мне необходимо знать его марку, если мне не удается распознать " +
					$"возможно Вы произнесите название марки с посторонними словами и мне не удается выделить " +
					$"среди них марку. Попробуйте произнести название модели отдельным словом без названия " +
					$"поколения Вашего автомобиля.";
		}
		
	}
}