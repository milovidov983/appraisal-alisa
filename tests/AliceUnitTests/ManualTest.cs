using AliceAppraisal.Controllers;
using AliceAppraisal.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace AliceUnitTests {
	public class ManualTest {
		[Fact]
		public async Task ManualTestStart() {
			AliceRequest aliceRequest = JsonSerializer.Deserialize<AliceRequest>(
				File.ReadAllText(@"D:\Source\PedProject\appraisal-alisa\tests\AliceUnitTests\BaseCommandTests\JsonRequests\MYearError.json"));

			var handler = new Handler();
			var response = await handler.FunctionHandler(aliceRequest);

			Assert.Contains($"Выберите нужный вариант поколения авто", response.Response.Text);
		}
	}
}
