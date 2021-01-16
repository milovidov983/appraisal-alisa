using System;
using System.Collections.Generic;
using System.Text;

namespace AliceAppraisal.Models {
	public class CustomException : Exception {
		public CustomException() {
		}

		public CustomException(string message) : base(message) {
		}
	}


	public class NotFoundExcteption : CustomException {
		public NotFoundExcteption() {
		}

		public NotFoundExcteption(string message) : base(message) {
		}
	}

	public class InvalidRequestException : CustomException {
		public InvalidRequestException() {
		}

		public InvalidRequestException(string message) : base(message) {
		}
	}
}
