using System;
using System.Collections.Generic;
using System.Text;

namespace AliceAppraisal.Models {
	public class CustomException : Exception {
		public string UserMessage { get; }
		public CustomException() {
		}

		public CustomException(string message, string userMessage) : base(message) {
			UserMessage = userMessage ?? message;
		}

	}


	public class NotFoundExcteption : CustomException {
		public NotFoundExcteption() {
		}

		public NotFoundExcteption(string message, string userMessage = null) : base(message, userMessage) {
		}
	}

	public class InvalidRequestException : CustomException {
		public InvalidRequestException() {
		}

		public InvalidRequestException(string message, string userMessage = null) : base(message, userMessage) {
		}
	}

	public class ExternalServiceException : CustomException {
		public ExternalServiceException() {
		}

		public ExternalServiceException(string message, string userMessage = null) : base(message, userMessage) {
		}
	}
	public class InternalErrorException : CustomException {
		public InternalErrorException() {
		}

		public InternalErrorException(string message, string userMessage = null) : base(message, userMessage) {
		}

		public const string StandardUserMessage 
			= "В процессе обработки вашего запроса произошла внутренняя ошибка " +
			"попробуйте повторить ваш запрос позднее. Разработчики уже уведомлены " +
			"о случившемся инциденте. Приносим свои извинения.";
	}
}
