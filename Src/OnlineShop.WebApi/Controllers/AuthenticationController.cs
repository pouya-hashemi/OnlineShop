using Microsoft.AspNetCore.Mvc;
using OnlineShop.Application.ApplicationServices.UserServices.Commands;
using OnlineShop.WebApi.Common;

namespace OnlineShop.WebApi.Controllers;

public class AuthenticationController:AppControllerBase
{
    [HttpPost]
    public async Task<string> Post(LoginUserCommand command)
    {
        return await MediatorSender.Send(command);
    }
}