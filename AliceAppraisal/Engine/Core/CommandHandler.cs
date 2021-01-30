using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AliceAppraisal.Engine.Core {
	public class CommandHandler {
		private IHandlerFatory handlerFatory;

		public CommandHandler(IHandlerFatory handlerFatory) {
			this.handlerFatory = handlerFatory;
		}
		public async Task<IHandlerResult> HandleRequest(IRequest request) {
			IRequestType type = request.GetRequestType();
			IRequestHandler handler = handlerFatory.CreateHandelr(type);
			IRequestHandleResult result = await handler.HandleRequest(request);

			IResponseHandler responseHandler = handlerFatory.CreateHandler(result);

			return responseHandler.CreateResponse(result);
		}
	}

	public interface IResponseHandler {
		IHandlerResult CreateResponse(IRequestHandleResult result);
	}

	/// <summary>
	/// Ответ
	/// </summary>
	public interface IHandlerResult {
	}

	public interface IResponseHandlerFatory {
	}

	/// <summary>
	/// Результат работы обработчика команды
	/// </summary>
	public interface IRequestHandleResult {
	}

	/// <summary>
	/// Обработчик запроса
	/// </summary>
	public interface IRequestHandler {
		Task<IRequestHandleResult> HandleRequest(IRequest request);
	}

	public interface IHandlerFatory {
		IRequestHandler CreateHandelr(IRequestType type);
		IResponseHandler CreateHandler(IRequestHandleResult status);
	}

	public interface IRequestType {
	}
}
