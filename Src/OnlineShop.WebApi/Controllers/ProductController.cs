using Microsoft.AspNetCore.Mvc;
using OnlineShop.Application.ApplicationServices.ProductServices.Commands;
using OnlineShop.Application.ApplicationServices.ProductServices.Queries;
using OnlineShop.Application.ApplicationServices.ProductServices.Responses;
using OnlineShop.Application.Common;
using OnlineShop.WebApi.Common;
using OnlineShop.WebApi.Exceptions;

namespace OnlineShop.WebApi.Controllers;

public class ProductController : AppControllerBase
{
    [HttpGet]
    public async Task<ActionResult<PaginationList<ProductDto>>> Get([FromQuery] GetAllProductsQuery query)
    {
        var result = await MediatorSender.Send(query);

        return Ok(result);
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<ProductDto>> GetById(int id)
    {
        return  Ok(await MediatorSender.Send(new GetProductByIdQuery()
        {
            ProductId = id
        }));
    }

    [HttpPost]
    public async Task<ActionResult<ProductDto>> Post([FromForm] CreateProductCommand command)
    {
        var dto = await MediatorSender.Send(command);
        return CreatedAtAction(nameof(GetById), new {Id = dto.Id}, dto);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Put(long id, [FromBody] UpdateProductCommand command)
    {
        if (id != command.ProductId)
        {
            throw new IdNoMatchDtoException();
        }

        await MediatorSender.Send(command);

        return NoContent();
    }

    [HttpPatch("{id}")]
    public async Task<ActionResult> Patch(long id, [FromForm] ChangeProductImageCommand command)
    {
        if (id != command.ProductId)
        {
            throw new IdNoMatchDtoException();
        }

        await MediatorSender.Send(command);

        return NoContent();
    }
    
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(long id)
    {
        await MediatorSender.Send(new DeleteProductCommand()
        {
            ProductId = id
        });

        return NoContent();
    }
}