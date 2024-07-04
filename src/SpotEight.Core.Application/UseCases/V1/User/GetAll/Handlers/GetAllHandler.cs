using SpotEight.Core.Application.Common.Handlers;
using SpotEight.Core.Domain.Dtos.User;
using SpotEight.Core.Domain.Interfaces.Repository;

namespace SpotEight.Core.Application.UseCases.V1.User.GetAll.Handlers;

public class GetAllHandler : Handler<GetAllRequest, GetAllUserResponse, ListUserDto>
{
    private readonly IUserRepository _userRepository;

    public GetAllHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public override async Task Process(GetAllRequest request, GetAllUserResponse response)
    {
        var entities = await _userRepository.GetAll();

        if (!entities.Any())
        {
            response.HasError = true;
            response.Error = "Nenhum user cadastrado";
            return;
        }

        response.Data.Users.AddRange(entities.Select(x => new UserDto()
        {
            Id = x.Id,
            Name = x.Name,
            Description = x.Description
        }));
    }
}