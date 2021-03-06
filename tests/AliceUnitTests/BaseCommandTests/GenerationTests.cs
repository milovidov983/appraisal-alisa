﻿using AliceAppraisal.Core.Engine.Strategy;
using AliceAppraisal.Models;
using System.Threading.Tasks;
using Xunit;

namespace AliceUnitTests.BaseCommandTests {
	public class GenerationTests {

		//public MainHandler CreateHandler(AliceRequest request) {
		//	var handler =  new MainHandlerMock(request);
		//	var m1 = Mock.Of<IServiceFactory>((s)=>s.GetLogger())
		//}


		[Fact]
		public async Task Set_correct_generation_result_saved() {
			var aliceRequest = RequestBuilder.Create()
				.WithActions(
					prev: typeof(ManufactureYearStrategy).FullName,
					next: typeof(GenerationStrategy).FullName)
				.WithGenerationChoise(single: false)
				.WithNumberIntent(1)
				.Build();

			var handler = MockHandler.Create();
			var response = await handler.FunctionHandler(aliceRequest);

			Assert.Equal(18, response.State.Request.GenerationId);
		}

		[Fact]
		public async Task Set_correct_generation_status_ok() {
			var aliceRequest = RequestBuilder.Create()
				.WithActions(
					prev: typeof(ManufactureYearStrategy).FullName,
					next: typeof(GenerationStrategy).FullName)
				.WithGenerationChoise(single: false)
				.WithNumberIntent(1)
				.Build();

			var handler = MockHandler.Create();
			var response = await handler.FunctionHandler(aliceRequest);

			Assert.Equal(StatusCodes.OK, response.State.StatusCode);
		}

		[Fact]
		public async Task Set_correct_generation_next_action_is_correct() {
			var aliceRequest = RequestBuilder.Create()
				.WithActions(
					prev: typeof(ManufactureYearStrategy).FullName,
					next: typeof(GenerationStrategy).FullName)
				.WithGenerationChoise(single: false)
				.WithNumberIntent(1)
				.Build();

			var handler = MockHandler.Create();
			var response = await handler.FunctionHandler(aliceRequest);

			Assert.Equal(typeof(BodyTypeStrategy).FullName, response.State.NextAction);
		}

		[Fact]
		public async Task Set_wrong_generation_return_warning_response() {
			var aliceRequest = RequestBuilder.Create()
				.WithActions(
					prev: typeof(ManufactureYearStrategy).FullName,
					next: typeof(GenerationStrategy).FullName)
				.WithGenerationChoise(single: false)
				.WithNumberIntent(99999)
				.Build();

			var handler = MockHandler.Create();
			var response = await handler.FunctionHandler(aliceRequest);

			Assert.Contains($"Выберите нужный вариант поколения авто",response.Response.Text);
		}




	}
}