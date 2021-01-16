using AliceAppraisal.Application;
using AliceAppraisal.Models;
using System.Threading.Tasks;

namespace AliceAppraisal.Controllers {
	public class Handler {
		private IHandlerFactory HandlerFactory { get; }

		public Handler() {
			HandlerFactory = new HandlerFactory();
		}
		public Handler(IHandlerFactory handlerFactory) {
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
