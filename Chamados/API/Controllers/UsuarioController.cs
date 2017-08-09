using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Produces("application/json")]
    [Route("api/Usuario")]
    public class UsuarioController : Controller
    {
        private readonly IUsuarioService _iUsuarioService;

        public UsuarioController(IUsuarioService iUsuarioService)
        {
            _iUsuarioService = iUsuarioService;
        }
        [HttpGet("Autenticar/{usuario}/{senha}")]
        public IActionResult Autenticar(string usuario, string senha)
        {
            try
            {
                return Ok(_iUsuarioService.Autenticar(Convert.ToInt32(usuario), senha));
            }
            catch (Exception e)
            {
                return StatusCode(403, e.Message);
            }
        }
    }
}