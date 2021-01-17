﻿using AliceAppraisal.Engine.Services;
using AliceAppraisal.Models;
using AliceAppraisal.Static;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AliceAppraisal.Engine.Strategy {
	public class StartAppraisalStrategy : BaseStrategy {
		public StartAppraisalStrategy(IServiceFactory serviceFactory) : base(serviceFactory) {
		}
		public override async Task<SimpleResponse> GetMessage(AliceRequest request, State state) {
			return await CreateFinalResult(state);
		}

		public override SimpleResponse GetMessageForUnknown(AliceRequest request, State state) {
			return new SimpleResponse {
				Text = $"Мне не удалось понять это поколение соответствует вашему или нет?"
			};
		}
		public override SimpleResponse GetHelp() {
			return new SimpleResponse {
				Text = $"Текущая команда запускает процесс оценки на основе всех данных которые были получены в ходе диалога. " +
				$"После успешной оценки, можно попросить переоценить авто с другим пробегом, " +
				$"вызывается командой: \"оцени такое же авто но с пробегом Х.\""
			};
		}

		protected override bool Check(AliceRequest request, State state) {
			return state.NextAction.Is(this.GetType());
		}

		protected override async Task<SimpleResponse> Respond(AliceRequest request, State state) {
			await Task.Yield();

			var nextAction = GetNextStrategy();
			return await nextAction.GetMessage(request, state);

		}

		private async Task<SimpleResponse> CreateFinalResult(State state) {
			var result = await externalService.GetAppraisalResponse(state.Request);

			if (result.Status != "success") {
				return new SimpleResponse {
					Text = $"К сожалению я не смог провести оценку вашего авто {state.Request.GetFullName()}. Мне не удалось найти аналогов по вашему запросу."
				};
			}
			var countAds = result.SampleByPrices.HighPriced + result.SampleByPrices.Normal + result.SampleByPrices.LowPriced;
			return new SimpleResponse {
				Text = $"Цена вашего авто {state.Request.GetFullName()} на вторичном рынке составляет {result.OneMonthPrice} руб., " +
				$"всего было проанализировано {countAds} аналогичных авто, разброс цен среди них был в пределах " +
				$"от {result.PriceRange.Min} до {result.PriceRange.Max} руб. " +
				$"Хотите еще оценить автомобиль?"
			};
		}
	}
}
