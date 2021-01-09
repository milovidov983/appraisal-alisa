using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AliceAppraisal.Static {
	public static class Buttons {
		public static string[] StartNew = new[] { "Начать с начала" };
		public static string[] Help = new[] { "Помощь" };
		public static string[] Exit = new[] { "Выйти" }; 
		public static string[] Replay = new[] { "Повторить последний шаг" }; 
		public static string[] Back = new[] { "Вернуться на предыдущий шаг" };
		public static string[] ExecuteAppraisal = new[] { "Выполнить оценку" };
		public static string[] SelectYear = new[] { "Изменить год выпуска" };



		public static string[] Base = Replay.Union(Help).Union(Exit).ToArray();
		public static string[] BaseExtended = StartNew.Union(Help).Union(Exit).ToArray();
	}
}