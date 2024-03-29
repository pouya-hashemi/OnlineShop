﻿using OnlineShop.Application.Common;

namespace OnlineShop.Application.ApplicationServices.UserServices.Responses;

public class UserDto:AuditableDto
{
    public long Id { get; set; }
    public string UserName { get; set; }
    public string UserTitle { get; set; }
}