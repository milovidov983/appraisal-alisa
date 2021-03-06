using AliceAppraisal.Models;
using System;
using System.Collections.Generic;

namespace AliceAppraisal.Application.Models {
	public class LogItem {
		public Guid Id { get => Guid.NewGuid(); }
		public DateTime CurrentTime { get => DateTime.UtcNow; }
		public DataInfo Data { get; set; }
		public ErrorInfo Errors { get; set; }

		public static DataInfo BuildLogItem(AliceRequest request, AliceResponse response) {
			var item = new DataInfo {
				Request = new DataInfo.Req {
					UserRequest = request.Request,
					Locale = request.Meta.Locale,
					Timezone = request.Meta.Timezone,
					ClientId = request.Meta.ClientId,
					NextAction = request.State.Session.NextAction,
					PrevAction = request.State.Session.PrevAction
				},
				Response = new DataInfo.Resp {
					MessageId = response.Session.MessageId,
					AppraisalRequest = response.State.Request,
					NextAction = response.State.NextAction,
					PrevAction = response.State.PrevAction,
					Text = response.Response.Text,
					UserId = response.Session.UserId
				},
				MisunderstandingCounter = response.State.MisunderstandingCounter
			};
			return item;
		}


		public LogItem(AliceRequest req, AliceResponse resp, Exception ex) {
			Data = BuildLogItem(req, resp);
			Errors = ex != null
				? new ErrorInfo {
					Message = ex.Message
				}
				: null;
		}

		public static LogItem Create(AliceRequest req, AliceResponse resp, Exception ex) {
			return new LogItem(req, resp, ex);
		}

		public class ErrorInfo {
			public string Message { get; set; }
		}


		public class DataInfo {
			public Req Request { get; set; }
			public Resp Response { get; set; }
			public Dictionary<string, int> MisunderstandingCounter { get; set; }

			public class Req {
				public string Locale { get; set; }
				public string Timezone { get; set; }
				public string ClientId { get; set; }
				public RequestInfo UserRequest { get; set; }
				public string PrevAction { get; set; }
				public string NextAction { get; set; }
			}
			public class Resp {
				public string Text { get; set; }
				public AppraisalQuoteRequest AppraisalRequest { get; set; }
				public string UserId { get; set; }
				public int MessageId { get; set; }
				public string PrevAction { get; set; }
				public string NextAction { get; set; }
			}
		}
	}
}