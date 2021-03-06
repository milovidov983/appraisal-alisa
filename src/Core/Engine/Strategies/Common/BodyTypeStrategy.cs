using AliceAppraisal.Core.Models;
using AliceAppraisal.Models;
using AliceAppraisal.Static;
using System.Linq;
using System.Threading.Tasks;

namespace AliceAppraisal.Core.Engine.Strategy {
	public class BodyTypeStrategy : BaseStrategy {
		public BodyTypeStrategy(IServiceFactory serviceFactory) : base(serviceFactory) {
		}

		public override Task<SimpleResponse> GetMessage(AliceRequest request, State state) {
			var bodyTypes = state.Characteristics.BodyTypes?.Any() == true
				? state.Characteristics.BodyTypes
				: Buttons.BodyTypesBtn;


			return Task.FromResult(new SimpleResponse {
				   Text = $"Какой тип кузова у вашего авто? Например " +
					$"{bodyTypes.ConcatToStringWithUnion()}",
				   Buttons = bodyTypes
			});
		}

		public override SimpleResponse GetMessageForUnknown(AliceRequest request, State state) {

			var additionalText = state.Characteristics.BodyTypes?.Any() == true
				? $"Насколько мне известно, указанное вами поколение " +
				$"{state.Request.MakeEntity} {state.Request.ModelEntity} {state.Request.GenerationValue} " +
				$"выпускалось с следующими типами кузовов: {state.Characteristics.BodyTypes.ConcatToString()} "
				: $"Существуют следующие типы кузовов: {Buttons.BodyTypesBtn.ConcatToString()} и другие. ";

			return new SimpleResponse {
				   Text =
				$"Я ожидала услышать от вас название типа кузова вашего автомобиля но " +
				$"мне не удалось распознать в ваших словах тип кузова. {additionalText}" +
				$"Попробуйте повторить запрос или попросите у меня подсказку.",
				   Buttons = state.Characteristics.BodyTypes ?? Buttons.BodyTypesBtn
			   };
		}
		
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
