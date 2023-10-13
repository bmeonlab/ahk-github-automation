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
}).AddHttpMessageHandler(sp => sp.GetRequiredService<AuthorizationMessageHandler>()
    .ConfigureHandler(authorizedUrls: new[] { builder.Configuration.GetSection("baseAddress").Value },
                      scopes: new[] { builder.Configuration.GetSection("TokenScope").Value }));

builder.Services.AddMsalAuthentication(options =>
{
    builder.Configuration.Bind("AzureAd", options.ProviderOptions.Authentication);
    options.ProviderOptions.LoginMode = "redirect";
    options.ProviderOptions.DefaultAccessTokenScopes.Add(builder.Configuration.GetSection("TokenScope").Value);
});

builder.Services.AddMudServices();
builder.Services.AddBlazoredLocalStorage();

var mapper = MapperConfig.InitializeAutomapper();

builder.Services.AddSingleton(mapper);
builder.Services.AddSingleton<SubmissionInfoService>();
builder.Services.AddSingleton<SubjectService>();
builder.Services.AddSingleton<GroupService>();
builder.Services.AddSingleton<AssignmentService>();

await builder.Build().RunAsync();
