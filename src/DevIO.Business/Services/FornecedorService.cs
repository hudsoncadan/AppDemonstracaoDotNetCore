using DevIO.Business.Interfaces;
using DevIO.Business.Models;
using DevIO.Business.Validations;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DevIO.Data.Services
{
    public class FornecedorService : BaseService, IFornecedorService
    {
        private readonly IFornecedorRepository _fornecedorRepository;
        private readonly IEnderecoRepository _enderecoRepository;

        public FornecedorService(
            IEnderecoRepository enderecoRepository, 
            IFornecedorRepository fornecedorRepository,
            INotificator notificator) : base(notificator)
        {
            _enderecoRepository = enderecoRepository;
            _fornecedorRepository = fornecedorRepository;
        }

        public async Task Add(Fornecedor fornecedor)
        {
            // Validar o estado da Entidade
            if (!RunValidation(new FornecedorValidation(), fornecedor)
                || !RunValidation(new EnderecoValidation(), fornecedor.Endereco))
            {
                return;
            }

            // Validar se existe fornecedor com o mesmo documento
            if ((await _fornecedorRepository.Search(f => f.Documento == fornecedor.Documento)).Any())
            {
                Notify("Documento já cadastrado");
                return;
            }

            await _fornecedorRepository.Add(fornecedor);
        }

        public async Task Update(Fornecedor fornecedor)
        {
            if (!RunValidation(new FornecedorValidation(), fornecedor))
            {
                return;
            }

            if ((await _fornecedorRepository.Search(f => f.Documento == fornecedor.Documento && f.Id != fornecedor.Id)).Any())
            {
                Notify("Documento já cadastrado");
                return;
            }

            await _fornecedorRepository.Update(fornecedor);
        }

        public async Task AtualizarEndereco(Endereco endereco)
        {
            if (!RunValidation(new EnderecoValidation(), endereco))
            {
                return;
            }

            await _enderecoRepository.Update(endereco);
        }

        public async Task Delete(Guid id)
        {
            var fornecedor = await _fornecedorRepository.GetFornecedorProdutosEndereco(id);
            if (fornecedor.Produtos.Any())
            {
                Notify("O Fornecedor possui produtos cadastrados!");
                return;
            }

            await _enderecoRepository.Delete(fornecedor.Endereco.Id);
            await _fornecedorRepository.Delete(id);
        }

        public void Dispose()
        {
            _fornecedorRepository?.Dispose();
            _enderecoRepository?.Dispose();
        }
    }
}
