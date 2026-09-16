using AI.CustomerSupport.Services;
using AI.CustomerSupport.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Controllers
builder.Services.AddControllers();

// Database
builder.Services.AddDbContext<SupportDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

// Services
builder.Services.AddScoped<ChatService>();
builder.Services.AddScoped<SearchService>();
builder.Services.AddSingleton<RagStoreService>();
builder.Services.AddScoped<TicketService>();
builder.Services.AddScoped<DocumentIngestionService>();

// Ollama - OpenAI service
builder.Services.AddHttpClient<OpenAIService>(client =>
{
    client.BaseAddress = new Uri("http://localhost:11434/");
});

// Ollama - Embedding service
builder.Services.AddHttpClient<EmbeddingService>(client =>
{
    client.BaseAddress = new Uri("http://localhost:11434/");
});

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AngularClient", policy =>
    {
        policy
            .WithOrigins(
                "http://localhost:4200",
                "https://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

// Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// HTTPS
app.UseHttpsRedirection();

// CORS
app.UseCors("AngularClient");

app.UseAuthorization();

app.MapControllers();

app.Run();