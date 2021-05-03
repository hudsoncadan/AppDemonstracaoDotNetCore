using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DevIO.App.ViewModels;
using DevIO.Business.Interfaces;
using AutoMapper;
using DevIO.Business.Models;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Authorization;
using DevIO.App.Extensions;

namespace DevIO.App.Controllers
{
    [Authorize]
    [Route("Produtos")]
    public class ProdutosController : BaseController
    {
        private readonly IProdutoRepository _produtoRepository;
        private readonly IFornecedorRepository _fornecedorRepository;
        private readonly IProdutoService _produtoService;
        private readonly IMapper _mapper;
        private readonly INotificator _notificator;

        public ProdutosController(
            IProdutoRepository repository,
            IFornecedorRepository fornecedorRepository,
            IMapper mapper,
            IProdutoService produtoService, 
            INotificator notificator) : base(notificator)
        {
            _produtoRepository = repository;
            _fornecedorRepository = fornecedorRepository;
            _mapper = mapper;
            _produtoService = produtoService;
            _notificator = notificator;
        }

        [AllowAnonymous]
        [Route("Listagem")]
        public async Task<IActionResult> Index()
        {
            return View(_mapper.Map<IEnumerable<ProdutoViewModel>>(await _produtoRepository.GetProdutosFornecedores()));
        }

        [AllowAnonymous]
        [Route("Detalhes/{id:guid}")]
        public async Task<IActionResult> Details(Guid id)
        {
            var produtoViewModel = await ObterProduto(id);
            if (produtoViewModel == null) return NotFound();

            return View(produtoViewModel);
        }

        [ClaimsAuthorize("Produtos", "Add")]
        [Route("Inserir")]
        public async Task<IActionResult> Create()
        {
            // ViewData["FornecedorId"] = new SelectList(_context.Set<FornecedorViewModel>(), "Id", "Documento");
            var produtoViewModel = await PopularFornecedores(new ProdutoViewModel());
            return View(produtoViewModel);
        }

        [ClaimsAuthorize("Produtos", "Add")]
        [Route("Inserir")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProdutoViewModel produtoViewModel)
        {
            produtoViewModel = await PopularFornecedores(produtoViewModel);

            if (!ModelState.IsValid) return View(produtoViewModel);

            var imgPrefixo = Guid.NewGuid() + "_";
            if (!await UploadArquivo(produtoViewModel.ImagemUpload, imgPrefixo))
            {
                return View(produtoViewModel);
            }
            produtoViewModel.Imagem = imgPrefixo + produtoViewModel.ImagemUpload.FileName;
            
            await _produtoService.Add(_mapper.Map<Produto>(produtoViewModel));
            
            if (!isValid()) return View(produtoViewModel);
            return RedirectToAction(nameof(Index));
        }

        [ClaimsAuthorize("Produtos", "Update")]
        [Route("Editar/{id:guid}")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var produtoViewModel = await ObterProduto(id);
            if (produtoViewModel == null) return NotFound();
            return View(produtoViewModel);
        }

        [ClaimsAuthorize("Produtos", "Update")]
        [Route("Editar/{id:guid}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, ProdutoViewModel produtoViewModel)
        {
            if (id != produtoViewModel.Id) return NotFound();

            var produtoDb = await ObterProduto(id);
            produtoViewModel.FornecedorId = produtoDb.FornecedorId;
            produtoViewModel.Imagem = produtoDb.Imagem;

            if (!ModelState.IsValid) return View(produtoViewModel);

            // Se houver intenção de atualizar a imagem
            if (produtoViewModel.ImagemUpload != null)
            {
                var imgPrefixo = Guid.NewGuid() + "_";
                if (!await UploadArquivo(produtoViewModel.ImagemUpload, imgPrefixo))
                {
                    return View(produtoViewModel);
                }
                produtoViewModel.Imagem = imgPrefixo + produtoViewModel.ImagemUpload.FileName;
            }

            // Atualiza apenas os campos permitidos, evitando alterar, por exemplo, o FornecedorId 
            //produtoDb.Nome = produtoViewModel.Nome;
            //produtoDb.Descricao = produtoViewModel.Descricao;
            //produtoDb.Valor = produtoViewModel.Valor;
            //produtoDb.Ativo = produtoViewModel.Ativo;

            await _produtoService.Update(_mapper.Map<Produto>(produtoViewModel));
            
            if (!isValid()) return View(produtoViewModel);
            return RedirectToAction(nameof(Index));
        }

        [ClaimsAuthorize("Produtos", "Delete")]
        [Route("Excluir/{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var produtoViewModel = await ObterProduto(id);
            if (produtoViewModel == null) return NotFound();

            return View(produtoViewModel);
        }

        [ClaimsAuthorize("Produtos", "Delete")]
        [Route("Excluir/{id:guid}")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var produtoViewModel = await ObterProduto(id);
            if (produtoViewModel == null) return NotFound();

            await _produtoService.Delete(produtoViewModel.Id);

            if (!isValid()) return View(produtoViewModel);

            TempData["Sucesso"] = "Produto excluído com sucesso";
            return RedirectToAction(nameof(Index));
        }

        private async Task<ProdutoViewModel> ObterProduto(Guid id)
        {
            var produto = _mapper.Map<ProdutoViewModel>(await _produtoRepository.GetProdutoComFornecedor(id));
            produto.Fornecedores = _mapper.Map<IEnumerable<FornecedorViewModel>>(await _fornecedorRepository.GetAll());
            return produto;
        }

        private async Task<ProdutoViewModel> PopularFornecedores(ProdutoViewModel produtoViewModel)
        {
            produtoViewModel.Fornecedores = _mapper.Map<IEnumerable<FornecedorViewModel>>(await _fornecedorRepository.GetAll());
            return produtoViewModel;
        }

        private async Task<bool> UploadArquivo(IFormFile arquivo, string imgPrefixo)
        {
            if (arquivo.Length < 0) return false;

            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/imagens", imgPrefixo + arquivo.FileName);
            if (System.IO.File.Exists(path))
            {
                ModelState.AddModelError(string.Empty, "Já existe um arquivo com este nome!");
                return false;
            }

            using(var stream = new FileStream(path, FileMode.Create))
            {
                await arquivo.CopyToAsync(stream);
            }
            return true;
        }
    }
}
