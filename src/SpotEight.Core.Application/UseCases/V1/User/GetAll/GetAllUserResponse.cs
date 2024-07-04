using SpotEight.Core.Domain.Dtos.Response;
using SpotEight.Core.Domain.Dtos.User;

namespace SpotEight.Core.Application.UseCases.V1.User.GetAll;

public class GetAllUserResponse : ResponseBase<ListUserDto>
{
    public GetAllUserResponse()
    {
        Data = new ListUserDto();
    }
}