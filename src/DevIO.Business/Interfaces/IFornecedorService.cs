using DevIO.Business.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DevIO.Business.Interfaces
{
    public interface IFornecedorService : IDisposable
    {
        Task Add(Fornecedor fornecedor);
        
        Task Update(Fornecedor fornecedor);
        
        Task Delete(Guid id);
        
        Task AtualizarEndereco(Endereco endereco);
    }
}
