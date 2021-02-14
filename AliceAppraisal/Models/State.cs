using AliceAppraisal.Core.Engine.Strategy;
using System;
using System.Collections.Generic;

namespace AliceAppraisal.Models {
	public class State {
		public AppraisalQuoteRequest Request { get; set; } = new AppraisalQuoteRequest();
		/// <summary>
		/// Предыдущее состояние
		/// </summary>
		public string PrevAction { get; set; }
		/// <summary>
		/// Следующее состояние
		/// </summary>
		public string NextAction { get; set; }
		/// <summary>
		/// Храним написание и произношение поколения для текущей марки модели года
		/// </summary>
		public Dictionary<string, IdAndName> GenerationChoise { get; set; } = new Dictionary<string, IdAndName>();
		/// <summary>
		/// Коды ответа от приложения
		/// </summary>
		public StatusCodes StatusCode { get; set; } = StatusCodes.OK;

		public void SetStatusCode(Exception exception) {
			switch (exception) {
				case NotFoundExcteption _: {
					StatusCode = StatusCodes.NotFound;
					break;
				}
				case InvalidRequestException _: {
					StatusCode = StatusCodes.InvalidRequest;
					break;
				}
				case ExternalServiceException _: {
					StatusCode = StatusCodes.ExternalError;
					break;
				}
				case Exception _: {
					StatusCode = StatusCodes.InternalError;
					break;
				}
			}
		}

		

		public IdAndName GetGenerationIdBySelected(string value) {
			GenerationChoise.TryGetValue(value, out var result);
			return result;
		}

		public void SaveCurrentAndNextStep(BaseStrategy strategy, string next) {
			PrevAction = strategy.GetType().FullName;
			NextAction = next;
		}
		public void SaveCurrentAndNextStep(string prev, string next) {
			if (prev.IsNullOrEmpty()) {
				throw new ArgumentNullException(
					$"Ошибка при попытке сохранить предыдущий шаг, аргумент {nameof(prev)} is null or empty");
			}
			if (next.IsNullOrEmpty()) {
				throw new ArgumentNullException(
					$"Ошибка при попытке сохранить следующий шаг, аргумент {nameof(next)} is null or empty");
			}

			PrevAction = prev;
			NextAction = next;
		}

		public void Clear() {
			GenerationChoise = new Dictionary<string, IdAndName>();
			Request = new AppraisalQuoteRequest();
			PrevAction = "";
		}

		#region UpdateMethods
		public bool UpdateEquipmentSet(string equipment) {
			
			if (Request.EquipmentType != equipment) {
				Request.EquipmentType = equipment;
				return true;
			}
			return false;
		}

		public bool UpdateModelId(int newModelId, string entity) {
			
			if (Request.ModelId != newModelId) {
				Request.ModelId = newModelId;

				Request.ModelEntity = entity;
				Request.ResetGenerationId();

				return true;
			}
			return false;
		}

		public bool UpdateRegion(int regionId, string entity) {
			
			if (Request.RegionId != regionId) {
				Request.RegionId = regionId;

				Request.CityName = entity;

				return true;
			}
			return false;
		}

		public bool UpdateRun(int run) {
			
			if (Request.Run != run) {
				Request.Run = run;
				return true;
			}
			return false;
		}

		public bool UpdateHorsePower(int horsePower) {
			
			if (Request.HorsePower != horsePower) {
				Request.HorsePower = horsePower;
				return true;
			}
			return false;
		}

		public bool UpdateDriveType(string drive) {
			
			if (Request.Drive != drive) {
				Request.Drive = drive;
				return true;
			}
			return false;
		}

		public bool UpdateEngineType(string engine) {
			
			if (Request.EngineType != engine) {
				Request.EngineType = engine;
				return true;
			}
			return false;
		}


		public bool UpdateGearbox(string gearbox) {
			
			if (Request.Gearbox != gearbox) {
				Request.Gearbox = gearbox;
				return true;
			}
			return false;
		}

		public bool UpdateGenerationId(int generationId, string entity) {
			
			if (Request.GenerationId != generationId) {
				Request.GenerationId = generationId;

				Request.GenerationValue = entity;

				return true;
			}
			return false;
		}

		public bool UpdateMake(int? makeId, string entity) {
			
			if (Request.MakeId != makeId) {
				Request.MakeId = makeId;
				Request.MakeEntity = entity;


				Request.ResetModelId();
				Request.ResetGenerationId();

				return true;
			}
			return false;
		}

		public bool UpdateBodyType(string body) {
			
			if (Request.BodyType != body) {
				Request.BodyType = body;
				return true;
			}
			return false;
		}

		public bool UpdateManufactureYear(int manufactureYear) {
			
			if (Request.ManufactureYear != manufactureYear) {
				Request.ManufactureYear = manufactureYear;

				Request.ResetGenerationId();

				return true;
			}
			return false;
		}


		#endregion
	}
}