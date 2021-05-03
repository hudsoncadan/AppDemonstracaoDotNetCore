using DevIO.Business.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DevIO.Business.Interfaces
{
    public interface IProdutoRepository : IRepository<Produto>
    {
        Task<IEnumerable<Produto>> GetProdutosPorFornecedor(Guid fornecedorId);
        
        Task<IEnumerable<Produto>> GetProdutosFornecedores();
        
        Task<Produto> GetProdutoComFornecedor(Guid produtoId);
    }
}
