using Microsoft.Extensions.DependencyInjection;


var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.talanlunch>("talanlunch");



builder.Build().Run();
