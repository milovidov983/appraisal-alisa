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
			await Task.Yield();
			var takeVerb = WordsCollection.GET_VERB.GetRand();
			return new SimpleResponse {
				Text = $"{takeVerb} тип привода у вашего авто, " +
				$"например {componentTypes.ConcatToString()}",
				Buttons = componentTypes
			};
		}

		public override SimpleResponse GetMessageForUnknown(AliceRequest request, State state) {
			return new SimpleResponse {
				Text = $"Не удалось распознать тип привода, " +
				$"попробуйте повторить запрос или попросите у меня подсказку.",
				Buttons = componentTypes
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
