using ESOC.CLTPull.ExecutionEngine.BusinessRules;
using ESOC.CLTPull.ExecutionEngine.Core.Contracts;
using ESOC.CLTPull.ExecutionEngine.Core.Entities.ExecutionEngine;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Threading.Tasks;

namespace ESOC.CLTPull.ExecutionEngine.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BusinessRulesController : ControllerBase
    {
        private readonly IBusinessRulesService _businessRulesService;

        public BusinessRulesController(IBusinessRulesService businessRulesService)
        {
            _businessRulesService = businessRulesService;
        }
        [HttpGet("BusinessRules/{ControlType}")]
        public IActionResult GetBusinessRules(string controlType)
        {
            return Ok(_businessRulesService.GetBusinessRules(controlType));
           
        }
    }
}
