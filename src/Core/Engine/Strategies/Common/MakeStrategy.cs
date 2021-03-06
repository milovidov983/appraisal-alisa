using AliceAppraisal.Core.Models;
using AliceAppraisal.Models;
using AliceAppraisal.Static;
using System;
using System.Threading.Tasks;

namespace AliceAppraisal.Core.Engine.Strategy {



	public class MakeStrategy : BaseStrategy {
		private readonly IDataProvider dataService;
		private readonly IntentName intentName = new IntentName {
			Intent = Intents.MakeName,
			Slot = Slots.Make
		};


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
			state.GeneralReset(); // На всякий случай т.к. это первый шаг оценки и данных быть не должно
			string intentValue = request.GetSlot(intentName);
			int makeId = ValidateAndGetId(intentValue);
			state.UpdateMake(makeId, intentValue);
			Task<SimpleResponse> messageForNextStep = CreateNextStepMessage(request, state);
			return messageForNextStep;
		}


		private int ValidateAndGetId(string intentValue) {
			if (intentValue.IsNullOrEmpty()) {
				throw new InvalidRequestException($"В запросе был найден intent {intentName.Intent} " +
					$"но при его получении возникла ошибка, полезная нагрузка оказалась пустой.",
					GetMessageForUnknownText());
			}
			return intentValue.ExtractIdOrNull()
				?? throw new InternalErrorException(
					$"Не удалось извлечь ID марки из сущности {intentValue}",
					InternalErrorException.DefaultUserMessage);
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
				$"ваши слова, возможно Вы произнесите название марки с посторонними словами и мне не удается выделить " +
				$"среди них марку. Попробуйте произнести название модели отдельным словом без названия " +
				$"поколения Вашего автомобиля.";
		}
		
	}
}