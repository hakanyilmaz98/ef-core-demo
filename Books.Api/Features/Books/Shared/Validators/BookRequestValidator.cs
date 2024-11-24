﻿using Books.Api.Features.Books.Shared.Contracts;
using FluentValidation;

namespace Books.Api.Features.Books.Shared.Validators;

public class BookRequestValidator : AbstractValidator<BookRequest>
{
    public BookRequestValidator()
    {
        RuleFor(r => r.Title)
                .NotEmpty()
                .MaximumLength(150);

        RuleFor(r => r.Isbn)
            .NotEmpty()
            .Length(13)
            .Must(isbn => isbn.All(char.IsDigit)).WithMessage("ISBN must be a 13-digit numeric value");
    }
}
