namespace ESOC.CLTPull.ExecutionEngine.API.Controllers
{
    using ESOC.CLTPull.ExecutionEngine.Core.Contracts;
    using ESOC.CLTPull.ExecutionEngine.Core.Models;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;

    [ApiController]
    [Route("[controller]")]
    public class ControlExecutionResultsController : ControllerBase
    {

        private readonly ILogger<ControlExecutionResultsController> _logger;
        private readonly IControlExecutionResultsService _service;

        public ControlExecutionResultsController(ILogger<ControlExecutionResultsController> logger,
            IControlExecutionResultsService service)
        {
            _logger = logger;
            _service = service;
        }



        [HttpGet("TriggerControlExecution")]
        [ProducesResponseType(typeof(List<ControlExecutionResult>), (int)HttpStatusCode.OK)]
        public IActionResult TriggerControlExecution(string controlEvent)
        {
            try
            {
                //TODO: change it to right type for deserialization 
                //TODO: Cleanup this code after writing the integration test case 
                string currentDirectory = Directory.GetCurrentDirectory();
                var filePath = Path.Combine(currentDirectory, "json1.json");
                controlEvent = System.IO.File.ReadAllText(filePath);
                var controlExecutionevent = new ControlExecutionEvent() { Message = controlEvent };
                var records = _service.TriggerControlExecution(controlExecutionevent);
                return Ok(records);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return BadRequest();
            }
        }

    }
}
