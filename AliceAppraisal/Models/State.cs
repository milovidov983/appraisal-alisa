using AliceAppraisal.Engine.Strategy;
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

		

		public IdAndName GetGenerationIdBySelected(string value) {
			GenerationChoise.TryGetValue(value, out var result);
			return result;
		}


		public void SaveCurrentStep(BaseStrategy strategy) {
            PrevAction = strategy.GetType().FullName;
			NextAction = strategy.NextStep;
		}

		public void Clear() {
			GenerationChoise = new Dictionary<string, IdAndName>();
			Request = new AppraisalQuoteRequest();
            PrevAction = "";
        }

		#region UpdateMethods
		public bool UpdateEquipmentSet(string equipment, BaseStrategy strategy) {
			SaveCurrentStep(strategy);
			if (Request.EquipmentType != equipment) {
				Request.EquipmentType = equipment;
				return true;
			}
			return false;
		}

		public bool UpdateModelId(int newModelId, string entity, BaseStrategy strategy) {
			SaveCurrentStep(strategy);
			if (Request.ModelId != newModelId) {
				Request.ModelId = newModelId;

				Request.ModelEntity = entity;
				Request.ResetGenerationId();

				return true;
			}
			return false;
		}

		public bool UpdateRegion(int regionId, string entity, BaseStrategy strategy) {
			SaveCurrentStep(strategy);
			if (Request.RegionId != regionId) {
				Request.RegionId = regionId;

				Request.CityName = entity;

				return true;
			}
			return false;
		}

		public bool UpdateRun(int run, BaseStrategy strategy) {
			SaveCurrentStep(strategy);
			if (Request.Run != run) {
				Request.Run = run;
				return true;
			}
			return false;
		}

		public bool UpdateHorsePower(int horsePower, BaseStrategy strategy) {
			SaveCurrentStep(strategy);
			if (Request.HorsePower != horsePower) {
				Request.HorsePower = horsePower;
				return true;
			}
			return false;
		}

		public bool UpdateDriveType(string drive, BaseStrategy strategy) {
			SaveCurrentStep(strategy);
			if (Request.Drive != drive) {
				Request.Drive = drive;
				return true;
			}
			return false;
		}

		public bool UpdateEngineType(string engine, BaseStrategy strategy) {
			SaveCurrentStep(strategy);
			if (Request.EngineType != engine) {
				Request.EngineType = engine;
				return true;
			}
			return false;
		}


		public bool UpdateGearbox(string gearbox, BaseStrategy strategy) {
			SaveCurrentStep(strategy);
			if (Request.Gearbox != gearbox) {
				Request.Gearbox = gearbox;
				return true;
			}
			return false;
		}

		public bool UpdateGenerationId(int generationId, string entity, BaseStrategy strategy) {
			SaveCurrentStep(strategy);
			if (Request.GenerationId != generationId) {
				Request.GenerationId = generationId;

				Request.GenerationValue = entity;

				return true;
			}
			return false;
		}

		public bool UpdateMake(int? makeId, string entity, BaseStrategy strategy) {
			SaveCurrentStep(strategy);
			if (Request.MakeId != makeId) {
				Request.MakeId = makeId;
				Request.MakeEntity = entity;


				Request.ResetModelId();
				Request.ResetGenerationId();

				return true;
			}
			return false;
		}

		public bool UpdateBodyType(string body, BaseStrategy strategy) {
			SaveCurrentStep(strategy);
			if (Request.BodyType != body) {
				Request.BodyType = body;
				return true;
			}
			return false;
		}

		public bool UpdateManufactureYear(int manufactureYear, BaseStrategy strategy) {
			SaveCurrentStep(strategy);
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