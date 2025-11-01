using Shouldly;

namespace DreamBig.Shortcuts.Results.Tests;

public sealed class ResultTests
{
    [Fact(DisplayName = "Creating a successful result with a value should set IsSuccess to true")]
    public void Success_WithValue_ShouldSetIsSuccessToTrue()
    {
        // Arrange
        var testValue = new TestClass { Name = "Test" };

        // Act
        var result = Result<TestClass>.Success(testValue);

        // Assert
        result.IsSuccess.ShouldBeTrue();
    }

    [Fact(DisplayName = "Creating a successful result with a value should set IsFailure to false")]
    public void Success_WithValue_ShouldSetIsFailureToFalse()
    {
        // Arrange
        var testValue = new TestClass { Name = "Test" };

        // Act
        var result = Result<TestClass>.Success(testValue);

        // Assert
        result.IsFailure.ShouldBeFalse();
    }

    [Fact(DisplayName = "Creating a successful result should store the provided value")]
    public void Success_WithValue_ShouldStoreTheProvidedValue()
    {
        // Arrange
        var testValue = new TestClass { Name = "Test" };

        // Act
        var result = Result<TestClass>.Success(testValue);

        // Assert
        result.Value.ShouldBe(testValue);
        result.Value?.Name.ShouldBe("Test");
    }

    [Fact(DisplayName = "Creating a successful result should set Error to null")]
    public void Success_WithValue_ShouldSetErrorToNull()
    {
        // Arrange
        var testValue = new TestClass { Name = "Test" };

        // Act
        var result = Result<TestClass>.Success(testValue);

        // Assert
        result.Error.ShouldBeNull();
    }

    [Fact(DisplayName = "Creating a successful result without status code should set StatusCode to null")]
    public void Success_WithoutStatusCode_ShouldSetStatusCodeToNull()
    {
        // Arrange
        var testValue = new TestClass { Name = "Test" };

        // Act
        var result = Result<TestClass>.Success(testValue);

        // Assert
        result.StatusCode.ShouldBeNull();
    }

    [Fact(DisplayName = "Creating a successful result with status code should store the status code")]
    public void Success_WithStatusCode_ShouldStoreTheStatusCode()
    {
        // Arrange
        var testValue = new TestClass { Name = "Test" };
        const int expectedStatusCode = 200;

        // Act
        var result = Result<TestClass>.Success(testValue, expectedStatusCode);

        // Assert
        result.StatusCode.ShouldBe(expectedStatusCode);
    }

    [Fact(DisplayName = "Creating a failure result with error message should set IsSuccess to false")]
    public void Failure_WithErrorMessage_ShouldSetIsSuccessToFalse()
    {
        // Arrange
        const string errorMessage = "Something went wrong";

        // Act
        var result = Result<TestClass>.Failure(errorMessage);

        // Assert
        result.IsSuccess.ShouldBeFalse();
    }

    [Fact(DisplayName = "Creating a failure result with error message should set IsFailure to true")]
    public void Failure_WithErrorMessage_ShouldSetIsFailureToTrue()
    {
        // Arrange
        const string errorMessage = "Something went wrong";

        // Act
        var result = Result<TestClass>.Failure(errorMessage);

        // Assert
        result.IsFailure.ShouldBeTrue();
    }

    [Fact(DisplayName = "Creating a failure result with error message should set Value to null")]
    public void Failure_WithErrorMessage_ShouldSetValueToNull()
    {
        // Arrange
        const string errorMessage = "Something went wrong";

        // Act
        var result = Result<TestClass>.Failure(errorMessage);

        // Assert
        result.Value.ShouldBeNull();
    }

    [Fact(DisplayName = "Creating a failure result should store the error message")]
    public void Failure_WithErrorMessage_ShouldStoreTheErrorMessage()
    {
        // Arrange
        const string errorMessage = "Something went wrong";

        // Act
        var result = Result<TestClass>.Failure(errorMessage);

        // Assert
        result.Error.ShouldNotBeNull();
        result.Error.Message.ShouldBe(errorMessage);
    }

