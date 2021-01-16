using AliceAppraisal.Controllers;
using AliceAppraisal.Engine.Strategy;
using AliceAppraisal.Models;
using AliceUnitTests.Mocks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;
using Moq;
using AliceAppraisal.Engine;

namespace AliceUnitTests.BaseCommandTests {
	public class GetManufatureYearTests {

		//public MainHandler CreateHandler(AliceRequest request) {
		//	var handler =  new MainHandlerMock(request);
		//	var m1 = Mock.Of<IServiceFactory>((s)=>s.GetLogger())
		//}


		[Fact]
		public async Task Set_correct_manufactureYear_year_is_saved() {
			var aliceRequest = RequestBuilder.Create()
				.WithActions(
					prev: typeof(GetModelStrategy).FullName,
					next: typeof(GetManufactureYearStrategy).FullName)
				.WithModelId()
				.WithIntentManufactureYear()
				.Build();

			var handler = new Handler();
			var response = await handler.FunctionHandler(aliceRequest);

			Assert.Equal(2012, response.State.Request.ManufactureYear);
		}

		[Fact]
		public async Task Set_correct_manufactureYear_response_text_is_correct() {
			var aliceRequest = RequestBuilder.Create()
				.WithActions(
					prev: typeof(GetModelStrategy).FullName,
					next: typeof(GetManufactureYearStrategy).FullName)
				.WithModelId()
				.WithIntentManufactureYear()
				.Build();

			var handler = new Handler();
			var response = await handler.FunctionHandler(aliceRequest);

			Assert.Contains("год", response.Response.Text);
		}

		[Fact]
		public async Task Set_correct_manufactureYear_next_action_is_correct() {
			var aliceRequest = RequestBuilder.Create()
				.WithActions(
					prev: typeof(GetManufactureYearStrategy).FullName,
					next: typeof(GetGenerationStrategy).FullName)
				.WithModelId()
				.WithIntentManufactureYear()
				.Build();

			var handler = new Handler();
			var response = await handler.FunctionHandler(aliceRequest);

			Assert.Equal(typeof(GetGenerationStrategy).FullName, response.State.NextAction);
		}

		[Fact]
		public async Task Set_wrong_manufactureYear_return_warning_response() {
			var aliceRequest = RequestBuilder.Create()
				.WithActions(
					prev: typeof(GetModelStrategy).FullName,
					next: typeof(GetManufactureYearStrategy).FullName)
				.Build();

			var handler = new Handler();
			var response = await handler.FunctionHandler(aliceRequest);

			Assert.Contains(
				$"Не удалось распознать модель вашего авто," +
				$" попробуйте повторить ваш запрос или попросите у меня подсказку.",
				response.Response.Text);
		}

	}
}