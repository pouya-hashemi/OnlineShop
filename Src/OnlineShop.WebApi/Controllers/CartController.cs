using Microsoft.AspNetCore.Mvc;
using OnlineShop.Application.ApplicationServices.CartServices.Commands;
using OnlineShop.Application.ApplicationServices.CartServices.Queries;
using OnlineShop.Application.ApplicationServices.CartServices.Responses;
using OnlineShop.Application.Common;
using OnlineShop.WebApi.Common;

namespace OnlineShop.WebApi.Controllers;

public class CartController:AppControllerBase
{
    [HttpGet]
    public async Task<ActionResult<PaginationList<CartDto>>> Get([FromQuery] GetCartsByTokenQuery query)
    {
        var result = await MediatorSender.Send(query);

        return Ok(result);
    }
    [HttpGet("{id}")]
    public async Task<ActionResult<PaginationList<CartDto>>> GetById(long id)
    {
        var result = await MediatorSender.Send(new GetCartByIdQuery(){CartId=id});

        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<CartDto>> Post([FromBody] CreateCartCommand command)
    {
        var dto = await MediatorSender.Send(command);

        return CreatedAtAction(nameof(GetById), new {id = dto.Id}, dto);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(long id)
    {
        await MediatorSender.Send(new DeleteCartCommand() {CartId = id});
        
        return NoContent();
    }
}