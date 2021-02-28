﻿using System;

namespace AliceAppraisal.Static {
	public static class Limits {
		public static int StartProductionYear { get; } = 2000;
		public static int EndProductionYear { get; } = DateTime.UtcNow.Year;
	}
}
