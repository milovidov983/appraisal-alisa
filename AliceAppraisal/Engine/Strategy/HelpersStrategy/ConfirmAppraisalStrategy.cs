using AliceAppraisal.Engine.Services;
using AliceAppraisal.Models;
using AliceAppraisal.Static;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AliceAppraisal.Engine.Strategy {
	public class ConfirmAppraisalStrategy : BaseStrategy {
		public ConfirmAppraisalStrategy(IServiceFactory serviceFactory) : base(serviceFactory) {
		}

		public override async Task<SimpleResponse> GetMessage(AliceRequest request, State state) {
			await Task.Yield();
			// TODO рандомизировать реплику
			return new SimpleResponse {
				Text = $"Скажите какой марки оцениваемое авто?"
			};
		}

		public override SimpleResponse GetMessageForUnknown(AliceRequest request, State state) {
			return SimpleResponse.Empty;
		}
		public override SimpleResponse GetHelp() {
			return new SimpleResponse {
				Text = $"Что бы начать оценку автомобиля скажите \"начать\"",
				Buttons = Buttons.YesNoExtended
			};
		}
		protected override bool Check(AliceRequest request, State state) {
			return (request.HasIntent(Intents.YandexConfirm) 
				|| request.Request.Command.Trim().ToLowerInvariant().StartsWith("начать") 
				&& state.PrevAction.Is(typeof(InitialStrategy))) 
				||
				 state.NextAction.Is(typeof(StartAppraisalStrategy)) && request.HasIntent(Intents.YandexConfirm); ;
		}

		protected override async Task<SimpleResponse> Respond(AliceRequest request, State state) {
			var nextAction = GetNextStrategy();
			return await nextAction.GetMessage(request, state);
		}
	}
}
