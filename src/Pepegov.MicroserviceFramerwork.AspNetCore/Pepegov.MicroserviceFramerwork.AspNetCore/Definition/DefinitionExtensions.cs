using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Pepegov.MicroserviceFramerwork.AspNetCore.Definition;

public static class DefinitionExtensions
{
    public static void AddDefinitions(this IServiceCollection services, WebApplicationBuilder builder,
        params Type[] entryPointsAssembly)
    {
        var definitions = GetDefinitionByAssembly(entryPointsAssembly);
        definitions.ForEach(definition => definition.ConfigureServicesAsync(services, builder));
        services.AddSingleton(definitions as IReadOnlyCollection<IDefinition>);
    }

    public static void AddDefinitionsToCollection(this WebApplicationBuilder builder, params Type[] entryPointsAssembly)
    {
        var definitions = GetDefinitionByAssembly(entryPointsAssembly);
        builder.Services.AddSingleton(definitions as IReadOnlyCollection<IDefinition>);
    }

    private static List<IDefinition> GetDefinitionByAssembly(Type[] entryPointsAssembly)
    {
        var definitions = new List<IDefinition>();

        foreach (var entryPoint in entryPointsAssembly)
        {
            var types = entryPoint.Assembly.ExportedTypes.Where(t =>
                !t.IsAbstract && typeof(IDefinition).IsAssignableFrom(t));
            var instances = types.Select(Activator.CreateInstance).Cast<IDefinition>();
            var list = instances.Where(x => x.Enabled == true);
            definitions.AddRange(list);
        }

        return definitions;
    }

    public static void UseDefinitions(this WebApplication application)
    {
        var definitions = application.Services.GetRequiredService<IReadOnlyCollection<IDefinition>>();
        foreach (var endpoint in definitions)
        {
            endpoint.ConfigureApplicationAsync(application);
        }
    }

    public static void AddDefinitionByType(this WebApplicationBuilder builder, params Type[] definitionTypes)
    {
        using (ServiceProvider serviceProvider = builder.Services.BuildServiceProvider())
        {
            var collection = serviceProvider.GetService<IReadOnlyCollection<IDefinition>>()
                .Where(x => definitionTypes.Any(o => o == x. GetType())).ToList();
            collection.ForEach(x => x.ConfigureServicesAsync(builder.Services, builder));
        }
    }
    
    public static void UseDefinitionByType(this WebApplication application, params Type[] definitionTypes)
    {
        using (var scope = application.Services.CreateScope())
        {
            var collection = scope.ServiceProvider.GetService<IReadOnlyCollection<IDefinition>>()
                .Where(x => definitionTypes.Any(o => o == x. GetType())).ToList();
            collection.ForEach(x => x.ConfigureApplicationAsync(application));
        }
    }
}