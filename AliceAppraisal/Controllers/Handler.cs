using AliceAppraisal.Configuration;
using AliceAppraisal.Engine;
using AliceAppraisal.Engine.Services;
using AliceAppraisal.Engine.Strategy;
using AliceAppraisal.Models;
using AliceAppraisal.Static;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AliceAppraisal.Controllers {
	public class Handler {
		
		public async Task<AliceResponse> FunctionHandler(AliceRequest request) {
			if(request is null) {
				var response = new AliceResponse(new AliceRequest()) {
					Response = new Response {
						Text = "У меня нет обработчика этого случая. " +
						"Я не понимаю что вы мне говорите. Приносим извинения.",
						EndSession = false
					}
				};
				return response;
			}

			var aliceRequest = request;
			if (aliceRequest.IsPing()) {
				var pongResponse = new AliceResponse(aliceRequest).ToPong();
				return  pongResponse;
			}

			var handler = CreateHandler(aliceRequest);
			var aliceResponse = await handler.HandleRequest(aliceRequest);

			return aliceResponse;
		}

		private MainHandler CreateHandler(AliceRequest aliceRequest) {
			return new MainHandler(aliceRequest);
		}

	}

	public class MainHandler {
		private static IStrategyFactory strategyFactory;
		private static List<BaseStrategy> strategies;
		private static readonly ILogger logger = new LoggerConfiguration()
			.WriteTo
			.Console()
			.MinimumLevel
			.Debug()
			.CreateLogger();

		private readonly State state;

		public MainHandler(AliceRequest request) {
			state = request.State?.Session ?? new State();
		}

		static MainHandler() {
			var factory = new ServiceFactory(logger);
			strategies = ReflectiveEnumerator.GetEnumerableOfType<BaseStrategy>(factory).ToList();
			strategyFactory = factory.GetStrategyFactory();
		}

		protected void ReInit(IServiceFactory factory) {
			strategies = ReflectiveEnumerator.GetEnumerableOfType<BaseStrategy>(factory).ToList();
			strategyFactory = factory.GetStrategyFactory();
		}


		public async Task<AliceResponse> HandleRequest(AliceRequest aliceRequest) {
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
					.Select(resp => resp.Result)
					.Where(resp => resp != null)
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
			} catch(NotFoundExcteption e) {
				response = AliceResponseBuilder.Create()
					.WithData(aliceRequest)
					.WithState(state)
					.WithText(new SimpleResponse { 
						Text = e.Message,
						Buttons = Buttons.BaseExtended
					})
					.Build();

			
			} catch (Exception e) {
				logger.Error("\n!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! ERROR");
				logger.Error(e, e.Message);
				throw e;
				response = new AliceResponse(aliceRequest) {
					Response = new Response {
						Text = "Произошла какая-то ошибка на сервере навыка, разработчик уже уведомлен. " +
							   "Приносим извинения."
					},
					State = state
				};
			}

			return response;
		}
	}
}
