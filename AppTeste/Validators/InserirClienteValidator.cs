using AppTeste.Models.DTOs;
using FluentValidation;

namespace AppTeste.Validators
{
    public class InserirClienteInputModelValidator : AbstractValidator<InserirClienteInputModel>
    {
        public InserirClienteInputModelValidator()
        {
            RuleFor(model => model.Nome)
            .NotEmpty()
            .WithMessage("O nome é obrigatório.")
            .MaximumLength(150)
            .WithMessage("O nome deve ter no máximo 150 caracteres.");

            RuleFor(model => model.RG)
                .NotEmpty()
                .WithMessage("O RG é obrigatório.")
                .MaximumLength(20)
                .WithMessage("O RG deve ter no máximo 20 caracteres.");

            RuleFor(model => model.CPF)
                 .NotEmpty()
                 .WithMessage("O CPF é obrigatório.")
                 .MaximumLength(20)
                 .WithMessage("O CPF deve ter no máximo 20 caracteres.")
                 .Must(BeValidCPF)
                 .WithMessage("CPF inválido.");

            RuleFor(model => model.DataNascimento)
                .NotNull()
                .WithMessage("A data de nascimento é obrigatória.");

            RuleFor(model => model.Telefone)
                .NotEmpty()
                .WithMessage("O telefone é obrigatório.")
                .MaximumLength(20)
                .WithMessage("O telefone deve ter no máximo 20 caracteres.");

            RuleFor(model => model.Email)
                .NotEmpty()
                .WithMessage("O email é obrigatório.")
                .MaximumLength(150)
                .WithMessage("O email deve ter no máximo 150 caracteres.");

            RuleFor(model => model.CodEmpresa)
                .NotNull()
                .WithMessage("O código da empresa é obrigatório.");

            RuleFor(model => model.Rua)
                .NotEmpty()
                .WithMessage("A rua é obrigatória.")
                .MaximumLength(255)
                .WithMessage("A rua deve ter no máximo 255 caracteres.");

            RuleFor(model => model.Bairro)
                .NotEmpty()
                .WithMessage("O bairro é obrigatório.")
                .MaximumLength(50)
                .WithMessage("O bairro deve ter no máximo 50 caracteres.");

            RuleFor(model => model.Numero)
                .NotEmpty()
                .WithMessage("O número é obrigatório.")
                .MaximumLength(50)
                .WithMessage("O número deve ter no máximo 50 caracteres.");

            RuleFor(model => model.Complemento)
                .NotEmpty()
                .WithMessage("O complemento é obrigatório.")
                .MaximumLength(100)
                .WithMessage("O complemento deve ter no máximo 100 caracteres.");

            RuleFor(model => model.CEP)
                .NotEmpty()
                .WithMessage("O CEP é obrigatório.")
                .MaximumLength(10)
                .WithMessage("O CEP deve ter no máximo 10 caracteres.");

            RuleFor(model => model.TipoEndereco)
                .NotNull()
                .WithMessage("O tipo de endereço é obrigatório.");

            RuleFor(model => model.Cidade)
                .NotEmpty()
                .WithMessage("A cidade é obrigatória.")
                .MaximumLength(100)
                .WithMessage("A cidade deve ter no máximo 100 caracteres.");

            RuleFor(model => model.Estado)
                .NotEmpty()
                .WithMessage("O estado é obrigatório.")
                .MaximumLength(2)
                .WithMessage("O estado deve ter no máximo 2 caracteres.");

        }

        private bool BeValidCPF(string cpf)
        {
            if (string.IsNullOrWhiteSpace(cpf))
                return false;

            // Remove qualquer caractere não numérico do CPF
            cpf = new string(cpf.Where(char.IsDigit).ToArray());

            // Verifica se o CPF tem 11 dígitos
            if (cpf.Length != 11)
                return false;

            // Verifica se todos os dígitos são iguais (CPF inválido)
            if (cpf.Distinct().Count() == 1)
                return false;

            // Calcula o primeiro dígito verificador
            int soma = 0;
            for (int i = 0; i < 9; i++)
                soma += int.Parse(cpf[i].ToString()) * (10 - i);
            int resto = soma % 11;
            int digitoVerificador1 = resto < 2 ? 0 : 11 - resto;

            // Verifica se o primeiro dígito verificador está correto
            if (int.Parse(cpf[9].ToString()) != digitoVerificador1)
                return false;

            // Calcula o segundo dígito verificador
            soma = 0;
            for (int i = 0; i < 10; i++)
                soma += int.Parse(cpf[i].ToString()) * (11 - i);
            resto = soma % 11;
            int digitoVerificador2 = resto < 2 ? 0 : 11 - resto;

            // Verifica se o segundo dígito verificador está correto
            if (int.Parse(cpf[10].ToString()) != digitoVerificador2)
                return false;

            return true;
        }
    }

