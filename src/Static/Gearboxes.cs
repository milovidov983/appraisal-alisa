using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace AliceAppraisal.Static {
	/// <summary>
	/// Типы КПП
	/// </summary>
	[Description("Типы КПП")]
	public enum Gearboxes {
		/// <summary>
		/// Механика
		/// </summary>
		[Description("Механика")]
		Manual = 1,
		/// <summary>
		/// Автомат
		/// </summary>
		[Description("Автомат")]
		Automatic,
		/// <summary>
		/// Вариатор
		/// </summary>
		[Description("Вариатор")]
		Variator,
		/// <summary>
		/// Робот
		/// </summary>
		[Description("Робот")]
		Robot
	}
}
