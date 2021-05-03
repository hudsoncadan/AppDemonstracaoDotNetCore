using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using DevIO.App.Models;

namespace DevIO.App.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        //public IActionResult Error()
        //{
        //    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //}

        [Route("erro/{id:length(3, 3)}")]
        public IActionResult Erros(int id)
        {
            var modelErro = new ErrorViewModel();

            switch (id)
            {
                case 500:
                    modelErro.Mensagem = "Ocorreu um erro! Tente novamente mais tarde ou contate nosso suporte.";
                    modelErro.Titulo = "Ocorreu um erro";
                    modelErro.StatusCode = id;
                    break;
                case 404:
                    modelErro.Mensagem = "Página não encontrada! Em caso de dúvidas, contate nosso suporte.";
                    modelErro.Titulo = "Ops... Página não encontrada";
                    modelErro.StatusCode = id;
                    break;
                case 403:
                    modelErro.Mensagem = "Parece que você não possui permissão para continuar.";
                    modelErro.Titulo = "Acesso Negado";
                    modelErro.StatusCode = id;
                    break;
                default:
                    return StatusCode(404);
            }

            return View("Error", modelErro);
        }
    }
}
