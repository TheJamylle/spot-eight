using FluentAssertions;
using Moq;
using SpotEight.Core.Application.UseCases.V1.User.GetAll;
using SpotEight.Core.Domain.Dtos.User;
using SpotEight.Core.Domain.Entities;
using SpotEight.Core.Domain.Interfaces.Repository;

namespace SpotEight.Tests.Application.UseCases.User;

public class GetAllUserHandlerTest : BaseTest
{
    private readonly Mock<IUserRepository> _buRepository = new();
    private readonly GetAllUserHandler _handler;

    public GetAllUserHandlerTest()
    {
        _handler = new GetAllUserHandler(_buRepository.Object);
    }

    [Fact]
    public async Task GivenGetAllRequest_PopulateDataFromResponse()
    {
        //Arrange
        GetAllRequest request = new();

        List<UserEntity> buEntities = new()
        {
            new UserEntity()
            {
                Id = Guid.NewGuid(),
                Name = "Test 1",
                Description = "Description 1"
            },
            new UserEntity()
            {
                Id = Guid.NewGuid(),
                Name = "Test 2",
                Description = "Description 2"
            }
        };

        _buRepository.Setup(x => x.GetAll()).ReturnsAsync(buEntities);

        //Act
        var response = await _handler.Handle(request, CancellationToken.None);

        //Assert
        response.Data.Should().BeOfType<ListUserDto>();
        response.Data.Users.Should().HaveCount(2);
        response.HasError.Should().BeFalse();
        response.Error.Should().BeNullOrEmpty();
    }

    [Fact]
    public async Task GivenGetAllRequest_ThrowsException()
    {
        //Arrange
        GetAllRequest request = new();

        _buRepository.Setup(x => x.GetAll()).ThrowsAsync(new Exception());

        //Act
        var exception = await Assert.ThrowsAsync<Exception>(() => _handler.Handle(request, CancellationToken.None));

        //Assert
        exception.Should().BeOfType<Exception>();
    }
}