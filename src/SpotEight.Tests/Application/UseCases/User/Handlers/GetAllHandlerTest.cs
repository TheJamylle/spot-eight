using FluentAssertions;
using Moq;
using SpotEight.Core.Application.UseCases.V1.User.GetAll;
using SpotEight.Core.Application.UseCases.V1.User.GetAll.Handlers;
using SpotEight.Core.Domain.Dtos.User;
using SpotEight.Core.Domain.Entities;
using SpotEight.Core.Domain.Interfaces.Repository;

namespace SpotEight.Tests.Application.UseCases.User.Handlers;

public class GetAllHandlerTest : BaseTest
{
    private readonly Mock<IUserRepository> _buRepository = new();
    private readonly GetAllHandler _handler;

    public GetAllHandlerTest()
    {
        _handler = new GetAllHandler(_buRepository.Object);
    }

    [Fact]
    public async Task GivenGetAllRequest_PopulateDataFromResponse()
    {
        //Arrange
        GetAllRequest request = new();
        GetAllUserResponse response = new();

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
        await _handler.Process(request, response);

        //Assert
        response.Data.Should().BeOfType<ListUserDto>();
        response.Data.Users.Should().HaveCount(2);
        response.HasError.Should().BeFalse();
        response.Error.Should().BeNullOrEmpty();
    }

    [Fact]
    public async Task GivenGetAllRequest_HasNoRegistries_DontPopulateData_HasErrorTrue()
    {
        //Arrange
        GetAllRequest request = new();
        GetAllUserResponse response = new();

        List<UserEntity> buEntities = new();

        _buRepository.Setup(x => x.GetAll()).ReturnsAsync(buEntities);

        //Act
        await _handler.Process(request, response);

        //Assert
        response.Data.Should().BeOfType<ListUserDto>();
        response.Data.Users.Should().HaveCount(0);
        response.HasError.Should().BeTrue();
        response.Error.Should().Be("Nenhum user cadastrado");
    }
}