using System;
using System.Collections.Generic;
using System.Linq;

namespace AliceAppraisal.Static {
	public static class VehicleComponents {
		public static readonly Dictionary<string,string> BodyTypes 
			= Enum.GetValues(typeof(BodyTypes)).Cast<BodyTypes>()
			.ToDictionary(
				x => x.ToString().ToLowerInvariant(),
				x => x.GetDescription()
				);
		public static string TryGetBodyTypeDescriptionOrDefault(this string id) {
			return TryGetParamName(id, BodyTypes);
		}

		public static readonly Dictionary<string, string> Gearboxes
			= Enum.GetValues(typeof(Gearboxes)).Cast<Gearboxes>()
			.ToDictionary(
				x => x.ToString().ToLowerInvariant(),
				x => x.GetDescription()
				);

		public static string TryGetGearboxesDescriptionOrDefault(this string id) {
			return TryGetParamName(id, Gearboxes);
		}

		public static readonly Dictionary<string, string> Drives
			= Enum.GetValues(typeof(Drives)).Cast<Drives>()
			.ToDictionary(
				x => x.ToString().ToLowerInvariant(),
				x => x.GetDescription()
				);

		public static string TryGetDrivesDescriptionOrDefault(this string id) {
			return TryGetParamName(id, Drives);
		}

		public static readonly Dictionary<string, string> EngineTypes
			= Enum.GetValues(typeof(EngineTypes)).Cast<EngineTypes>()
			.ToDictionary(
				x => x.ToString().ToLowerInvariant(),
				x => x.GetDescription()
				);

		public static string TryGetEngineTypesDescriptionOrDefault(this string id) {
			return TryGetParamName(id, EngineTypes);
		}

		public static readonly Dictionary<string, string> Equipments
			= Enum.GetValues(typeof(Equipments)).Cast<Equipments>()
			.ToDictionary(
				x => x.ToString().ToLowerInvariant(),
				x => x.GetDescription()
				);

		public static string TryGetEquipmentsDescriptionOrDefault(this string id) {
			return TryGetParamName(id, Equipments);
		}

		private static string TryGetParamName(string id, Dictionary<string, string> data) {
			data.TryGetValue(id ?? "", out var desc);
			return desc?.ToLower() ?? id ?? "";
		}
	}
}