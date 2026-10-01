using FunBooksAndVideos.Application.Contracts.Responses;
using FunBooksAndVideos.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace FunBooksAndVideos.Api.Controllers;

/// <summary>Customer accounts and their active memberships.</summary>
[ApiController]
[Route("api/customers")]
[Produces("application/json")]
public sealed class CustomersController : ControllerBase
{
    private readonly ICustomerService _customerService;

    public CustomersController(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    /// <summary>Lists all customers.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<CustomerResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<CustomerResponse>>> GetAll(CancellationToken cancellationToken) =>
        Ok(await _customerService.GetAllAsync(cancellationToken));

    /// <summary>Gets a customer, including their current membership.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(CustomerResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CustomerResponse>> GetById(int id, CancellationToken cancellationToken)
    {
        var customer = await _customerService.GetByIdAsync(id, cancellationToken);
        return customer is null ? NotFound() : Ok(customer);
    }
}
