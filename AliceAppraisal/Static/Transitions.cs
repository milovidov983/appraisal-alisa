using AliceAppraisal.Engine.Strategy;

namespace AliceAppraisal.Static {
	public static class Transitions {
		public static string GetNextStep(BaseStrategy strategy) => strategy switch {
			// Основной путь алгоритма
			// может изменятся внутри стратегий путем переопределения 
			// метода BaseStrategy.SetNextStep
			InitStrategy _ => typeof(GetMakeStrategy).FullName,
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
			StartAppraisalStrategy _ => typeof(InitStrategy).FullName,
			// 
			ConfirmGenerationStrategy _ => typeof(GetBodyTypeStrategy).FullName,
			ChangeParamStrategy _ => typeof(StartAppraisalStrategy).FullName,
			SelectYearStrategy _ => typeof(GetManufactureYearStrategy).FullName,
			_ => strategy?.GetType().FullName
		};
	}
}