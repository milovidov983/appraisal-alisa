using AliceAppraisal.Models;
using AliceAppraisal.Static;
using System.Linq;
using System.Threading.Tasks;
using AliceAppraisal.Core.Models;

namespace AliceAppraisal.Core.Engine.Strategy {
	public class GearboxStrategy : BaseStrategy {
		private static readonly string[] componentTypes = VehicleComponents.Gearboxes.Values.ToArray();
		public GearboxStrategy(IServiceFactory serviceFactory) : base(serviceFactory) {
		}

		public override async Task<SimpleResponse> GetMessage(AliceRequest request, State state) {
			if (state.NextAction.IsOneOf(
					typeof(GenerationStrategy),
					typeof(ConfirmGenerationStrategy),
					typeof(BodyTypeStrategy)
					)) {
				var wasFilledAutomatically = state.Request.Gearbox != null;
				if (wasFilledAutomatically) {
					var customNextStep = typeof(EngineTypeStrategy).FullName;
					return await CreateNextStepMessage(request, state, customNextStep);
				}
			} 
			var gearboxes = state.Characteristics.GearboxTypes?.Any() == true
				? state.Characteristics.GearboxTypes
				: componentTypes;

			return new SimpleResponse {
				Text = $"Какая коробка передач установлена в вашем авто? Например " +
					$"{gearboxes.ConcatToStringWithUnion()}",
				Buttons = gearboxes
			};

		}

		public override SimpleResponse GetMessageForUnknown(AliceRequest request, State state) {
			var additionalText = state.Characteristics.GearboxTypes?.Any() == true
				? $"Насколько мне известно, указанное вами поколение " +
				$"{state.Request.MakeEntity} {state.Request.ModelEntity} {state.Request.GenerationValue} " +
				$"выпускалось с следующими типами коробок передач: {state.Characteristics.GearboxTypes.ConcatToString()} "
				: " ";
			return new SimpleResponse {
				Text = 
				$"Я ожидала услышать от вас название типа коробки передач, " +
				$"но в ваших словах мне не удалось распознать его. {additionalText}" +
				$"Попробуйте повторить запрос или попросите у меня подсказку.",
				Buttons = state.Characteristics.GearboxTypes?.Any() == true
				? state.Characteristics.GearboxTypes
				: componentTypes
			};
		}

		public override SimpleResponse GetHelp() {
			return new SimpleResponse {
				Text = $"Для оценки автомобиля мне необходимо знать его тип коробки передач, " +
				$"существуют следующие типы коробок: Автомат, Робот, Механика, Вариатор. " +
				$"Попробуйте произнести название приблизив микрофон ближе."
			};
		}
		protected override bool Check(AliceRequest request, State state) {
			return request.HasIntent(Intents.GearboxType) && state.NextAction.Is(this.GetType());
		}

		protected override Task<SimpleResponse> Respond(AliceRequest request, State state) {
			var value = request.GetSlot(Intents.GearboxType, Slots.Gearbox);

			if (value.IsNullOrEmpty()) {
				return GetMessageForUnknown(request, state).FromTask();
			}

			state.UpdateGearbox(value);
			return CreateNextStepMessage(request, state);
		}
	}
}