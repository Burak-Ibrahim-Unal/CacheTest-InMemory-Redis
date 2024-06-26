using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RedisExample.Cache;
using RedisExampleApi.Context;
using RedisExampleApi.Repository;
using RedisExampleApi.Services;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IProductService,ProductService>();
builder.Services.AddScoped<IProductRepository>(serviceProvider =>
{
    var appDbContext = serviceProvider.GetRequiredService<AppDbContext>();
    var productRepository = new ProductRepository(appDbContext);
    var redisRepository = serviceProvider.GetRequiredService<RedisRepository>();

    return new ProductRepositoryWithCacheDecorator(redisRepository, productRepository);
});

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseInMemoryDatabase("TestDatabase");
});

builder.Services.AddSingleton<RedisRepository>(serviceProvider =>
{
    return new RedisRepository(builder.Configuration["CacheOptions:Url"]);
});

builder.Services.AddSingleton<IDatabase>(serviceProvider =>
{
    var redisService = serviceProvider.GetRequiredService<RedisRepository>();

    return redisService.GetDb(0);
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbcontext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbcontext.Database.EnsureCreated();
}

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
