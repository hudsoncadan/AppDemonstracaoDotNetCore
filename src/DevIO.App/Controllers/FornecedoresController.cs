using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DevIO.App.ViewModels;
using DevIO.Business.Interfaces;
using AutoMapper;
using DevIO.Business.Models;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using DevIO.App.Extensions;

namespace DevIO.App.Controllers
{
    [Authorize]
    [Route("Fornecedores")]
    public class FornecedoresController : BaseController
    {
        private readonly IFornecedorRepository _fornecedorRepository;
        private readonly IFornecedorService _fornecedorService;
        private readonly IMapper _mapper;
        private readonly INotificator _notificator;

        public FornecedoresController(
            IFornecedorRepository fornecedorRepository,
            IFornecedorService fornecedorService,
            IMapper mapper, 
            INotificator notificator) : base(notificator)
        {
            _fornecedorRepository = fornecedorRepository;
            _fornecedorService = fornecedorService;
            _mapper = mapper;
            _notificator = notificator;
        }

        [AllowAnonymous]
        [Route("Listagem")]
        public async Task<IActionResult> Index()
        {
            return View(_mapper.Map<IEnumerable<FornecedorViewModel>>(await _fornecedorRepository.GetAll()));
        }

        [AllowAnonymous]
        [Route("Detalhes/{id:guid}")]
        public async Task<IActionResult> Details(Guid id)
        {
            var fornecedorViewModel = await ObterFornecedorProdutosEndereco(id);
            if (fornecedorViewModel == null)
            {
                return NotFound();
            }

            return View(fornecedorViewModel);
        }

        [ClaimsAuthorize("Fornecedores", "Add")]
        [Route("Inserir")]
        public IActionResult Create()
        {
            return View();
        }

        [ClaimsAuthorize("Fornecedores", "Add")]
        [Route("Inserir")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FornecedorViewModel fornecedorViewModel)
        {
            if (!ModelState.IsValid) return View(fornecedorViewModel);

            var fornecedor = _mapper.Map<Fornecedor>(fornecedorViewModel);
            await _fornecedorService.Add(fornecedor);

            if (!isValid()) return View(fornecedorViewModel);
            return RedirectToAction(nameof(Index));
        }

        [ClaimsAuthorize("Fornecedores", "Update")]
        [Route("Editar/{id:guid}")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var fornecedorViewModel = await ObterFornecedorProdutosEndereco(id);
            if (fornecedorViewModel == null)
            {
                return NotFound();
            }
            return View(fornecedorViewModel);
        }

        [ClaimsAuthorize("Fornecedores", "Update")]
        [Route("Editar/{id:guid}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, FornecedorViewModel fornecedorViewModel)
        {
            if (id != fornecedorViewModel.Id) return NotFound();
            if (!ModelState.IsValid) return View(fornecedorViewModel);

            var fornecedor = _mapper.Map<Fornecedor>(fornecedorViewModel);
            await _fornecedorService.Update(fornecedor);
          
            if (!isValid()) return View(await ObterFornecedorProdutosEndereco(id));
            return RedirectToAction(nameof(Index));
        }

        [ClaimsAuthorize("Fornecedores", "Delete")]
        [Route("Excluir/{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var fornecedorViewModel = await ObterFornecedorEndereco(id);
            if (fornecedorViewModel == null) return NotFound();

            return View(fornecedorViewModel);
        }

        [ClaimsAuthorize("Fornecedores", "Delete")]
        [Route("Excluir/{id:guid}")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var fornecedorViewModel = await ObterFornecedorEndereco(id);
            if (fornecedorViewModel == null) return NotFound();

            await _fornecedorService.Delete(id);
          
            if (!isValid()) return View(fornecedorViewModel);
       
            TempData["Sucesso"] = "Fornecedor excluído com sucesso";
            return RedirectToAction(nameof(Index));
        }

        [AllowAnonymous]
        [Route("ObterEndereco/{id:guid}")]
        public async Task<IActionResult> ObterEndereco(Guid id)
        {
            var fornecedor = await ObterFornecedorEndereco(id);
            
            if (fornecedor == null)
            {
                return NotFound();
            }

            return PartialView("_EnderecoDetailsTable", fornecedor);
        }

        [ClaimsAuthorize("Fornecedores", "Update")]
        [Route("AtualizarEndereco/{fornecedorId:guid}")]
        public async Task<IActionResult> AtualizarEndereco(Guid fornecedorId)
        {
            var fornecedor = await ObterFornecedorEndereco(fornecedorId);

            if (fornecedor == null)
            {
                return NotFound();
            }

            return PartialView("_AtualizarEnderecoModal", fornecedor);
        }

        [ClaimsAuthorize("Fornecedores", "Update")]
        [Route("AtualizarEndereco/{fornecedorId:guid}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AtualizarEndereco(FornecedorViewModel fornecedor)
        {
            // É possível remmover properties do ModelState para que eles não sejam validados
            // ModelState.Remove("Nome");
            // ModelState.Remove("Documento");
            if (!ModelState.IsValid) return PartialView("_AtualizarEnderecoModal", fornecedor);

            await _fornecedorService.AtualizarEndereco(_mapper.Map<Endereco>(fornecedor.Endereco));
            if (!isValid()) return PartialView("_AtualizarEnderecoModal", fornecedor);

            var url = Url.Action("ObterEndereco", "Fornecedores", new { id = fornecedor.Endereco.FornecedorId });
            // Retorna o objeto Json com a URL que atualizará novamenta o EnderecoDetailsTable
            return Json(new { success = true, update = new { url, id = "EnderecoTarget" } });
        }

        private async Task<FornecedorViewModel> ObterFornecedorEndereco(Guid id)
        {
            return _mapper.Map<FornecedorViewModel>(await _fornecedorRepository.GetFornecedorEndereco(id));
        }

        private async Task<FornecedorViewModel> ObterFornecedorProdutosEndereco(Guid id)
        {
            return _mapper.Map<FornecedorViewModel>(await _fornecedorRepository.GetFornecedorProdutosEndereco(id));
        }
    }
}
