using EasyIdentity.Models;
using EasyIdentity.Options;
using EasyIdentity.Services;
using Microsoft.Extensions.Options;
using Moq;

namespace EasyIdentity.Test.Services;

public class JwtTokenGeneratorTests : TestBase
{
    private readonly MockRepository _mockRepository;

    private readonly Mock<IJsonSerializer> _mockJsonSerializer;
    private readonly Mock<ICredentialsManager> _mockCredentialsManager;
    private readonly Mock<IOptions<EasyIdentityOptions>> _mockOptions;

    public JwtTokenGeneratorTests()
    {
        _mockRepository = new MockRepository(MockBehavior.Strict);

        _mockJsonSerializer = _mockRepository.Create<IJsonSerializer>();
        _mockCredentialsManager = _mockRepository.Create<ICredentialsManager>();
        _mockOptions = _mockRepository.Create<IOptions<EasyIdentityOptions>>();
    }

    private JwtTokenGenerator CreateJwtTokenGenerator()
    {
        return new JwtTokenGenerator(
            _mockJsonSerializer.Object,
            _mockCredentialsManager.Object,
            _mockOptions.Object);
    }

    //[Fact]
    //public async Task CreateSecurityTokenAsync()
    //{
    //    // Arrange
    //    var jwtTokenGenerator = CreateJwtTokenGenerator();
    //    Client client = GetClient();
    //    TokenDescriber? tokenDescriber = null;
    //    CancellationToken cancellationToken = default(CancellationToken);

    //    // Act
    //    var result = await jwtTokenGenerator.CreateSecurityTokenAsync(
    //        client,
    //        tokenDescriber!,
    //        cancellationToken);

    //    // Assert
    //    Assert.True(result != null);

    //    _mockRepository.VerifyAll();
    //}

    //[Fact]
    //public async Task CreateRefreshTokenAsync_StateUnderTest_ExpectedBehavior()
    //{
    //    // Arrange
    //    var jwtTokenGenerator = CreateJwtTokenGenerator();
    //    Client client = GetClient();
    //    TokenDescriber? tokenDescriber = null;
    //    CancellationToken cancellationToken = default(CancellationToken);

    //    // Act
    //    var result = await jwtTokenGenerator.CreateRefreshTokenAsync(
    //        client!,
    //        tokenDescriber!,
    //        cancellationToken);

    //    // Assert
    //    Assert.True(false);
    //    _mockRepository.VerifyAll();
    //}
}
