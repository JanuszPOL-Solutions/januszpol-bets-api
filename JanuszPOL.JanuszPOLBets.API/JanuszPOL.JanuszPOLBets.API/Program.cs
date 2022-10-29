using JanuszPOL.JanuszPOLBets.API.Extensions;
using JanuszPOL.JanuszPOLBets.Data._DbContext;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options =>
{
    options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.RegisterDataContext(builder.Configuration);
builder.Services.RegisterRepositories();
builder.Services.RegisterApplicationServices();
builder.Services.AddIdentityServices(builder.Configuration);

const string AllowAll = "AllowAll";
builder.Services.AddCors(options =>
{
    options.AddPolicy(AllowAll, policy =>
    {
        policy.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

using (var scope = app.Services.CreateScope())
{
    var dataContext = scope.ServiceProvider.GetRequiredService<DataContext>();
    dataContext.Database.Migrate();
}

app.UseCors(AllowAll);

if (app.Environment.IsDevelopment())
{
    // for prod this will be handled by the nginx reverse proxy
    app.UseHttpsRedirection();
}

// Authentication & Authorization
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();