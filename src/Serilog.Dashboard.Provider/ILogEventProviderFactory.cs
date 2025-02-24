namespace Serilog.Dashboard.Provider;

public interface ILogEventProviderFactory
{
    ILogEventProvider CreateProvider();
}