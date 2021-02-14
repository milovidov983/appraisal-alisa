using AliceAppraisal.Application;
using AliceAppraisal.Core.Engine;
using AliceAppraisal.Core.Engine.Services;
using AliceAppraisal.Core.Engine.Strategy;
using AliceAppraisal.Models;
using AliceUnitTests.Builders;
using Serilog;
using System;
using System.Threading.Tasks;
using Xunit;

namespace AliceUnitTests.BaseCommandTests {
	public class GetManufatureYearTests {
		private ILogger logger = new LoggerConfiguration()
					.WriteTo
					.Console()
					.MinimumLevel
					.Debug()
					.CreateLogger();


		[Fact]
		public async Task Set_correct_manufactureYear_year_is_saved() {
			var aliceRequest = RequestBuilder.Create()
				.WithActions(
					prev: typeof(GetModelStrategy).FullName,
					next: typeof(GetManufactureYearStrategy).FullName)
				.WithModelId()
				.WithIntentManufactureYear()
				.WithGenerationChoise()
				.Build();

			var handler = new MockHandler();
			var response = await handler.FunctionHandler(aliceRequest);

			Assert.Equal(2012, response.State.Request.ManufactureYear);
		}

		[Fact]
		public async Task Set_correct_manufactureYear_response_ok() {
			var aliceRequest = RequestBuilder.Create()
				.WithActions(
					prev: typeof(GetModelStrategy).FullName,
					next: typeof(GetManufactureYearStrategy).FullName)
				.WithModelId()
				.WithIntentManufactureYear()
				.WithGenerationChoise()
				.Build();

			var externalServiceMock = ExternalServiceBuilder
				.Create()
				.WithTwoGeneration()
				.Build();
			IServiceFactory serviceFactory = new ServiceFactory(logger, externalServiceMock);
			IApplicationFactory handlerFactory = new ApplicationFactory(serviceFactory);


			var handler = new MockHandler(handlerFactory);
			var response = await handler.FunctionHandler(aliceRequest);

			Assert.Equal(StatusCodes.OK, response.State.StatusCode);
		}

		[Fact]
		public async Task Set_correct_manufactureYear_next_action_is_correct() {
			var aliceRequest = RequestBuilder.Create()
				.WithActions(
					prev: typeof(GetModelStrategy).FullName,
					next: typeof(GetManufactureYearStrategy).FullName)
				.WithModelId()
				.WithIntentManufactureYear()
				.WithGenerationChoise()
				.Build();

			var handler = new MockHandler();
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

			var handler = new MockHandler();
			var response = await handler.FunctionHandler(aliceRequest);

			Assert.Equal(StatusCodes.InvalidRequest, response.State.StatusCode);
		}

		[Fact]
		public async Task Find_one_generation_set_special_action() {
			var aliceRequest = RequestBuilder.Create()
					.WithActions(
						prev: typeof(GetModelStrategy).FullName,
						next: typeof(GetManufactureYearStrategy).FullName)
					.WithModelId()
					.WithIntentManufactureYear()
					.WithGenerationChoise(single: true)
					.Build();

			var externalServiceMock = ExternalServiceBuilder
				.Create()
				.WithOneGeneration()
				.Build();
			IServiceFactory serviceFactory = new ServiceFactory(logger, externalServiceMock);
			IApplicationFactory handlerFactory = new ApplicationFactory(serviceFactory);


			var handler = new MockHandler(handlerFactory);
			var response = await handler.FunctionHandler(aliceRequest);

			Assert.Equal(typeof(ConfirmGenerationStrategy).FullName, response.State.NextAction);
		}

	}
}