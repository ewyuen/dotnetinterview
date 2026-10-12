using GraphQLServer.Data;
using GraphQLServer.GraphQL;

var builder = WebApplication.CreateBuilder(args);

// Register the repository as a singleton
builder.Services.AddSingleton<IBookRepository, BookRepository>();

// Add GraphQL server with Hot Chocolate
builder.Services
    .AddGraphQLServer()
    .AddQueryType<Query>();

var app = builder.Build();

// Map the GraphQL endpoint
app.MapGraphQL();

// Welcome message
app.MapGet("/", () => "GraphQL Server is running! Navigate to /graphql for Banana Cake Pop IDE.");

app.Run();
