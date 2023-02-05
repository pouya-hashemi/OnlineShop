using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Application.ApplicationServices.RoleServices.Commands;
using OnlineShop.Application.ApplicationServices.RoleServices.Queries;
using OnlineShop.Application.ApplicationServices.RoleServices.Responses;
using OnlineShop.Application.Common;
using OnlineShop.WebApi.Common;
using OnlineShop.WebApi.Exceptions;

namespace OnlineShop.WebApi.Controllers;
[Authorize]
public class RoleController:AppControllerBase
{
    [HttpGet]
    public async Task<ActionResult<PaginationList<RoleDto>>> Get([FromQuery] GetAllRolesQuery query)
    {
        return  Ok(await MediatorSender.Send(query));
    }
    [HttpGet("{id}")]
    public async Task<ActionResult<PaginationList<RoleDto>>> GetById(int id)
    {
        return  Ok(await MediatorSender.Send(new GetRoleByIdQuery()
        {
            RoleId = id
        }));
    }

    [HttpPost]
    public async Task<ActionResult<RoleDto>> Post([FromBody] CreateRoleCommand command)
    {
        var dto = await MediatorSender.Send(command);
        return CreatedAtAction(nameof(GetById), new {Id=dto.Id}, dto);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Put(long id, [FromBody] UpdateRoleCommand command)
    {
        if (id!=command.RoleId)
        {
            throw new IdNoMatchDtoException();
        }

        await MediatorSender.Send(command);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        await MediatorSender.Send(new DeleteRoleCommand()
        {
            RoleId = id
        });

        return NoContent();
    } 
}