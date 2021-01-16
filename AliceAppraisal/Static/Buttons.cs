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


		public static string[] YesNoExtended = YesNo.Union(Help).Union(Exit).ToArray();
		public static string[] Base = StartNew.Union(Help).Union(Exit).ToArray();
		public static string[] BaseExtended = StartNew.Union(Help).Union(Exit).ToArray();
	}
}