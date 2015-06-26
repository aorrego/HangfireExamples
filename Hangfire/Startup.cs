using Hangfire.Models;
using Hangfire.SqlServer;
using Microsoft.Owin;
using Owin;
using System;

[assembly: OwinStartupAttribute(typeof(Hangfire.Startup))]
namespace Hangfire
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
                        
            GlobalConfiguration.Configuration
                .UseSqlServerStorage("HangfireDB", 
                new SqlServerStorageOptions
                {
                    // set to true to have HangFire create your schema
                    PrepareSchemaIfNecessary = true,
                    // set to false and run HangFire.sql to create your sql.
                    //PrepareSchemaIfNecessary = false, 
                    QueuePollInterval = TimeSpan.FromSeconds(1)
                });

            app.UseHangfireDashboard();
            app.UseHangfireServer();          
            
            RecurringJob.AddOrUpdate<EmailSender>(x => x.Send("[YOUR EMAIL HERE]", "[SUBJECT HERE]", "[BODY HERE]"), Cron.Minutely);
            //BackgroundJob.Enqueue(() => TextBuffer.WriteLine("Probando esto"));
        }
    }
}
