using AliceAppraisal.Engine.Services;
using AliceAppraisal.Models;
using AliceAppraisal.Static;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AliceAppraisal.Engine.Strategy {
	public class GetModelStrategy : BaseStrategy {
		public GetModelStrategy(IServiceFactory serviceFactory) : base(serviceFactory) {
		}

		public override SimpleResponse GetHelp() {
			return new SimpleResponse {
				Text = $"Для оценки автомобиля мне необходимо знать его модель, если мне не удается распознать " +
					$"название модели которое вы говорите, то возможно этой модели у меня просто нету в базе. " +
					$"Попробуйте произнести название приблизив микрофон ближе."
			};
		}

		public override async Task<SimpleResponse> GetMessage(AliceRequest request, State state) {
			await Task.Yield();
			return new SimpleResponse {
				Text = $"{textGeneratorService.GetRandTakeVerb()} пожалуйста модель вашего автомобиля?"
			};
		}

		public override SimpleResponse GetMessageForUnknown(AliceRequest request, State state) {
			return new SimpleResponse {
				Text = $"Не удалось распознать модель вашего авто," +
				$" попробуйте повторить ваш запрос или попросите у меня подсказку."
			};
		}

		protected override bool Check(AliceRequest request, State state) {
			return request.HasIntent(Intents.ModelName) && state.PrevAction.Is(typeof(GetMakeStrategy));
		}

		protected override async Task<SimpleResponse> Respond(AliceRequest request, State state) {
			var value = request.GetSlot(Intents.ModelName, Slots.Model);

			if (value.IsNullOrEmpty()) {
				return GetMessageForUnknown(request, state);
			}

			var newModelId = value.ExtractId() 
				?? throw new ArgumentException($"Не удалось извлечь ID модели из сущности {value}");

			state.UpdateModelId(newModelId, value, this);


			var nextAction = GetNextStrategy();
			return await nextAction.GetMessage(request, state);

		}
	}
}
