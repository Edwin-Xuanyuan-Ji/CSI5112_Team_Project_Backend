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

builder.Services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => {
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "CSI 5112 BackEnd API",
        Description = "An ASP.NET Core Web API with MongoDB",
        Contact = new OpenApiContact
        {
            Name = "Data Structure",
            Url = new Uri("https://app.diagrams.net/#G19hSHSHkBbbwpSPiB6_9oLpAzXla3YvcQ")
        },
        License = new OpenApiLicense
        {
            Name = "Document",
            Url = new Uri("https://docs.google.com/document/d/1VUlc_1GzPz6BdAvtF8bId2pfDTvfxX-A/edit")
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
