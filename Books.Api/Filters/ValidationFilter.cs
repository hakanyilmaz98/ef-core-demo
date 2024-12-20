﻿using FluentValidation;

namespace Books.Api.Filters;

public class ValidationFilter<T>(IValidator<T> validator) : IEndpointFilter
{
    private readonly IValidator<T> _validator = validator;

    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var request = context.Arguments.OfType<T>().FirstOrDefault();

        if (request is null) return TypedResults.Problem(statusCode: StatusCodes.Status400BadRequest);

        var validationResult = await _validator.ValidateAsync(request);

        return validationResult.IsValid ? await next(context) : TypedResults.ValidationProblem(validationResult.ToDictionary());
    }
}
