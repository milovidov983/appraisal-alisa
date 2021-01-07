using AliceAppraisal.Engine.Stratagy;
using System;
using System.Collections.Generic;

namespace AliceAppraisal.Models {
	public class State {
        public AppraisalQuoteRequest AppraisalRequest { get; set; } = new AppraisalQuoteRequest();
        public string PrevAction { get; set; }
		/// <summary>
		/// Храним написание и произношение поколения для текущей марки модели года
		/// </summary>
		public Dictionary<string, int> GenerationChose { get; set; } = new Dictionary<string, int>();

		public UserKnowledge UserKnowledge { get; set; } = new UserKnowledge();


		public int? GetGenerationIdBySelected(string value) {
			GenerationChose.TryGetValue(value, out var generationId);
			return generationId;
		}

		public void SetPrevAction(BaseStratagy stratagy) {
            PrevAction = stratagy.GetType().FullName;
		}

		public void Clear() {
			GenerationChose = new Dictionary<string, int>();
			AppraisalRequest = new AppraisalQuoteRequest();
            PrevAction = "";
        }


		public bool UpdateEquipmentSet(string equipment, BaseStratagy stratagy) {
			SetPrevAction(stratagy);
			if (AppraisalRequest.EquipmentType != equipment) {
				AppraisalRequest.EquipmentType = equipment;
				return true;
			}
			return false;
		}

		public bool UpdateModelId(int newModelId, BaseStratagy stratagy) {
			SetPrevAction(stratagy);
			if (AppraisalRequest.ModelId != newModelId) {
				AppraisalRequest.ModelId = newModelId;
				return true;
			}
			return false;
		}

		public bool UpdateRegion(int regionId, BaseStratagy stratagy) {
			SetPrevAction(stratagy);
			if (AppraisalRequest.RegionId != regionId) {
				AppraisalRequest.RegionId = regionId;
				return true;
			}
			return false;
		}

		public bool UpdateRun(int run, BaseStratagy stratagy) {
			SetPrevAction(stratagy);
			if (AppraisalRequest.Run != run) {
				AppraisalRequest.Run = run;
				return true;
			}
			return false;
		}


		public bool UpdateHorsePower(int horsePower, BaseStratagy stratagy) {
			SetPrevAction(stratagy);
			if (AppraisalRequest.HorsePower != horsePower) {
				AppraisalRequest.HorsePower = horsePower;
				return true;
			}
			return false;
		}

		public bool UpdateDriveType(string drive, BaseStratagy stratagy) {
			SetPrevAction(stratagy);
			if (AppraisalRequest.Drive != drive) {
				AppraisalRequest.Drive = drive;
				return true;
			}
			return false;
		}

		public bool UpdateEngineType(string engine, BaseStratagy stratagy) {
			SetPrevAction(stratagy);
			if (AppraisalRequest.EngineType != engine) {
				AppraisalRequest.EngineType = engine;
				return true;
			}
			return false;
		}

		public bool UpdateGearbox(string gearbox, BaseStratagy stratagy) {
			SetPrevAction(stratagy);
			if (AppraisalRequest.Gearbox != gearbox) {
				AppraisalRequest.Gearbox = gearbox;
				return true;
			}
			return false;
		}

		public bool UpdateGenerationId(int generationId, BaseStratagy stratagy) {
			SetPrevAction(stratagy);
			if (AppraisalRequest.GenerationId != generationId) {
				AppraisalRequest.GenerationId = generationId;
				return true;
			}
			return false;
		}

		public bool UpdateMakeId(int makeId, BaseStratagy stratagy) {
			SetPrevAction(stratagy);
			if (AppraisalRequest.MakeId != makeId) {
				AppraisalRequest.MakeId = makeId;
				return true;
			}
			return false;
		}

		public bool UpdateBodyType(string body, BaseStratagy stratagy) {
			SetPrevAction(stratagy);
			if (AppraisalRequest.BodyType != body) {
				AppraisalRequest.BodyType = body;
				return true;
			}
			return false;
		}


		public bool UpdateManufactureYear(int manufactureYear,BaseStratagy stratagy) {
			SetPrevAction(stratagy);
			if (AppraisalRequest.ManufactureYear != manufactureYear) {
				AppraisalRequest.ManufactureYear = manufactureYear;
				return true;
			}
			return false;
		}

	
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