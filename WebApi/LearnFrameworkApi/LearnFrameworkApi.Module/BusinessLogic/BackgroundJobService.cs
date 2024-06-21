using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using LearnFrameworkApi.Module.Datas;
using Serilog;

namespace nc_backend.module.BusinessLogic
{
    public static class BackgroundJobService
    {
        public static async Task ExecuteQueue(AppDbContext context, int interval, CancellationToken stoppingToken)
        {
            try
            {
                Log.Information(@"BackgroundJobService.ExecuteQueue Start");

                //your job

                Log.Information(@"BackgroundJobService.ExecuteQueue Finish");
            }
            catch (Exception ex)
            {
                Log.Information($"BackgroundJobService.ExecuteQueue Exception \nMessage: {ex.Message} \nInner Exception: {ex.InnerException}");
            }
            await Task.Delay(interval, stoppingToken);
        }
    }
}
