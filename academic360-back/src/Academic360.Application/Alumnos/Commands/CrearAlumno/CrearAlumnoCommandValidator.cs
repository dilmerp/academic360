using FluentValidation;

namespace Academic360.Application.Alumnos.Commands.CrearAlumno;

public class CrearAlumnoCommandValidator : AbstractValidator<CrearAlumnoCommand>
{
    public CrearAlumnoCommandValidator()
    {
        RuleFor(v => v.Datos.Paterno)
            .NotEmpty().WithMessage("El apellido paterno es obligatorio.")
            .MaximumLength(50).WithMessage("El apellido paterno no debe exceder los 50 caracteres.");

        RuleFor(v => v.Datos.Nombres)
            .NotEmpty().WithMessage("Los nombres son obligatorios.")
            .MaximumLength(50).WithMessage("Los nombres no deben exceder los 50 caracteres.");

        RuleFor(v => v.Datos.IdTenor)
            .GreaterThan(0).WithMessage("Debe seleccionar un tenor.");

        RuleFor(v => v.Datos.IdCivil)
            .GreaterThan(0).WithMessage("Debe seleccionar un estado civil.");

        RuleFor(v => v.Datos.Email)
            .EmailAddress().When(v => !string.IsNullOrEmpty(v.Datos.Email))
            .WithMessage("El formato del E-Mail no es válido.");

        RuleFor(v => v.Datos.EmailOpcional)
            .EmailAddress().When(v => !string.IsNullOrEmpty(v.Datos.EmailOpcional))
            .WithMessage("El formato del E-Mail opcional no es válido.");

        RuleFor(v => v.Datos.IdPaisNacimiento)
            .GreaterThan(0).WithMessage("Debe seleccionar el país de nacimiento.");

        RuleFor(v => v.Datos.IdPais)
            .GreaterThan(0).WithMessage("Debe seleccionar el país de residencia.");

        RuleFor(v => v.Datos.NumeroDocumento)
            .NotEmpty().When(v => v.Datos.IdDocumento > 0)
            .WithMessage("Debe ingresar el número de documento.");

        RuleFor(v => v.Datos.IdDocumento)
            .GreaterThan(0).When(v => !string.IsNullOrEmpty(v.Datos.NumeroDocumento))
            .WithMessage("Debe seleccionar el tipo de documento.");

        RuleFor(v => v.Datos.Ruc)
            .Matches(@"^\d{11}$").When(v => !string.IsNullOrEmpty(v.Datos.Ruc))
            .WithMessage("El número de RUC no es válido. Debe contener 11 dígitos numéricos.");
    }
}