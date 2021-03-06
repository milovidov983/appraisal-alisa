using AliceAppraisal.Core.Engine.Services;
using AliceAppraisal.Core.Models;
using AliceAppraisal.Models;
using AliceAppraisal.Static;
using System.Linq;
using System.Threading.Tasks;

namespace AliceAppraisal.Core.Engine.Strategy {
	public class DriveTypeStrategy : BaseStrategy {
		private static readonly string[] componentTypes = VehicleComponents.Drives.Values.ToArray();
		public DriveTypeStrategy(IServiceFactory serviceFactory) : base(serviceFactory) {
		}
		public override async Task<SimpleResponse> GetMessage(AliceRequest request, State state) {
			if (state.NextAction.IsOneOf(
					typeof(GenerationStrategy),
					typeof(ConfirmGenerationStrategy),
					typeof(BodyTypeStrategy),
					typeof(GearboxStrategy),
					typeof(EngineTypeStrategy)
					)) {
				var wasFilledAutomatically = state.Request.Drive != null;
				if (wasFilledAutomatically) {
					var customNextStep = typeof(HorsePowerStrategy).FullName;
					return await CreateNextStepMessage(request, state, customNextStep);
				}
			}
			var driveTypes = state.Characteristics.DriveTypes?.Any() == true
							? state.Characteristics.DriveTypes
							: componentTypes;

			var takeVerb = WordsCollection.GET_VERB.GetRand();
			return new SimpleResponse {
				Text = $"{takeVerb} тип привода у вашего авто, " +
				$"например {driveTypes.ConcatToStringWithUnion()}",
				Buttons = driveTypes
			};

		}

		public override SimpleResponse GetMessageForUnknown(AliceRequest request, State state) {
			var additionalText = state.Characteristics.DriveTypes?.Any() == true
			? $"Насколько мне известно, указанное вами поколение " +
			$"{state.Request.MakeEntity} {state.Request.ModelEntity} {state.Request.GenerationValue} " +
			$"выпускалось с следующими типами привода: {state.Characteristics.DriveTypes.ConcatToString()}"
			: "";
			return new SimpleResponse {
				Text =
				$"Я ждала от вас тип привода, но мне " +
				$"не удалось распознать его в ваших словах. {additionalText} " +
				$"Попробуйте повторить запрос или попросите у меня подсказку.",
				Buttons = state.Characteristics.DriveTypes?.Any() == true
				? state.Characteristics.DriveTypes
				: componentTypes
			};
		
		}

		public override SimpleResponse GetHelp() {
			return new SimpleResponse {
				Text = $"Для оценки автомобиля мне необходимо знать его тип привода, существуют следующие " +
				$"типы: {componentTypes.ConcatToString()}. " +
				$"Попробуйте произнести название приблизив микрофон ближе.",
				Buttons = componentTypes
			};
		}
		protected override bool Check(AliceRequest request, State state) {
			return request.HasIntent(Intents.DriveType) && state.NextAction.Is(this.GetType());
		}

		protected override  Task<SimpleResponse> Respond(AliceRequest request, State state) {
			var value = request.GetSlot(Intents.DriveType, Slots.Drive);

			if (value.IsNullOrEmpty()) {
				return GetMessageForUnknown(request, state).FromTask();
			}

			state.UpdateDriveType(value);

			
			return CreateNextStepMessage(request, state);

		}
	}
}
