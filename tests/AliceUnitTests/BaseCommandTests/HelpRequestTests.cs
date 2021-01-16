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
	public class HelpRequestTests {

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