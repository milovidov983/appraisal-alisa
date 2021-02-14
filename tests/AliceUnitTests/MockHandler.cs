using AliceAppraisal.Application;
using AliceAppraisal.Models;
using System.Threading.Tasks;

namespace AliceUnitTests {
	public class MockHandler {
		private IApplicationFactory HandlerFactory { get; }

		public MockHandler() {
			HandlerFactory = new ApplicationFactory();
		}
		public MockHandler(IApplicationFactory handlerFactory) {
			HandlerFactory = handlerFactory;
		}

		public async Task<AliceResponse> FunctionHandler(AliceRequest request) {
			if(request is null) {
				return AliceResponse.CreateMissResponse();
			}

			if (request.IsPing()) {
				return new AliceResponse(request).ToPong();
			}

			IMainHandler handler = HandlerFactory.CreateHandler ();
			var (r,_) = await handler.HandleRequest(request);
			return r;
		}

	}
}
