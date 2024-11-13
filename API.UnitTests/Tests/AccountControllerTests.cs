namespace API.UnitTests.Tests;

using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using API.Controllers;
using API.DTOs;
using API.UnitTests.Helpers;
using Newtonsoft.Json.Linq;
using Xunit;

public class AccountControllerTests
{
    private readonly string apiRoute = "api/account";
    private readonly HttpClient _client;
    private HttpResponseMessage httpResponse;
    private string requestUrl;
    private string registerObject;
    private string loginObject;
    private HttpContent httpContent;

    public AccountControllerTests()
    {
        _client = TestHelper.Instance.Client;
    }

    [Fact]
    public async Task RegisterShouldReturnBadRequestWhenUsernameAlreadyExists()
    {
        // Arrange
        var registerRequest = new RegisterRequest
        {
            Username = "perlita",
            Password = "123456"
        };

        registerObject = GetRegisterObject(registerRequest);
        httpContent = GetHttpContent(registerObject);
        requestUrl = $"{apiRoute}/register";

        // Act
        httpResponse = await _client.PostAsync(requestUrl, httpContent);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, httpResponse.StatusCode);
        var responseContent = await httpResponse.Content.ReadAsStringAsync();
        Assert.Contains("Username already in use", responseContent);
    }

    [Fact]
    public async Task RegisterShouldReturnOkWhenNewUserIsCreated()
    {
        // Arrange
        var registerRequest = new RegisterRequest
        {
            Username = "pablo",
            Password = "123456"
        };

        registerObject = GetRegisterObject(registerRequest);
        httpContent = GetHttpContent(registerObject);
        requestUrl = $"{apiRoute}/register";

        // Act
        httpResponse = await _client.PostAsync(requestUrl, httpContent);

        // Assert
        Assert.Equal(HttpStatusCode.OK, httpResponse.StatusCode);
    }

    [Fact]
    public async Task LoginShouldReturnUnauthorizedWhenUserDoesNotExist()
    {
        // Arrange
        var loginRequest = new LoginRequest
        {
            Username = "veronica",
            Password = "123456"
        };

        loginObject = GetLoginObject(loginRequest);
        httpContent = GetHttpContent(loginObject);
        requestUrl = $"{apiRoute}/login";

        // Act
        httpResponse = await _client.PostAsync(requestUrl, httpContent);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, httpResponse.StatusCode);
        var responseContent = await httpResponse.Content.ReadAsStringAsync();
        Assert.Contains("Invalid username or password", responseContent);
    }

    [Fact]
    public async Task LoginShouldReturnUnauthorizedWhenPasswordHashDoesNotMatch()
    {
        // Arrange
        var loginRequest = new LoginRequest
        {
            Username = "perlita",
            Password = "incorrectPassword" // A password that will trigger hash mismatch
        };

        loginObject = GetLoginObject(loginRequest);
        httpContent = GetHttpContent(loginObject);
        requestUrl = $"{apiRoute}/login";

        // Act
        httpResponse = await _client.PostAsync(requestUrl, httpContent);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, httpResponse.StatusCode);
        var responseContent = await httpResponse.Content.ReadAsStringAsync();
        Assert.Contains("Invalid username or password", responseContent);
}

    [Fact]
    public async Task LoginShouldReturnOkForValidCredentials()
    {
        // Arrange
        var loginRequest = new LoginRequest
        {
            Username = "patricio",
            Password = "123456"
        };

        loginObject = GetLoginObject(loginRequest);
        httpContent = GetHttpContent(loginObject);
        requestUrl = $"{apiRoute}/login";

        // Act
        httpResponse = await _client.PostAsync(requestUrl, httpContent);

        // Assert
        Assert.Equal(HttpStatusCode.OK, httpResponse.StatusCode);
        var responseContent = await httpResponse.Content.ReadAsStringAsync();
        var userResponse = JsonSerializer.Deserialize<UserResponse>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        Assert.NotNull(userResponse);
        Assert.Equal(loginRequest.Username, userResponse.Username);
        Assert.False(string.IsNullOrEmpty(userResponse.Token));
    }

    #region Private Methods

    private static string GetRegisterObject(RegisterRequest registerDto)
    {
        var entityObject = new JObject()
        {
            { nameof(registerDto.Username), registerDto.Username },
            { nameof(registerDto.Password), registerDto.Password }
        };

        return entityObject.ToString();
    }

    private static string GetLoginObject(LoginRequest loginDto)
    {
        var entityObject = new JObject()
        {
            { nameof(loginDto.Username), loginDto.Username },
            { nameof(loginDto.Password), loginDto.Password }
        };

        return entityObject.ToString();
    }

    private static StringContent GetHttpContent(string objectToCode) =>
        new(objectToCode, Encoding.UTF8, "application/json");

    #endregion
}

