﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WFConFin.Models;

namespace WFConFin.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : Controller
    {
        private static List<State> states = new List<State>();

        #region GetStates
        [HttpGet("states")]
        public IActionResult GetStatus()
        {
            return Ok(states);
        }
        #endregion

        #region PostState
        [HttpPost("state")]
        public IActionResult PostState([FromBody] State state)
        {
            states.Add(state);
            return Ok("Estado cadastrado com sucesso!");
        }
        #endregion

        [HttpGet("info1")]
        public IActionResult Index1()
        {
            var result = "Busca concuida com sucesso";
            return Ok("Estado cadastrado com sucesso!");
        }

        [HttpGet("info2")]
        public IActionResult Index2()
        {
            var result = "Busca concuida com sucesso";
            return Ok(result);
        }

        [HttpGet("info3/{valor}")]
        public IActionResult Index3([FromRoute] string valor)
        {
            var result = $"A informação é {valor}";
            return Ok(result);
        }

        [HttpPost("info4")]
        public IActionResult Index4([FromQuery] string valor)
        {
            var result = $"A informação do body é {valor}";
            return Ok(result);
        }

        [HttpPost("info5")]
        public IActionResult Index5([FromHeader] string valor)
        {
            var result = $"A informação do body é {valor}";
            return Ok(result);
        }

        [HttpPost("info6")]
        public IActionResult Index6([FromBody] Body Body)
        {
            var result = $"A informação do body é {Body.valor}";
            return Ok(result);
        }
    }

    public class Body
    {
        public string valor { get; set; }
    }
}
