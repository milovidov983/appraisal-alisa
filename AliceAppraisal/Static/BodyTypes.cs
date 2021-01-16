using System.ComponentModel;
using System.Text;

namespace AliceAppraisal.Static {

	[Description("Типы корпусов")]
	public enum BodyTypes {
		/// <summary>
		/// Внедорожник
		/// </summary>
		[Description("Внедорожник")]
		Offroad = 1,
		/// <summary>
		/// Купе
		/// </summary>
		[Description("Купе")]
		Coupe = 2,
		/// <summary>
		/// Кабриолет
		/// </summary>
		[Description("Кабриолет")]
		Cabriolet = 3,
		/// <summary>
		/// Родстер
		/// </summary>
		[Description("Родстер")]
		Roadster = 4,
		/// <summary>
		/// Минивэн
		/// </summary>
		[Description("Минивэн")]
		Minivan = 5,
		/// <summary>
		/// Микроавтобус
		/// </summary>
		[Description("Микроавтобус")]
		Minibus = 6,
		/// <summary>
		/// Пикап
		/// </summary>
		[Description("Пикап")]
		Pickup = 7,
		/// <summary>
		/// Седан
		/// </summary>
		[Description("Седан")]
		Sedan = 8,
		/// <summary>
		/// Универсал
		/// </summary>
		[Description("Универсал")]
		EstateCar = 9,
		/// <summary>
		/// Фургон
		/// </summary>
		[Description("Фургон")]
		Caravan = 10,
		/// <summary>
		/// Грузо-пассажирский (комби)
		/// </summary>
		[Description("Грузо-пассажирский (комби)")]
		Combi = 11,
		/// <summary>
		/// Хэтчбек
		/// </summary>
		[Description("Хэтчбек")]
		Hatchback = 12,
		/// <summary>
		/// Шасси
		/// </summary>
		[Description("Шасси")]
		Chassis = 13,
		/// <summary>
		/// Компактвэн
		/// </summary>
		[Description("Компактвэн")]
		Compactvan = 14,
		/// <summary>
		/// Лифтбэк
		/// </summary>
		[Description("Лифтбэк")]
		Liftback = 15,
		/// <summary>
		/// Микровэн
		/// </summary>
		[Description("Микровэн")]
		Microvan = 18,
		/// <summary>
		/// Лимузин
		/// </summary>
		[Description("Лимузин")]
		Limousine = 19,
		/// <summary>
		/// Бортовой
		/// </summary>
		[Description("Бортовой")]
		Flatbed = 20,
		/// <summary>
		/// Цистерна
		/// </summary>
		[Description("Цистерна")]
		Tanker = 21,
		/// <summary>
		/// Вилочный погрузчик
		/// </summary>
		[Description("Вилочный погрузчик")]
		Forklift = 22,
		/// <summary>
		/// Тягач
		/// </summary>
		[Description("Тягач")]
		Tractor = 23,
		/// <summary>
		/// Рефрижератор
		/// </summary>
		[Description("Рефрижератор")]
		Refrigerator = 24
	}
}