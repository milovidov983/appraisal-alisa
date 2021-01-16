using System.ComponentModel;

namespace AliceAppraisal.Static {
	/// <summary>
	/// Комплектация
	/// </summary>
	[Description("Комплектация")]
	public enum Equipments {
		/// <summary>
		/// Базовая
		/// </summary>
		[Description("Базовая")]
		Basic = 1,
		/// <summary>
		/// Стандартная
		/// </summary>
		[Description("Стандартная")]
		Standard,
		/// <summary>
		/// Максимальная
		/// </summary>
		[Description("Максимальная")]
		Max
	}
}