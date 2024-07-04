using System.Diagnostics.CodeAnalysis;
using MediatR;
using SpotEight.Core.Application.UseCases.V1.User.GetAll.Handlers;
using SpotEight.Core.Domain.Interfaces.Repository;

namespace SpotEight.Core.Application.UseCases.V1.User.GetAll;

[ExcludeFromCodeCoverage]
public class GetAllUserHandler : IRequestHandler<GetAllRequest, GetAllUserResponse>
{
    private readonly IUserRepository _userRepository;

    public GetAllUserHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<GetAllUserResponse> Handle(GetAllRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var response = new GetAllUserResponse();

            var h1 = new GetAllHandler(_userRepository);

            await h1.Process(request, response);

            return response;
        }
        catch (Exception ex)
        {
            throw;
        }
    }
}