    public class AlterarClienteInputModelValidator : AbstractValidator<AlterarClienteInputModel>
    {
        public AlterarClienteInputModelValidator()
        {
            RuleFor(model => model.Id)
                .GreaterThan(0)
                .WithMessage("O ID do cliente deve ser maior que 0.");

            RuleFor(model => model.Nome)
                .MaximumLength(150)
                .WithMessage("O nome deve ter no máximo 150 caracteres.");

            RuleFor(model => model.RG)
                .MaximumLength(20)
                .WithMessage("O RG deve ter no máximo 20 caracteres.");

            RuleFor(model => model.CPF)
                .NotEmpty()
                .WithMessage("O CPF é obrigatório.")
                .MaximumLength(20)
                .WithMessage("O CPF deve ter no máximo 20 caracteres.")
                .Must(BeValidCPF)
                .WithMessage("CPF inválido.");

            //RuleFor(model => model.DataNascimento)
            //    .NotNull()
            //    .WithMessage("A data de nascimento é obrigatória.");

            RuleFor(model => model.Telefone)
                .MaximumLength(20)
                .WithMessage("O telefone deve ter no máximo 20 caracteres.");

            RuleFor(model => model.Email)
                .MaximumLength(150)
                .WithMessage("O email deve ter no máximo 150 caracteres.");

            RuleFor(model => model.Rua)
                .MaximumLength(255)
                .WithMessage("A rua deve ter no máximo 255 caracteres.");

            RuleFor(model => model.Bairro)
                .MaximumLength(50)
                .WithMessage("O bairro deve ter no máximo 50 caracteres.");

            RuleFor(model => model.Numero)
                .MaximumLength(50)
                .WithMessage("O número deve ter no máximo 50 caracteres.");

            RuleFor(model => model.Complemento)
                .MaximumLength(100)
                .WithMessage("O complemento deve ter no máximo 100 caracteres.");

            RuleFor(model => model.CEP)
                .MaximumLength(10)
                .WithMessage("O CEP deve ter no máximo 10 caracteres.");

            RuleFor(model => model.Cidade)
                .MaximumLength(100)
                .WithMessage("A cidade deve ter no máximo 100 caracteres.");

            RuleFor(model => model.Estado)
                .MaximumLength(2)
                .WithMessage("O estado deve ter no máximo 2 caracteres.");
        }

        private bool BeValidCPF(string cpf)
        {

            if (string.IsNullOrWhiteSpace(cpf))
                return false;


            // Remove qualquer caractere não numérico do CPF
            cpf = new string(cpf.Where(char.IsDigit).ToArray());

            // Verifica se o CPF tem 11 dígitos
            if (cpf.Length != 11)
                return false;

            // Verifica se todos os dígitos são iguais (CPF inválido)
            if (cpf.Distinct().Count() == 1)
                return false;

            // Calcula o primeiro dígito verificador
            int soma = 0;
            for (int i = 0; i < 9; i++)
                soma += int.Parse(cpf[i].ToString()) * (10 - i);
            int resto = soma % 11;
            int digitoVerificador1 = resto < 2 ? 0 : 11 - resto;

            // Verifica se o primeiro dígito verificador está correto
            if (int.Parse(cpf[9].ToString()) != digitoVerificador1)
                return false;

            // Calcula o segundo dígito verificador
            soma = 0;
            for (int i = 0; i < 10; i++)
                soma += int.Parse(cpf[i].ToString()) * (11 - i);
            resto = soma % 11;
            int digitoVerificador2 = resto < 2 ? 0 : 11 - resto;

            // Verifica se o segundo dígito verificador está correto
            if (int.Parse(cpf[10].ToString()) != digitoVerificador2)
                return false;

            return true;
        }
    }

    public class ListarTodosClientesInputModelValidator : AbstractValidator<ListarTodosClientesInputModel>
    {
        public ListarTodosClientesInputModelValidator()
        {
            RuleFor(x => x.CodEmpresa).NotNull().WithMessage("O campo Código da Empresa é obrigatório.");
        }
    }
}
