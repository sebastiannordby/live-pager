using LivePager.API.Features.Authentication;
using LivePager.API.Features.Authentication.CreateUser.Logic;

namespace LivePager.API.Tests.Unit.Features.User
{
    public class CreateUserServiceTests
    {
        private readonly CreateUserService _sut;

        public CreateUserServiceTests()
        {
            var passwordHasher = new PasswordHasher();

            _sut = new CreateUserService(passwordHasher);
        }

        [Fact]
        public async Task ShouldValidateInput()
        {
            // Given
            var input = new CreateUserCommand()
            {
                Username = "",
                Email = "",
                Password = ""
            };

            // When
            var output = await _sut.ExecuteAsync(input);

            // Then
            var expectedRequiredFields = new[]
            {
                nameof(CreateUserCommand.Username),
                nameof(CreateUserCommand.Password),
                nameof(CreateUserCommand.Email)
            };

            Assert.False(output.Success);
            Assert.True(expectedRequiredFields
                .All(key => output.ValidationErrors?.ContainsKey(key) == true),
                "One or more validation errors are missing.");
        }

        [Fact]
        public async Task ShouldValidateEmailAddress()
        {
            // Given
            var input = new CreateUserCommand()
            {
                Username = "ValidUsername",
                Email = "ThisEmailDoesNotHaveAnAT",
                Password = "ValidPassword"
            };

            // When
            var output = await _sut.ExecuteAsync(input);

            // Then
            var emailValidationMessages = output.ValidationErrors!
                .First(x => x.Key == nameof(CreateUserCommand.Email))
                .Value;

            Assert.Contains(emailValidationMessages, x => x.Contains("@"));
        }

        [Fact]
        public async Task ShouldReturnHashedPassword()
        {
            // Given
            var input = new CreateUserCommand()
            {
                Username = "ValidUsername",
                Email = "ThisEmailDoesNotHaveAnAT",
                Password = "ValidPassword"
            };

            // When
            var output = await _sut.ExecuteAsync(input);

            // Then
            Assert.NotEqual(output.HashedPassword, input.Password);
            Assert.DoesNotContain(output.HashedPassword!, input.Password);
        }
    }
}
