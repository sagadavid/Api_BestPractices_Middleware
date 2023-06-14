var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.Use(async(context, next) =>
{
    Console.WriteLine("before- next delegate- use method");
    await next.Invoke();
    Console.WriteLine("after- next delegate- use method");
});

//terminal middleware, accepts only httpcontext parameter
app.Run(async context =>
{
    Console.WriteLine($"response to the client- run method");
    context.Response.StatusCode = 200;
    await context.Response.WriteAsync("this is middleware component !");
}
);

app.MapControllers();

app.Run();
