using AliceAppraisal.Core.Engine.Services;
using AliceAppraisal.Core.Models;
using AliceAppraisal.Models;
using AliceAppraisal.Static;
using System.Linq;
using System.Threading.Tasks;

namespace AliceAppraisal.Core.Engine.Strategy {
	public class EngineTypeStrategy : BaseStrategy {
		private static readonly string[] componentTypes = VehicleComponents.EngineTypes.Values.ToArray();

		public EngineTypeStrategy(IServiceFactory serviceFactory) : base(serviceFactory) {
		}
		public override async Task<SimpleResponse> GetMessage(AliceRequest request, State state) {
			await Task.Yield();
			var engineTypes = state.Characteristics.EngineTypes?.Any() == true
				? state.Characteristics.EngineTypes
				: componentTypes;

			var randGiveWord = WordsCollection.GET_VERB.GetRand();
			return new SimpleResponse {
				Text = $"{randGiveWord} тип двигателя вашего авто, " +
				$"например {engineTypes.ConcatToStringWithUnion()}",
				Buttons = engineTypes
			};
		}

		public override SimpleResponse GetMessageForUnknown(AliceRequest request, State state) {
			var additionalText = state.Characteristics.EngineTypes?.Any() == true
				? $"Насколько мне известно, указанное вами поколение " +
				$"{state.Request.MakeEntity} {state.Request.ModelEntity} {state.Request.GenerationValue} " +
				$"выпускалось с следующими типами двигателей: {state.Characteristics.EngineTypes.ConcatToString()}"
				: "";

			return new SimpleResponse {
				Text = 
				$"Я ожидала услышать тип двигателя вашего автомобиля, но " +
				$"мне не удалось распознать его в ваших словах. {additionalText} " +
				$"попробуйте повторить запрос или попросите у меня подсказку.",
				Buttons = state.Characteristics.EngineTypes?.Any() == true
				? state.Characteristics.EngineTypes
				: componentTypes

			};
		}

		public override SimpleResponse GetHelp() {
			return new SimpleResponse {
				Text = $"Для оценки автомобиля мне необходимо знать его тип двигателя, существуют следующие " +
				$"типы: {componentTypes.ConcatToString()} " +
				$"Попробуйте произнести название приблизив микрофон ближе.",
				Buttons = componentTypes
			};
		}
		protected override bool Check(AliceRequest request, State state) {
			return request.HasIntent(Intents.EngineType) &&  state.NextAction.Is(this.GetType());
		}

		protected override Task<SimpleResponse> Respond(AliceRequest request, State state) {
			var value = request.GetSlot(Intents.EngineType, Slots.Engine);

			if (value.IsNullOrEmpty()) {
				return GetMessageForUnknown(request, state).FromTask();
			}
			state.UpdateEngineType(value);
			
			return CreateNextStepMessage(request, state);
		}
	}
}
