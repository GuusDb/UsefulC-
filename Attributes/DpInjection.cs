// Attribute to be used
[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public sealed class DpInjectorAttribute : Attribute
{
    public Type InterfaceType { get; }

    public DpInjectorAttribute(Type interfaceType = null)
    {
        InterfaceType = interfaceType;
    }
}


// Example usage of a class you want to use dependency injection on
[DpInjector/**(typeof(IData))**/]
public partial class Data(/****/)
{
    public async Task SomeFunction(Request entity)
    {
       ...
    }
}


// Registering the services using Scrutor
using Scrutor;
builder.Services.Scan(scan => scan
    .FromAssemblies(Assembly.GetExecutingAssembly()) // Scan the current assembly
    .AddClasses(classes => classes.WithAttribute<DpInjectorAttribute>())  // Look for classes with the DpInjector attribute
    .UsingRegistrationStrategy(RegistrationStrategy.Skip) // Skip classes without an interface in the attribute
    .As(type =>
    {
        var attribute = type.GetCustomAttribute<DpInjectorAttribute>();
        return attribute?.InterfaceType != null
            ? new[] { attribute.InterfaceType }
                : new[] { type };
    })
    // you could expand the attribute to define the scope in the attribute or make different attributes per lifetime
    .WithLifetime(ServiceLifetime.Scoped)