using System.ComponentModel;

namespace AliceAppraisal.Static {
	/// <summary>
	/// Типы двигателей
	/// </summary>
	[Description("Типы двигателей")]
	public enum EngineTypes {
		/// <summary>
		/// Бензиновый
		/// </summary>
		[Description("Бензиновый")]
		Gasoline = 1,
		/// <summary>
		/// Дизельный
		/// </summary>
		[Description("Дизельный")]
		Diesel,
		/// <summary>
		/// Гибридный
		/// </summary>
		[Description("Гибридный")]
		Hybrid,
		/// <summary>
		/// Электрический
		/// </summary>
		[Description("Электрический")]
		Electric
	}
}
