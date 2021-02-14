using AliceAppraisal.Application;
using AliceAppraisal.Infrastructure;
using AliceAppraisal.Models;
using System;
using System.Threading.Tasks;

namespace AliceAppraisal.Application {
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
			AliceResponse response = null;
			Exception ex = null;
			try {
				var serviceResponse = GetServiceResponseOrDefault(request);

				if (serviceResponse is null) {
					IMainHandler handler = factory.CreateHandler();
					var (r, e) = await handler.HandleRequest(request);

					response = r;
					ex = e;
				} else {
					response = serviceResponse;
				}
			} catch(Exception e) {
				ex = e;
			} finally {
				await storageService.Insert(new {
					Request = request,
					Response = response,
					Error = ex
				});
			}
			return response;
		}

		private AliceResponse GetServiceResponseOrDefault(AliceRequest request) {
			if (request is null) {
				return AliceResponse.CreateMissResponse();
			}

			if (request.IsPing()) {
				return new AliceResponse(request).ToPong();
			}

			return null;
		}

	}
}
