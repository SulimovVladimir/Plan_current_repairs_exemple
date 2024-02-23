using Microsoft.Extensions.Options;
using Plan_current_repairs.Data.Interface;
using Plan_current_repairs.Data.Models;
using Quartz;
using System;
using System.Threading.Tasks;

namespace Plan_current_repairs.Jobs
{
    public class AddNewYearInAppJob :IJob
    {
        public readonly IYears years;
        public readonly Settings settings;
        public AddNewYearInAppJob(IYears years, IOptionsMonitor<Settings> _settings)
        {
            this.years = years;
            settings = _settings.CurrentValue;
        }

        public Task Execute(IJobExecutionContext context)
        {
            settings.CurrentYear++;
            Year year = new Year() { Years = (ushort)settings.CurrentYear};
            years.AddAndEditYear(year);
            settings.LoadSetting(settings);
            return Task.CompletedTask;
        }
    }
}
