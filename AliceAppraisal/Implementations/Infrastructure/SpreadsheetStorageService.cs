using AliceAppraisal.Infrastructure;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AliceAppraisal.Store {
	public class SheetConfig {
		public string SpreadsheetId { get; set; }
		public string ClientId { get; set; }
		public string ClientSecret { get; set; }
		public string User { get; set; }
	}

	public class SpreadsheetStorageService : IStorageService {
		private readonly Database db;
		public SpreadsheetStorageService(SheetConfig config) {
			string spreadsheetId = config.SpreadsheetId; //https://docs.google.com/spreadsheets/d/1cHYd0kfb-f1ATHkmyD9g1TMdGi6AgCwhAfeEFPhcD8E/edit?usp=sharing


			UserCredential credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
			new ClientSecrets {
				ClientId = config.ClientId,//"699445886162-i0chpihi32i6a9jp9c6csh8k13m65lno.apps.googleusercontent.com",
				ClientSecret = config.ClientSecret// "ypzXA5afc46D-Lv6eeNb9SVk",
			},
			new[] { SheetsService.Scope.Spreadsheets },
			config.User,//"dbuser",
			CancellationToken.None).Result;


			var sheetsService = new SheetsService(new BaseClientService.Initializer() {
				HttpClientInitializer = credential,
				ApplicationName = "store",
			});
			this.db = new Database(spreadsheetId, sheetsService);
		}

		public Task Insert<T>(T data) {
			var json = JsonConvert.SerializeObject(data);
			return db.CreateItem(json);
		}
	}
	public class Database {

		private readonly SheetsService sheetsService;
		private readonly string spreadsheetId;

		private const string IdRange = "D1";

		public Database(string spreadsheetId, SheetsService sheetsService) {
			this.spreadsheetId = spreadsheetId;
			this.sheetsService = sheetsService;
		}

		public async Task CreateItem(string data) {
			var (id, changedAt) = await CreateServiceData();
			await Write(id, data, changedAt);
		}
		private string CreateWriteRange(int rowNumber) {
			return $"A{rowNumber}:C{rowNumber}";
		}

		private async Task Write(int id, string data, DateTime changedAt) {
			var range = CreateWriteRange(id);
			var payload = CreatePayload(id, data, changedAt);
			await WriteNewValue(range, payload);
		}

		private async Task<(int id, DateTime changedAt)> CreateServiceData() {
			int id = await GetNextId();
			var changedAt = DateTime.UtcNow;
			return (id, changedAt);
		}

		private async Task<int> GetNextId() {
			var request = sheetsService.Spreadsheets.Values.Get(spreadsheetId, IdRange);
			var response = await request.ExecuteAsync();

			int currentId = GetCurrentId(response);
			var nextId = 1 + currentId;
			var payload = CreatePayload(nextId);

			await WriteNewValue(IdRange, payload);

			return nextId;
		}

		private List<IList<object>> CreatePayload(params object[] items) {
			return new List<IList<object>>() {
				items.ToList()
			};
		}

		private static int GetCurrentId(ValueRange response) {
			IList<IList<object>> values = response.Values;
			var v = values?.FirstOrDefault();
			var idObj = v?.FirstOrDefault() ?? "0";

			int.TryParse((string)idObj, out var currentId);
			return currentId;
		}

		private async Task<int?> WriteNewValue(string range, List<IList<object>> vals) {
			ValueRange requestBody = new ValueRange {
				Values = vals
			};
			SpreadsheetsResource.ValuesResource.UpdateRequest request
				= sheetsService.Spreadsheets.Values.Update(requestBody, spreadsheetId, range);

			request.ValueInputOption
				= SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.USERENTERED;

			var response = await request.ExecuteAsync();

			return response.UpdatedCells;
		}
	}
}
