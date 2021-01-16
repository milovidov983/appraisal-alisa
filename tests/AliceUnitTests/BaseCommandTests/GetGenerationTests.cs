using AliceAppraisal.Controllers;
using AliceAppraisal.Engine.Strategy;
using System;
using System.Threading.Tasks;
using Xunit;

namespace AliceUnitTests.BaseCommandTests {
	public class GetGenerationTests {

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
					prev: typeof(GetModelStrategy).FullName,
					next: typeof(GetManufactureYearStrategy).FullName)
				.WithModelId()
				.WithIntentManufactureYear()
				.Build();

			var handler = new Handler();
			var response = await handler.FunctionHandler(aliceRequest);

			Assert.Equal(typeof(GetGenerationStrategy).FullName, response.State.NextAction);
		}

		[Fact]
		public async Task Set_wrong_manufactureYear_return_warning_response() {
			var wrongManufactureYear = DateTime.UtcNow.Year + 1;
			var aliceRequest = RequestBuilder.Create()
				.WithActions(
					prev: typeof(GetModelStrategy).FullName,
					next: typeof(GetManufactureYearStrategy).FullName)
				.WithModelId()
				.WithIntentManufactureYear(wrongManufactureYear)
				.Build();

			var handler = new Handler();
			var response = await handler.FunctionHandler(aliceRequest);

			Assert.Contains(
				$"Кажется указанный вами {wrongManufactureYear} год выпуска еще наступил, попробуйте еще раз.",
				response.Response.Text);
		}

	}
}