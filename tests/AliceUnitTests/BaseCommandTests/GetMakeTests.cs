using AliceAppraisal.Core.Engine.Strategy;
using System.Threading.Tasks;
using Xunit;

namespace AliceUnitTests.BaseCommandTests {
	public class GetMakeTests {
		[Fact]
		public async Task Set_correct_make_id_is_saved() {
			var aliceRequest = RequestBuilder.Create()
				.WithActions(
					prev: typeof(InitStrategy).FullName,
					next: typeof(GetMakeStrategy).FullName)
				.WithIntentMake()
				.Build();

			var handler = new MockHandler();
			var response = await handler.FunctionHandler(aliceRequest);

			Assert.Equal(135, response.State.Request.MakeId);
		}

		[Fact]
		public async Task Set_correct_make_response_text_is_correct() {
			var aliceRequest = RequestBuilder.Create()
				.WithActions(
					prev: typeof(InitStrategy).FullName,
					next: typeof(GetMakeStrategy).FullName)
				.WithIntentMake()
				.Build();

			var handler = new MockHandler();
			var response = await handler.FunctionHandler(aliceRequest);

			Assert.Contains("пожалуйста модель вашего автомобиля", response.Response.Text);
		}

		[Fact]
		public async Task Set_correct_make_next_action_is_correct() {
			var aliceRequest = RequestBuilder.Create()
				.WithActions(
					prev: typeof(GetMakeStrategy).FullName,
					next: typeof(GetModelStrategy).FullName)
				.WithIntentMake()
				.Build();

			var handler = new MockHandler();
			var response = await handler.FunctionHandler(aliceRequest);

			Assert.Equal(typeof(GetModelStrategy).FullName, response.State.NextAction);
		}

		[Fact]
		public async Task Set_wrong_make_return_warning_response() {
			var aliceRequest = RequestBuilder.Create()
				.WithActions(
					prev: typeof(InitStrategy).FullName,
					next: typeof(GetMakeStrategy).FullName)
				.Build();

			var handler = new MockHandler();
			var response = await handler.FunctionHandler(aliceRequest);

			Assert.Contains(
				"Что бы оценить авто мне надо знать его марку, " +
				"пожалуйста попробуйте повторить запрос или попросите у меня подсказку.", 
				response.Response.Text);
		}

	}
}