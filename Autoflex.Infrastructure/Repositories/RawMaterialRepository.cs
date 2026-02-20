using Autoflex.Domain.Entities; 
using Autoflex.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Autoflex.Infrastructure.Repositories;

public class RawMaterialRepository : IRawMaterialRepository
{
    private readonly AppDbContext _context;

    public RawMaterialRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<RawMaterial>> GetAllAsync()
        => await _context.RawMaterials.ToListAsync();

    public async Task AddAsync(RawMaterial material)
    {
        await _context.RawMaterials.AddAsync(material);
        await _context.SaveChangesAsync();
    }

    public async Task<RawMaterial> GetByIdAsync(int id)
    {
        return await _context.RawMaterials.FindAsync(id);
    }

    public async Task UpdateAsync(RawMaterial material)
    {
        _context.RawMaterials.Update(material);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(RawMaterial material)
    {
        _context.RawMaterials.Remove(material);
        await _context.SaveChangesAsync();
    }
}