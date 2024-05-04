using DotnetTest3;

RunServer();

static void RunServer()
{
    MemoryCollector.Run();
    MemoryPatch.Install();

    var builder = WebApplication.CreateBuilder();
    builder.Services.AddControllers();

    var app = builder.Build();
    app.MapControllers();
    app.Run();
}