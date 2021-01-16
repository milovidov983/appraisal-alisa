using AliceAppraisal.Application;
using AliceAppraisal.Controllers;
using AliceAppraisal.Engine;
using AliceAppraisal.Engine.Services;
using AliceAppraisal.Engine.Strategy;
using AliceAppraisal.Models;
using AliceUnitTests.Builders;
using Moq;
using Serilog;
using System;
using System.Linq;
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
				.Build();

			var handler = new Handler();
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
				.Build();

			var externalServiceMock = ExternalServiceBuilder
				.Create()
				.WithTwoGeneration()
				.Build();
			IServiceFactory serviceFactory = new ServiceFactory(logger, externalServiceMock);
			IHandlerFactory handlerFactory = new HandlerFactory(serviceFactory);


			var handler = new Handler(handlerFactory);
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

			Assert.Equal(StatusCodes.InvalidRequest, response.State.StatusCode);
		}

	}
}