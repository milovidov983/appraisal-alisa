using AliceAppraisal.Engine.Strategy;
using System;
using System.Collections.Generic;
using System.Text;

namespace AliceAppraisal.Static {
	public static class Transitions {

		public static string GetNextStep(BaseStrategy strategy) {
			var nextTransition = strategy switch {
				
				GetMakeStrategy _ => typeof(GetModelStrategy).FullName,
				GetModelStrategy _ => typeof(GetManufactureYearStrategy).FullName,
				GetManufactureYearStrategy _ => typeof(GetGenerationStrategy).FullName,
				GetGenerationStrategy _ => typeof(GetBodyTypeStrategy).FullName,
				GetBodyTypeStrategy _ => typeof(GearboxStrategy).FullName,
				GearboxStrategy _ => typeof(GetEngineTypeStrategy).FullName,
				GetEngineTypeStrategy _ => typeof(GetDriveTypeStrategy).FullName,
				GetDriveTypeStrategy _ => typeof(GetHorsePowerStrategy).FullName,
				GetHorsePowerStrategy _ => typeof(GetRunStrategy).FullName,
				GetRunStrategy _ => typeof(GetEquipmentSetStrategy).FullName,
				GetEquipmentSetStrategy _ => typeof(GetCityStrategy).FullName,
				GetCityStrategy _ => typeof(StartAppraisalStrategy).FullName,
				//
				ChangeParamStrategy _ => typeof(StartAppraisalStrategy).FullName,
				SelectYearStrategy _ => typeof(GetManufactureYearStrategy).FullName,
				InitStrategy _ => typeof(GetMakeStrategy).FullName,
				ConfirmGenerationStrategy _ => typeof(GetBodyTypeStrategy).FullName,
				StartAppraisalStrategy _ => typeof(InitStrategy).FullName,
				_ => throw new Exception($"Для типа {strategy?.GetType()?.FullName} не описан переход")
			};
			return nextTransition;
		}
	}
}