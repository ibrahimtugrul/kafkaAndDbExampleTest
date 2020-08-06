using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace kafkaAndDbPairingTest.IntegrationTests
{
    public class DefaultTestStartup : AbstractTestStartup
    {
        public DefaultTestStartup(IConfiguration configuration) : base(configuration)
        {
        }

        protected override void AddExtraServices(IServiceCollection services)
        {
            
        }
    }
}