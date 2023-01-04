using Microsoft.AspNetCore.Mvc;
using OnlineShop.Application.ApplicationServices.UserServices.Commands;
using OnlineShop.Application.ApplicationServices.UserServices.Queries;
using OnlineShop.Application.ApplicationServices.UserServices.Responses;
using OnlineShop.Application.Common;
using OnlineShop.WebApi.Common;
using OnlineShop.WebApi.Exceptions;

namespace OnlineShop.WebApi.Controllers;

public class UserController:AppControllerBase
{
    [HttpGet]
    public async Task<ActionResult<PaginationList<UserDto>>> Get([FromQuery] GetAllUsersQuery query)
    {
        return  Ok(await MediatorSender.Send(query));
    }
    [HttpGet("{id}")]
    public async Task<ActionResult<PaginationList<UserDto>>> GetById(long id)
    {
        return  Ok(await MediatorSender.Send(new GetUserByIdQuery()
        {
            UserId = id
        }));
    }

    [HttpPost]
    public async Task<ActionResult<UserDto>> Post([FromBody] CreateUserCommand command)
    {
        var userDto = await MediatorSender.Send(command);
        return CreatedAtAction(nameof(GetById), new {Id=userDto.Id}, userDto);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Put(long id, [FromBody] UpdateUserCommand command)
    {
        if (id!=command.UserId)
        {
            throw new IdNoMatchDtoException();
        }

        await MediatorSender.Send(command);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(long id)
    {
        await MediatorSender.Send(new DeleteUserCommand()
        {
            UserId = id
        });

        return NoContent();
    } 
}