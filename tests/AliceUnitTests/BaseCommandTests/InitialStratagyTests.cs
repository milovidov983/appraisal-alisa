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
			var aliceHelpRequest = RequestBuilder.Create()
				.SetNew()
				.WithMessageId(0)
				.Build();

			var handler = new MainHandler(aliceHelpRequest);

			var response = await handler.HandleRequest(aliceHelpRequest);

			Assert.Contains("стоимость подержанных автомобилей", response.Response.Text);
		}

		[Fact]
		public async Task Initial_bot_next_action_is_correct() {
			var aliceHelpRequest = RequestBuilder.Create()
				.SetNew()
				.WithMessageId(0)
				.Build();

			var handler = new MainHandler(aliceHelpRequest);

			var response = await handler.HandleRequest(aliceHelpRequest);

			Assert.Equal(typeof(ConfirmAppraisalStrategy).FullName, response.State.NextAction);
		}
	}
}