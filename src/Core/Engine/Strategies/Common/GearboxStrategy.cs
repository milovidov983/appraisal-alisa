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
			await Task.Yield();
			return new SimpleResponse {
				Text = "Какая коробка передач установлена в вашем авто? Например автомат, механика и так далее",
				Buttons = componentTypes
			};
		}

		public override SimpleResponse GetMessageForUnknown(AliceRequest request, State state) {
			return new SimpleResponse {
				Text = $"Не удалось распознать тип коробки передач, " +
				$"попробуйте повторить запрос или попросите у меня подсказку."
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