using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
namespace OnlineShop.WebApi.Common;

[ApiController]
[Route("[controller]")]
public class AppControllerBase:ControllerBase
{
    private ISender _sender;
    public ISender MediatorSender =>(ISender)HttpContext?.RequestServices?.GetService(typeof(ISender));
}