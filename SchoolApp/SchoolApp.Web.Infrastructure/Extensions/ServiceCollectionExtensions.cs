using System.Reflection;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

using SchoolApp.Data.Repository.Contracts;
using SchoolApp.Data.Repository;
using SchoolApp.Data;
using Microsoft.AspNetCore.Identity;
using SchoolApp.Data.Models;

namespace SchoolApp.Web.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));
            services.AddDatabaseDeveloperPageExceptionFilter();

            return services;
        }

        public static IServiceCollection AddApplicationIdentity(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {
                options.Password.RequireDigit =
                    configuration.GetValue<bool>("Identity:Password:RequireDigits");
                options.Password.RequireLowercase =
                    configuration.GetValue<bool>("Identity:Password:RequireLowercase");
                options.Password.RequireUppercase =
                    configuration.GetValue<bool>("Identity:Password:RequireUppercase");
                options.Password.RequireNonAlphanumeric =
                    configuration.GetValue<bool>("Identity:Password:RequireNonAlphanumeric");
                options.Password.RequiredLength =
                    configuration.GetValue<int>("Identity:Password:RequiredLength");
                options.Password.RequiredUniqueChars =
                    configuration.GetValue<int>("Identity:Password:RequiredUniqueCharacters");

                options.SignIn.RequireConfirmedAccount =
                    configuration.GetValue<bool>("Identity:SignIn:RequiredConfirmedAccount");
                options.SignIn.RequireConfirmedEmail =
                    configuration.GetValue<bool>("Identity:SignIn:RequireConfirmedEmail");
                options.SignIn.RequireConfirmedPhoneNumber =
                    configuration.GetValue<bool>("Identity:SignIn:RequireConfirmedPhoneNumber");

                options.User.RequireUniqueEmail =
                    configuration.GetValue<bool>("Identity:User:RequireUniqueEmail");

            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders()
            .AddRoles<ApplicationRole>()
            .AddSignInManager<SignInManager<ApplicationUser>>()
            .AddUserManager<UserManager<ApplicationUser>>()
            .AddErrorDescriber<CustomIdentityErrorDescriber>()
            .AddDefaultUI();


            return services;
        }

        public class CustomIdentityErrorDescriber : IdentityErrorDescriber
        {
            public override IdentityError PasswordRequiresUpper()
            {
                return new IdentityError
                {
                    Code = nameof(PasswordRequiresUpper),
                    Description = "Паролата трябва да съдържа поне една главна буква (А-Я)."
                };
            }

            public override IdentityError PasswordRequiresLower()
            {
                return new IdentityError
                {
                    Code = nameof(PasswordRequiresLower),
                    Description = "Паролата трябва да съдържа поне една малка буква (а-я)."
                };
            }

            public override IdentityError PasswordRequiresDigit()
            {
                return new IdentityError
                {
                    Code = nameof(PasswordRequiresDigit),
                    Description = "Паролата трябва да съдържа поне една цифра (0-9)."
                };
            }
        }

        public static void RegisterRepositories(this IServiceCollection services)
        {
            services.AddScoped<IRepository, BaseRepository>();
        }

        public static void RegisterUserDefinedServices(this IServiceCollection services, Assembly serviceAssembly)
        {
            Type[] serviceInterfaceTypes = serviceAssembly
                .GetTypes()
                .Where(t => t.IsInterface)
                .ToArray();
            Type[] serviceTypes = serviceAssembly
                .GetTypes()
                .Where(t => !t.IsInterface && !t.IsAbstract &&
                             t.Name.ToLower().EndsWith("service"))
                .ToArray();

            foreach (Type serviceInterfaceType in serviceInterfaceTypes)
            {
                Type? serviceType = serviceTypes
                    .SingleOrDefault(t => "i" + t.Name.ToLower() == serviceInterfaceType.Name.ToLower());
                if (serviceType == null)
                {
                    throw new NullReferenceException($"Service type could not be obtained for the service {serviceInterfaceType.Name}");
                }

                services.AddScoped(serviceInterfaceType, serviceType);
            }
        }
    }
}