    [Fact(DisplayName = "Creating a failure result with error message should set Exception in Error to null")]
    public void Failure_WithErrorMessage_ShouldSetExceptionToNull()
    {
        // Arrange
        const string errorMessage = "Something went wrong";

        // Act
        var result = Result<TestClass>.Failure(errorMessage);

        // Assert
        result.Error.ShouldNotBeNull();
        result.Error.Exception.ShouldBeNull();
    }

    [Fact(DisplayName = "Creating a failure result with error message and status code should store the status code")]
    public void Failure_WithErrorMessageAndStatusCode_ShouldStoreTheStatusCode()
    {
        // Arrange
        const string errorMessage = "Not found";
        const int expectedStatusCode = 404;

        // Act
        var result = Result<TestClass>.Failure(errorMessage, expectedStatusCode);

        // Assert
        result.StatusCode.ShouldBe(expectedStatusCode);
    }

    [Fact(DisplayName = "Creating a failure result with exception should set IsSuccess to false")]
    public void Failure_WithException_ShouldSetIsSuccessToFalse()
    {
        // Arrange
        var exception = new InvalidOperationException("Test exception");

        // Act
        var result = Result<TestClass>.Failure(exception);

        // Assert
        result.IsSuccess.ShouldBeFalse();
    }

    [Fact(DisplayName = "Creating a failure result with exception should set IsFailure to true")]
    public void Failure_WithException_ShouldSetIsFailureToTrue()
    {
        // Arrange
        var exception = new InvalidOperationException("Test exception");

        // Act
        var result = Result<TestClass>.Failure(exception);

        // Assert
        result.IsFailure.ShouldBeTrue();
    }

    [Fact(DisplayName = "Creating a failure result with exception should store the exception")]
    public void Failure_WithException_ShouldStoreTheException()
    {
        // Arrange
        var exception = new InvalidOperationException("Test exception");

        // Act
        var result = Result<TestClass>.Failure(exception);

        // Assert
        result.Error.ShouldNotBeNull();
        result.Error.Exception.ShouldBe(exception);
    }

    [Fact(DisplayName = "Creating a failure result with exception should set Message in Error to null")]
    public void Failure_WithException_ShouldSetMessageToNull()
    {
        // Arrange
        var exception = new InvalidOperationException("Test exception");

        // Act
        var result = Result<TestClass>.Failure(exception);

        // Assert
        result.Error.ShouldNotBeNull();
        result.Error.Message.ShouldBeNull();
    }

    [Fact(DisplayName = "Creating a failure result with exception and status code should store the status code")]
    public void Failure_WithExceptionAndStatusCode_ShouldStoreTheStatusCode()
    {
        // Arrange
        var exception = new InvalidOperationException("Test exception");
        const int expectedStatusCode = 500;

        // Act
        var result = Result<TestClass>.Failure(exception, expectedStatusCode);

        // Assert
        result.StatusCode.ShouldBe(expectedStatusCode);
    }

    [Fact(DisplayName = "Creating a failure result with message and exception should store both")]
    public void Failure_WithMessageAndException_ShouldStoreBoth()
    {
        // Arrange
        const string errorMessage = "An error occurred";
        var exception = new InvalidOperationException("Test exception");

        // Act
        var result = Result<TestClass>.Failure(errorMessage, exception);

        // Assert
        result.Error.ShouldNotBeNull();
        result.Error.Message.ShouldBe(errorMessage);
        result.Error.Exception.ShouldBe(exception);
    }

    [Fact(DisplayName = "Creating a failure result with message and exception should set IsFailure to true")]
    public void Failure_WithMessageAndException_ShouldSetIsFailureToTrue()
    {
        // Arrange
        const string errorMessage = "An error occurred";
        var exception = new InvalidOperationException("Test exception");

        // Act
        var result = Result<TestClass>.Failure(errorMessage, exception);

        // Assert
        result.IsFailure.ShouldBeTrue();
    }

