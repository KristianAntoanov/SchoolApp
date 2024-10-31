using System.Reflection;

using Microsoft.Extensions.DependencyInjection;

using SchoolApp.Data.Models;
using SchoolApp.Data.Repository.Contracts;
using SchoolApp.Data.Repository;

namespace SchoolApp.Web.Infrastructure.Extensions
{
	public static class ServiceCollectionExtensions
	{
		public static void RegisterRepositories(this IServiceCollection services, Assembly modelsAssembly)
		{
			//TODO Re-write this ! 

			Type[] typesToExclude = new Type[] { typeof(ApplicationUser) };

			Type[] modelTypes = modelsAssembly.GetTypes()
				.Where(t => !t.IsAbstract && !t.IsInterface)
				.ToArray();

			foreach (var type in modelTypes)
			{
				if (!typesToExclude.Contains(type))
				{
					Type repositoryInterface = typeof(IRepository<,>);
					Type repositoryInstanceType = typeof(BaseRepository<,>);

					PropertyInfo? idPropInfo = type
						.GetProperties()
						.Where(p => p.Name.ToLower() == "id")
						.FirstOrDefault();

					Type[] constructArgs = new Type[2];
					constructArgs[0] = type;

                    if (idPropInfo == null)
					{
						constructArgs[1] = typeof(object);
                    }
					else
					{
						constructArgs[1] = idPropInfo.PropertyType;
                    }
					repositoryInterface = repositoryInterface.MakeGenericType(constructArgs);
					repositoryInstanceType = repositoryInstanceType.MakeGenericType(constructArgs);

					services.AddScoped(repositoryInterface, repositoryInstanceType);
				}
			}
		}
	}
}