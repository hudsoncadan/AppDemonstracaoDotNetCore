# App MVC Demonstra��o .Net Core 3.1
App MVC .Net Core 3.1  para o curso "Dominando o ASP.NET MVC Core" de [Desenvolvedor.IO](https://desenvolvedor.io/).

## Objetivos e Funcionalidades
Projeto focado em facilitar a manuten��o e inclus�o de novas funcionalidades.

- Login e Controle de Permiss�es com Claims:
  - Cria��o do __ClaimsAuthorizeAttribute__ atrav�s do *TypeFilterAttribute* para autoriza��o nas classes Controllers. Veja exemplo de aplica��o em [Exemplos de C�digo](#-exemplos-de-c�digo).

- Cadastro de Produtos:
  - Vincula��o de Fornecedor (FK);
  - Possibilidade de inclus�o de Imagens;
  - Globaliza��o de valores reais para pt-BR.

- Cadastro de Fornecedores:
  - Formata��o de Documentos com Extensions para RazorPage;
  - Listagem de Endere�o e Produtos com PartialView;
  - Edi��o de Endere�o com Bootstrap Modal;
  - Consulta de CEP por Ajax ViaCep.

## Estrutura
A Solu��o est� estruturada em tr�s Projetos: __UI__, __Business__ e __Data__.
  - O c�digo fonte est� dispon�vel na pasta *src*.
  - Um resumo da estrutura do projeto est� dispon�vel em [AppDemonstracao](http://appdemonstracao.us-east-1.elasticbeanstalk.com/).

## Exemplos de C�digo
- SummaryViewComponent
  - ViewComponent criado para exibir as mensagens de valida��es da Camada de Business.

  - Exemplos de como visualizar o SummaryViewComponent:
    - Cadastre um Fornecedor com Documento inv�lido;
    - Exclua uma Fornecedor com Produtos cadastrados.

- TagHelpers
  - Cria��o de TagHelpers para exibir/ocultar elementos HTML na p�gina baseado na Action que o usu�rio est� ou nas Claims do Usu�rio.

O c�digo abaixo exibe um link para a Controller Produtos, Action Delete, apenas se o usu�rio estiver na p�gina *Index* ou *Edit*; e ainda apenas se o usu�rio possuir a Claim *"Produtos.Delete"*.

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

- __ClaimsAuthorizeAttribute__ para controle de autoriza��o.

O c�digo abaixo exemplifica como utilizar o __ClaimsAuthorizeAttribute__. 
Apenas usu�rios que possu�rem a Claim *Produtos.Delete* poder�o excluir produtos:

```csharp
[ClaimsAuthorize("Produtos", "Delete")]
[Route("Excluir/{id:guid}")]
public async Task<IActionResult> Delete(Guid id)
{
    // hidden for brevity
}
```