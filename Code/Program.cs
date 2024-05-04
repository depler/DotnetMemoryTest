namespace DotnetMemoryTest.Code;

static class Program
{
    static void Main()
    {
        MemoryCollector.Run();
        MemoryPatch.Install();

        var builder = WebApplication.CreateBuilder();
        builder.Services.AddControllers();

        var app = builder.Build();
        app.MapControllers();
        app.Run();
    }
}