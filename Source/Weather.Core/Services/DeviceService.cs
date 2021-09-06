using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Weather.Core.Enums;
using Weather.Core.Interfaces.Repositories;
using Weather.Core.Interfaces.Services;
using Weather.Core.Models.Response;

namespace Weather.Core.Services
{
    public class DeviceService : IDeviceService
    {
        private readonly IDeviceRepository _deviceRepository;
        public DeviceService(IDeviceRepository deviceRepository)
        {
            _deviceRepository = deviceRepository;
        }

        public async Task<IEnumerable<DeviceResponse>> GetDataAsync(string deviceId, string date, SensorTypeEnum? sensorType)
        {
            var toReturn = Enumerable.Empty<DeviceResponse>();

            if(sensorType.HasValue)
            {
                var dataContent = await _deviceRepository.GetDeviceDataAsync(deviceId, date, sensorType?.ToString()?.ToLower()).ConfigureAwait(false);
                toReturn = await GetDeviceModelsAsync(dataContent).ConfigureAwait(false);
            }
            else
            {
                var tempData = await _deviceRepository.GetDeviceDataAsync(deviceId, date, SensorTypeEnum.Temperature.ToString()?.ToLower()).ConfigureAwait(false);
                var rainfalldata = await _deviceRepository.GetDeviceDataAsync(deviceId, date, SensorTypeEnum.Rainfall.ToString()?.ToLower()).ConfigureAwait(false);
                var humidityData = await _deviceRepository.GetDeviceDataAsync(deviceId, date, SensorTypeEnum.Humidity.ToString()?.ToLower()).ConfigureAwait(false);

                toReturn = toReturn.ToList()
                    .Union(await GetDeviceModelsAsync(tempData).ConfigureAwait(false))
                    .Union(await GetDeviceModelsAsync(rainfalldata).ConfigureAwait(false))
                    .Union(await GetDeviceModelsAsync(humidityData).ConfigureAwait(false));
            }

            return toReturn;
        }

        private async Task<IEnumerable<DeviceResponse>> GetDeviceModelsAsync(Stream content)
        {
            var toReturn = new List<DeviceResponse>();
            if (content?.Length > 0)
            {
                using (var streamReader = new StreamReader(content))
                {
                    while (!streamReader.EndOfStream)
                    {
                        var line = await streamReader.ReadLineAsync();
                        var columns = line.Split(',');

                        toReturn.Add(new DeviceResponse
                        {
                            Humidity = columns[0],
                            RainFall = columns[1],
                            Temprature = columns[2]

                        });
                    }
                }
            }
            return toReturn;
        }
    }
}
