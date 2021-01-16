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
	public class GetMakeTests {

		[Fact]
		public async Task Get_make_name_correct_save_id() {
			var aliceRequest = RequestBuilder.Create()
				.WithActions(
					prev: typeof(ConfirmAppraisalStrategy).FullName,
					next: typeof(GetMakeStrategy).FullName)
				.WithMake()
				.Build();

			var handler = new MainHandler(aliceRequest);

			var response = await handler.HandleRequest(aliceRequest);

			Assert.Equal(135, response.State.Request.MakeId);
		}

		[Fact]
		public async Task Set_correct_make_response_text_is_correct() {
			var aliceRequest = RequestBuilder.Create()
				.WithActions(
					prev: typeof(ConfirmAppraisalStrategy).FullName,
					next: typeof(GetMakeStrategy).FullName)
				.WithMake()
				.Build();

			var handler = new MainHandler(aliceRequest);

			var response = await handler.HandleRequest(aliceRequest);

			Assert.Contains("пожалуйста модель вашего автомобиля", response.Response.Text);
		}

	}
}