using AspNetCoreProject.Models.Services;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Embeddings;
using Microsoft.SemanticKernel.Connectors.Ollama;
using System; 

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews(); // Use AddControllersWithViews for MVC

// Configure LLM and Embedding client using Ollama
builder.Services.AddKernel()
    .AddOllamaChatCompletion(
        endpoint: new Uri("http://localhost:11434"), // Change string to Uri object
        modelId: "llama3" // Specify your chat model (e.g., llama3, mistral)
    )
    .AddOllamaTextEmbeddingGeneration(
        endpoint: new Uri("http://localhost:11434"), // Change string to Uri object
        modelId: "nomic-embed-text" // Specify your embedding model
    );


// Register your DocumentService for Q&A
builder.Services.AddSingleton<DocumentService>();

// For initial testing, add some documents when the app starts
// In a real app, these would come from a database, files, etc.
// This block will run once when the application starts.
using (var scope = builder.Services.BuildServiceProvider().CreateScope())
{
    var documentService = scope.ServiceProvider.GetRequiredService<DocumentService>();
    // Make sure these documents are added
    await documentService.AddDocument("The capital of France is Paris. Paris is known for the Eiffel Tower.");
    await documentService.AddDocument("Mount Everest is the highest mountain in the world. It is located in the Himalayas.");
    await documentService.AddDocument("The quick brown fox jumps over the lazy dog. The lazy dog then barked.");
    await documentService.AddDocument("ASP.NET Core MVC is a framework for building web applications using the Model-View-Controller design pattern.");
    await documentService.AddDocument("Ollama is a tool for running large language models locally on your machine.");
}

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles(); // Essential for serving CSS, JS, etc.

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"); // Default route for MVC

app.Run();