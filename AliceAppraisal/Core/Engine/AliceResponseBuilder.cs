using AliceAppraisal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AliceAppraisal.Core.Engine {
	public class AliceResponseBuilder {
		private AliceRequest aliceRequest;
		private SimpleResponse simpleResponse;
		private State state;

		public static AliceResponseBuilder Create() {
			return new AliceResponseBuilder();
		}

		public AliceResponseBuilder WithData(AliceRequest aliceRequest) {
			this.aliceRequest = aliceRequest;
			return this;
		}

		public AliceResponseBuilder WithText(SimpleResponse simpleResponse) {
			this.simpleResponse = simpleResponse;
			return this;
		}

		public AliceResponseBuilder WithState(State state) {
			this.state = state;
			return this;
		}

		public AliceResponse Build() {
			var response = new AliceResponse(aliceRequest);
			var simple = simpleResponse;

			response.State = state;
			response.Response.Text = simple.Text;
			response.Response.Tts = string.IsNullOrEmpty(simple.Tts) ? simple.Text : simple.Tts;
			if (simple.Buttons != null) {
				response.Response.Buttons = simple.Buttons.Select(t => new Button { Title = t }).ToList();
			}

			return response;
		}
	}
}
