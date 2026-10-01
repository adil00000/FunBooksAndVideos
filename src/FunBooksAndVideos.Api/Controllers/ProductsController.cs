using FunBooksAndVideos.Application.Contracts.Responses;
using FunBooksAndVideos.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace FunBooksAndVideos.Api.Controllers;

/// <summary>Catalogue of books, videos and memberships.</summary>
[ApiController]
[Route("api/products")]
[Produces("application/json")]
public sealed class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    /// <summary>Lists all products.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<ProductResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<ProductResponse>>> GetAll(CancellationToken cancellationToken) =>
        Ok(await _productService.GetAllAsync(cancellationToken));

    /// <summary>Gets a product by id.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ProductResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProductResponse>> GetById(int id, CancellationToken cancellationToken)
    {
        var product = await _productService.GetByIdAsync(id, cancellationToken);
        return product is null ? NotFound() : Ok(product);
    }
}
