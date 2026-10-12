# Lab 4.2: Async Web Requests

## Goal
Learn asynchronous HTTP operations in C# using HttpClient and understand proper web API interaction patterns.

## Key Concepts
- **HttpClient**: The primary class for sending HTTP requests and receiving HTTP responses
- **Async HTTP Operations**: Non-blocking web requests using `GetAsync()`, `PostAsync()`, etc.
- **JSON Handling**: Parsing and serializing JSON data asynchronously
- **Retry Logic**: Implementing exponential backoff for resilient web requests
- **Error Handling**: Proper handling of HTTP errors and network issues
- **Cancellation**: Supporting cancellation in web requests

## Implementation
This lab demonstrates essential async web request patterns:

1. **Basic HTTP Operations**: GET, POST, PUT, DELETE requests
2. **JSON Data Handling**: Serializing/deserializing JSON with System.Net.Http.Json
3. **Retry Logic**: Exponential backoff retry patterns for resilient requests
4. **Error Handling**: Proper HTTP status code and exception handling
5. **Cancellation Support**: Cancelling long-running web requests
6. **Concurrent Requests**: Making multiple web requests simultaneously

## Key Features Demonstrated
- `HttpClient.GetAsync()`, `PostAsync()`, `PutAsync()`, `DeleteAsync()`
- `ReadFromJsonAsync<T>()` and `PostAsJsonAsync<T>()`
- Exponential backoff retry logic with jitter
- `HttpRequestException` and status code handling
- `CancellationToken` support in HTTP operations
- `Task.WhenAll()` for concurrent web requests
- Proper `HttpClient` lifecycle management

## Expected Output
The program will show:
- Successful HTTP GET/POST operations with JSON data
- Retry logic demonstrations with simulated failures
- Error handling for various HTTP scenarios
- Concurrent request processing
- Cancellation of web operations

## Important Notes
- Always dispose of HttpClient properly or use IHttpClientFactory
- Handle common HTTP status codes appropriately
- Implement retry logic for transient failures
- Use appropriate timeouts for web requests
- Consider rate limiting and circuit breaker patterns
- Validate SSL certificates in production
- Use HttpClientFactory for dependency injection scenarios

## References
- [HttpClient Class](https://docs.microsoft.com/en-us/dotnet/api/system.net.http.httpclient)
- [Make HTTP requests with the HttpClient class](https://docs.microsoft.com/en-us/dotnet/csharp/tutorials/console-webapiclient)
- [Use IHttpClientFactory to implement resilient HTTP requests](https://docs.microsoft.com/en-us/dotnet/core/extensions/httpclient-factory)