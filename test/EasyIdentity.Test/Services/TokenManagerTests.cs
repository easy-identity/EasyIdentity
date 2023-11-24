using System.Security.Claims;
using EasyIdentity.Models;
using EasyIdentity.Options;
using EasyIdentity.Services;
using EasyIdentity.Stores;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace EasyIdentity.Test.Services;

public class TokenManagerTests : TestBase
{
    private readonly MockRepository _mockRepository;

    private readonly Mock<ITokenGenerator> _mockTokenGenerator;
    private readonly Mock<ITokenStore> _mockTokenStore;
    private readonly Mock<IOptions<EasyIdentityOptions>> _mockOptions;
    private readonly Mock<IIssuerUriProvider> _mockIssuerUriProvider;
    private readonly Mock<ILogger<TokenManager>> _mockLogger;

    public TokenManagerTests()
    {
        _mockRepository = new MockRepository(MockBehavior.Strict);

        _mockTokenGenerator = _mockRepository.Create<ITokenGenerator>();
        _mockTokenStore = _mockRepository.Create<ITokenStore>();
        _mockOptions = _mockRepository.Create<IOptions<EasyIdentityOptions>>();
        _mockIssuerUriProvider = _mockRepository.Create<IIssuerUriProvider>();
        _mockLogger = _mockRepository.Create<ILogger<TokenManager>>();
    }

    //private TokenManager CreateManager()
    //{
    //    return new TokenManager(
    //        _mockTokenGenerator.Object,
    //        _mockTokenStore.Object,
    //        _mockOptions.Object,
    //        _mockIssuerUriProvider.Object,
    //        _mockLogger.Object);
    //}

    //[Fact]
    //public async Task GenerateAsync_StateUnderTest_ExpectedBehavior()
    //{
    //    // Arrange
    //    var manager = CreateManager();
    //    Client client = GetClient();
    //    ClaimsPrincipal claimsPrincipal = GetClaimsPrincipal();
    //    string[]? scopes = ["scope-1"];
    //    IRequestCollection requestData = new IRequestCollection();
    //    const bool identityToken = false;
    //    const bool refreshToken = false;
    //    CancellationToken cancellationToken = default(global::System.Threading.CancellationToken);

    //    // Act
    //    var result = await manager.GenerateAsync(
    //        client,
    //        claimsPrincipal,
    //        scopes,
    //        requestData,
    //        identityToken,
    //        refreshToken,
    //        cancellationToken);

    //    // Assert
    //    Assert.True(false);
    //    _mockRepository.VerifyAll();
    //}

    //[Fact]
    //public async Task CreateTokenAsync_StateUnderTest_ExpectedBehavior()
    //{
    //    // Arrange
    //    var manager = CreateManager();
    //    Client client = GetClient();
    //    ClaimsPrincipal claimsPrincipal = GetClaimsPrincipal();
    //    string[]? scopes = ["scope-1"];
    //    const string tokenType = "my-type";
    //    CancellationToken cancellationToken = default(global::System.Threading.CancellationToken);

    //    // Act
    //    var result = await manager.CreateTokenAsync(
    //        client,
    //        tokenType,
    //        scopes,
    //        claimsPrincipal,
    //        cancellationToken);

    //    // Assert
    //    Assert.True(false);
    //    _mockRepository.VerifyAll();
    //}

    //[Fact]
    //public async Task ValidateAccessTokenAsync_StateUnderTest_ExpectedBehavior()
    //{
    //    // Arrange
    //    var manager = CreateManager();
    //    Client? client = null;
    //    const string? accessToken = null;
    //    CancellationToken cancellationToken = default(global::System.Threading.CancellationToken);

    //    // Act
    //    var result = await manager.ValidateAccessTokenAsync(
    //        client!,
    //        accessToken!,
    //        cancellationToken);

    //    // Assert
    //    Assert.True(false);
    //    _mockRepository.VerifyAll();
    //}

    //[Fact]
    //public async Task ValidateRefreshTokenAsync_StateUnderTest_ExpectedBehavior()
    //{
    //    // Arrange
    //    var manager = CreateManager();
    //    Client? client = null;
    //    const string? refreshToken = null;
    //    CancellationToken cancellationToken = default(global::System.Threading.CancellationToken);

    //    // Act
    //    var result = await manager.ValidateRefreshTokenAsync(
    //        client!,
    //        refreshToken!,
    //        cancellationToken);

    //    // Assert
    //    Assert.True(false);
    //    _mockRepository.VerifyAll();
    //}
}
