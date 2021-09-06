using System.Collections.Generic;
using System.Threading.Tasks;
using Weather.Core.Enums;
using Weather.Core.Models.Response;

namespace Weather.Core.Interfaces.Services
{
    public interface IDeviceService
    {
        Task<IEnumerable<DeviceResponse>> GetDataAsync(string deviceId, string date, SensorTypeEnum? sensorType);
    }
}
