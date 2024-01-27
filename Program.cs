using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Console;
using signiel.Contexts;
using signiel.Core.Middleware;
using signiel.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddLogging(logging =>
    logging.AddSimpleConsole(options => {
        options.SingleLine = true;
        options.TimestampFormat = "HH:mm:ss ";
        options.ColorBehavior = LoggerColorBehavior.Enabled;
    })
);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => {
    options.IncludeXmlComments(Path.Combine(
        AppContext.BaseDirectory,
        $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"
    ));
});

// DB Context
var databaseConfig = builder.Configuration.GetSection("Database");
var connectionString = builder.Configuration.GetConnectionString("Database.SQL");
var dbOptionsBuilder = (DbContextOptionsBuilder options) => {
    options.UseMySql(
        connectionString,
        ServerVersion.AutoDetect(connectionString),
        options => options.EnableRetryOnFailure(
            maxRetryCount: databaseConfig.GetValue<int>("MaxRetryCount"),
            maxRetryDelay: TimeSpan.FromSeconds(databaseConfig.GetValue<int>("MaxRetryDelay")),
            errorNumbersToAdd: null
        )
    );
};

builder.Services.AddDbContextPool<SignielContext>(dbOptionsBuilder, databaseConfig.GetValue<int>("MaxPoolSize"));
builder.Services.AddDbContextFactory<SignielContext>(dbOptionsBuilder);

// Services
builder.Services.AddTransient<TripService>();

var app = builder.Build();

if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<RequestLoggingMiddleware>();
app.UseHttpsRedirection();
app.UseAuthorization();
app.UseAuthentication();

app.MapControllers();

app.Run();
