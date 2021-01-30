using AliceAppraisal.Engine;
using AliceAppraisal.Engine.Strategy;
using AliceAppraisal.Models;
using AliceAppraisal.Static;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace AliceAppraisal.Application {
	public class MainHandler : IMainHandler {
		private readonly IStrategyFactory strategyFactory;
		private readonly List<BaseStrategy> strategies;
		private readonly ILogger logger;

		public MainHandler(IStrategyFactory strategyFactory, List<BaseStrategy> strategies, ILogger logger) {
			this.strategyFactory = strategyFactory;
			this.strategies = strategies;
			this.logger = logger;
		}
		public async Task<AliceResponse> HandleRequest(AliceRequest aliceRequest) {
			logger.Debug($"ALICE_REQUEST: {JsonSerializer.Serialize(aliceRequest)}");
			State state = aliceRequest.State?.Session ?? new State();
			AliceResponse response;
			try {
				var suitableStrategy = strategies
					.Where(stratagy => stratagy.IsSuitableStrategy(aliceRequest, state))
					.ToArray();

				var tasks = suitableStrategy
					.Select(async (strategy) => await strategy.Run(aliceRequest, state))
					.ToArray();

				await Task.WhenAll(tasks);

				response = tasks
					.Where(resp => resp != null)
					.Select(resp => resp.Result)
					.FirstOrDefault();

				if (response is null) {
					var currentActionName = state.NextAction;
					var currentAction = strategyFactory.GetStrategy(currentActionName);

					if (currentAction is null) {
						currentAction = strategyFactory.GetDefaultStrategy();
					}

					var simple = currentAction.GetMessageForUnknown(aliceRequest, state);
					response = AliceResponseBuilder.Create()
						.WithData(aliceRequest)
						.WithState(state)
						.WithText(simple)
						.Build();
				}
			} catch (CustomException e) {
				state.SetStatusCode(e);
				logger.Error($"{e.GetType().Name} {e.Message}");
				response = AliceResponseBuilder.Create()
					.WithData(aliceRequest)
					.WithState(state)
					.WithText(new SimpleResponse {
						Text = e.UserMessage
					})
					.Build();
			} catch (Exception e) {
				state.SetStatusCode(e);
				logger.Error("\n!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! ERROR");
				logger.Error(e, e.Message);
				response = new AliceResponse(aliceRequest) {
					Response = new Response {
						Text = "Произошла какая-то ошибка на сервере навыка, разработчик уже уведомлен. " +
							   "Приносим извинения."
					},
					State = state
				};
			}
			logger.Debug($"ALICE_RESPONSE: {JsonSerializer.Serialize(response)}");
			return response;
		}
	}
}
