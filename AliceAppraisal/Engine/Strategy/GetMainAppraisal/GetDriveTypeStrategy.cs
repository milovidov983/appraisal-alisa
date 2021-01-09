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

		protected override bool Check(AliceRequest request, State state) {
			return request.HasIntent(Intents.DriveType);
		}

		protected override async Task<SimpleResponse> Respond(AliceRequest request, State state) {
			await Task.Yield();
			var drive = request.GetSlot(Intents.DriveType, Slots.Drive);

			if (drive.IsNullOrEmpty()) {
				return new SimpleResponse {
					Text = $"Не удалось распознать тип привода, попробуйте повторить ваш запрос.",
					Buttons = new[] { "Оценить другой авто", "Выйти" }
				};
			}

			state.UpdateDriveType(drive, this);

			return textGeneratorService.CreateNextTextRequest(this);

		}
	}
}
