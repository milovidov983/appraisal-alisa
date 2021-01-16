using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace AliceAppraisal.Models {
	/// <summary>
	/// Статус обработки сообщения
	/// </summary>
	[Flags]
	public enum StatusCodes {
		/// <summary>
		/// Обработано
		/// </summary>
		[Description("Обработано")]
		OK = 0x00,
		/// <summary>
		/// Некорректный запрос
		/// </summary>
		[Description("Некорректный запрос")]
		InvalidRequest = 0x01,
		/// <summary>
		/// Не найдено
		/// </summary>
		[Description("Не найдено")]
		NotFound = 0x02,
		/// <summary>
		/// Ошибка стороннего сервиса
		/// </summary>
		[Description("Ошибка стороннего сервиса")]
		ExternalError = 0x08,
		/// <summary>
		/// Во время обработки запроса произошла внутрення ошибка
		/// </summary>
		[Description("Во время обработки запроса произошла внутренняя ошибка")]
		InternalError = 0x10000000
	}
}
