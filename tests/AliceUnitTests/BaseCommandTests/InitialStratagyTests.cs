using AliceAppraisal.Controllers;
using AliceAppraisal.Engine.Strategy;
using AliceAppraisal.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace AliceUnitTests.BaseCommandTests {
	public class InitialStratagyTests {

		[Fact]
		public async Task Initial_bot_response_is_correct() {
			var aliceRequest = RequestBuilder.Create()
				.SetNew()
				.WithMessageId(0)
				.Build();

			var handler = new MainHandler(aliceRequest);

			var response = await handler.HandleRequest(aliceRequest);

			Assert.Contains("стоимость подержанных автомобилей", response.Response.Text);
		}

		[Fact]
		public async Task Initial_bot_next_action_is_correct() {
			var aliceRequest = RequestBuilder.Create()
				.SetNew()
				.WithMessageId(0)
				.Build();

			var handler = new MainHandler(aliceRequest);

			var response = await handler.HandleRequest(aliceRequest);

			Assert.Equal(typeof(ConfirmAppraisalStrategy).FullName, response.State.NextAction);
		}
	}
}