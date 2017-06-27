using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    [Produces("application/json")]
    [Route("api/Filial")]
    public class FilialController : Controller
    {
        private readonly IFilialService _iFilialService;

        public FilialController(IFilialService iFilialService)
        {
            _iFilialService = iFilialService;
        }

        [HttpGet("BuscarFilial")]
        public IEnumerable<Filial> Index()
        {
            return _iFilialService.BuscarListaPorNome("%");
        }
    }
}
