using AliceAppraisal.Engine.Services;
using AliceAppraisal.Models;
using AliceAppraisal.Static;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AliceAppraisal.Engine.Strategy {
	public class GetDriveTypeStrategy : BaseStrategy {
		public GetDriveTypeStrategy(IServiceFactory serviceFactory) : base(serviceFactory) {
		}
		public override async Task<SimpleResponse> GetMessage(AliceRequest request, State state) {
			await Task.Yield();
			return new SimpleResponse {
				Text = $"{textGeneratorService.GetRandTakeVerb()} тип привода у вашего авто? " +
				$"Например Переднеприводный, Заднеприводный или Полноприводный.",
				Buttons = new[] { "Переднеприводный", "Заднеприводный", "Полноприводный" }
			};
		}

		public override SimpleResponse GetMessageForUnknown(AliceRequest request, State state) {
			return new SimpleResponse {
				Text = $"Не удалось распознать тип привода, попробуйте повторить запрос или попросите у меня подсказку."
			};
		}

		public override SimpleResponse GetHelp() {
			return new SimpleResponse {
				Text = $"Для оценки автомобиля мне необходимо знать его тип привода, существуют следующие " +
				$"типы: Переднеприводный, Заднеприводный или Полноприводный. " +
				$"Попробуйте произнести название приблизив микрофон ближе."
			};
		}
		protected override bool Check(AliceRequest request, State state) {
			return request.HasIntent(Intents.DriveType) && state.NextAction.Is(this.GetType());
		}

		protected override async Task<SimpleResponse> Respond(AliceRequest request, State state) {
			await Task.Yield();
			var value = request.GetSlot(Intents.DriveType, Slots.Drive);

			if (value.IsNullOrEmpty()) {
				return GetMessageForUnknown(request, state);
			}

			state.UpdateDriveType(value, this);

			var nextAction = GetNextStrategy();
			return await nextAction.GetMessage(request, state);

		}
	}
}
