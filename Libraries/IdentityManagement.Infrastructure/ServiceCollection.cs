using IdentityManagement.Infrastructure.Persistence;
using IdentityManagement.Infrastructure.Persistence.Contexts;
using IdentityManagement.Infrastructure.Services;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityManagement.Infrastructure
{
    public static class ServiceCollection
    {
        public static void AddIdentityServerConfig(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddIdentity<AppUser, AppRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.Password.RequiredLength = 0;
                options.Password.RequiredUniqueChars = 0;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireDigit = false;
                options.Password.RequireNonAlphanumeric = false;
                // options.User.AllowedUserNameCharacters = "abcçdefgğhijklmnoöpqrsştuüvwxyzABCÇDEFGĞHIİJKLMNOÖPQRSŞTUÜVWXYZ0123456789-._@{}#&%";
            })
            .AddEntityFrameworkStores<AppIdentityDbContext>()
            .AddDefaultTokenProviders();

            services.AddIdentityServer()
                    .AddDeveloperSigningCredential()
                    .AddOperationalStore(options =>
                    {
                        options.ConfigureDbContext = builder => builder.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
                        options.EnableTokenCleanup = true;
                    })
                    .AddConfigurationStore(options =>
                    {
                        options.ConfigureDbContext = builder => builder.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
                    })
                    .AddAspNetIdentity<AppUser>();
        }

        public static void AddServices<TUser>(this IServiceCollection services) where TUser : IdentityUser<int>, new()
        {
            services.AddTransient<IProfileService, IdentityClaimsProfileService>();
        }

        public static void AddDataBaseConfiguration(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<AppIdentityDbContext>(options => { options.UseNpgsql(connectionString); });
            services.AddDbContext<AppPersistedGrantDbContext>(options => { options.UseNpgsql(connectionString); });
            services.AddDbContext<AppConfigurationDbContext>(options => { options.UseNpgsql(connectionString); });
        }
    }
}
