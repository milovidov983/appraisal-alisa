using AliceAppraisal.Models;
using AliceAppraisal.Static;
using System;
using System.Threading.Tasks;

namespace AliceAppraisal.Engine.Strategy {
	public class ChangeParamStrategy : BaseStrategy {
		public ChangeParamStrategy(IServiceFactory serviceFactory) : base(serviceFactory) {
		}
		public override async Task<SimpleResponse> GetMessage(AliceRequest request, State state) {
			await Task.Yield();
			return GetHelp();
		}

		public override SimpleResponse GetMessageForUnknown(AliceRequest request, State state)
			=> new SimpleResponse {
				Text = $"Не удалось распознать указанный вами пробег, попробуйте повторить ваш запрос.",
			};

		public override SimpleResponse GetHelp()
			=> new SimpleResponse {
				Text = $"Изменить пробег у оцененного авто. " +
				$"Вызывается командой \"Оцени такое же авто но с пробегом Х\", " +
				$"где Х это пробег в километрах."
			};
		
		protected override bool Check(AliceRequest request, State state) 
			=> 
			request.HasIntent(Intents.ChangeParamRun) 
			&& 
			state.NextAction.Is(typeof(StartAppraisalStrategy));
		

		protected override async Task<SimpleResponse> Respond(AliceRequest request, State state) {
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
