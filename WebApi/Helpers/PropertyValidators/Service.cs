using FluentValidation;
using FluentValidation.AspNetCore;

using WebApi.DTOs.AuthDTO;
using WebApi.DTOs.CartDTO;
using WebApi.DTOs.CategoryDTO;
using WebApi.DTOs.OrderDTO;
using WebApi.DTOs.OrderProductDTO;
using WebApi.DTOs.ProductCategoryDTO;
using WebApi.DTOs.ProductDTO;
using WebApi.DTOs.UserDTO;

namespace Microsoft.Extensions.DependencyInjection;

public static class PropertyValidators
{
    public static IServiceCollection AddPropertyValidator(this IServiceCollection services, Action<FluentValidationAutoValidationConfiguration> configurationExpression = null)
    {
        services.AddFluentValidationAutoValidation();
        services.AddValidatorsFromAssemblyContaining<AuthDetailDTO>();
        services.AddValidatorsFromAssemblyContaining<CartDetailDTO>();
        services.AddValidatorsFromAssemblyContaining<CartProductDetailDTO>();
        services.AddValidatorsFromAssemblyContaining<CategoryDetailDTO>();
        services.AddValidatorsFromAssemblyContaining<OrderDetailDTO>();
        services.AddValidatorsFromAssemblyContaining<OrderProductDetailDTO>();
        services.AddValidatorsFromAssemblyContaining<ProductCategoryDetailDTO>();
        services.AddValidatorsFromAssemblyContaining<ProductDetailDTO>();
        services.AddValidatorsFromAssemblyContaining<UserDetailDTO>();

        return services;
    }
}