using Contracts.Common;
using Domain.Users.User;
using System.Security.Claims;

namespace APIServerTests
{

    internal class TestUtils
    {
        private DbContextOptions<ApplicationDbContext> _options;
        public ClaimsPrincipal ClaimsPrincipal { get; private set;} 
    
        public TestUtils()
        {
            _options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

           ClaimsPrincipal = new ClaimsPrincipal(
               new ClaimsIdentity(
                   new List<Claim>() { new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()) }
                   )
               );
        }

        public  ApplicationDbContext dbContext()
            => new ApplicationDbContext(_options);

        public UserId UserIdFromClaimsPrincipal()
            => new UserId(Guid.Parse(ClaimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier)!));

        public UserIdDto UserIdDtoFromClaimsPrincipal()
            => new UserIdDto(ClaimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier)!);
    }
}
