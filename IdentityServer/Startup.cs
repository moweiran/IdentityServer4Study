using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.EntityFramework;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Linq;
using System.Reflection;

namespace IdentityServer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            #region 未设置EFCore前的配置
            //var builder = services.AddIdentityServer()
            //    .AddInMemoryIdentityResources(Config.IdentityResources)
            //    .AddDeveloperSigningCredential() //This is for dev only scenarios when you don’t have a certificate to use.
            //    .AddInMemoryApiScopes(Config.ApiScopes)
            //    .AddInMemoryClients(Config.Clients);
            #endregion

            #region 使用EFCore存储
            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;
            string connectionString = Configuration.GetConnectionString("MySqlDbConnectString");

            services.AddIdentityServer()
                .AddDeveloperSigningCredential() // resolved Keyset is missing
                .AddConfigurationStore(options =>
                {
                    options.ConfigureDbContext = b => b.UseMySql(connectionString,
                        sql => sql.MigrationsAssembly(migrationsAssembly));
                })
                .AddOperationalStore(options =>
                {
                    options.ConfigureDbContext = b => b.UseMySql(connectionString,
                        sql => sql.MigrationsAssembly(migrationsAssembly));
                    options.EnableTokenCleanup = true;
                    options.TokenCleanupInterval = 30;
                })
                .AddResourceOwnerValidator<ResourcePasswordValidator>();
                //.AddTestUsers(Config.GetUsers);
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // InitializeDatabase(app);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseIdentityServer();
            //app.UseRouting();

            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapGet("/", async context =>
            //    {
            //        await context.Response.WriteAsync("Hello World!");
            //    });
            //});
        }

        private void InitializeDatabase(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                serviceScope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();

                var context = serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
                context.Database.Migrate();
                foreach (var client in Config.Clients)
                {
                    if (!context.Clients.Any(o => o.ClientId == client.ClientId))
                    {
                        context.Clients.Add(client.ToEntity());
                    }
                }
                //context.SaveChanges();

                foreach (var identityResource in Config.IdentityResources)
                {
                    if (!context.IdentityResources.Any(o => o.Name == identityResource.Name))
                    {
                        context.IdentityResources.Add(identityResource.ToEntity());
                    }
                }

                foreach (var resource in Config.ApiResources)
                {
                    if (!context.ApiResources.Any(o => o.Name == resource.Name))
                    {
                        context.ApiResources.Add(resource.ToEntity());
                    }
                }
                //context.SaveChanges();

                foreach (var scope in Config.ApiScopes)
                {
                    if (!context.ApiScopes.Any(o => o.Name == scope.Name))
                    {
                        context.ApiScopes.Add(scope.ToEntity());
                    }
                }
                context.SaveChanges();
            }
        }
    }
}
