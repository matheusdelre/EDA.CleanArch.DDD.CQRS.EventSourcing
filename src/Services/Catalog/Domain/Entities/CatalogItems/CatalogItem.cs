﻿using Domain.Abstractions.Entities;
using Domain.ValueObjects.Products;

namespace Domain.Entities.CatalogItems;

public class CatalogItem : Entity<Guid, CatalogItemValidator>
{
    public CatalogItem(Guid id, Guid inventoryId, Product product, decimal unitPrice, string sku, int quantity)
    {
        Id = id;
        InventoryId = inventoryId;
        Product = product;
        UnitPrice = unitPrice;
        Sku = sku;
        Quantity = quantity;
    }

    public Guid InventoryId { get; }
    public Product Product { get; }
    public decimal UnitPrice { get; }
    public string  Sku { get; }
    public int Quantity { get; private set; }

    public void Increase(int quantity)
        => Quantity += quantity;

    public void Decrease(int quantity)
        => Quantity += quantity;
}