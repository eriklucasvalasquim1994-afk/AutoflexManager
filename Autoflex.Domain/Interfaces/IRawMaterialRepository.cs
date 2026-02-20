using Autoflex.Domain.Entities;

public interface IRawMaterialRepository
{
    Task<IEnumerable<RawMaterial>> GetAllAsync();
    Task AddAsync(RawMaterial material);
    Task<RawMaterial> GetByIdAsync(int id);
    Task UpdateAsync(RawMaterial material);
    Task DeleteAsync(RawMaterial material);
}