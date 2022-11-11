
using DataAccessLayer.Entity;
using DataAccessLayer.Repository;
using DataAccessLayer.Repository.RepositoryImplementation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WebApplication3.Mappers.MapperImplementation;
using WebApplication3.Mappers.MapperInterface;
using DataAccessLayer.Contexts;
using DataAccessLayer.Repository.RepositoryInterfaces;
using WebApplication3.UserViewRequestsModel;
using Serilog;

// TODO: 
/// <summary>
/// products sections
/// filters
/// shopping basket
/// personal area
/// purchase history
/// validation
/// add ability to administrator to block 
/// </summary>

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.

builder.Host.UseSerilog((hostingContext, loggerConfiguration) =>
loggerConfiguration.ReadFrom.Configuration(hostingContext.Configuration));
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



builder.Services.AddMvc();
builder.Services.AddRazorPages();
builder.Services.AddDbContext<Context>(options => options.UseSqlServer(builder.Configuration["ConnectionStrings:DefaultConnection"],
x => x.MigrationsAssembly(typeof(Context).Assembly.GetName().Name)));
builder.Services.AddTransient<IProductRepository, ProductRepository>();
builder.Services.AddTransient<ICategoryRepository, CategoryRepository>();
builder.Services.AddTransient<IMapper<Category, CategoryViewRequestModel>, CategoryMapper>();
builder.Services.AddTransient<IMapper<Product, ProductViewRequestModel>, ProductMapper>();

builder.Services.AddIdentityCore<IdentityUser>(options =>
{
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireDigit = false;
    options.Password.RequireUppercase = false;
})
.AddEntityFrameworkStores<Context>()
.AddDefaultTokenProviders();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["Jwt:Audience"],
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))

        };
    });

builder.Services.AddAuthorization();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapRazorPages();
});

app.Run();
