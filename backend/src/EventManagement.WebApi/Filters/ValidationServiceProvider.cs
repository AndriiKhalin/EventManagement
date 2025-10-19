using FluentValidation;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace EventManagement.WebApi.Filters;

public class ValidationServiceProvider
{
    private readonly IServiceProvider _serviceProvider;

    public ValidationServiceProvider(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task ValidateAsync(object model, ModelStateDictionary modelState, Type? validatorType = null)
    {
        if (model == null) return;

        var modelType = model.GetType();
        var genericValidatorType = validatorType ?? typeof(IValidator<>).MakeGenericType(modelType);

        if (_serviceProvider.GetService(genericValidatorType) is IValidator validator)
        {
            var result = await validator.ValidateAsync(
                new ValidationContext<object>(model)
            );

            foreach (var error in result.Errors) modelState.AddModelError(error.PropertyName, error.ErrorMessage);
        }
    }
}