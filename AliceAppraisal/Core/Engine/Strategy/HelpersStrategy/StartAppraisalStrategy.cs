using AliceAppraisal.Core.Engine.Services;
using AliceAppraisal.Models;
using AliceAppraisal.Static;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AliceAppraisal.Core.Engine.Strategy {
	public class StartAppraisalStrategy : BaseStrategy {
		public StartAppraisalStrategy(IServiceFactory serviceFactory) : base(serviceFactory) {
		}

		public override async Task<SimpleResponse> GetMessage(AliceRequest request, State state) {
			var result = await CreateFinalResult(state);
			return result;
		}
		

		public override SimpleResponse GetMessageForUnknown(AliceRequest request, State state) 
			=> GetHelp();

		public override SimpleResponse GetHelp() 
			=> new SimpleResponse {
				Text = $"Текущая команда запускает процесс оценки на основе всех данных которые были получены в ходе диалога. " +
				$"После успешной оценки, можно попросить переоценить авто с другим пробегом, " +
				$"вызывается командой: \"оцени такое же авто но с пробегом Х.\""
			};
		
		protected override bool Check(AliceRequest request, State state) 
			=> state.NextAction.Is(this.GetType()) && !request.HasIntent(Intents.YandexReject);
		
		protected override Task<SimpleResponse> Respond(AliceRequest request, State state) 
			=> CreateNextStepMessage(request, state);
		
		private async Task<SimpleResponse> CreateFinalResult(State state) {
			var result = await externalService.GetAppraisalResponse(state.Request);

			SimpleResponse response = null;
			if (result.Status != "success") {
				response = new SimpleResponse {
					Text = $"Мне не удалось провести оценку вашего авто {state.Request.GetFullName()}. " +
					$"К сожалению у меня нет аналогов по вашему запросу. " +
					$"Возможно вы не очень точно указали параметры, либо ваше автомобиль очень редкий. " +
					$"Хотите попробовать скорректировать запрос?",
					Buttons = Buttons.YesNo
				};
			} else {
				var countAds = result.SampleByPrices.HighPriced + result.SampleByPrices.Normal + result.SampleByPrices.LowPriced;
				response = new SimpleResponse {
					Text = $"Цена вашего авто {state.Request.GetFullName()} на вторичном рынке составляет {result.OneMonthPrice} руб., " +
					$"всего было проанализировано {countAds} аналогичных авто, разброс цен среди них был в пределах " +
					$"от {result.PriceRange.Min} до {result.PriceRange.Max} руб. " +
					$"Хотите еще оценить автомобиль?",
					Buttons = Buttons.YesNo
				};
			}

			return response;
		}
	}
}
