using AliceAppraisal.Core.Engine.Strategy;
using AliceAppraisal.Models;
using System.Threading.Tasks;
using Xunit;

namespace AliceUnitTests.BaseCommandTests {
	public class GetModelTests {

		[Fact]
		public async Task Set_correct_model_id_is_saved() {
			var aliceRequest = RequestBuilder.Create()
				.WithActions(
					prev: typeof(GetMakeStrategy).FullName,
					next: typeof(GetModelStrategy).FullName)
				.WithIntentModel()
				.WithStoredMake(130)
				.Build();

			var handler = MockHandler.Create();
			var response = await handler.FunctionHandler(aliceRequest);

			Assert.Equal(1427, response.State.Request.ModelId);
		}

		[Fact]
		public async Task Set_correct_model_response_is_correct() {
			var aliceRequest = RequestBuilder.Create()
				.WithActions(
					prev: typeof(GetMakeStrategy).FullName,
					next: typeof(GetModelStrategy).FullName)
				.WithIntentModel()
				.WithStoredMake(130)
				.Build();

			var handler = MockHandler.Create();
			var response = await handler.FunctionHandler(aliceRequest);

			Assert.Equal(StatusCodes.OK, response.State.StatusCode);
		}

		[Fact]
		public async Task Set_correct_model_next_action_is_correct() {
			var aliceRequest = RequestBuilder.Create()
				.WithActions(
					prev: typeof(GetModelStrategy).FullName,
					next: typeof(GetManufactureYearStrategy).FullName)
				.WithIntentModel()
				.Build();

			var handler = MockHandler.Create();
			var response = await handler.FunctionHandler(aliceRequest);

			Assert.Equal(typeof(GetManufactureYearStrategy).FullName, response.State.NextAction);
		}

		[Fact]
		public async Task Set_wrong_model_return_warning_response() {
			var aliceRequest = RequestBuilder.Create()
				.WithActions(
					prev: typeof(GetMakeStrategy).FullName,
					next: typeof(GetModelStrategy).FullName)
				.Build();

			var handler = MockHandler.Create();
			var response = await handler.FunctionHandler(aliceRequest);

			Assert.Contains(
				$"Не удалось распознать модель вашего авто," +
				$" попробуйте повторить ваш запрос или попросите у меня подсказку.",
				response.Response.Text);
		}

		[Fact]
		public async Task Correct_model_when_similar_name() {
			var aliceRequest = RequestBuilder.Create()
				.WithActions(
					prev: typeof(GetMakeStrategy).FullName,
					next: typeof(GetModelStrategy).FullName)
				.WithIntentMake("bmw_18")
				.WithIntentModelSimilar()
				.WithMakeId(18)
				.Build();

			var handler = MockHandler.Create();
			
			var response = await handler.FunctionHandler(aliceRequest);

			Assert.Contains(
				typeof(GetManufactureYearStrategy).FullName,
				response.State.NextAction);
			Assert.Equal(
				146,
				response.State.Request.ModelId ?? -1);
		}

	}
}