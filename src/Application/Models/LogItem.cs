using AliceAppraisal.Models;
using System;

namespace AliceAppraisal.Application.Models {
	public class LogItem {
		public Guid Id { get => Guid.NewGuid(); }
		public DateTime CurrentTime { get => DateTime.UtcNow; }
		public DataInfo Data { get; set; }
		public ErrorInfo Errors { get; set; }


		public class DataInfo {
			public AliceRequest Request { get; set; }
			public AliceResponse Response { get; set; }
		}

		public class ErrorInfo {
			public string Message { get; set; }
		}

		public LogItem(AliceRequest req, AliceResponse resp, Exception ex) {
			Data = new DataInfo {
				Request = req,
				Response = resp
			};
			Errors = ex != null
				? new ErrorInfo {
					Message = ex.Message
				}
				: null;
		}

		public static LogItem Create(AliceRequest req, AliceResponse resp, Exception ex) {
			return new LogItem(req, resp, ex);
		}
	}
}
