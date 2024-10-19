using LivePager.API.Features.Authentication.CreateUser.Logic;

namespace LivePager.API.Features.Authentication.CreateUser
{
    public class CreateUserServiceOrchestrator
    {
        private readonly CreateUserService _createUserService;

        public CreateUserServiceOrchestrator(
            CreateUserService createUserService)
        {
            _createUserService = createUserService;
        }

        public async Task ExecuteAsync(
            CreateUserRequest request,
            CancellationToken cancellationToken = default)
        {
            var createUserCommand = new CreateUserCommand()
            {
                Email = request.Email,
                Username = request.Username,
                Password = request.Password
            };

            // Call repository to validate username is unique
            // Call repository to validate email is unique

            var createUserResult = _createUserService
                .ExecuteAsync(createUserCommand, cancellationToken);

            // Use createUserResult.Username, createUserResult.HashedPassword, createUserResult.Email
            // to persist a new user entity

            // Return some other response actually intended for user.
        }
    }
}
