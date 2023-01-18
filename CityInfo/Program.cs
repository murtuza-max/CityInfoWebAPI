using CityInfo.Context;
using CityInfo.Services;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using NLog.Extensions.Logging;
using NLog.Web;



var builder = WebApplication.CreateBuilder(args);

var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
// Add services to the container.
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddNLog();



builder.Services.AddControllers();
builder.Services.AddMvc().AddMvcOptions(o => o.OutputFormatters.Add(new XmlDataContractSerializerOutputFormatter())); // outputformater
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<ICityInfoRepo,CityInfoRepo>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
#if DEBUG
builder.Services.AddTransient<IMailService, LocalMailService>();
#else
        builder.Services.AddTransient<IMailService,CloudMailService>();
#endif
var cs = builder.Configuration.GetConnectionString("CityDb");
builder.Services.AddDbContext<CityInfoContext>(options =>
    {
        options.UseSqlServer(cs);
        //options.UseSqlServer(builder.Configuration.GetConnectionString("CityDb"));
    });
var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
    app.UseSwaggerUI();
    }

app.UseAuthorization();

app.UseRouting();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"); // indicates route that will take controller/action
logger.Info("app is Running...");
using (var scope = app.Services.CreateScope())
{
    try
    {
        var context = scope.ServiceProvider.GetService<CityInfoContext>();

        // for demo purposes, delete the database & migrate on startup so 
        // we can start with a clean slate
        context.Database.EnsureDeleted();
        context.Database.Migrate();
    }
    catch (Exception ex)
    {
        logger.Error(ex, "An error occurred while migrating the database.");
    }
}
app.Run();
