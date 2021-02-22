using AliceAppraisal.Application;
using AliceAppraisal.Core.Engine;
using AliceAppraisal.Models;
using System.Threading.Tasks;

namespace AliceUnitTests {
	public class MockHandler {
		private IMainHandlerFactory HandlerFactory { get; }

		private MockHandler(IServiceFactory serviceFactory) {
			HandlerFactory = MainHandlerFactory.Create(serviceFactory);
		}
		public MockHandler(IMainHandlerFactory handlerFactory) {
			HandlerFactory = handlerFactory;
		}

		public async Task<AliceResponse> FunctionHandler(AliceRequest request) {
			if(request is null) {
				return AliceResponse.CreateMissResponse();
			}

			if (request.IsPing()) {
				return new AliceResponse(request).ToPong();
			}

			IMainHandler handler = HandlerFactory.GetHandler();
			var (r,_) = await handler.HandleRequest(request);
			return r;
		}

		public static MockHandler Create(IServiceFactory serviceFactory = null) {
			return new MockHandler(serviceFactory ?? ServiceFactoryBuilder.Create().GetServiceFactory());
		}

	}
}
