using ChatWithSLM.UI;
using ChatWithSLM.UI.Components;
using ChatWithSLM.UI.Utils;

var builder = WebApplication.CreateBuilder(args);



// Add service defaults & Aspire components.
builder.AddServiceDefaults();
builder.AddRedisOutputCache("cache");

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();


builder.Services.AddHttpClient<SLMClient>(client =>
    {
        client.BaseAddress = new("https+http://chatwithslmservices");
    });

// builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https+http://chatwithslmservices") });


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();


app.UseOutputCache();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();


app.MapDefaultEndpoints();


app.Run();
