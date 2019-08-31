using System.Threading.Tasks;
using IAFProject.BLL.Interfaces;
using Quartz;

namespace IAFProjectUserDeleteByLastEntryDateJob.Jobs
{
    public class UserDeleteProcessorJob : IJob
    {
        private readonly IUserDeleteProcessor _userDeleteProcessor;

        public UserDeleteProcessorJob(IUserDeleteProcessor userDeleteProcessor)
        {
            _userDeleteProcessor = userDeleteProcessor;
        }
        public Task Execute(IJobExecutionContext context)
        {
            return _userDeleteProcessor.StartProcessingAsync();
        }
    }
}
