using System.IO;
using System.Threading.Tasks;

namespace Weather.Core.Interfaces.Repositories
{
    public interface IDeviceRepository
    {
        Task<Stream> GetDeviceDataAsync(string deviceId, string date, string sensorType);
    }
}
