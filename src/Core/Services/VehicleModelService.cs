using AliceAppraisal.Application.Configuration;
using AliceAppraisal.Core.Engine;
using AliceAppraisal.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace AliceAppraisal.Core {
	public class VehicleModelService : IVehicleModelService {

		public async Task<(int modelId, string name)> GetModelData(string value, int makeId, string makeName, string command) {
			var (similarModel, modelsIdsForCurrentMake) = await GetModelMap(makeId);

			/// Так как существуют модели с схожими названиями например Мазда 5 Бмв 5
			/// необходимо убедится что указанная пользователем модель является такого типа названием
			var isSimilarSoundModelName = similarModel.SimilarModelNames.TryGetValue(value, out var similarModelNameIds);

			int newModelId;
			if (isSimilarSoundModelName) {
				newModelId = similarModelNameIds.FirstOrDefault(x => modelsIdsForCurrentMake.Contains(x));

				if (newModelId == default) {
					throw new ArgumentException($"Не удалось найти модели из похожих {value}");
				}
			} else {
				newModelId = value.ExtractIdOrNull()
					?? throw new ArgumentException($"Не удалось извлечь ID модели из сущности {value}");

				var isFitModel = modelsIdsForCurrentMake.Contains(newModelId);
				if (!isFitModel) {
					throw new ArgumentException($"Выбранная модель {value.ExtractName()} не принадлежит к марке {makeName}");
				}
			}
			var name = isSimilarSoundModelName ? command : value;
			return (newModelId, name);
		}


		/// <summary>
		/// TODO тащить с собой названия моделей что бы было что подставлять в ModelEntity
		/// </summary>
		private async Task<(SimilarNames, HashSet<int> MakeModelsMap)> GetModelMap(int makeId) {
			using var client = new WebClient();
			var similarStream = client.OpenRead(Settings.SimilarNamesFullUrl);
			var similarReader = new StreamReader(similarStream);
			var similarContentTask = similarReader.ReadToEndAsync();

			var makeModelMapFileName = $"{makeId}.json";
			var makeModelMapStream = client.OpenRead(
				$"{Settings.MakeModelMapPartUrl}{makeModelMapFileName}"
				);
			var makeModelMapReader = new StreamReader(makeModelMapStream);
			var makeModelMapContentTask = makeModelMapReader.ReadToEndAsync();

			await Task.WhenAll(similarContentTask, makeModelMapContentTask);


			var similarContent = similarContentTask.Result;
			var makeModelMapContent = makeModelMapContentTask.Result;

			var similarNames = JsonConvert.DeserializeObject<SimilarNames>(similarContent);
			var makeModelMapNames = JsonConvert.DeserializeObject<MakeModelsMap>(makeModelMapContent)
										.ModelIds
										.ToHashSet();


			return (similarNames, makeModelMapNames);
		}
	}
}
