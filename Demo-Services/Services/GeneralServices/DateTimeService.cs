using Demo_Models;
using System.Net.Http;
using System.Text.Json;

namespace Demo_Services.Services.DateTimeServices
{
    public class DateTimeService : IDateTimeService
    {
        public DateTime GetNow()
        {
            return DateTime.Now;
        }
    }
}