﻿using Contracts.DataTransferObjects;
using Domain.Abstractions.ValueObjects;

namespace Domain.ValueObjects.Products;

public record Product(string Description, string Name, string PictureUrl, string Brand, string Category, string Unit, string Sku)
    : ValueObject<ProductValidator>
{
    public static implicit operator Product(Dto.Product product)
        => new(product.Description, product.Name, product.PictureUrl, product.Brand, product.Category, product.Unit, product.Sku);

    public static implicit operator Dto.Product(Product product)
        => new(product.Description, product.Name, product.PictureUrl, product.Brand, product.Category, product.Unit, product.Sku);

    public static bool operator ==(Product product, Dto.Product dto)
        => dto == (Dto.Product) product;

    public static bool operator !=(Product product, Dto.Product dto)
        => dto != (Dto.Product) product;
}