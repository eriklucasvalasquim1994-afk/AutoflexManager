using Autoflex.Application.DTOs;

namespace Autoflex.Application.Interfaces;

public interface IProductionService
{
    Task<ProductionSuggestionDTO> GetProductionSuggestionAsync();
}