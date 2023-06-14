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

app.MapWhen(context=> context.Request.Query.ContainsKey("mapwhen"), builder =>
{

    builder.Run(async context =>
    {
        Console.WriteLine("app.mapwhen/builder.run");
        await context.Response.WriteAsync("new brach pga query mapwhen");
    });

}
    
    
    
    );

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

/* 
 * branchingviamap
Content root path: C:\Users\SAGAWIN\source\repos\Api_BestPractices_Middleware\Api_BestPractices_Middleware\
app.use mehtod, before- next delegate
app.run, response to the client
app.use mehtod,after- next delegate
app.use mehtod, before- next delegate
app.run, response to the client
app.use mehtod,after- next delegate
app.use mehtod, before- next delegate
builder.use method, before next delegate, branching via map
builder.run, response to client, branch via map
builder.use method, after next delegate, branching via map
app.use mehtod,after- next delegate
app.use mehtod, before- next delegate
app.run, response to the client
app.use mehtod,after- next delegate
 */

/*
 * appwhen 
 * Content root path: C:\Users\SAGAWIN\source\repos\Api_BestPractices_Middleware\Api_BestPractices_Middleware\
app.use mehtod, before- next delegate
app.run, response to the client
app.use mehtod,after- next delegate
app.use mehtod, before- next delegate
app.run, response to the client
app.use mehtod,after- next delegate
app.use mehtod, before- next delegate
app.run, response to the client
app.use mehtod,after- next delegate
app.use mehtod, before- next delegate
app.run, response to the client
app.use mehtod,after- next delegate
app.use mehtod, before- next delegate
app.mapwhen/builder.run
app.use mehtod,after- next delegate
app.use mehtod, before- next delegate
app.run, response to the client
app.use mehtod,after- next delegate

 
 */