using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using PriceParser.Data.Entities;
using System.Threading.Tasks;

namespace PriceParser.Domain.Tests
{
    [TestFixture]
    public class UserJWTServiceTests
    {
        private const string userName = "test@test.com";
        private const string password = "12345678";
        private UserJWTService _userJWTService;
        private Mock<IConfiguration> _configuration;
        private Mock<UserManager<ApplicationUser>> _userManager;
        private Mock<ApplicationDbContext> _context;
        [SetUp]
        public void Setup()
        {
            _configuration = new Mock<IConfiguration>();
            var store = new Mock<IUserStore<ApplicationUser>>();
            _userManager = new Mock<UserManager<ApplicationUser>>(store.Object, null, null, null, null, null, null, null, null);
            _context = new Mock<ApplicationDbContext>(new DbContextOptions<ApplicationDbContext>());
            
            _configuration.SetupGet(x => x[It.Is<string>(s => s == "JWT:ValidAudience")])
                .Returns("Audience");
            _configuration.SetupGet(x => x[It.Is<string>(s => s == "JWT:ValidIssuer")])
                .Returns("Issuer");
            _configuration.SetupGet(x => x[It.Is<string>(s => s == "JWT:Secret")])
                .Returns("FC89DF9B751D1A9C38EC5A1B0B2D193C");

            var user = new ApplicationUser() { UserName = userName };

            _userManager.Setup(x => x.FindByNameAsync(It.Is<string>(y => y == userName))).ReturnsAsync(user);
            _userManager.Setup(x => x.FindByNameAsync(It.Is<string>(y => y != userName))).ReturnsAsync((ApplicationUser)null);
            _userManager.Setup(x => x.CheckPasswordAsync(It.Is<ApplicationUser>(user => user.UserName == userName), It.Is<string>(pass => pass == password))).ReturnsAsync(true);
            _userManager.Setup(x => x.CheckPasswordAsync(It.Is<ApplicationUser>(user => user.UserName == userName), It.Is<string>(pass => pass != password))).ReturnsAsync(false);
            _userManager.Setup(x => x.GetRolesAsync(It.Is<ApplicationUser>(user => user.UserName == userName))).ReturnsAsync(new[] { "Admin", "User" });

            _userJWTService = new UserJWTService(_context.Object, _userManager.Object, _configuration.Object);
        }

        [Test]
        public async Task AuthenticateAsync_OnCorrectUser_ReturnsObjectWithToken()
        {
            var authDto = await _userJWTService.AuthenticateAsync(userName, password, "awd");

            Assert.IsNotEmpty(authDto.JwtToken);
        }

        [Test]
        public async Task AuthenticateAsync_OnWrongPassword_ReturnsNull()
        {
            Assert.IsNull(await _userJWTService.AuthenticateAsync(userName,"awdw","dawd"));
        }

        [Test]
        public async Task AuthenticateAsync_OnWrongUserName_ReturnsNull()
        {
            Assert.IsNull(await _userJWTService.AuthenticateAsync("aklwd","awddw","awdwd"));
        }
    }
}