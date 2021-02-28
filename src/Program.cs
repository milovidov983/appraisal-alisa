using AliceAppraisal.Application.Configuration;
using AliceAppraisal.Application.Infrastructure;
using AliceAppraisal.Application.Infrastructure.Models;
using AliceAppraisal.Helpers;
using AliceAppraisal.Implementations.Infrastructure;
using Serilog;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AliceAppraisal {
	class Program {
		static async Task Main(string[] args) {
			// AliceAppraisal.EntryPoint.FunctionHandler стартовая функция

			//DataProviderService service = new DataProviderService();
			//var a = Settings.Instance.Domain;
			//await service.GetPupularMakes();

			await Task.Yield();
		}
	}
}
