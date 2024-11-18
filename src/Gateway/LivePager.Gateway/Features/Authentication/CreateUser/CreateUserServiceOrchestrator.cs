using FluentValidation;
using LivePager.Gateway.Features.Authentication.Contracts.CreateUser;
using LivePager.Gateway.Features.Authentication.CreateUser.Logic;
using LivePager.Gateway.Infrastructure;
using LivePager.Gateway.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace LivePager.Gateway.Features.Authentication.CreateUser
{
    public class CreateUserServiceOrchestrator
    {
        private readonly CreateUserService _createUserService;
        private readonly LiverPagerDbContext _dbContext;

        public CreateUserServiceOrchestrator(
            CreateUserService createUserService,
            LiverPagerDbContext dbContext)
        {
            _createUserService = createUserService;
            _dbContext = dbContext;
        }

        public async Task<CreateUserResponse> ExecuteAsync(
            CreateUserRequest request,
            CancellationToken cancellationToken = default)
        {
            var createUserCommand = new CreateUserCommand()
            {
                Email = request.Email,
                Username = request.Username,
                Password = request.Password
            };

            var userExists = await _dbContext.Users
                .AnyAsync(x => x.Username == request.Username, cancellationToken);
            if (userExists)
                throw new ValidationException("User already exists.");

            var createUserResult = await _createUserService
                .ExecuteAsync(createUserCommand, cancellationToken);
            if (!createUserResult.Success)
                throw new ValidationException("Not valid user.");

            var user = new User()
            {
                Username = createUserResult.User!.Username,
                DisplayName = createUserResult.User!.DisplayName,
                Email = createUserResult.User!.Email,
                PasswordHash = createUserResult.User!.HashedPassword
            };

            await _dbContext.Users.AddAsync(user, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            var response = new CreateUserResponse()
            {
                Id = user.Id,
                Username = user.Username!
            };

            return response;
        }
    }
}
