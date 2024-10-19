namespace LivePager.API.Features.Authentication.CreateUser.Logic
{
    public sealed class CreateUserService
    {
        private readonly PasswordHasher _passwordHasher;

        public CreateUserService(PasswordHasher passwordHasher)
        {
            _passwordHasher = passwordHasher;
        }

        public async Task<CreateUserResult> ExecuteAsync(
            CreateUserCommand command,
            CancellationToken cancellationToken = default)
        {
            var result = new CreateUserResult();
            var validationErrors = new Dictionary<string, List<string>>();

            if (string.IsNullOrWhiteSpace(command.Username))
                validationErrors.Add(nameof(command.Username), ["Cannot be null/empty"]);

            if (string.IsNullOrWhiteSpace(command.Password))
                validationErrors.Add(nameof(command.Password), ["Cannot be null/empty"]);

            if (string.IsNullOrWhiteSpace(command.Email))
                validationErrors.Add(nameof(command.Email), ["Cannot be null/empty"]);
            else if (!command.Email.Contains("@"))
                validationErrors.Add(nameof(command.Email), ["Does not contain @"]);


            result.ValidationErrors = validationErrors;
            result.Success = validationErrors.Any() == false;

            result.Username = command.Username;
            result.Email = command.Email;
            result.HashedPassword = _passwordHasher
                .HashPassword(command.Password);

            return await Task.FromResult(result);
        }
    }
}
