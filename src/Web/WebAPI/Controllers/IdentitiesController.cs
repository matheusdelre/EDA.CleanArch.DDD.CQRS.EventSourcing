﻿using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using ECommerce.Contracts.Identity;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Abstractions;

namespace WebAPI.Controllers;

[Route("api/[controller]/[action]")]
public class IdentitiesController : ApplicationController
{
    public IdentitiesController(IBus bus, IMapper mapper)
        : base(bus, mapper) { }

    [HttpGet]
    public Task<IActionResult> GetUserAuthenticationDetails([FromQuery] Queries.GetUserAuthenticationDetails query, CancellationToken cancellationToken)
        => GetResponseAsync<Queries.GetUserAuthenticationDetails, Responses.UserAuthenticationDetails>(query, cancellationToken);

    [HttpPost]
    public Task<IActionResult> RegisterUser(Commands.RegisterUser command, CancellationToken cancellationToken)
        => SendCommandAsync(command, cancellationToken);

    [HttpPut]
    public Task<IActionResult> ChangeUserPassword(Commands.ChangeUserPassword command, CancellationToken cancellationToken)
        => SendCommandAsync(command, cancellationToken);

    [HttpDelete]
    public Task<IActionResult> DeleteUser(Commands.DeleteUser command, CancellationToken cancellationToken)
        => SendCommandAsync(command, cancellationToken);
}