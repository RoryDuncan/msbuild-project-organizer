using csproj_sorter.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace csproj_sorter.Services
{
    public interface IGroupingService
    {
        void GroupBy();
    }

    public class GroupingService : IGroupingService
    {
        private readonly ILogger<TestService> _logger;
        private readonly AppSettings _config;

        public GroupingService(ILogger<TestService> logger,
            IOptions<AppSettings> config)
        {
            _logger = logger;
            _config = config.Value;
        }

        public void GroupBy()
        {
            throw new System.NotImplementedException();
        }
    }
}