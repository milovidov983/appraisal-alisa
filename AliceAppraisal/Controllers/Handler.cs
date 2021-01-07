using AliceAppraisal.Configuration;
using AliceAppraisal.Engine;
using AliceAppraisal.Engine.Services;
using AliceAppraisal.Engine.Stratagy;
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
		private static readonly IServiceFactory serviceFactory = new ServiceFactory();
		private static readonly List<BaseStratagy> stratagy;
		private static readonly ITextGeneratorService textGenerator;
		private readonly ILogger logger = new LoggerConfiguration().WriteTo.Console()
												.MinimumLevel.Debug().CreateLogger();
		private readonly State _state;


		public MainHandler(AliceRequest request) {
			_state = request.State?.Session ?? new State();
		}

		static MainHandler() {
			stratagy = ReflectiveEnumerator.GetEnumerableOfType<BaseStratagy>(serviceFactory).ToList();
			textGenerator = serviceFactory.GetTextGeneratorService();
		}


		public async Task<AliceResponse> HandleRequest(AliceRequest aliceRequest) {
			AliceResponse response;
			try {
				var suitableStrategy = stratagy
					.Where(stratagy => stratagy.IsSuitableStrategy(aliceRequest, _state))
					.ToArray();
		
				var tasks = suitableStrategy
					.Select(async (strategy) => {
						return await strategy.Run(aliceRequest, _state);
						})
					.ToArray();

				await Task.WhenAll(tasks);

				response = tasks
					.Select(resp => resp.Result)
					.Where(resp => resp != null)
					.FirstOrDefault();

				if (response is null) {
					var simple = textGenerator.CreateAnsverForUnexpectedCommand();
					response = AliceResponseBuilder.Create()
						.WithData(aliceRequest)
						.WithState(_state)
						.WithText(simple)
						.Build();
				}
			} catch (Exception e) {
				logger.Error("\n!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! ERROR");
				logger.Error(e, e.Message);
				response = new AliceResponse(aliceRequest) {
					Response = new Response {
						Text = "Произошла какая-то ошибка на сервере навыка, разработчик уже уведомлен. " +
							   "Приносим извинения."
					},
					State = _state
				};
			}

			return response;
		}
	}
}
