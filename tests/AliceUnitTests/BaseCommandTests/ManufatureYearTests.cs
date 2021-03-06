﻿using AliceAppraisal.Application;
using AliceAppraisal.Core.Engine;
using AliceAppraisal.Core.Engine.Strategy;
using AliceAppraisal.Models;
using AliceUnitTests.Builders;
using System;
using System.Threading.Tasks;
using Xunit;

namespace AliceUnitTests.BaseCommandTests {
	public class ManufatureYearTests {
		[Fact]
		public async Task Set_correct_manufactureYear_year_is_saved() {
			var aliceRequest = RequestBuilder.Create()
				.WithActions(
					prev: typeof(ModelStrategy).FullName,
					next: typeof(ManufactureYearStrategy).FullName)
				.WithModelId()
				.WithIntentManufactureYear()
				.WithGenerationChoise()
				.Build();

			var handler = MockHandler.Create();
			var response = await handler.FunctionHandler(aliceRequest);

			Assert.Equal(2012, response.State.Request.ManufactureYear);
		}

		[Fact]
		public async Task Set_correct_manufactureYear_response_ok() {
			var aliceRequest = RequestBuilder.Create()
				.WithActions(
					prev: typeof(ModelStrategy).FullName,
					next: typeof(ManufactureYearStrategy).FullName)
				.WithModelId()
				.WithIntentManufactureYear()
				.WithGenerationChoise()
				.Build();

			var externalServiceMock = ExternalServiceBuilder
				.Create()
				.WithTwoGeneration()
				.Build();
			IServiceFactory serviceFactory = new ServiceFactory(null, externalServiceMock);
			IMainHandlerFactory handlerFactory = MainHandlerFactory.Create(serviceFactory);


			var handler = new MockHandler(handlerFactory);
			var response = await handler.FunctionHandler(aliceRequest);

			Assert.Equal(StatusCodes.OK, response.State.StatusCode);
		}

		[Fact]
		public async Task Set_correct_manufactureYear_next_action_is_correct() {
			var aliceRequest = RequestBuilder.Create()
				.WithActions(
					prev: typeof(ModelStrategy).FullName,
					next: typeof(ManufactureYearStrategy).FullName)
				.WithModelId()
				.WithIntentManufactureYear()
				.WithGenerationChoise()
				.Build();

			var handler = MockHandler.Create();
			var response = await handler.FunctionHandler(aliceRequest);

			Assert.Equal(typeof(GenerationStrategy).FullName, response.State.NextAction);
		}

		[Fact]
		public async Task Set_wrong_manufactureYear_return_warning_response() {
			var wrongManufactureYear = DateTime.UtcNow.Year + 1;
			var aliceRequest = RequestBuilder.Create()
				.WithActions(
					prev: typeof(ModelStrategy).FullName,
					next: typeof(ManufactureYearStrategy).FullName)
				.WithModelId()
				.WithIntentManufactureYear(wrongManufactureYear)
				.Build();

			var handler = MockHandler.Create();
			var response = await handler.FunctionHandler(aliceRequest);

			Assert.Equal(StatusCodes.InvalidRequest, response.State.StatusCode);
		}

		[Fact]
		public async Task Find_one_generation_set_special_action() {
			var aliceRequest = RequestBuilder.Create()
					.WithActions(
						prev: typeof(ModelStrategy).FullName,
						next: typeof(ManufactureYearStrategy).FullName)
					.WithModelId()
					.WithIntentManufactureYear()
					.WithGenerationChoise(single: true)
					.Build();

			var externalServiceMock = ExternalServiceBuilder
				.Create()
				.WithOneGeneration()
				.Build();
			IServiceFactory serviceFactory = new ServiceFactory(null, externalServiceMock);
			IMainHandlerFactory handlerFactory = MainHandlerFactory.Create(serviceFactory);


			var handler = new MockHandler(handlerFactory);
			var response = await handler.FunctionHandler(aliceRequest);

			Assert.Equal(typeof(ConfirmGenerationStrategy).FullName, response.State.NextAction);
		}

	}
}