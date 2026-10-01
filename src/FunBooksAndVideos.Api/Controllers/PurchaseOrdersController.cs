using FunBooksAndVideos.Application.Contracts.Requests;
using FunBooksAndVideos.Application.Contracts.Responses;
using FunBooksAndVideos.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace FunBooksAndVideos.Api.Controllers;

/// <summary>Purchase orders.</summary>
[ApiController]
[Route("api/purchase-orders")]
[Produces("application/json")]
public sealed class PurchaseOrdersController : ControllerBase
{
    private readonly IPurchaseOrderService _purchaseOrderService;

    public PurchaseOrdersController(IPurchaseOrderService purchaseOrderService)
    {
        _purchaseOrderService = purchaseOrderService;
    }

    /// <summary>
    /// Creates a purchase order and processes it. Memberships are activated immediately (BR1)
    /// and a shipping slip is generated for physical products (BR2).
    /// </summary>
    /// <remarks>
    /// The example order from the brief (total 48.50):
    ///
    ///     POST /api/purchase-orders
    ///     {
    ///       "customerId": 4567890,
    ///       "items": [
    ///         { "productId": 1, "quantity": 1 },
    ///         { "productId": 2, "quantity": 1 },
    ///         { "productId": 3, "quantity": 1 }
    ///       ]
    ///     }
    /// </remarks>
    [HttpPost]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(ProcessedPurchaseOrderResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProcessedPurchaseOrderResponse>> Create(
        [FromBody] CreatePurchaseOrderRequest request, CancellationToken cancellationToken)
    {
        var result = await _purchaseOrderService.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.PurchaseOrder.Id }, result);
    }

    /// <summary>Lists all purchase orders.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<PurchaseOrderResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<PurchaseOrderResponse>>> GetAll(CancellationToken cancellationToken) =>
        Ok(await _purchaseOrderService.GetAllAsync(cancellationToken));

    /// <summary>Gets a purchase order by its PO id.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(PurchaseOrderResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PurchaseOrderResponse>> GetById(int id, CancellationToken cancellationToken)
    {
        var order = await _purchaseOrderService.GetByIdAsync(id, cancellationToken);
        return order is null ? NotFound() : Ok(order);
    }
}
