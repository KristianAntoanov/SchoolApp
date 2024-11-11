using System.Reflection;

using Microsoft.Extensions.DependencyInjection;

using SchoolApp.Data.Repository.Contracts;
using SchoolApp.Data.Repository;

namespace SchoolApp.Web.Infrastructure.Extensions
{
	public static class ServiceCollectionExtensions
	{
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