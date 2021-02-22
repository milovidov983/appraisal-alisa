using AliceAppraisal.Application;
using AliceAppraisal.Core.Engine;
using AliceAppraisal.Infrastructure;
using AliceAppraisal.Models;
using Serilog;
using System;
using System.Threading.Tasks;

namespace AliceAppraisal {
	public class EntryPoint {
		private IMainHandlerFactory handlerFactory;
		private IStorageService storageService;
		private ILogger logger;

		public EntryPoint() {
			var serviceFactory = ServiceFactoryBuilder.Create().GetServiceFactory();
			Init(serviceFactory);
		}

		public EntryPoint(IServiceFactory serviceFactory) {
			Init(serviceFactory);
		}

		private void Init(IServiceFactory serviceFactory) {
			handlerFactory = MainHandlerFactory.Create(serviceFactory);
			storageService = serviceFactory.GetStorageService();
			logger = serviceFactory.GetLoggerFactory().GetLogger();
		}

		public async Task<AliceResponse> FunctionHandler(AliceRequest request) {
			logger.Information("Running FunctionHandler function");
			AliceResponse response = null;
			Exception ex = null;
			try {
				response = GetServiceResponseOrDefault(request);

				if (response is null) {
					IMainHandler handler = handlerFactory.GetHandler();
					var (r, e) = await handler.HandleRequest(request);

					if(e != null) {
						logger.Error(e, e.Message);
						ex = e;
					}

					response = r;
				}
			}
			catch(Exception e) {
				logger.Error(e, e.Message);
				ex = e;
			} finally {
				if (!response.IsServiceResponse()) {
					await storageService.Insert(new {
						Request = request,
						Response = response,
						Error = ex
					});
				}
			}
			return response;
		}

		private AliceResponse GetServiceResponseOrDefault(AliceRequest request) {
			if (request is null) {
				return AliceResponse.CreateMissResponse();
			}

			if (request.IsPing()) {
				return AliceResponse.CreatePong(request);
			}

			return null;
		}

	}
}
