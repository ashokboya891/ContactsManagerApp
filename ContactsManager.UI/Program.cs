using Entities;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Repositories;
using RepositoryContracts;
using ServiceContracts;
using Services;
using Serilog;
using CRUDE.Filters.ActionFilters;
using CRUDE.StartUpExtensions;
using CRUDE.Middleware;

var builder = WebApplication.CreateBuilder(args);

//serilog
builder.Host.UseSerilog((HostBuilderContext context, IServiceProvider services, LoggerConfiguration configuration) => {
    configuration
    .ReadFrom.Configuration(context.Configuration) //reads configuration from built-inconfiguration
    .ReadFrom.Services(services);           //readout current apps services make themavaialable in serilog
});

builder.Services.ConfigureServices(builder.Configuration);  //we have added configure services into  iservicescollection type

//Logging
//builder.Host.ConfigureLogging(loggingprovider =>
//{
//    loggingprovider.ClearProviders();
//    loggingprovider.AddConsole();
//    loggingprovider.AddDebug();

//});

var app = builder.Build();

if(builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
   app.UseExceptionHandler("/Error");
    app.UseMiddleware<ExceptionHandlingMiddleware>();
}

app.UseSerilogRequestLogging();
app.UseHttpLogging();

//app.Logger.LogDebug("debug-message");
//app.Logger.LogInformation("debug-message");
//app.Logger.LogWarning("debug-message");
//app.Logger.LogError("debug-message");
//app.Logger.LogCritical("debug-message");



if (builder.Environment.IsEnvironment("Test")==false)   //in customwebapplicationfactory while invoking virtual copy of progra.cs we wont invoke this below code 
{
Rotativa.AspNetCore.RotativaConfiguration.Setup("wwwroot", wkhtmltopdfRelativePath: "Rotativa");

}

app.UseStaticFiles();
app.UseRouting();       //identifing action method base route
app.UseAuthentication();   //reading  identity cokie 
app.UseAuthorization();     //validates access permissions of the user
app.MapControllers();    //executing filter pipeline action+filters
app.UseEndpoints(end =>
{
end.MapControllerRoute( //here we have appliced convetional routing for all controller and action globally but attribute routing will take more precidence than conventional
        name: "default",
        pattern:"{controller}/{action}"
        );
});
app.Run();

public partial class Program { } //make the auto-generated Program accessible programmatically
                                 //compaimer will compiles above all code into single unit and adds into predefinded publi static void main method itself so we are creating partial below so that we can use it in integration test as seperate application rather suing real application just like virtual copy of thir program

//before this we have add itemgroup inside this line   <InternalsVisibleTo Include="TestUnits"/> to allow to use program.cs virtual copy in testunits project