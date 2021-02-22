using AliceAppraisal.Application;
using AliceAppraisal.Infrastructure;
using AliceAppraisal.Models;
using System;
using System.Threading.Tasks;

namespace AliceAppraisal {
	public class EntryPoint {
		private readonly IApplicationFactory factory;
		private readonly IStorageService storageService;
		public EntryPoint() {
			factory = new ApplicationFactory();
			storageService = factory.GetStorage();
		}
		public EntryPoint(IApplicationFactory handlerFactory) {
			factory = handlerFactory;
			storageService = factory.GetStorage();
		}

		public async Task<AliceResponse> FunctionHandler(AliceRequest request) {
			var logger = factory.CreateLogger();
			logger.Information("Running FunctionHandler function");
			AliceResponse response = null;
			Exception ex = null;
			try {
				response = GetServiceResponseOrDefault(request);

				if (response is null) {
					IMainHandler handler = factory.CreateHandler();
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
