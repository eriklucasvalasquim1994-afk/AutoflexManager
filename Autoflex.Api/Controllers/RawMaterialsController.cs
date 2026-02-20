using Autoflex.Domain.Entities;
using Autoflex.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Autoflex.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RawMaterialsController : ControllerBase
{
    private readonly IRawMaterialRepository _materialRepo;

    public RawMaterialsController(IRawMaterialRepository materialRepo)
    {
        _materialRepo = materialRepo;
    }

    // RF002 - Listar todas as matérias-primas
    [HttpGet]
    public async Task<ActionResult<IEnumerable<RawMaterial>>> GetMaterials()
    {
        var materials = await _materialRepo.GetAllAsync();
        return Ok(materials);
    }

    // RF002 - Cadastrar nova matéria-prima
    [HttpPost]
    public async Task<ActionResult> CreateMaterial(RawMaterial material)
    {
        // Aqui o material vem do Front com Nome e Quantidade em estoque
        await _materialRepo.AddAsync(material);
        return Ok();
    }

    // Deletar Material
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteMaterial(int id)
    {
        var material = await _materialRepo.GetByIdAsync(id);
        if (material == null) return NotFound();
        await _materialRepo.DeleteAsync(material);
        return Ok();
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateStock(int id, RawMaterial material)
    {
        // 1. Busca o material real que está no banco
        var existingMaterial = await _materialRepo.GetByIdAsync(id);

        if (existingMaterial == null) return NotFound();

        // 2. Atualiza APENAS a quantidade (StockQuantity) do objeto que o EF está rastreando
        existingMaterial.StockQuantity = material.StockQuantity;

        // 3. Salva a mudança
        await _materialRepo.UpdateAsync(existingMaterial);

        return Ok();
    }
}