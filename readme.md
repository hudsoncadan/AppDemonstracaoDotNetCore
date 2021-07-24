# App MVC Demonstração .Net Core 3.1
App MVC .Net Core 3.1  para o curso "Dominando o ASP.NET MVC Core" de [Desenvolvedor.IO](https://desenvolvedor.io/).

## Objetivos e Funcionalidades
Projeto focado em facilitar a manutenção e inclusão de novas funcionalidades.

- Login e Controle de Permissões com Claims:
  - Criação do __ClaimsAuthorizeAttribute__ através do *TypeFilterAttribute* para autorização nas classes Controllers. Veja exemplo de aplicação em [Exemplos de Código](#-exemplos-de-código).

- Cadastro de Produtos:
  - Vinculação de Fornecedor (FK);
  - Possibilidade de inclusão de Imagens;
  - Globalização de valores reais para pt-BR.

- Cadastro de Fornecedores:
  - Formatação de Documentos com Extensions para RazorPage;
  - Listagem de Endereço e Produtos com PartialView;
  - Edição de Endereço com Bootstrap Modal;
  - Consulta de CEP por Ajax ViaCep.

## Estrutura
A Solução está estruturada em três Projetos: __UI__, __Business__ e __Data__.
  - O código fonte está disponível na pasta *src*.
  - Um resumo da estrutura do projeto está disponível em [AppDemonstracao](http://appdemonstracao.us-east-1.elasticbeanstalk.com/).

## Exemplos de Código
- SummaryViewComponent
  - ViewComponent criado para exibir as mensagens de validações da Camada de Business.

  - Exemplos de como visualizar o SummaryViewComponent:
    - Cadastre um Fornecedor com Documento inválido;
    - Exclua uma Fornecedor com Produtos cadastrados.

- TagHelpers
  - Criação de TagHelpers para exibir/ocultar elementos HTML na página baseado na Action que o usuário está ou nas Claims do Usuário.

O código abaixo exibe um link para a Controller Produtos, Action Delete, apenas se o usuário estiver na página *Index* ou *Edit*; e ainda apenas se o usuário possuir a Claim *"Produtos.Delete"*.

```html
<a 
    show-by-action="Index,Edit" 
    suppress-by-claim-name="Produtos" 
    suppress-by-claim-value="Delete" 
    asp-controller="Produtos" 
    asp-action="Delete" 
    asp-route-id="@item.Id" 
    class="btn btn-outline-info" 
    data-tooltip="tooltip" 
    title="Excluir Produto" 
    data-placement="bottom">
        <span class="fa fa-trash"></span>
</a>
```

- __ClaimsAuthorizeAttribute__ para controle de autorização.

O código abaixo exemplifica como utilizar o __ClaimsAuthorizeAttribute__. 
Apenas usuários que possuírem a Claim *Produtos.Delete* poderão excluir produtos:

```csharp
[ClaimsAuthorize("Produtos", "Delete")]
[Route("Excluir/{id:guid}")]
public async Task<IActionResult> Delete(Guid id)
{
    // hidden for brevity
}
```