﻿using AliceAppraisal.Application.Configuration;
using AliceAppraisal.Core.Engine;
using AliceAppraisal.Models;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

namespace AliceAppraisal.Application.Infrastructure {
	public class DataProviderService : IAppraisalProvider {

		public async Task<AppraisalRawResult> GetAppraisalResponse(AppraisalQuoteRequest appraisalRequest) {
			string domain = Settings.Instance.Domain;
			
			try {
				var handler = new HttpClientHandler {
					AutomaticDecompression = ~DecompressionMethods.All
				};

				using var httpClient = new HttpClient(handler);
				using var request = new HttpRequestMessage(new HttpMethod("POST"), $"https://{domain}/api/v2/appraisal");

				EnrichHeaders(request, domain);

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
				ThrowExternalServiceException(e);
			}
			return default;
		}



		public async Task<TextAndValue[]> GetGenerationsFor(int modelId, int manufactureYear) {
			string domain = Settings.Instance.Domain;
			try {
				var handler = new HttpClientHandler {
					AutomaticDecompression = ~DecompressionMethods.All
				};

				using var httpClient = new HttpClient(handler);
				using var request = new HttpRequestMessage(new HttpMethod("GET"), $"https://automama.ru/api/v2/taxonomy/generations?modelId={modelId}&manufactureYear={manufactureYear}");
				EnrichHeaders(request, domain);

				var response = await httpClient.SendAsync(request);
				var jsonResult = await response.Content.ReadAsStringAsync();

				return JsonSerializer.Deserialize<TextAndValue[]>(jsonResult);
			} catch (Exception e) {
				ThrowExternalServiceException(e);
			}
			return default;
		}

		private static void EnrichHeaders(HttpRequestMessage request, string domain) {
			return;
			request.Headers.TryAddWithoutValidation("authority", domain);
			request.Headers.TryAddWithoutValidation("pragma", "no-cache");
			request.Headers.TryAddWithoutValidation("cache-control", "no-cache");
			request.Headers.TryAddWithoutValidation("accept", "application/json, text/plain, */*");
			request.Headers.TryAddWithoutValidation("user-agent", "AliceBot Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/87.0.4280.88 Safari/537.36");
			request.Headers.TryAddWithoutValidation("sec-fetch-site", "same-origin");
			request.Headers.TryAddWithoutValidation("sec-fetch-mode", "cors");
			request.Headers.TryAddWithoutValidation("sec-fetch-dest", "empty");
			request.Headers.TryAddWithoutValidation("referer", $"https://{domain}/ocenka-avto");
			request.Headers.TryAddWithoutValidation("accept-language", "en-US,en;q=0.9,ru;q=0.8");
		}

		private static void ThrowExternalServiceException(Exception e) {
			throw new ExternalServiceException(e.Message,
									"Извините, но ваш запрос не может быть обработан, " +
									"так как внешние сервисы от которых я завишу в данный момент недоступны, " +
									"попытайтесь повторить вашу команду позднее.");
		}
	}
}