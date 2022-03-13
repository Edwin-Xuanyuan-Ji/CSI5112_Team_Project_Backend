using CSI5112BackEndApi.Models;
using CSI5112BackEndApi.Services;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<CSI5112BackEndDataBaseSettings>(builder.Configuration.GetSection("CSI5112BackendDataBase"));

builder.Services.AddSingleton<CustomersService>();
builder.Services.AddSingleton<ProductsService>();
builder.Services.AddSingleton<CartItemsService>();
builder.Services.AddSingleton<MerchantsService>();
builder.Services.AddSingleton<ShippingAddressService>();
builder.Services.AddSingleton<AnswersService>();
builder.Services.AddSingleton<QuestionsService>();
builder.Services.AddSingleton<SalesOrdersService>();

builder.Services.AddCors(options => {
    options.AddPolicy(name: "policy",
        builder => {
            builder.WithOrigins("https://localhost:7027")
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
        }
    );
});

builder.Services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => {
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "CSI 5112 BackEnd API",
        Description = "An ASP.NET Core Web API with MongoDB",
        TermsOfService = new Uri("https://cloud.mongodb.com/v2/62211b47e91d7e6f2615be09#clusters"),
        Contact = new OpenApiContact
        {
            Name = "Data Structure",
            Url = new Uri("https://drive.google.com/file/d/19hSHSHkBbbwpSPiB6_9oLpAzXla3YvcQ/view?usp=sharing")
        },
        License = new OpenApiLicense
        {
            Name = "Document",
            Url = new Uri("https://docs.google.com/document/d/1NMq69Xqf4LZchNlQu6PfEojGmDLJqnAPf4AUvYSSdPA/edit")
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseCors("policy");

app.UseAuthorization();

app.MapControllers();

app.Run();
