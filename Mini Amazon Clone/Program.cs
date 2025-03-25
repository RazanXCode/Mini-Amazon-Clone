using Dapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Mini_Amazon_Clone.Data;
using System;
using System.Data;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Add Database service
builder.Services.AddDbContext<MyAppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


// Dapper support
builder.Services.AddScoped<IDbConnection>(db => new SqlConnection(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Load JWT settings
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = Encoding.UTF8.GetBytes(jwtSettings["Secret"]);

// Add Authentication services
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(secretKey)
        };
    });


//Authorization Policies
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("CanViewOrdersPolicy", policy =>
        policy.RequireClaim("CanViewOrders", "true"));

    options.AddPolicy("CanRefundOrdersPolicy", policy =>
        policy.RequireRole("Admin").RequireClaim("CanRefundOrders", "true"));
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

// Call functions
//await FetchOrdersWithProducts(app.Services);
//await FetchCustomersOrders(app.Services, 2);
//await FetchProductById(app.Services, 1);

app.Run();

//------------------------------------------------------------------------//



// 1. Eager Loading Query to Fetch Orders with Their Products
async Task FetchOrdersWithProducts(IServiceProvider services)
{
    using (var scope = services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<MyAppDbContext>();

        var orders = await context.Orders
            .Include(o => o.OrderItems)  // Include OrderItems
                .ThenInclude(oi => oi.Product)  // Include Products in OrderItems
            .Include(o => o.User)  // Include User who placed the order
            .ToListAsync();

        Console.WriteLine("Orders with Products:");
        foreach (var order in orders)
        {
            Console.WriteLine($"Order ID: {order.OrderID}, User: {order.User.Name}, Total: {order.TotalAmount}, Status: {order.Status}");

            foreach (var item in order.OrderItems)
            {
                Console.WriteLine($"  - Product: {item.Product.Name}, Quantity: {item.Quantity}, Price: {item.Price}");
            }
        }
    }
}

// 2. Dapper Query to Fetch a Customer's Orders by Customer ID
async Task FetchCustomersOrders(IServiceProvider services, int customerId)
{
    using (var scope = services.CreateScope())
    {
        var dbConnection = scope.ServiceProvider.GetRequiredService<IDbConnection>();

        string query = @"SELECT o.OrderID, o.OrderDate, o.TotalAmount, o.Status, u.Name as UserName 
                         FROM Orders o
                         JOIN Users u ON o.UserID = u.UserID
                         WHERE u.UserID = @CustomerId";

        var orders = await dbConnection.QueryAsync(query, new { CustomerId = customerId });

        Console.WriteLine($"\nCustomer {customerId} Orders:");
        foreach (var order in orders)
        {
            Console.WriteLine($"Order ID: {order.OrderID}, Date: {order.OrderDate}, Total: {order.TotalAmount}, Status: {order.Status}");
        }
    }
}

// 3. Dapper Query to Fetch a Product by ID
async Task FetchProductById(IServiceProvider services, int productId)
{
    using (var scope = services.CreateScope())
    {
        var dbConnection = scope.ServiceProvider.GetRequiredService<IDbConnection>();

        string query = @"SELECT ProductID, Name, Description, Price, Stock
                         FROM Products
                         WHERE ProductID = @ProductId";

        var product = await dbConnection.QueryFirstOrDefaultAsync(query, new { ProductId = productId });

        if (product != null)
        {
            Console.WriteLine($"\nProduct ID: {product.ProductID}, Name: {product.Name}, Price: {product.Price}, Stock: {product.Stock}");
        }
        else
        {
            Console.WriteLine("\nProduct not found.");
        }
    }
}



//------------------------------------------------------------------------//
