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
			InitStrategy _ => typeof(MakeStrategy).FullName,
			MakeStrategy _ => typeof(ModelStrategy).FullName,
			ModelStrategy _ => typeof(ManufactureYearStrategy).FullName,
			MakeModelYearStratagy _ => typeof(ManufactureYearStrategy).FullName,
			ManufactureYearStrategy _ => typeof(GenerationStrategy).FullName,
			GenerationStrategy _ => typeof(BodyTypeStrategy).FullName,
			BodyTypeStrategy _ => typeof(GearboxStrategy).FullName,
			GearboxStrategy _ => typeof(EngineTypeStrategy).FullName,
			EngineTypeStrategy _ => typeof(DriveTypeStrategy).FullName,
			DriveTypeStrategy _ => typeof(HorsePowerStrategy).FullName,
			HorsePowerStrategy _ => typeof(RunStrategy).FullName,
			RunStrategy _ => typeof(EquipmentSetStrategy).FullName,
			EquipmentSetStrategy _ => typeof(CityStrategy).FullName,
			CityStrategy _ => typeof(StartAppraisalStrategy).FullName,
			StartAppraisalStrategy _ => typeof(InitStrategy).FullName,
			// Extended
			SpecifyAnotherCityStratagy _ => typeof(CityStrategy).FullName,
			ConfirmGenerationStrategy _ => typeof(BodyTypeStrategy).FullName,
			ChangeRunStrategy _ => typeof(StartAppraisalStrategy).FullName,
			ChangeCityStrategy _ => typeof(StartAppraisalStrategy).FullName,
			SelectYearStrategy _ => typeof(ManufactureYearStrategy).FullName,
			_ => currentStep?.GetType().FullName
		};
	}
}