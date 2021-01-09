using System;
using System.Collections.Generic;
using System.Text;

namespace AliceAppraisal.Models {
	public class NotFoundExcteption : Exception {
		public NotFoundExcteption() {
		}

		public NotFoundExcteption(string message) : base(message) {
		}
	}
}
