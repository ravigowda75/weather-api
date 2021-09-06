using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Weather.Core.Enums;
using Weather.Core.Interfaces.Services;

namespace Weather.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class DevicesController : ControllerBase
    {
        private readonly IDeviceService _deviceService;

        public DevicesController(IDeviceService deviceService)
        {
            _deviceService = deviceService;
        }

        /// <summary>
        /// 1. Collect all of the measurements for one day, one sensor type, and one unit.
        /// Ex: /api/v1/devices/testdevice/data/2018-09-18/temperature
        /// 2. Collect all data points for one unit and one day.
        /// Ex: /api/v1/devices/testdevice/data/2018-09-18
        /// </summary>
        /// <param name="deviceId">required parameter</param>
        /// <param name="date">required parameter</param>
        /// <param name="sensorType">optional parameter</param>
        /// <returns></returns>
        [HttpGet]
        [Route("{deviceId}/data/{date}/{sensorType?}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDataAsync(string deviceId, string date, SensorTypeEnum? sensorType)
        {
            bool isDate = DateTime.TryParse(date, out DateTime forDate);

            if (!isDate)
                return BadRequest("Invalid Date format");

            var result = await _deviceService.GetDataAsync(deviceId, date, sensorType).ConfigureAwait(false);
            return Ok(result);
        }
    }
}
