using AliceAppraisal.Configuration;
using AliceAppraisal.Controllers;
using AliceAppraisal.Engine.Strategy;
using AliceAppraisal.Models;
using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace AliceAppraisal {
	class Program {
		static async Task Main(string[] args) {
			var requets = File.ReadAllText("Tests/request.json");
			var ar = JsonSerializer.Deserialize<AliceRequest>(requets);

			var h = new Handler();

			var resp = await h.FunctionHandler(ar);

			Console.WriteLine("Hello World!");
		}
	}
}
