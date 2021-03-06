using AliceAppraisal.Core.Engine.Services;
using AliceAppraisal.Models;
using AliceAppraisal.Static;
using System.Linq;
using System.Threading.Tasks;
using AliceAppraisal.Core.Models;

namespace AliceAppraisal.Core.Engine.Strategy {
	public class EquipmentSetStrategy : BaseStrategy {
		private static readonly string[] componentTypes = VehicleComponents.Equipments.Values.ToArray();
		public EquipmentSetStrategy(IServiceFactory serviceFactory) : base(serviceFactory) {
		}
		public override async Task<SimpleResponse> GetMessage(AliceRequest request, State state) {
			await Task.Yield();
			var randGiveWord = WordsCollection.GET_VERB.GetRand();
			return new SimpleResponse {
				Text = $"{randGiveWord} комплектацию вашего авто, " +
				$"например {componentTypes.ConcatToString()}",
				Buttons = componentTypes
			};
		}

		public override SimpleResponse GetMessageForUnknown(AliceRequest request, State state)
			=> new SimpleResponse {
				Text = $"Не удалось распознать тип комплектации, " +
				$"попробуйте повторить запрос или попросите у меня подсказку.",
				Buttons = componentTypes
			};
		

		public override SimpleResponse GetHelp()
			=> new SimpleResponse {
				Text = $"Для оценки автомобиля мне необходимо знать его комплектацию, существуют следующие " +
				$"типы: {componentTypes.ConcatToString()} " +
				$"Попробуйте произнести название приблизив микрофон ближе.",
				Buttons = componentTypes
			};
		
		protected override bool Check(AliceRequest request, State state)
			=> request.HasIntent(Intents.EquipmentType) && state.NextAction.Is(this.GetType());
		

		protected override Task<SimpleResponse> Respond(AliceRequest request, State state) {
			var value = request.GetSlot(Intents.EquipmentType, Slots.Equipment);

			if (value.IsNullOrEmpty()) {
				return GetMessageForUnknown(request, state).FromTask();
			}

			state.UpdateEquipmentSet(value);

			
			return CreateNextStepMessage(request, state);

		}
	}
}
