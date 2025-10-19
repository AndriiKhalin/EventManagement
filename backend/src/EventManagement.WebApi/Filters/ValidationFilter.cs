using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EventManagement.WebApi.Filters;

public class ValidationFilter : IAsyncActionFilter
{
    private readonly ValidationServiceProvider _validationProvider;

    public ValidationFilter(ValidationServiceProvider validationProvider)
    {
        _validationProvider = validationProvider;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        // Validate each parameter that has a validator
        foreach (var (key, value) in context.ActionArguments)
            if (value != null)
                await _validationProvider.ValidateAsync(value, context.ModelState);

        if (!context.ModelState.IsValid)
        {
            context.Result = new BadRequestObjectResult(context.ModelState);
            return;
        }

        await next();
    }
}