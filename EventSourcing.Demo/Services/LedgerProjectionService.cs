using EventSourcing.Demo.Models;
using EventSourcing.Demo.Repositories;
using System.ComponentModel.DataAnnotations;

namespace EventSourcing.Demo.Services;

public interface ILedgerProjectionService
{
    Task<bool> CreateAsync(LedgerProjection ledger);

    Task<LedgerProjection?> GetAsync(string id);

    Task<bool> UpdateAsync(LedgerProjection customer);

    Task<bool> DeleteAsync(string id);
}
public class LedgerProjectionService : ILedgerProjectionService
{
    private readonly ILedgerProjectionRepository _ledgerProjectionRepository;

    public LedgerProjectionService(ILedgerProjectionRepository ledgerProjectionRepository)
    {
        _ledgerProjectionRepository = ledgerProjectionRepository;
    }

    public async Task<bool> CreateAsync(LedgerProjection ledger)
    {
        var existingUser = await _ledgerProjectionRepository.GetAsync(ledger.Id);
        if (existingUser is not null)
        {
            var message = $"A user with id {ledger.Id} already exists";
            throw new ValidationException(message);
        }

        return await _ledgerProjectionRepository.CreateAsync(ledger);
    }

    public async Task<LedgerProjection?> GetAsync(string id)
    {
        var ledger = await _ledgerProjectionRepository.GetAsync(id);
        return ledger;
    }

    public async Task<bool> UpdateAsync(LedgerProjection ledger)
    {
        return await _ledgerProjectionRepository.UpdateAsync(ledger);
    }

    public async Task<bool> DeleteAsync(string id)
    {
        return await _ledgerProjectionRepository.DeleteAsync(id);
    }
}
