using Autoflex.Application.DTOs;
using Autoflex.Application.Interfaces;
using Autoflex.Domain.Entities;
using Autoflex.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;


namespace Autoflex.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductRepository _productRepo;
    private readonly IProductionService _productionService;

    public ProductsController(IProductRepository productRepo, IProductionService productionService)
    {
        _productRepo = productRepo;
        _productionService = productionService;
    }

    // RF001 & RF003 - Listar Produtos
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
    {
        var products = await _productRepo.GetAllWithRecipesAsync();
        return Ok(products);
    }

    // RF001 - Cadastrar Produto
    [HttpPost]
    public async Task<ActionResult> CreateProduct(Product product)
    {
        await _productRepo.AddAsync(product);
        return Ok();
    }

    // RF004 - O endpoint da Sugestão de Produção!
    [HttpGet("suggestion")]
    public async Task<ActionResult<ProductionSuggestionDTO>> GetSuggestion()
    {
        var suggestion = await _productionService.GetProductionSuggestionAsync();
        return Ok(suggestion);
    }

    [HttpPost("associate-material")]
    public async Task<ActionResult> AssociateMaterial(ProductRecipe recipe)
    {
        // Use o repositório que já está lá em cima no seu código!
        await _productRepo.AddRecipeAsync(recipe);
        return Ok();
    }
}