    [Fact(DisplayName = "Creating a failure result with message, exception and status code should store all values")]
    public void Failure_WithMessageExceptionAndStatusCode_ShouldStoreAllValues()
    {
        // Arrange
        const string errorMessage = "Server error";
        var exception = new InvalidOperationException("Test exception");
        const int expectedStatusCode = 500;

        // Act
        var result = Result<TestClass>.Failure(errorMessage, exception, expectedStatusCode);

        // Assert
        result.Error.ShouldNotBeNull();
        result.Error.Message.ShouldBe(errorMessage);
        result.Error.Exception.ShouldBe(exception);
        result.StatusCode.ShouldBe(expectedStatusCode);
    }

    [Fact(DisplayName = "Creating a failure result with empty string should still create a valid failure result")]
    public void Failure_WithEmptyString_ShouldCreateValidFailureResult()
    {
        // Arrange
        const string emptyMessage = "";

        // Act
        var result = Result<TestClass>.Failure(emptyMessage);

        // Assert
        result.IsFailure.ShouldBeTrue();
        result.Error.ShouldNotBeNull();
        result.Error.Message.ShouldBe(emptyMessage);
    }

    [Fact(DisplayName = "Creating a failure result with whitespace should store the whitespace as message")]
    public void Failure_WithWhitespace_ShouldStoreWhitespaceAsMessage()
    {
        // Arrange
        const string whitespaceMessage = "   ";

        // Act
        var result = Result<TestClass>.Failure(whitespaceMessage);

        // Assert
        result.IsFailure.ShouldBeTrue();
        result.Error.ShouldNotBeNull();
        result.Error.Message.ShouldBe(whitespaceMessage);
    }

    [Fact(DisplayName = "Creating a successful result with status code zero should store zero")]
    public void Success_WithStatusCodeZero_ShouldStoreZero()
    {
        // Arrange
        var testValue = new TestClass { Name = "Test" };
        const int statusCode = 0;

        // Act
        var result = Result<TestClass>.Success(testValue, statusCode);

        // Assert
        result.StatusCode.ShouldBe(statusCode);
    }

    [Fact(DisplayName = "Creating a failure result with negative status code should store negative value")]
    public void Failure_WithNegativeStatusCode_ShouldStoreNegativeValue()
    {
        // Arrange
        const string errorMessage = "Error";
        const int negativeStatusCode = -1;

        // Act
        var result = Result<TestClass>.Failure(errorMessage, negativeStatusCode);

        // Assert
        result.StatusCode.ShouldBe(negativeStatusCode);
    }

    [Fact(DisplayName = "Converting ErrorInfo with message and exception should create failure result")]
    public void ErrorInfo_ImplicitConversion_WithMessageAndException_ShouldCreateFailureResult()
    {
        // Arrange
        var errorInfo = new ErrorInfo
        {
            Message = "Test error",
            Exception = new InvalidOperationException("Test exception")
        };

        // Act
        Result<TestClass> result = errorInfo;

        // Assert
        result.IsFailure.ShouldBeTrue();
        result.Error.ShouldNotBeNull();
        result.Error.Message.ShouldBe("Test error");
        result.Error.Exception.ShouldNotBeNull();
    }

    [Fact(DisplayName = "Converting ErrorInfo with only message should create failure result with exception")]
    public void ErrorInfo_ImplicitConversion_WithOnlyMessage_ShouldCreateFailureResultWithException()
    {
        // Arrange
        var errorInfo = new ErrorInfo
        {
            Message = "Test error"
        };

        // Act
        Result<TestClass> result = errorInfo;

        // Assert
        result.IsFailure.ShouldBeTrue();
        result.Error.ShouldNotBeNull();
        result.Error.Message.ShouldBe("Test error");
        result.Error.Exception.ShouldNotBeNull();
        result.Error.Exception?.Message.ShouldBe("Unknown error");
    }

    [Fact(DisplayName = "Converting ErrorInfo with only exception should create failure result with empty message")]
    public void ErrorInfo_ImplicitConversion_WithOnlyException_ShouldCreateFailureResultWithEmptyMessage()
    {
        // Arrange
        var exception = new InvalidOperationException("Original exception");
        var errorInfo = new ErrorInfo
        {
            Exception = exception
        };

        // Act
        Result<TestClass> result = errorInfo;

        // Assert
        result.IsFailure.ShouldBeTrue();
        result.Error.ShouldNotBeNull();
        result.Error.Message.ShouldBe(string.Empty);
        result.Error.Exception.ShouldBe(exception);
    }

