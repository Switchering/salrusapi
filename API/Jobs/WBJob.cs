using Quartz;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
 
namespace QuartzApp.Jobs
{
    public class WBJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
           
        }

        private async Task ExecuteWBApi2()
        {
            
        }
    }
}