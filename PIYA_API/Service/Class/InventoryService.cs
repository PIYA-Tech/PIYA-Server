using Microsoft.EntityFrameworkCore;
using PIYA_API.Data;
using PIYA_API.Model;
using PIYA_API.Service.Interface;

namespace PIYA_API.Service.Class;

public class InventoryService(
    PharmacyApiDbContext context,
    IAuditService auditService,
    ILogger<InventoryService> logger) : IInventoryService
{
    private readonly PharmacyApiDbContext _context = context;
    private readonly IAuditService _auditService = auditService;
    private readonly ILogger<InventoryService> _logger = logger;

    public async Task<PharmacyInventory> AddOrUpdateInventoryAsync(PharmacyInventory inventory)
    {
        var existing = await _context.PharmacyInventories
            .FirstOrDefaultAsync(i => 
                i.PharmacyId == inventory.PharmacyId && 
                i.MedicationId == inventory.MedicationId &&
                i.BatchNumber == inventory.BatchNumber);

        if (existing != null)
        {
            // Update existing inventory
            existing.QuantityInStock = inventory.QuantityInStock;
            existing.MinimumStockLevel = inventory.MinimumStockLevel;
            existing.Price = inventory.Price;
            existing.Currency = inventory.Currency;
            existing.ExpirationDate = inventory.ExpirationDate;
            existing.LastRestockedAt = DateTime.UtcNow;
            existing.UpdatedAt = DateTime.UtcNow;
            
            await _context.SaveChangesAsync();
            return existing;
        }
        else
        {
            // Create new inventory record
            inventory.Id = Guid.NewGuid();
            inventory.LastRestockedAt = DateTime.UtcNow;
            inventory.UpdatedAt = DateTime.UtcNow;
            
            _context.PharmacyInventories.Add(inventory);
            await _context.SaveChangesAsync();

            await _auditService.LogEntityActionAsync(
                "AddInventory",
                "PharmacyInventory",
                inventory.Id.ToString(),
                null,
                $"Added inventory for medication {inventory.MedicationId} at pharmacy {inventory.PharmacyId}"
            );

            return inventory;
        }
    }

    public async Task<List<PharmacyInventory>> GetPharmacyInventoryAsync(Guid pharmacyId)
    {
        return await _context.PharmacyInventories
            .Include(i => i.Medication)
            .Include(i => i.Pharmacy)
            .Where(i => i.PharmacyId == pharmacyId)
            .Where(i => i.QuantityInStock > 0)
            .OrderBy(i => i.Medication.BrandName)
            .ToListAsync();
    }

    public async Task<List<PharmacyInventory>> GetPharmaciesWithMedicationAsync(Guid medicationId, int minimumQuantity = 1)
    {
        return await _context.PharmacyInventories
            .Include(i => i.Medication)
            .Where(i => i.MedicationId == medicationId)
            .Where(i => i.QuantityInStock >= minimumQuantity)
            .ToListAsync();
    }

    public async Task<bool> IsInStockAsync(Guid pharmacyId, Guid medicationId, int requiredQuantity = 1)
    {
        var totalStock = await _context.PharmacyInventories
            .Where(i => i.PharmacyId == pharmacyId && i.MedicationId == medicationId)
            .Where(i => i.ExpirationDate > DateTime.UtcNow) // Only count non-expired stock
            .SumAsync(i => i.QuantityInStock);

        return totalStock >= requiredQuantity;
    }

    public async Task<PharmacyInventory> UpdateStockAsync(Guid inventoryId, int newQuantity)
    {
        var inventory = await _context.PharmacyInventories.FindAsync(inventoryId);
        if (inventory == null)
        {
            throw new InvalidOperationException("Inventory item not found");
        }

        int oldQuantity = inventory.QuantityInStock;
        inventory.QuantityInStock = newQuantity;
        inventory.UpdatedAt = DateTime.UtcNow;

        if (newQuantity > oldQuantity)
        {
            inventory.LastRestockedAt = DateTime.UtcNow;
        }

        await _context.SaveChangesAsync();

        await _auditService.LogEntityActionAsync(
            "UpdateStock",
            "PharmacyInventory",
            inventoryId.ToString(),
            null,
            $"Stock updated from {oldQuantity} to {newQuantity}"
        );

        return inventory;
    }

    public async Task<PharmacyInventory> DecreaseStockAsync(Guid pharmacyId, Guid medicationId, int quantity)
    {
        if (quantity <= 0)
        {
            throw new ArgumentException("Quantity must be positive", nameof(quantity));
        }

        // Use FIFO - get oldest batches first (by expiration date)
        var inventories = await _context.PharmacyInventories
            .Where(i => i.PharmacyId == pharmacyId && i.MedicationId == medicationId)
            .Where(i => i.QuantityInStock > 0)
            .Where(i => i.ExpirationDate > DateTime.UtcNow)
            .OrderBy(i => i.ExpirationDate)
            .ToListAsync();

        int remainingToDecrease = quantity;

        foreach (var inventory in inventories)
        {
            if (remainingToDecrease <= 0)
                break;

            int decreaseAmount = Math.Min(inventory.QuantityInStock, remainingToDecrease);
            inventory.QuantityInStock -= decreaseAmount;
            inventory.UpdatedAt = DateTime.UtcNow;
            remainingToDecrease -= decreaseAmount;
        }

        if (remainingToDecrease > 0)
        {
            throw new InvalidOperationException($"Insufficient stock. Required: {quantity}, Available: {quantity - remainingToDecrease}");
        }

        await _context.SaveChangesAsync();

        await _auditService.LogEntityActionAsync(
            "DecreaseStock",
            "PharmacyInventory",
            $"{pharmacyId}|{medicationId}",
            null,
            $"Decreased stock by {quantity}"
        );

        return inventories.First();
    }

    public async Task<PharmacyInventory> IncreaseStockAsync(Guid pharmacyId, Guid medicationId, int quantity)
    {
        if (quantity <= 0)
        {
            throw new ArgumentException("Quantity must be positive", nameof(quantity));
        }

        // Find existing inventory or create new
        var inventory = await _context.PharmacyInventories
            .FirstOrDefaultAsync(i => i.PharmacyId == pharmacyId && i.MedicationId == medicationId);

        if (inventory == null)
        {
            throw new InvalidOperationException("Inventory item not found. Use AddOrUpdateInventoryAsync to create new inventory.");
        }

        inventory.QuantityInStock += quantity;
        inventory.LastRestockedAt = DateTime.UtcNow;
        inventory.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        await _auditService.LogEntityActionAsync(
            "IncreaseStock",
            "PharmacyInventory",
            inventory.Id.ToString(),
            null,
            $"Increased stock by {quantity}"
        );

        return inventory;
    }

    public async Task<List<PharmacyInventory>> GetLowStockItemsAsync(Guid pharmacyId)
    {
        return await _context.PharmacyInventories
            .Include(i => i.Medication)
            .Where(i => i.PharmacyId == pharmacyId)
            .Where(i => i.QuantityInStock <= i.MinimumStockLevel)
            .OrderBy(i => i.QuantityInStock)
            .ToListAsync();
    }

    public async Task<List<PharmacyInventory>> GetExpiringItemsAsync(Guid pharmacyId, int daysThreshold = 30)
    {
        var thresholdDate = DateTime.UtcNow.AddDays(daysThreshold);

        return await _context.PharmacyInventories
            .Include(i => i.Medication)
            .Where(i => i.PharmacyId == pharmacyId)
            .Where(i => i.ExpirationDate <= thresholdDate && i.ExpirationDate > DateTime.UtcNow)
            .Where(i => i.QuantityInStock > 0)
            .OrderBy(i => i.ExpirationDate)
            .ToListAsync();
    }

    public async Task<(bool CanFulfill, List<Guid> MissingMedicationIds)> CanFulfillPrescriptionAsync(
        Guid pharmacyId, 
        List<Guid> medicationIds)
    {
        var missingMedicationIds = new List<Guid>();

        foreach (var medicationId in medicationIds)
        {
            bool inStock = await IsInStockAsync(pharmacyId, medicationId, 1);
            if (!inStock)
            {
                missingMedicationIds.Add(medicationId);
            }
        }

        return (missingMedicationIds.Count == 0, missingMedicationIds);
    }

    public async Task<PharmacyInventory?> GetByIdAsync(Guid id)
    {
        return await _context.PharmacyInventories
            .Include(i => i.Medication)
            .Include(i => i.Pharmacy)
            .FirstOrDefaultAsync(i => i.Id == id);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var inventory = await GetByIdAsync(id);
        if (inventory == null)
        {
            return false;
        }

        _context.PharmacyInventories.Remove(inventory);
        await _context.SaveChangesAsync();

        await _auditService.LogEntityActionAsync(
            "DeleteInventory",
            "PharmacyInventory",
            id.ToString(),
            null,
            $"Deleted inventory item for medication {inventory.MedicationId}"
        );

        return true;
    }
}
