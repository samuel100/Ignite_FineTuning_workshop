using Microsoft.ML.OnnxRuntimeGenAI;
using ChatWithSLM.API.Utils;
using ChatWithSLM.API.Domains;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddProblemDetails();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();



app.MapGet("/", () => "Welcome to access ONNXRuntime GenAI Model API Service");


app.MapPost("/v1/chat/completions", (ChatRequestPrompt request) =>
{
    if(request.Model == "Phi-3.5-Instruct")
    {
        var response = GenAI.ChatWithGenAIONNXOpenAIAsync(request.Messages);

        return response.generated_text;

    }
    else
    {
        var response = GenAI.ChatWithGenAIONNXOpenAIAsync(request.Messages, true);

        return response.generated_text;
    }
});



app.Run();


