# DreamBig.Shortcuts.Results

[![NuGet](https://img.shields.io/nuget/v/DreamBig.Shortcuts.Results.svg)](https://www.nuget.org/packages/DreamBig.Shortcuts.Results/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/DreamBig.Shortcuts.Results.svg)](https://www.nuget.org/packages/DreamBig.Shortcuts.Results/)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

A lightweight, type-safe Result pattern implementation for .NET applications. This library helps you handle success and failure outcomes elegantly, making error handling more explicit and your code more maintainable.

## Features

- ✅ **Type-safe**: Generic `Result<T>` ensures compile-time type safety
- ✅ **Explicit error handling**: No more hidden exceptions or null checks
- ✅ **Flexible error information**: Support for error messages, exceptions, or both
- ✅ **HTTP status code support**: Optional status codes for web API scenarios
- ✅ **Immutable**: All properties are initialized once and cannot be modified
- ✅ **Railway-oriented programming**: Enables functional programming patterns
- ✅ **Zero dependencies**: No external packages required

## Installation

Install via NuGet Package Manager:

```bash
dotnet add package DreamBig.Shortcuts.Results
```

Or via Package Manager Console:

```powershell
Install-Package DreamBig.Shortcuts.Results
```

## Quick Start

### Creating a Successful Result

```csharp
using DreamBig.Shortcuts.Results;

// Simple success
var user = new User { Id = 1, Name = "John Doe" };
var result = Result<User>.Success(user);

// Success with HTTP status code
var result = Result<User>.Success(user, httpStatusCode: 200);
```

### Creating a Failure Result

```csharp
// Failure with error message
var result = Result<User>.Failure("User not found");

// Failure with error message and status code
var result = Result<User>.Failure("User not found", httpStatusCode: 404);

// Failure with exception
try
{
    // Some operation
}
catch (Exception ex)
{
    var result = Result<User>.Failure(ex, httpStatusCode: 500);
}

// Failure with both message and exception
var result = Result<User>.Failure(
    "Failed to retrieve user from database", 
    exception, 
    httpStatusCode: 500
);
```

### Checking and Using Results

```csharp
var result = GetUser(userId);

if (result.IsSuccess)
{
    Console.WriteLine($"User found: {result.Value.Name}");
}
else
{
    Console.WriteLine($"Error: {result.Error.Message}");
    if (result.Error.Exception != null)
    {
        Console.WriteLine($"Exception: {result.Error.Exception.Message}");
    }
}

// Or use IsFailure
if (result.IsFailure)
{
    // Handle error
}
```

## Real-World Examples

### Repository Pattern

```csharp
public class UserRepository
{
    public Result<User> GetById(int id)
    {
        try
        {
            var user = _dbContext.Users.Find(id);
            
            if (user == null)
            {
                return Result<User>.Failure("User not found", httpStatusCode: 404);
            }
            
            return Result<User>.Success(user, httpStatusCode: 200);
        }
        catch (Exception ex)
        {
            return Result<User>.Failure(
                "Database error occurred", 
                ex, 
                httpStatusCode: 500
            );
        }
    }
    
    public Result<User> Create(User user)
    {
        try
        {
            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();
            
            return Result<User>.Success(user, httpStatusCode: 201);
        }
        catch (DbUpdateException ex)
        {
            return Result<User>.Failure(
                "Failed to create user", 
                ex, 
                httpStatusCode: 500
            );
        }
    }
}
```

### ASP.NET Core Web API

```csharp
[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly UserRepository _repository;
    
    [HttpGet("{id}")]
    public IActionResult GetUser(int id)
    {
        var result = _repository.GetById(id);
        
        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }
        
        return result.StatusCode switch
        {
            404 => NotFound(new { error = result.Error.Message }),
            500 => StatusCode(500, new { error = result.Error.Message }),
            _ => BadRequest(new { error = result.Error.Message })
        };
    }
    
    [HttpPost]
    public IActionResult CreateUser([FromBody] User user)
    {
        var result = _repository.Create(user);
        
        return result.IsSuccess 
            ? CreatedAtAction(nameof(GetUser), new { id = result.Value.Id }, result.Value)
            : StatusCode(result.StatusCode ?? 500, new { error = result.Error.Message });
    }
}
```

### Service Layer

```csharp
public class UserService
{
    private readonly UserRepository _repository;
    private readonly IEmailService _emailService;
    
    public Result<User> RegisterUser(string email, string password)
    {
        // Validate input
        if (string.IsNullOrWhiteSpace(email))
        {
            return Result<User>.Failure("Email is required", httpStatusCode: 400);
        }
        
        try
        {
            // Check if user exists
            var existingUser = _repository.GetByEmail(email);
            if (existingUser.IsSuccess)
            {
                return Result<User>.Failure(
                    "User with this email already exists", 
                    httpStatusCode: 409
                );
            }
            
            // Create user
            var user = new User { Email = email, Password = HashPassword(password) };
            var createResult = _repository.Create(user);
            
            if (createResult.IsFailure)
            {
                return createResult;
            }
            
            // Send welcome email
            _emailService.SendWelcomeEmail(user.Email);
            
            return Result<User>.Success(createResult.Value, httpStatusCode: 201);
        }
        catch (Exception ex)
        {
            return Result<User>.Failure(
                "An error occurred during registration", 
                ex, 
                httpStatusCode: 500
            );
        }
    }
}
```

### Validation Scenarios

```csharp
public class ProductService
{
    public Result<Product> ValidateAndCreateProduct(ProductDto dto)
    {
        // Multiple validation checks
        if (string.IsNullOrWhiteSpace(dto.Name))
        {
            return Result<Product>.Failure("Product name is required", httpStatusCode: 400);
        }
        
        if (dto.Price <= 0)
        {
            return Result<Product>.Failure("Product price must be greater than zero", httpStatusCode: 400);
        }
        
        if (dto.Price > 10000)
        {
            return Result<Product>.Failure("Product price exceeds maximum allowed", httpStatusCode: 400);
        }
        
        var product = new Product
        {
            Name = dto.Name,
            Price = dto.Price
        };
        
        return Result<Product>.Success(product, httpStatusCode: 200);
    }
}
```

## API Reference

### Result<T> Class

#### Properties

- `bool IsSuccess` - Indicates whether the operation was successful
- `bool IsFailure` - Indicates whether the operation failed (opposite of IsSuccess)
- `T? Value` - The value returned by a successful operation (null for failures)
- `ErrorInfo? Error` - Error information for failed operations (null for successes)
- `int? StatusCode` - Optional HTTP status code

#### Factory Methods

**Success Methods:**
```csharp
Result<T>.Success(T value)
Result<T>.Success(T value, int? httpStatusCode)
```

**Failure Methods:**
```csharp
Result<T>.Failure(string error)
Result<T>.Failure(string error, int? httpStatusCode)
Result<T>.Failure(Exception exception)
Result<T>.Failure(Exception exception, int? httpStatusCode)
Result<T>.Failure(string message, Exception exception)
Result<T>.Failure(string message, Exception exception, int? httpStatusCode)
```

### ErrorInfo Class

#### Properties

- `string? Message` - Human-readable error message
- `Exception? Exception` - The exception that caused the failure

#### Implicit Conversion

ErrorInfo can be implicitly converted to Result<T>:

```csharp
var errorInfo = new Result<User>.ErrorInfo
{
    Message = "Something went wrong",
    Exception = new InvalidOperationException()
};

Result<User> result = errorInfo; // Implicit conversion
```

## Design Decisions

### Why Generic Constraint `where T : class`?

The generic constraint requires `T` to be a reference type. This ensures:
- Consistent null semantics for the `Value` property
- Clear distinction between "no value" (null) and valid values
- Simpler API without dealing with nullable value types

### Why Immutable Properties?

All properties use `init` setters, making Result objects immutable after creation. This:
- Prevents accidental modification after creation
- Makes the code more predictable and thread-safe
- Aligns with functional programming principles

### Why Private Constructor?

The private constructor with factory methods:
- Enforces invariants (e.g., failures must have errors)
- Provides a clear, expressive API
- Prevents creation of invalid Result objects

## Common Patterns

### Pattern Matching (C# 8.0+)

```csharp
var message = result switch
{
    { IsSuccess: true } => $"Success: {result.Value.Name}",
    { Error.Exception: not null } => $"Error: {result.Error.Exception.Message}",
    _ => $"Error: {result.Error.Message}"
};
```

### Chaining Operations

```csharp
public Result<Order> PlaceOrder(int userId, OrderDto orderDto)
{
    var userResult = _userRepository.GetById(userId);
    if (userResult.IsFailure)
        return Result<Order>.Failure(userResult.Error.Message, userResult.StatusCode);
    
    var validationResult = ValidateOrder(orderDto);
    if (validationResult.IsFailure)
        return validationResult;
    
    var orderResult = _orderRepository.Create(validationResult.Value);
    return orderResult;
}
```

## Target Framework

- .NET 9.0

Compatible with .NET 9.0 and later versions.

## Contributing

Contributions are welcome! Please feel free to submit issues or pull requests.

## License

This project is licensed under the MIT License - see the LICENSE file for details.

## Support

If you encounter any issues or have questions:
- Open an issue on GitHub
- Check existing issues for solutions
- Review the comprehensive test suite for usage examples
