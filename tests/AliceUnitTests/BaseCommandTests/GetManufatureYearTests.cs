using AliceAppraisal.Controllers;
using AliceAppraisal.Engine.Strategy;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace AliceUnitTests.BaseCommandTests {
	public class GetManufatureYearTests {

		[Fact]
		public async Task Set_correct_manufactureYear_year_is_saved() {
			var aliceRequest = RequestBuilder.Create()
				.WithActions(
					prev: typeof(GetModelStrategy).FullName,
					next: typeof(GetManufactureYearStrategy).FullName)
				.WithModel()
				.WithManufactureYear()
				.Build();

			var handler = new MainHandler(aliceRequest);

			var response = await handler.HandleRequest(aliceRequest);

			Assert.Equal(2012, response.State.Request.ManufactureYear);
		}

		[Fact]
		public async Task Set_correct_manufactureYear_response_text_is_correct() {
			var aliceRequest = RequestBuilder.Create()
				.WithActions(
					prev: typeof(GetModelStrategy).FullName,
					next: typeof(GetManufactureYearStrategy).FullName)
				.WithManufactureYear()
				.Build();

			var handler = new MainHandler(aliceRequest);

			var response = await handler.HandleRequest(aliceRequest);

			Assert.Contains("год выпуска вашего автомобиля", response.Response.Text);
		}

		[Fact]
		public async Task Set_correct_manufactureYear_next_action_is_correct() {
			var aliceRequest = RequestBuilder.Create()
				.WithActions(
					prev: typeof(GetManufactureYearStrategy).FullName,
					next: typeof(GetGenerationStrategy).FullName)
				.WithManufactureYear()
				.Build();

			var handler = new MainHandler(aliceRequest);

			var response = await handler.HandleRequest(aliceRequest);

			Assert.Equal(typeof(GetGenerationStrategy).FullName, response.State.NextAction);
		}

		[Fact]
		public async Task Set_wrong_manufactureYear_return_warning_response() {
			var aliceRequest = RequestBuilder.Create()
				.WithActions(
					prev: typeof(GetModelStrategy).FullName,
					next: typeof(GetManufactureYearStrategy).FullName)
				.Build();

			var handler = new MainHandler(aliceRequest);

			var response = await handler.HandleRequest(aliceRequest);

			Assert.Contains(
				$"Не удалось распознать модель вашего авто," +
				$" попробуйте повторить ваш запрос или попросите у меня подсказку.",
				response.Response.Text);
		}

	}
}