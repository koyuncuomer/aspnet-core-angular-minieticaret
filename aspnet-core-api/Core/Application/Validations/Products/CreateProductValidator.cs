using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.ViewModels.Products;
using FluentValidation;

namespace Application.Validations.Products
{
    public class CreateProductValidator : AbstractValidator<VM_Create_Product>
    {
        public CreateProductValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                    .WithMessage("Name is required")
                .NotNull()
                    .WithMessage("Name is required")
                .MaximumLength(255)
                    .WithMessage("Name must be between 2 and 255 characters")
                .MinimumLength(2)
                    .WithMessage("Name must be between 2 and 255 characters");

            RuleFor(x => x.Stock)
                //.NotEmpty()
                .NotNull()
                    .WithMessage("Stock is required")
                .Must(x => x >= 0)
                    .WithMessage("Stock must be greater than or equal to 0");

            RuleFor(x => x.Price)
                //.NotEmpty()
                .NotNull()
                    .WithMessage("Price is required")
                .Must(x => x >= 0)
                    .WithMessage("Price must be greater than or equal to 0");
        }
    }
}
