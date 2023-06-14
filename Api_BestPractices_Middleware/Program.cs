using Microsoft.AspNetCore.Mvc.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.Use(async(context, next) =>
{
    Console.WriteLine("app.use mehtod, before- next delegate");
    await next.Invoke();
    Console.WriteLine("app.use mehtod,after- next delegate");
});

app.Map("/branchingviamap", builder =>
{
    builder.Use(async (context, next) =>
    {
        Console.WriteLine("builder.use method, before next delegate, branching via map");
        await next.Invoke();
        Console.WriteLine("builder.use method, after next delegate, branching via map");

    });

    builder.Run(async context =>
    {
        Console.WriteLine("builder.run, response to client, branch via map");
        await context.Response.WriteAsync("now in a new branched middleware");
    }
    );
    
});

//terminal middleware, accepts only httpcontext parameter
app.Run(async context =>
{
    Console.WriteLine($"app.run, response to the client");
    context.Response.StatusCode = 200;
    await context.Response.WriteAsync("this is a sample utforing av middleware component !");
}
);

app.MapControllers();

app.Run();
