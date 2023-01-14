using Microsoft.AspNetCore.Mvc;
using OnlineShop.Application.ApplicationServices.CategoryServices.Commands;
using OnlineShop.Application.ApplicationServices.CategoryServices.Queries;
using OnlineShop.Application.ApplicationServices.CategoryServices.Responses;
using OnlineShop.Application.Common;
using OnlineShop.WebApi.Common;
using OnlineShop.WebApi.Exceptions;

namespace OnlineShop.WebApi.Controllers;

public class CategoryController:AppControllerBase
{
    [HttpGet]
    public async Task<ActionResult<PaginationList<CategoryDto>>> Get([FromQuery] GetAllCategoryQuery query)
    {
        return  Ok(await MediatorSender.Send(query));
    }
    [HttpGet("{id}")]
    public async Task<ActionResult<CategoryDto>> GetById(int id)
    {
        return  Ok(await MediatorSender.Send(new GetCategoryByIdQuery()
        {
            CategoryId = id
        }));
    }

    [HttpPost]
    public async Task<ActionResult<CategoryDto>> Post([FromBody] CreateCategoryCommand command)
    {
        var dto = await MediatorSender.Send(command);
        return CreatedAtAction(nameof(GetById), new {Id=dto.Id}, dto);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Put(int id, [FromBody] UpdateCategoryCommand command)
    {
        if (id!=command.CategoryId)
        {
            throw new IdNoMatchDtoException();
        }

        await MediatorSender.Send(command);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        await MediatorSender.Send(new DeleteCategoryCommand()
        {
            CategoryId = id
        });

        return NoContent();
    } 
}