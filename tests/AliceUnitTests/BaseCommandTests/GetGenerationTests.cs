using AliceAppraisal.Controllers;
using AliceAppraisal.Engine.Strategy;
using AliceAppraisal.Models;
using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace AliceUnitTests.BaseCommandTests {
	public class GetGenerationTests {

		//public MainHandler CreateHandler(AliceRequest request) {
		//	var handler =  new MainHandlerMock(request);
		//	var m1 = Mock.Of<IServiceFactory>((s)=>s.GetLogger())
		//}


		[Fact]
		public async Task Set_correct_generation_result_saved() {
			var aliceRequest = RequestBuilder.Create()
				.WithActions(
					prev: typeof(GetManufactureYearStrategy).FullName,
					next: typeof(GetGenerationStrategy).FullName)
				.WithGenerationChoise(single: false)
				.WithNumberIntent(1)
				.Build();

			var handler = new Handler();
			var response = await handler.FunctionHandler(aliceRequest);

			Assert.Equal(12345, response.State.Request.GenerationId);
		}

		[Fact]
		public async Task Set_correct_generation_status_ok() {
			var aliceRequest = RequestBuilder.Create()
				.WithActions(
					prev: typeof(GetManufactureYearStrategy).FullName,
					next: typeof(GetGenerationStrategy).FullName)
				.WithGenerationChoise(single: false)
				.WithNumberIntent(1)
				.Build();

			var handler = new Handler();
			var response = await handler.FunctionHandler(aliceRequest);

			Assert.Equal(StatusCodes.OK, response.State.StatusCode);
		}

		[Fact]
		public async Task Set_correct_generation_next_action_is_correct() {
			var aliceRequest = RequestBuilder.Create()
				.WithActions(
					prev: typeof(GetManufactureYearStrategy).FullName,
					next: typeof(GetGenerationStrategy).FullName)
				.WithGenerationChoise(single: false)
				.WithNumberIntent(1)
				.Build();

			var handler = new Handler();
			var response = await handler.FunctionHandler(aliceRequest);

			Assert.Equal(typeof(GetBodyTypeStrategy).FullName, response.State.NextAction);
		}

		[Fact]
		public async Task Set_wrong_generation_return_warning_response() {
			var aliceRequest = RequestBuilder.Create()
				.WithActions(
					prev: typeof(GetManufactureYearStrategy).FullName,
					next: typeof(GetGenerationStrategy).FullName)
				.WithGenerationChoise(single: false)
				.WithNumberIntent(99999)
				.Build();

			var handler = new Handler();
			var response = await handler.FunctionHandler(aliceRequest);

			Assert.Contains($"Выберите нужный вариант поколения авто",response.Response.Text);
		}

		[Fact]
		public async Task Test() {
			//AliceRequest aliceRequest = JsonSerializer.Deserialize<AliceRequest>(
			//	File.ReadAllText(@"D:\Source\PedProject\appraisal-alisa\tests\AliceUnitTests\BaseCommandTests\JsonRequests\anomaly.json"));

			//var handler = new Handler();
			//var response = await handler.FunctionHandler(aliceRequest);

			//Assert.Contains($"Выберите нужный вариант поколения авто", response.Response.Text);
		}


	}
}