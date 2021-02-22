using System.ComponentModel;

namespace AliceAppraisal.Static {
	/// <summary>
	/// Типы приводов
	/// </summary>
	[Description("Приводы")]
	public enum Drives {
		/// <summary>
		/// Задний
		/// </summary>
		[Description("Задний")]
		Rear = 1,
		/// <summary>
		/// Передний
		/// </summary>
		[Description("Передний")]
		Front,
		/// <summary>
		/// Полный
		/// </summary>
		[Description("Полный")]
		Full
	}
}
