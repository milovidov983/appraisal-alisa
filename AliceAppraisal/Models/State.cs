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
		public Dictionary<string, int> GenerationChose { get; set; } = new Dictionary<string, int>();

		

		public int? GetGenerationIdBySelected(string value) {
			GenerationChose.TryGetValue(value, out var generationId);
			return generationId;
		}

		public void SaveCurrentStep(BaseStrategy strategy) {
            PrevAction = strategy.GetType().FullName;
			NextAction = strategy.NextStep;
		}

		public void Clear() {
			GenerationChose = new Dictionary<string, int>();
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

		public bool UpdateModelId(int newModelId, BaseStrategy strategy) {
			SaveCurrentStep(strategy);
			if (Request.ModelId != newModelId) {
				Request.ModelId = newModelId;

				Request.ResetGenerationId();

				return true;
			}
			return false;
		}

		public bool UpdateRegion(int regionId, BaseStrategy strategy) {
			SaveCurrentStep(strategy);
			if (Request.RegionId != regionId) {
				Request.RegionId = regionId;
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

		public bool UpdateGenerationId(int generationId, BaseStrategy strategy) {
			SaveCurrentStep(strategy);
			if (Request.GenerationId != generationId) {
				Request.GenerationId = generationId;
				return true;
			}
			return false;
		}

		public bool UpdateMake(int? makeId, string token, BaseStrategy strategy) {
			SaveCurrentStep(strategy);
			if (Request.MakeId != makeId) {
				Request.MakeId = makeId;
				Request.MakeToken = token;


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
	public enum Step {
        None,
        Make,
        Model,
        ManufactureYear,
        Gen,
        Body,
        Gearbox,
        EngineType,
        Drive,
        HorsPower,
        Run,
        EquipmentSet,
        Reqion
    }
}