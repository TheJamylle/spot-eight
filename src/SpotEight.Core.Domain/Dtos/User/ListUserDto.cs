namespace SpotEight.Core.Domain.Dtos.User;

public class ListUserDto
{
    public ListUserDto()
    {
        Users = new List<UserDto>();
    }

    public List<UserDto> Users { get; set; }
}