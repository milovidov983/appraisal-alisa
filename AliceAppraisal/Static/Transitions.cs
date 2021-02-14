using AliceAppraisal.Core.Engine;
using AliceAppraisal.Core.Engine.Strategy;

namespace AliceAppraisal.Static {
	/// <summary>
	/// Описание переходов между шагами
	/// </summary>
	public static class Transitions {
		/// <summary>
		/// Основной путь алгоритма
		/// может изменятся внутри стратегий путем переопределения 
		/// метода BaseStrategy.SetNextStep
		/// </summary>
		/// <param name="currentStep">
		/// Текущий шаг
		/// </param>
		/// <returns>
		/// Следующий шаг для текущего
		/// </returns>
		public static string GetDeafultNextStep(BaseStrategy currentStep) => currentStep switch {
			// Main
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
			// Extended
			SpecifyAnotherCityStratagy _ => typeof(GetCityStrategy).FullName,
			ConfirmGenerationStrategy _ => typeof(GetBodyTypeStrategy).FullName,
			ChangeRunStrategy _ => typeof(StartAppraisalStrategy).FullName,
			ChangeCityStrategy _ => typeof(StartAppraisalStrategy).FullName,
			SelectYearStrategy _ => typeof(GetManufactureYearStrategy).FullName,
			_ => currentStep?.GetType().FullName
		};
	}
}