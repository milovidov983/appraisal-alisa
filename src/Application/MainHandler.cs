using AliceAppraisal.Application.Configuration;
using AliceAppraisal.Core.Engine;
using AliceAppraisal.Core.Engine.Strategy;
using AliceAppraisal.Core.Models;
using AliceAppraisal.Models;
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
		private readonly Settings settings;

		public MainHandler(IStrategyFactory strategyFactory, List<BaseStrategy> strategies, ILogger logger) {
			this.strategyFactory = strategyFactory;
			this.strategies = strategies;
			this.logger = logger;
			this.settings = Settings.Instance;
		}
		public async Task<(AliceResponse, Exception)> HandleRequest(AliceRequest aliceRequest) {
			
			
			State state = aliceRequest.State?.Session ?? new State();
			AliceResponse response = null;
			Exception ex = null;
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

					simple.AddMisunderstanding();
					state.AddMisunderstanding();

					response = AliceResponseBuilder.Create()
						.WithData(aliceRequest)
						.WithState(state)
						.WithText(simple)
						.Build();
				}
			} catch (CustomException e) {
				response = HandleException(aliceRequest, state, e);
				ex = e;
			} catch (Exception e) {
				response = HandleUnhandledException(aliceRequest, state, e);
				ex = e;
			}
			if(ex != null) {
				logger.Information($"- Exception req/res data: {ex.Message}");
				logger.Information($"ALICE_REQUEST:");
				logger.Information(aliceRequest.ToJson());
				logger.Information($"ALICE_RESPONSE:");
				logger.Information(response.ToJson());
			}
			return (response, ex);
		}

		private AliceResponse HandleUnhandledException(AliceRequest aliceRequest, State state, Exception e) {
			state.SetStatusCode(e);
			logger.Error("\n!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! ERROR");
			logger.Error(e, e.Message);
			return new AliceResponse(aliceRequest) {
				Response = new Response {
					Text = "Произошла какая-то ошибка на сервере навыка, разработчик уже уведомлен. " +
						   "Приносим извинения."
				},
				State = state
			};
		
		}

		private AliceResponse HandleException(AliceRequest aliceRequest, State state, CustomException e) {
			state.SetStatusCode(e);
			logger.Error($"{e.GetType().Name} {e.Message}");
			return AliceResponseBuilder.Create()
				.WithData(aliceRequest)
				.WithState(state)
				.WithText(new SimpleResponse {
					Text = e.UserMessage ?? InternalErrorException.DefaultUserMessage
				})
				.Build();

		}
	}
}