    [Fact(DisplayName = "Converting ErrorInfo with null values should create failure result with defaults")]
    public void ErrorInfo_ImplicitConversion_WithNullValues_ShouldCreateFailureResultWithDefaults()
    {
        // Arrange
        var errorInfo = new ErrorInfo();

        // Act
        Result<TestClass> result = errorInfo;

        // Assert
        result.IsFailure.ShouldBeTrue();
        result.Error.ShouldNotBeNull();
        result.Error.Message.ShouldBe(string.Empty);
        result.Error.Exception.ShouldNotBeNull();
        result.Error.Exception?.Message.ShouldBe("Unknown error");
    }

    [Fact(DisplayName = "IsFailure property should always return opposite of IsSuccess for successful result")]
    public void IsFailure_ForSuccessResult_ShouldReturnOppositeOfIsSuccess()
    {
        // Arrange
        var testValue = new TestClass { Name = "Test" };

        // Act
        var result = Result<TestClass>.Success(testValue);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.IsFailure.ShouldBe(!result.IsSuccess);
    }

    [Fact(DisplayName = "IsFailure property should always return opposite of IsSuccess for failure result")]
    public void IsFailure_ForFailureResult_ShouldReturnOppositeOfIsSuccess()
    {
        // Arrange
        const string errorMessage = "Error";

        // Act
        var result = Result<TestClass>.Failure(errorMessage);

        // Assert
        result.IsSuccess.ShouldBeFalse();
        result.IsFailure.ShouldBe(!result.IsSuccess);
    }

    [Fact(DisplayName = "Creating successful results with different types should work correctly")]
    public void Success_WithDifferentTypes_ShouldWorkCorrectly()
    {
        // Arrange & Act
        var stringResult = Result<TestClass>.Success(new TestClass { Name = "String Test" });
        var anotherTypeResult = Result<AnotherTestClass>.Success(new AnotherTestClass { Id = 123 });

        // Assert
        stringResult.IsSuccess.ShouldBeTrue();
        stringResult.Value?.Name.ShouldBe("String Test");

        anotherTypeResult.IsSuccess.ShouldBeTrue();
        anotherTypeResult.Value?.Id.ShouldBe(123);
    }

    [Fact(DisplayName = "Creating failure results with different types should work correctly")]
    public void Failure_WithDifferentTypes_ShouldWorkCorrectly()
    {
        // Arrange & Act
        var stringResult = Result<TestClass>.Failure("Error 1");
        var anotherTypeResult = Result<AnotherTestClass>.Failure("Error 2");

        // Assert
        stringResult.IsFailure.ShouldBeTrue();
        stringResult.Error?.Message.ShouldBe("Error 1");

        anotherTypeResult.IsFailure.ShouldBeTrue();
        anotherTypeResult.Error?.Message.ShouldBe("Error 2");
    }

    [Theory(DisplayName = "Creating successful results with common HTTP success status codes should work correctly")]
    [InlineData(200)] // OK
    [InlineData(201)] // Created
    [InlineData(202)] // Accepted
    [InlineData(204)] // No Content
    public void Success_WithCommonHttpSuccessCodes_ShouldStoreCorrectly(int statusCode)
    {
        // Arrange
        var testValue = new TestClass { Name = "Test" };

        // Act
        var result = Result<TestClass>.Success(testValue, statusCode);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.StatusCode.ShouldBe(statusCode);
    }

    [Theory(DisplayName = "Creating failure results with common HTTP error status codes should work correctly")]
    [InlineData(400)] // Bad Request
    [InlineData(401)] // Unauthorized
    [InlineData(403)] // Forbidden
    [InlineData(404)] // Not Found
    [InlineData(500)] // Internal Server Error
    [InlineData(503)] // Service Unavailable
    public void Failure_WithCommonHttpErrorCodes_ShouldStoreCorrectly(int statusCode)
    {
        // Arrange
        const string errorMessage = "HTTP Error";

        // Act
        var result = Result<TestClass>.Failure(errorMessage, statusCode);

        // Assert
        result.IsFailure.ShouldBeTrue();
        result.StatusCode.ShouldBe(statusCode);
    }

