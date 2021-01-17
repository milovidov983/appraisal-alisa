using AliceAppraisal.Engine.Services;
using AliceAppraisal.Models;
using AliceAppraisal.Static;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AliceAppraisal.Engine.Strategy {
	public class GetMakeStrategy : BaseStrategy {
		public GetMakeStrategy(IServiceFactory serviceFactory) : base(serviceFactory) {
		}

		protected override bool Check(AliceRequest request, State state) {
			return request.Session.New
					||
					(
						request.HasIntent(Intents.MakeName)
						&& 
						(
							state.NextAction.Is(typeof(GetMakeStrategy)) 
							|| 
							state.NextAction.Is(typeof(AppraisalOtherStrategy))
						)
					);
		}

		protected override async Task<SimpleResponse> Respond(AliceRequest request, State state) {
			var value = request.GetSlot(Intents.MakeName, Slots.Make);

			if (value.IsNullOrEmpty()) {
				return GetMessageForUnknown(request, state);
			}

			var makeId = value.ExtractId() 
				?? throw new ArgumentException($"Не удалось извлечь ID марки из сущности {value}");

			state.UpdateMake(makeId, value, this);

			var nextAction = GetNextStrategy();
			return await nextAction.GetMessage(request, state);
		}


		public static readonly string[] Messages = new[] {
			$"Назовите марку авто который вы хотите оценить.",
			$"Назовите марку автомобиля который вы хотите оценить.",
			$"Скажите мне пожалуйста название марки автомобиля, который вы хотите оценить.",
			$"Скажите название марки автомобиля который вы хотите оценить.",
			$"Укажите марку авто которое вы хотите оценить"
		};

		 

		public override async Task<SimpleResponse> GetMessage(AliceRequest request, State state) {
			await Task.Yield();
			return new SimpleResponse {
				Text = Messages.GetRand()
			};
		}

		public override SimpleResponse GetMessageForUnknown(AliceRequest request, State state){
			return new SimpleResponse {
				Text = $"Не удалось распознать марку вашего авто, попробуйте повторить запрос или попросите у меня подсказку."
			};
		}

		public override SimpleResponse GetHelp() {
			return new SimpleResponse {
				Text = $"Для оценки автомобиля мне необходимо знать его марку, если мне не удается распознать " +
				$"название марки которое вы говорите, то возможно этой марки у меня просто нету в базе. " +
				$"Попробуйте произнести название приблизив микрофон ближе. "
			};
		}
	}
}