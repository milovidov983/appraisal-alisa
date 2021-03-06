using AliceAppraisal.Core.Models;
using AliceAppraisal.Models;
using AliceAppraisal.Static;
using System.Threading.Tasks;

namespace AliceAppraisal.Core.Engine.Strategy {
	public class BodyTypeStrategy : BaseStrategy {
		public BodyTypeStrategy(IServiceFactory serviceFactory) : base(serviceFactory) {
		}

		public override Task<SimpleResponse> GetMessage(AliceRequest request, State state)
			=> Task.FromResult(new SimpleResponse {
				Text = $"Какой тип кузова у вашего авто? Например седан, " +
				$"{Buttons.BodyTypesBtn.ConcatToString()} и так далее. ",
				Buttons = Buttons.BodyTypesBtn
			});

		public override SimpleResponse GetMessageForUnknown(AliceRequest request, State state) 
			=> new SimpleResponse {
				Text = $"Не удалось распознать тип кузова вашего авто, " +
				$"существуют следующие типы кузовов: {Buttons.BodyTypesBtn.ConcatToString()} и другие. " +
				$"Попробуйте повторить запрос или попросите у меня подсказку.",
				Buttons = Buttons.BodyTypesBtn
			};
		
		public override SimpleResponse GetHelp() 
			=> new SimpleResponse {
				Text = $"Для оценки автомобиля мне необходимо знать его тип кузова, существуют следующие " +
				$"типы кузовов: {Buttons.BodyTypesBtn.ConcatToString()} и другие. " +
				$"Попробуйте произнести название приблизив микрофон ближе.",
				Buttons = Buttons.BodyTypesBtn
			};
		
		protected override bool Check(AliceRequest request, State state)
			=> request.HasIntent(Intents.BodyType) &&  state.NextAction.Is(this.GetType());
		

		protected override Task<SimpleResponse> Respond(AliceRequest request, State state) {
			var value = request.GetSlot(Intents.BodyType, Slots.Body);

			if (value.IsNullOrEmpty()) {
				return GetMessageForUnknown(request, state).FromTask();
			}

			state.UpdateBodyType(value);

			return CreateNextStepMessage(request, state);
		}
	}
}
