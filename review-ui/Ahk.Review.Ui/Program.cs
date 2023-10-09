using Ahk.Review.Ui;
using Ahk.Review.Ui.Services;

using Blazored.LocalStorage;

using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddHttpClient("ApiClient", httpClient =>
{
    httpClient.BaseAddress = new Uri(builder.Configuration.GetSection("baseAddress").Value);

}).AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

builder.Services.AddMsalAuthentication(options =>
{
    builder.Configuration.Bind("AzureAd", options.ProviderOptions.Authentication);
    options.ProviderOptions.LoginMode = "redirect";
});

builder.Services.AddMudServices();
builder.Services.AddBlazoredLocalStorage();

var mapper = MapperConfig.InitializeAutomapper();

builder.Services.AddSingleton(mapper);
builder.Services.AddSingleton<Ahk.Review.Ui.Services.SubmissionInfoService>();
builder.Services.AddSingleton<SubjectService>();
builder.Services.AddSingleton<GroupService>();
builder.Services.AddSingleton<AssignmentService>();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
            policy =>
            {
                policy.WithOrigins(builder.Configuration.GetSection("baseAddress").Value + "*")
                                  .AllowAnyHeader()
                                  .AllowAnyMethod();
            });
});

await builder.Build().RunAsync();
