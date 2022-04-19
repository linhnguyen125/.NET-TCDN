using MISA.Core.Interfaces;
using MISA.Core.Interfaces.Base;
using MISA.Core.Services;
using MISA.Infrastructure.Postgres.Repository;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
        builder =>
        {
            builder.WithOrigins("http://localhost:8080", "http://localhost:8081", "http://localhost:5278").WithMethods("GET", "POST", "PUT", "DELETE").AllowAnyHeader();
        });
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Xử lý về DI - Dependency Injection
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();

builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
builder.Services.AddScoped<IDepartmentService, DepartmentService>();

builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IAccountService, AccountService>();

builder.Services.AddScoped<IAccountObjectService, AccountObjectService>();
builder.Services.AddScoped<IAccountObjectRepository, AccountObjectRepository>();

builder.Services.AddScoped<ILayoutService, LayoutService>();
builder.Services.AddScoped<ILayoutRepository, LayoutRepository>();

builder.Services.AddScoped<ICaPaymentService, CaPaymentService>();
builder.Services.AddScoped<ICaPaymentRepository, CaPaymentRepository>();

builder.Services.AddScoped<ICaPaymentDetailService, CaPaymentDetailService>();
builder.Services.AddScoped<ICaPaymentDetailRepository, CaPaymentDetailRepository>();

builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
builder.Services.AddScoped(typeof(IBaseService<>), typeof(BaseService<>));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(MyAllowSpecificOrigins);

app.UseAuthorization();

app.MapControllers();

app.Run();
