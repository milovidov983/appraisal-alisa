using System;
using System.Collections.Generic;
using System.Text;

namespace AliceAppraisal.Core {
	public interface ILoggerFactory {
		Serilog.Core.Logger GetLogger();
	}
}
