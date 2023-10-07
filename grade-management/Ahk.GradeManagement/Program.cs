using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ahk.GradeManagement.Data;
using Ahk.GradeManagement.Data.Entities;
using Ahk.GradeManagement.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Ahk.GradeManagement.Services.AssignmentService;
using Ahk.GradeManagement.Services.GroupService;
using Ahk.GradeManagement.Services.SetGradeService;
using Ahk.GradeManagement.Services.StatusTrackingService;
using Ahk.GradeManagement.Helpers;
using Ahk.GradeManagement.Services.SubjectService;
using Microsoft.Extensions.Configuration;

namespace Ahk.GradeManagement
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = new HostBuilder()
                .ConfigureFunctionsWorkerDefaults()
                .ConfigureServices(services =>
                {
                    services.AddMemoryCache(setup =>
                    {
                        setup.ExpirationScanFrequency = TimeSpan.FromMinutes(4);
                    });
                    services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
                    services.AddSingleton<ResultProcessing.IResultProcessor, ResultProcessing.ResultProcessor>();
                    services.AddSingleton<Services.ITokenManagementService, Services.TokenManagementService>();
                    services.AddSingleton<ISetGradeService, SetGradeService>();
                    services.AddSingleton<ListGrades.IGradeListing, ListGrades.GradeListing>();
                    services.AddSingleton<IStatusTrackingService, StatusTrackingService>();
                    services.AddSingleton<IGroupService, GroupService>();
                    services.AddSingleton<IAssignmentService, AssignmentService>();
                    services.AddSingleton<IGradeService, GradeService>();
                    services.AddSingleton<ISubjectService, SubjectService>();

                    services.AddCors();

                    var mapper = MapperConfig.InitializeAutomapper();

                    services.AddSingleton(mapper);

                    var configuration = services.BuildServiceProvider().GetService<IConfiguration>();
                    var connString = configuration.GetConnectionString("SQLAZURECONNSTR_AHK_ConnString");

                    string azureSqlConnString = string.IsNullOrEmpty(connString) ? Environment.GetEnvironmentVariable("SQLAZURECONNSTR_AHK_ConnString") : connString;

                    var dbContextBuilder = new DbContextOptionsBuilder<AhkDbContext>();
                    dbContextBuilder.UseSqlServer(azureSqlConnString);

                    AhkDbContext ahkDbContext = new AhkDbContext(dbContextBuilder.Options);

                    services.AddSingleton(ahkDbContext);
                })
                .Build();

            //using (var context = new AhkDbContextFactory().CreateDbContext(args))
            //{
            //    context.Database.EnsureCreated();
            //    var token = context.WebhookTokens.FirstOrDefault();
            //    if (token == null)
            //    {
            //        var id = Guid.NewGuid().ToString("N");
            //        var secret = Guid.NewGuid().ToString("N");
            //        var webHookToken = new WebhookToken() { Id = id, Secret = secret };
            //        context.WebhookTokens.Add(webHookToken);
            //        context.SaveChanges();
            //    }

            //    SampleDataSeeder seeder = new SampleDataSeeder(context);
            //    //seeder.ClearData();
            //    //seeder.SeedData();
            //}

            await host.RunAsync();
        }
    }
}
