﻿using AliceAppraisal.Core.Engine;
using AliceAppraisal.Core.Engine.Strategy;
using AliceAppraisal.Models;
using AliceAppraisal.Static;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AliceAppraisal.Core.Models {
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
		/// <summary>
		/// Счётчик непонимания
		/// key - шаг
		/// value - кол-во попаданий в ветку "Ой, мне не удалось понять вас."
		/// </summary>
		/// <remarks>
		/// Идея хранения количества неудачных попыток понять собеседника состоит 
		/// в том что бы не вести себя как робот который твердит одно и тоже, но
		/// хотелось бы на основе этих данных делать корректировку поведения бота
		/// </remarks>
		public Dictionary<string, int> MisunderstandingCounter { get; set; } = new Dictionary<string, int>();
		/// <summary>
		/// Характеристики выбронного поколения
		/// </summary>
		public AvailableCharacteristics Characteristics { get; set; } = new AvailableCharacteristics();


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
				case InternalErrorException _:
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


		public void AddMisunderstanding() {
			if (MisunderstandingCounter.ContainsKey(NextAction)) {
				MisunderstandingCounter[NextAction]++;
			} else {
				MisunderstandingCounter.Add(NextAction, 1);
			}
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
		public void GeneralReset() {
			Request = new AppraisalQuoteRequest();
			GenerationChoise.Clear();
			Characteristics = new AvailableCharacteristics();
		}
		public void FullReset() {
			Request = new AppraisalQuoteRequest();
			Characteristics = new AvailableCharacteristics();
			GenerationChoise.Clear();
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

		public bool UpdateCharacteristics(AvailableCharacteristics characteristics) {
			Characteristics = characteristics;
			return true;
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

		/// <summary>
		/// Параметры авто в единственном экземпляре для выбранного поколения,
		/// заполняем сразу и не проходим на шаг их заполнения
		/// </summary>
		public void TryFillSingleCharacteristics(AvailableCharacteristics characteristics) {
			ResetCharacteristics();

			if (characteristics.BodyTypes.Length == 1) {
				var singleBodyType = VehicleComponents.BodyTypes.FirstOrDefault(x => x.Value == characteristics.BodyTypes.First());
				UpdateBodyType(singleBodyType.Key);
			}
			if (characteristics.DriveTypes.Length == 1) {
				var singleDriveType = VehicleComponents.Drives.FirstOrDefault(x => x.Value == characteristics.DriveTypes.First());
				UpdateDriveType(singleDriveType.Key);
			}
			if (characteristics.EngineTypes.Length == 1) {
				var engineType = VehicleComponents.EngineTypes.FirstOrDefault(x => x.Value == characteristics.EngineTypes.First());
				UpdateEngineType(engineType.Key);
			}
			if (characteristics.GearboxTypes.Length == 1) {
				var gearboxType = VehicleComponents.Gearboxes.FirstOrDefault(x => x.Value == characteristics.GearboxTypes.First());
				UpdateGearbox(gearboxType.Key);
			}
		}

		private void ResetCharacteristics() {
			UpdateBodyType(null);
			UpdateDriveType(null);
			UpdateEngineType(null);
			UpdateGearbox(null);
		}
	}
}