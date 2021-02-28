﻿using AliceAppraisal.Application.Configuration;
using AliceAppraisal.Core.Engine;
using AliceAppraisal.Models;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AliceAppraisal.Application.Infrastructure {
	public class DataProviderService : IAppraisalProvider {
		private readonly string domain = Settings.Instance.Domain;



		public async Task<AppraisalRawResult> GetAppraisalResponse(AppraisalQuoteRequest appraisalRequest) {
			try {
				var handler = new HttpClientHandler {
					AutomaticDecompression = ~DecompressionMethods.All
				};

				using var httpClient = new HttpClient(handler);
				using var request = new HttpRequestMessage(new HttpMethod("POST"), $"https://{domain}/api/v2/appraisal");


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
		public async Task<string[]> GetPupularModels(int makeId) {
			var url = $"https://{domain}/api/v2/taxonomy/PopularModels?makeId={makeId}";
			var result = await Get<PopularList>(url, false);

			return result?.Popular?.Select(x => x.Text)?.ToArray() ?? Array.Empty<string>();
		}

		public async Task<string[]> GetPupularMakes() {
			var url = $"https://{domain}/api/v2/taxonomy/PopularMakes";
			var result = await Get<PopularList>(url, false);

			return result?.Popular?.Select(x => x.Text)?.ToArray() ?? Array.Empty<string>();
		}

		public async Task<TextAndValue[]> GetGenerationsFor(int modelId, int manufactureYear) {
			var url = $"https://{domain}/api/v2/taxonomy/generations?modelId={modelId}&manufactureYear={manufactureYear}";
			var result = await Get<TextAndValue[]>(url);

			return result ?? Array.Empty<TextAndValue>(); 
		}

		public async Task<Result> Get<Result>(string url, bool throwException = true) {
			try {
				var handler = new HttpClientHandler {
					AutomaticDecompression = ~DecompressionMethods.All
				};

				using var httpClient = new HttpClient(handler);
				using var request = new HttpRequestMessage(new HttpMethod("GET"), url);

				var response = await httpClient.SendAsync(request);
				var jsonResult = await response.Content.ReadAsStringAsync();

				return JsonSerializer.Deserialize<Result>(jsonResult);
			} catch (Exception e) {
				if (throwException) {
					ThrowExternalServiceException(e);
				}
			}
			return default;
		}



		private static void ThrowExternalServiceException(Exception e) {
			throw new ExternalServiceException(e.Message,
									"Извините, но ваш запрос не может быть обработан, " +
									"так как внешние сервисы от которых я завишу в данный момент недоступны, " +
									"попытайтесь повторить вашу команду позднее.");
		}



		public class TextWithValue<T> where T : struct {
			[JsonPropertyName("value")]
			public T Value { get; set; }
			[JsonPropertyName("text")]
			public string Text { get; set; }
		}
		public class PopularList {
			[JsonPropertyName("popular")]
			public TextWithValue<int>[] Popular { get; set; }
			[JsonPropertyName("all")]
			public TextWithValue<int>[] All { get; set; }
		}
	}
}
