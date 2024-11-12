var builder = DistributedApplication.CreateBuilder(args);


var cache = builder.AddRedis("cache");


var chatwithslmservices = builder.AddProject<Projects.ChatWithSLM_API>("chatwithslmservices");


builder.AddProject<Projects.ChatWithSLM_UI>("chatwithslmui")
    .WithExternalHttpEndpoints()
    .WithReference(cache)
    .WithReference(chatwithslmservices);


builder.Build().Run();
