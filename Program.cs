using TodoList.Data;

var builder = WebApplication.CreateBuilder(args);

// Configurando serviços
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDbContext<AppDbContext>();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.UseRouting();
app.UseAuthorization();
app.MapControllers();

app.Run();
