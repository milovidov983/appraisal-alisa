using AliceAppraisal.Core.Engine;
using AliceAppraisal.Models;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

namespace AliceAppraisal.Application.Infrastructure {
	public class ExternalService : IExternalService {
		public string Token { get; set; } = "Independet";

		public async Task<AppraisalRawResult> GetAppraisalResponse(AppraisalQuoteRequest appraisalRequest) {
			try {
				var handler = new HttpClientHandler {
					AutomaticDecompression = ~DecompressionMethods.All
				};

				using var httpClient = new HttpClient(handler);
				using var request = new HttpRequestMessage(new HttpMethod("POST"), "https://automama.ru/api/v2/appraisal");

				request.Headers.TryAddWithoutValidation("authority", "automama.ru");
				request.Headers.TryAddWithoutValidation("pragma", "no-cache");
				request.Headers.TryAddWithoutValidation("cache-control", "no-cache");
				request.Headers.TryAddWithoutValidation("accept", "application/json, text/plain, */*");
				request.Headers.TryAddWithoutValidation("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/87.0.4280.88 Safari/537.36");
				request.Headers.TryAddWithoutValidation("origin", "https://automama.ru");
				request.Headers.TryAddWithoutValidation("sec-fetch-site", "same-origin");
				request.Headers.TryAddWithoutValidation("sec-fetch-mode", "cors");
				request.Headers.TryAddWithoutValidation("sec-fetch-dest", "empty");
				request.Headers.TryAddWithoutValidation("referer", "https://automama.ru/ocenka-avto");
				request.Headers.TryAddWithoutValidation("accept-language", "en-US,en;q=0.9,ru;q=0.8");
				request.Headers.TryAddWithoutValidation("cookie", "_ga=GA1.2.601973403.1561362907; _ym_uid=1561362907411175090; _cmg_csstGqVBC=1566714496; _comagic_idGqVBC=2171157626.3508152273.1566714493; top100_id=t1.4481343.743503018.1569155853866; last_visit=1569145053869::1569155853869; experimentation_subject_id=ImU2ZWM0NmJjLWM0N2MtNDlmNS05MDMzLTM2MWEwODYwZTA0NiI^%^3D--6875894c97b6f3c39ccc7718c8ab3bf6e482804e; _ym_d=1604308351; _fbp=fb.1.1607977412170.1321371793; fs_uid=rs.fullstory.com^#E4PCJ^#6297710352318464:4932373657337856/1618906409; _gid=GA1.2.1144757724.1609959721; _ym_isad=1");

				var jsonRequest = JsonSerializer.Serialize(appraisalRequest, new JsonSerializerOptions {
					PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
					IgnoreNullValues = true
				});

				request.Content = new StringContent(jsonRequest);
				request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

				var response = await httpClient.SendAsync(request);
				var jsonResult = await response.Content.ReadAsStringAsync();

				return JsonSerializer.Deserialize<AppraisalRawResult>(jsonResult);
			} catch(Exception e) {
				throw new ExternalServiceException(e.Message,
						"Извините, но ваш запрос не может быть обработан, " +
						"так как внешние сервисы от которых я завишу в данный момент недоступны, " +
						"попытайтесь повторить вашу команду позднее.");
			}
		}

		public async Task<TextAndValue[]> GetGenerationsFor(int modelId, int year) {
			try {
				var handler = new HttpClientHandler {
					AutomaticDecompression = ~DecompressionMethods.All
				};

				using var httpClient = new HttpClient(handler);
				using var request = new HttpRequestMessage(new HttpMethod("GET"), $"https://automama.ru/api/v2/taxonomy/generations?modelId={modelId}&manufactureYear={year}");
				request.Headers.TryAddWithoutValidation("authority", "automama.ru");
				request.Headers.TryAddWithoutValidation("pragma", "no-cache");
				request.Headers.TryAddWithoutValidation("cache-control", "no-cache");
				request.Headers.TryAddWithoutValidation("accept", "application/json, text/plain, */*");
				request.Headers.TryAddWithoutValidation("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/87.0.4280.88 Safari/537.36");
				request.Headers.TryAddWithoutValidation("sec-fetch-site", "same-origin");
				request.Headers.TryAddWithoutValidation("sec-fetch-mode", "cors");
				request.Headers.TryAddWithoutValidation("sec-fetch-dest", "empty");
				request.Headers.TryAddWithoutValidation("referer", "https://automama.ru/ocenka-avto");
				request.Headers.TryAddWithoutValidation("accept-language", "en-US,en;q=0.9,ru;q=0.8");
				request.Headers.TryAddWithoutValidation("cookie", "_ga=GA1.2.601973403.1561362907; _ym_uid=1561362907411175090; _cmg_csstGqVBC=1566714496; _comagic_idGqVBC=2171157626.3508152273.1566714493; top100_id=t1.4481343.743503018.1569155853866; last_visit=1569145053869::1569155853869; experimentation_subject_id=ImU2ZWM0NmJjLWM0N2MtNDlmNS05MDMzLTM2MWEwODYwZTA0NiI%3D--6875894c97b6f3c39ccc7718c8ab3bf6e482804e; _ym_d=1604308351; _fbp=fb.1.1607977412170.1321371793; fs_uid=rs.fullstory.com#E4PCJ#6297710352318464:4932373657337856/1618906409; _gid=GA1.2.1144757724.1609959721; _ym_isad=1");

				var response = await httpClient.SendAsync(request);
				var jsonResult = await response.Content.ReadAsStringAsync();

				return JsonSerializer.Deserialize<TextAndValue[]>(jsonResult);
			} catch(Exception e) {
				throw new ExternalServiceException(e.Message,
					"Извините, но ваш запрос не может быть обработан, " +
					"так как внешние сервисы от которых я завишу в данный момент недоступны, " +
					"попытайтесь повторить вашу команду позднее.");
			}
		}
	}
}
