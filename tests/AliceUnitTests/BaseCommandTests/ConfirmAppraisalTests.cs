using AliceAppraisal.Controllers;
using AliceAppraisal.Engine.Strategy;
using AliceAppraisal.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace AliceUnitTests.BaseCommandTests {
	public class ConfirmAppraisalTests {

		[Fact]
		public async Task Set_yes_bot_response_is_correct() {
			var aliceHelpRequest = RequestBuilder.Create()
				.WithActions(
					prev: typeof(InitialStrategy).FullName,
					next: typeof(ConfirmAppraisalStrategy).FullName)
				.WithConfirm()
				.Build();

			var handler = new MainHandler(aliceHelpRequest);

			var response = await handler.HandleRequest(aliceHelpRequest);

			Assert.Contains(GetMakeStrategy.Messages, (x) => x.Any(y=> response.Response.Text.Contains(y)));
		}

		[Fact]
		public async Task Initial_bot_next_action_is_correct() {
			var aliceHelpRequest = RequestBuilder.Create()
				.WithActions(
					prev: typeof(ConfirmAppraisalStrategy).FullName,
					next: typeof(GetMakeStrategy).FullName)
				.WithConfirm()
				.Build();

			var handler = new MainHandler(aliceHelpRequest);

			var response = await handler.HandleRequest(aliceHelpRequest);

			Assert.Equal(typeof(GetMakeStrategy).FullName, response.State.NextAction);
		}

		[Fact]
		public async Task Get_help_on_first_step_return_help() {
			var aliceHelpRequest = RequestBuilder.Create()
				.WithActions(
					prev: typeof(InitialStrategy).FullName,
					next: typeof(ConfirmAppraisalStrategy).FullName)
				.WithHelp()
				.Build();

			var handler = new MainHandler(aliceHelpRequest);

			var response = await handler.HandleRequest(aliceHelpRequest);

			Assert.Equal("Что бы начать оценку автомобиля скажите \"начать\"", response.Response.Text);
		}
	}
}