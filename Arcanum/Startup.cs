using Arcanum.Auth.Models;
using Arcanum.Auth.Models.Interfaces;
using Arcanum.Auth.Models.Interfaces.Services;
using Arcanum.Data;
using Arcanum.ImageBlob.Interfaces;
using Arcanum.ImageBlob.Interfaces.Services;
using Arcanum.Models.Interfaces;
using Arcanum.Models.Interfaces.Services;
using Azure.Core;
using Azure.Core.Extensions;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Azure.Storage.Blobs;
using Azure.Storage.Queues;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace Arcanum
{
    public class Startup
    {
        public IConfiguration _config { get; }

        public Startup(IConfiguration configuration)
        {
            _config = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();

            services.AddDbContext<ArcanumDbContext>(options =>
            {
                // LocalConnection for local DB, DefaultConnection affects live DB on Azure
                string connectionString = _config.GetConnectionString("LocalConnection");
                options.UseSqlServer(connectionString);
            });

            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<ArcanumDbContext>();

            services.AddAuthentication();

            //services.AddAuthorization(options =>
            //{
            //    options.AddPolicy("create", policy => policy.RequireClaim("permissions", "create"));
            //    options.AddPolicy("read", policy => policy.RequireClaim("permissions", "read"));
            //    options.AddPolicy("update", policy => policy.RequireClaim("permissions", "update"));
            //    options.AddPolicy("delete", policy => policy.RequireClaim("permissions", "delete"));
            //});

            services.AddTransient<IUserService, IdentityUserService>();
            services.AddTransient<IArtistAdmin, ArtistAdminService>();
            services.AddTransient<IWizardLord, WizardLordService>();
            services.AddTransient<IUpload, UploadService>();
            services.AddTransient<ISite, SiteService>();

            services.AddControllers().AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );

            services.AddAzureClients(builder =>
            {
                builder.AddBlobServiceClient(_config["StorageBlob:ConnectionString"], preferMsi: true);
                //builder.AddQueueServiceClient(_config["StorageBlob:ConnectionString:queue"], preferMsi: true);
            });
        }

        

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            SecretClientOptions options = new SecretClientOptions()
            {
                Retry =
                {
                    Delay= TimeSpan.FromSeconds(2),
                    MaxDelay = TimeSpan.FromSeconds(16),
                    MaxRetries = 5,
                    Mode = RetryMode.Exponential
                 }
            };
            var client = new SecretClient(new Uri(_config["KeyVaultUri"]), new DefaultAzureCredential(), options);

            //KeyVaultSecret secret = client.GetSecret("<mySecret>");

            //string secretValue = secret.Value;

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }

    /// <summary>
    /// Azure Blob sorcery
    /// </summary>
    internal static class StartupExtensions
    {
        public static IAzureClientBuilder<BlobServiceClient, BlobClientOptions> AddBlobServiceClient(this AzureClientFactoryBuilder builder, string serviceUriOrConnectionString, bool preferMsi)
        {
            if (preferMsi && Uri.TryCreate(serviceUriOrConnectionString, UriKind.Absolute, out Uri serviceUri))
            {
                return builder.AddBlobServiceClient(serviceUri);
            }
            else
            {
                return builder.AddBlobServiceClient(serviceUriOrConnectionString);
            }
        }
        public static IAzureClientBuilder<QueueServiceClient, QueueClientOptions> AddQueueServiceClient(this AzureClientFactoryBuilder builder, string serviceUriOrConnectionString, bool preferMsi)
        {
            if (preferMsi && Uri.TryCreate(serviceUriOrConnectionString, UriKind.Absolute, out Uri serviceUri))
            {
                return builder.AddQueueServiceClient(serviceUri);
            }
            else
            {
                return builder.AddQueueServiceClient(serviceUriOrConnectionString);
            }
        }
    }
}
