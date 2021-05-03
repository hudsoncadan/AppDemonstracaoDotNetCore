using DevIO.Business.Interfaces;
using DevIO.Business.Models;
using DevIO.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevIO.Data.Repository
{
    public class ProdutoRepository : Repository<Produto>, IProdutoRepository
    {
        public ProdutoRepository(AppDemonstracaoContext context) : base(context)
        {
        }

        public async Task<Produto> GetProdutoComFornecedor(Guid produtoId)
        {
            return await DbContext.Produtos
                .AsNoTracking()
                .Include(p => p.Fornecedor)
                .FirstOrDefaultAsync(p => p.Id == produtoId);
        }

        public async Task<IEnumerable<Produto>> GetProdutosFornecedores()
        {
            return await DbContext.Produtos
                .AsNoTracking()
                .Include(p => p.Fornecedor)
                .OrderBy(p => p.Nome)
                .ToListAsync();
        }

        public async Task<IEnumerable<Produto>> GetProdutosPorFornecedor(Guid fornecedorId)
        {
            return await Search(p => p.FornecedorId == fornecedorId);
        }
    }
}
