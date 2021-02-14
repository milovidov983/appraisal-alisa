using AliceAppraisal.Application;
using AliceAppraisal.Models;
using System.Threading.Tasks;

namespace AliceUnitTests {
	public class MockHandler {
		private IHandlerFactory HandlerFactory { get; }

		public MockHandler() {
			HandlerFactory = new HandlerFactory();
		}
		public MockHandler(IHandlerFactory handlerFactory) {
			HandlerFactory = handlerFactory;
		}

		public async Task<AliceResponse> FunctionHandler(AliceRequest request) {
			if(request is null) {
				return AliceResponse.CreateMissResponse();
			}

			if (request.IsPing()) {
				return new AliceResponse(request).ToPong();
			}

			IMainHandler handler = HandlerFactory.Create();
			return await handler.HandleRequest(request);
		}

	}
}
