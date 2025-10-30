var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.E_Commerce_Shop_Api>("Api");
builder.AddProject<Projects.EcommerceUI>("UI");

builder.Build().Run();
