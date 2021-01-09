using AliceAppraisal.Engine.Services;
using AliceAppraisal.Models;
using AliceAppraisal.Static;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AliceAppraisal.Engine.Strategy {
	public class ChangeParamStrategy : BaseStrategy {
		public ChangeParamStrategy(IServiceFactory serviceFactory) : base(serviceFactory) {
		}
		public override async Task<SimpleResponse> GetMessage(AliceRequest request, State state) {
			await Task.Yield();
			return SimpleResponse.Empty;
		}

		public override SimpleResponse GetMessageForUnknown(AliceRequest request, State state) {
			return new SimpleResponse {
				Text = $"Не удалось распознать указанный вами пробег, попробуйте повторить ваш запрос.",
			};
		}
		public override SimpleResponse GetHelp() {
			return new SimpleResponse {
				Text = $"Изменить пробег у оцененного авто."
			};
		}
		protected override bool Check(AliceRequest request, State state) {
			return request.HasIntent(Intents.ChangeParamRun) 
				&& state.NextAction.Is(typeof(StartAppraisalStrategy));
		}

		protected override async Task<SimpleResponse> Respond(AliceRequest request, State state) {
			await Task.Yield();
			var runStr = request.GetSlot(Intents.ChangeParamRun, Slots.Run);

			if (runStr.IsNullOrEmpty()) {
				return GetMessageForUnknown(request,state);
			}
			if (!Int32.TryParse(runStr, out var run)) {
				return GetMessageForUnknown(request, state);
			}

			state.UpdateRun(run, this);

			var nextAction = GetNextStrategy();
			return await nextAction.GetMessage(request, state);
		}
	}
}
