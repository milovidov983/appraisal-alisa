using System.Linq;

namespace AliceAppraisal.Application.Configuration {
	public static class StringUtils {
		public static string ToSnakeCase(this string str) {
			return string.Concat(str.Select((x, i) => i > 0 && char.IsUpper(x) 
			? "_" + x.ToString() 
			: x.ToString())).ToLower();
		}

	}
}
