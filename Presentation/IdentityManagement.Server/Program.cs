using IdentityManagement.Infrastructure;
using IdentityManagement.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDataBaseConfiguration(builder.Configuration.GetConnectionString("DefaultConnection"));
builder.Services.AddIdentityServerConfig(builder.Configuration);
builder.Services.AddServices<AppUser>();

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddMvc(options =>
{
    options.EnableEndpointRouting = false;
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
{
    SeedData.EnsureSeedData(scope.ServiceProvider);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles();
app.UseIdentityServer();
app.UseMvc(routes =>
{
    routes.MapRoute(name: "Default", template: "{controller/{action}");
});

app.UseHttpsRedirection();

app.Run();
