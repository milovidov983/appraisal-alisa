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
		public async Task Confirm_appraisal_bot_response_is_correct() {
			var aliceRequest = RequestBuilder.Create()
				.WithActions(
					prev: typeof(InitialStrategy).FullName,
					next: typeof(ConfirmAppraisalStrategy).FullName)
				.WithConfirm()
				.Build();

			var handler = new Handler();
			var response = await handler.FunctionHandler(aliceRequest);

			Assert.Equal(StatusCodes.OK, response.State.StatusCode);
		}

		[Fact]
		public async Task Confirm_appraisal_next_action_is_correct() {
			var aliceRequest = RequestBuilder.Create()
				.WithActions(
					prev: typeof(ConfirmAppraisalStrategy).FullName,
					next: typeof(GetMakeStrategy).FullName)
				.WithConfirm()
				.Build();

			var handler = new Handler();
			var response = await handler.FunctionHandler(aliceRequest);

			Assert.Equal(typeof(GetMakeStrategy).FullName, response.State.NextAction);
		}

		[Fact]
		public async Task Get_help_on_confirm_appraisal_return_help() {
			var aliceRequest = RequestBuilder.Create()
				.WithActions(
					prev: typeof(InitialStrategy).FullName,
					next: typeof(ConfirmAppraisalStrategy).FullName)
				.WithHelp()
				.Build();

			var handler = new Handler();
			var response = await handler.FunctionHandler(aliceRequest);

			Assert.Equal("Что бы начать оценку автомобиля скажите \"начать\"", response.Response.Text);
		}
	}
}