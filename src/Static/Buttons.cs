using AliceAppraisal.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AliceAppraisal.Static {
	public static class Buttons {
		public static string[] Exit = new[] { "Выйти из навыка" }; 
		public static string[] StartNew = new[] { "Начать с начала" };
		public static string[] Help = new[] { "Помощь" };

		public static string[] Replay = new[] { "Повторить последний шаг" }; 
		public static string[] Back = new[] { "Вернуться на предыдущий шаг" };
		public static string[] ExecuteAppraisal = new[] { "Выполнить оценку" };
		public static string[] SelectYear = new[] { "Изменить год выпуска" };
		public static string[] YesNo = new[] { "Да", "Нет" };



		public static string[] BaseSet = StartNew.Union(Exit).Union(Exit).ToArray();

		public static string[] BodyTypesBtn = new[] {
			BodyTypes.Sedan.GetDescription(),
			BodyTypes.Hatchback.GetDescription(),
			BodyTypes.Offroad.GetDescription(),
			BodyTypes.EstateCar.GetDescription(),
			BodyTypes.Coupe.GetDescription(),
			BodyTypes.Liftback.GetDescription()
		};

		public static string[] GetCityButtons() {
			return RegionItem
				.FavoriteCityIndex
				.Select(x => RegionItem.All[x])
				.Select(x => x.CapitalName)
				.ToArray();
		}

		public static string[] GetMilage() {
			return Enumerable.Range(1, 10).Select(x => (20000 * x).ToString()).ToArray();
		}
	}
}