using DevIO.Business.Interfaces;
using DevIO.Business.Models;
using DevIO.Business.Validations;
using System;
using System.Threading.Tasks;

namespace DevIO.Data.Services
{
    public class ProdutoService : BaseService, IProdutoService
    {
        private readonly IProdutoRepository _produtoRepository;

        public ProdutoService(
            IProdutoRepository produtoRepository,
            INotificator notificator) : base(notificator)
        {
            _produtoRepository = produtoRepository;
        }

        public async Task Add(Produto produto)
        {
            if (!RunValidation(new ProdutoValidation(), produto))
            {
                return;
            }

            await _produtoRepository.Add(produto);
        }

        public async Task Update(Produto produto)
        {
            if (!RunValidation(new ProdutoValidation(), produto))
            {
                return;
            }
           
            await _produtoRepository.Update(produto);
        }

        public async Task Delete(Guid id)
        {
            await _produtoRepository.Delete(id);
        }

        public void Dispose()
        {
            _produtoRepository?.Dispose();
        }
    }
}
