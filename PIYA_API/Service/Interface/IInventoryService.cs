using PIYA_API.Model;

namespace PIYA_API.Service.Interface;

/// <summary>
/// Service for managing pharmacy inventory
/// </summary>
public interface IInventoryService
{
    /// <summary>
    /// Add or update inventory for a pharmacy
    /// </summary>
    Task<PharmacyInventory> AddOrUpdateInventoryAsync(PharmacyInventory inventory);
    
    /// <summary>
    /// Get inventory item by ID
    /// </summary>
    Task<PharmacyInventory?> GetByIdAsync(Guid id);
    
    /// <summary>
    /// Get all inventory for a pharmacy
    /// </summary>
    Task<List<PharmacyInventory>> GetPharmacyInventoryAsync(Guid pharmacyId);
    
    /// <summary>
    /// Get pharmacies that have a specific medication in stock
    /// </summary>
    Task<List<PharmacyInventory>> GetPharmaciesWithMedicationAsync(Guid medicationId, int minimumQuantity = 1);
    
    /// <summary>
    /// Check if pharmacy has medication in stock
    /// </summary>
    Task<bool> IsInStockAsync(Guid pharmacyId, Guid medicationId, int requiredQuantity = 1);
    
    /// <summary>
    /// Update stock quantity
    /// </summary>
    Task<PharmacyInventory> UpdateStockAsync(Guid inventoryId, int newQuantity);
    
    /// <summary>
    /// Decrease stock (after sale/fulfillment)
    /// </summary>
    Task<PharmacyInventory> DecreaseStockAsync(Guid pharmacyId, Guid medicationId, int quantity);
    
    /// <summary>
    /// Increase stock (restock)
    /// </summary>
    Task<PharmacyInventory> IncreaseStockAsync(Guid pharmacyId, Guid medicationId, int quantity);
    
    /// <summary>
    /// Get low stock items for a pharmacy
    /// </summary>
    Task<List<PharmacyInventory>> GetLowStockItemsAsync(Guid pharmacyId);
    
    /// <summary>
    /// Get items expiring soon
    /// </summary>
    Task<List<PharmacyInventory>> GetExpiringItemsAsync(Guid pharmacyId, int daysThreshold = 30);
    
    /// <summary>
    /// Check if pharmacy can fulfill prescription
    /// </summary>
    Task<(bool CanFulfill, List<Guid> MissingMedicationIds)> CanFulfillPrescriptionAsync(Guid pharmacyId, List<Guid> medicationIds);
    
    /// <summary>
    /// Delete inventory item
    /// </summary>
    Task<bool> DeleteAsync(Guid id);
}
