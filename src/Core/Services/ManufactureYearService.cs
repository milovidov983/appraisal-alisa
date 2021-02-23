using AliceAppraisal.Models;
using AliceAppraisal.Static;
using System;

namespace AliceAppraisal.Core {
	public class ManufactureYearService : IManufactureYearService {
		public int? GetYearFromUserInputOrNull(string valueYear) {
			var isCorrectConverted = Int32.TryParse(valueYear, out var manufactureYear);
			if (!isCorrectConverted) {
				return null;
			}

			string error = Validate(manufactureYear);
			if (error != null) {
				throw new InvalidRequestException(error);
			}

			return manufactureYear;
		}

		private string Validate(int manufactureYear) {
			if (manufactureYear > DateTime.UtcNow.Year) {
				return $"Кажется указанный вами {manufactureYear} год выпуска еще наступил, попробуйте еще раз.";
			}
			if (manufactureYear < 2000) {
				return "Кажется год который вы указали выходит за нижний предел ограничения, " +
					 $"минимально возможным годом является {Limits.StartProductionYear}. " +
					 $"Попробуйте еще раз.";
			}
			return null;
		}
	}
}
