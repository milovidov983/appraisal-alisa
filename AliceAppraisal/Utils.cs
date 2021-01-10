using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AliceAppraisal {
	public static class ReflectiveEnumerator {
        static ReflectiveEnumerator() { }

        public static IEnumerable<T> GetEnumerableOfType<T>(params object[] constructorArgs) where T : class {
            List<T> objects = new List<T>();
            foreach (Type type in
                Assembly.GetAssembly(typeof(T)).GetTypes()
                .Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(T)))) {
                objects.Add((T)Activator.CreateInstance(type, constructorArgs));
            }
            return objects;
        }
    }
    public static class Utils {

        public static bool ContainsStartWith(this IEnumerable<string> list, string start) {
            return list.Any(element => element.ToLowerInvariant().Trim().StartsWith(start));
        }
        public static bool IsNullOrEmpty(this string s) {
            return string.IsNullOrEmpty(s);
        }

        public static string CapitalizeFirst(this string s) {
            return s.IsNullOrEmpty() ? "" : s.Substring(0, 1).ToUpper() + s.Substring(1);
        }


        /// <summary>
        /// Паттерн записи моделей марок имямарки_001 где 001 id
        /// </summary>
        public static int? ExtractId(this string val) {
            if (val.IsNullOrEmpty()) {
                return null;
            }
            var makeParts = val.Split("_");
            if (makeParts.Length != 2) {
                return null;
            }
            var strId = makeParts.Last();
            if (Int32.TryParse(strId, out var id)) {
                return id;
            }

            return null;
        }
        public static  string ExtractName(this string val) {
            if (val.IsNullOrEmpty()) {
                return null;
            }
            var makeParts = val.Split("_");
            if (makeParts.Length != 2) {
                return null;
            }
            return makeParts.First();
        }

        public static bool Is(this string self, Type target) {
            return self == target.FullName;
		}

        public static Random rand = new Random((int)DateTime.UtcNow.Ticks);
        public static string GetRand(this string[] data) {
            var start = 0;
            var end = data.Length - 1;
            var index = rand.Next(start, end);
            return data[index];
        }
    }
}
