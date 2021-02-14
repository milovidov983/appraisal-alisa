using AliceAppraisal.Models;
using AliceAppraisal.Static;
using System;
using System.Threading.Tasks;

namespace AliceAppraisal.Core.Engine.Strategy {
	public class ChangeRunStrategy : BaseStrategy {
		public ChangeRunStrategy(IServiceFactory serviceFactory) : base(serviceFactory) {
		}
		public override Task<SimpleResponse> GetMessage(AliceRequest request, State state) 
			=> GetHelp().FromTask();
		

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
		

		protected override Task<SimpleResponse> Respond(AliceRequest request, State state) {
			var runStr = request.GetSlot(Intents.ChangeParamRun, Slots.Run);

			if (runStr.IsNullOrEmpty()) {
				return GetMessageForUnknown(request,state).FromTask();
			}
			if (!Int32.TryParse(runStr, out var run)) {
				return GetMessageForUnknown(request, state).FromTask();
			}

			state.UpdateRun(run);

			return CreateNextStepMessage(request, state);
		}
	}
}