    [Fact(DisplayName = "Simulating a successful database query should return proper result")]
    public void RealWorld_SuccessfulDatabaseQuery_ShouldReturnProperResult()
    {
        // Arrange
        var dbRecord = new TestClass { Name = "John Doe" };

        // Act
        var result = Result<TestClass>.Success(dbRecord, 200);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldNotBeNull();
        result.Value.Name.ShouldBe("John Doe");
        result.Error.ShouldBeNull();
        result.StatusCode.ShouldBe(200);
    }

    [Fact(DisplayName = "Simulating a failed database query with exception should return proper result")]
    public void RealWorld_FailedDatabaseQuery_ShouldReturnProperResult()
    {
        // Arrange
        var dbException = new InvalidOperationException("Connection timeout");

        // Act
        var result = Result<TestClass>.Failure("Database operation failed", dbException, 500);

        // Assert
        result.IsFailure.ShouldBeTrue();
        result.Value.ShouldBeNull();
        result.Error.ShouldNotBeNull();
        result.Error.Message.ShouldBe("Database operation failed");
        result.Error.Exception.ShouldBe(dbException);
        result.StatusCode.ShouldBe(500);
    }

    [Fact(DisplayName = "Simulating a not found scenario should return proper result")]
    public void RealWorld_NotFoundScenario_ShouldReturnProperResult()
    {
        // Act
        var result = Result<TestClass>.Failure("Record not found", 404);

        // Assert
        result.IsFailure.ShouldBeTrue();
        result.Value.ShouldBeNull();
        result.Error.ShouldNotBeNull();
        result.Error.Message.ShouldBe("Record not found");
        result.StatusCode.ShouldBe(404);
    }

    [Fact(DisplayName = "Simulating an unauthorized access scenario should return proper result")]
    public void RealWorld_UnauthorizedAccess_ShouldReturnProperResult()
    {
        // Act
        var result = Result<TestClass>.Failure("Access denied", 401);

        // Assert
        result.IsFailure.ShouldBeTrue();
        result.Error.ShouldNotBeNull();
        result.Error.Message.ShouldBe("Access denied");
        result.StatusCode.ShouldBe(401);
    }

    [Fact(DisplayName = "Creating Result with success true and non-null error should throw ArgumentException")]
    public void Constructor_WithSuccessTrueAndError_ShouldThrowArgumentException()
    {
        // Arrange
        var errorInfo = new ErrorInfo { Message = "Error" };
        var testValue = new TestClass { Name = "Test" };

        // Act & Assert
        var exception = Should.Throw<System.Reflection.TargetInvocationException>(() =>
        {
            var constructor = typeof(Result<TestClass>)
                .GetConstructor(
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance,
                    null,
                    _paramTypes,
                    null);

            constructor?.Invoke([true, testValue, errorInfo, null]);
        });

        exception.InnerException.ShouldBeOfType<ArgumentException>();
        exception.InnerException?.Message.ShouldBe("A successful result cannot have an error.");
    }

    [Fact(DisplayName = "Creating Result with success false and null error should throw ArgumentException")]
    public void Constructor_WithSuccessFalseAndNullError_ShouldThrowArgumentException()
    {
        // Arrange
        var testValue = new TestClass { Name = "Test" };

        // Act & Assert
        var exception = Should.Throw<System.Reflection.TargetInvocationException>(() =>
        {
            var constructor = typeof(Result<TestClass>)
                .GetConstructor(
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance,
                    null,
                    _paramTypes,
                    null);

            constructor?.Invoke([false, testValue, null, null]);
        });

        exception.InnerException.ShouldBeOfType<ArgumentException>();
        exception.InnerException?.Message.ShouldBe("A failure result must have an error.");
    }

    private readonly Type[] _paramTypes = [typeof(bool), typeof(TestClass), typeof(ErrorInfo), typeof(int?)];

    private sealed class TestClass
    {
        public string? Name { get; set; }
    }

    private sealed class AnotherTestClass
    {
        public int Id { get; set; }
    }
}
