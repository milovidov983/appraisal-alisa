using AliceAppraisal.Models;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace AliceUnitTests {
	public class ManualTest {
		[Fact]
		public async Task ManualTestStart() {
			AliceRequest aliceRequest = JsonSerializer.Deserialize<AliceRequest>(
				File.ReadAllText(@"D:\Source\PedProject\appraisal-alisa\tests\AliceUnitTests\BaseCommandTests\JsonRequests\Krasnodar.json"));

			var handler = new MockHandler();
			var response = await handler.FunctionHandler(aliceRequest);

			Assert.Contains($"Выберите нужный вариант поколения авто", response.Response.Text);
		}
	}
}
