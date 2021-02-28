using AliceAppraisal.Core.Engine.Strategy;
using System.Threading.Tasks;
using Xunit;

namespace AliceUnitTests.BaseCommandTests {
	public class MakeTests {
		[Fact]
		public async Task Set_correct_make_id_is_saved() {
			var aliceRequest = RequestBuilder.Create()
				.WithActions(
					prev: typeof(InitStrategy).FullName,
					next: typeof(MakeStrategy).FullName)
				.WithIntentMake()
				.Build();

			var handler = MockHandler.Create();
			var response = await handler.FunctionHandler(aliceRequest);

			Assert.Equal(135, response.State.Request.MakeId);
		}

		[Fact]
		public async Task Set_correct_make_response_text_is_correct() {
			var aliceRequest = RequestBuilder.Create()
				.WithActions(
					prev: typeof(InitStrategy).FullName,
					next: typeof(MakeStrategy).FullName)
				.WithIntentMake()
				.Build();

			var handler = MockHandler.Create();
			var response = await handler.FunctionHandler(aliceRequest);

			Assert.Contains("пожалуйста модель вашего автомобиля", response.Response.Text);
		}

		[Fact]
		public async Task Set_correct_make_next_action_is_correct() {
			var aliceRequest = RequestBuilder.Create()
				.WithActions(
					prev: typeof(MakeStrategy).FullName,
					next: typeof(ModelStrategy).FullName)
				.WithIntentMake()
				.Build();

			var handler = MockHandler.Create();
			var response = await handler.FunctionHandler(aliceRequest);

			Assert.Equal(typeof(ModelStrategy).FullName, response.State.NextAction);
		}

		[Fact]
		public async Task Set_wrong_make_return_warning_response() {
			var aliceRequest = RequestBuilder.Create()
				.WithActions(
					prev: typeof(InitStrategy).FullName,
					next: typeof(MakeStrategy).FullName)
				.Build();

			var handler = MockHandler.Create();
			var response = await handler.FunctionHandler(aliceRequest);

			Assert.Contains(
				"Что бы оценить авто мне надо знать его марку", 
				response.Response.Text);
		}

	}
}