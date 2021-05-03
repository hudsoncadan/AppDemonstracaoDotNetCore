// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(function () {
    $('[data-tooltip="tooltip"]').tooltip()
})


function AddModalConfig() {
    $(document).bind('DOMSubtreeModified', function () {

        // Show the dialog
        $("a[data-modal]").on("click", function (e) {
            // Loads the PartialView to show modal
            $("#modalContent").load(this.href, function () {
                $("#modal").modal({ keyboard: true }, 'show');
                // $.validator.unobtrusive.parse($("form"));
                bindForm(this);
            });
            return false;
        });
    });

    // On submitting the Form, either refresh the Target on the Page or the Dialog
    // The result from the form's submit must be a JSON { success: boolean, update: { url, id } } 
    function bindForm(dialog) {
        $("form", dialog).submit(function () {
            $.ajax({
                url: this.action,
                type: this.method,
                data: $(this).serialize(),
                success: function (result) {
                    if (result.success) {
                        $("#modal").modal('hide');

                        // Se no result conter "update", atualizar o PartialView
                        if (result.update) {
                            $(`#${result.update.id}`).load(result.update.url);
                        }
                    } else {
                        $("#modalContent").html(result);
                        bindForm(dialog);
                    }
                }
            });
            return false;
        });
    }
}

function AddBuscaCepConfig() {
    $(document).ready(function () {

        function limpa_formulário_cep() {
            // Limpa valores do formulário de cep
            $("#Endereco_Logradouro").val("");
            $("#Endereco_Numero").val("");
            $("#Endereco_Complemento").val("");
            $("#Endereco_Bairro").val("");
            $("#Endereco_Cidade").val("");
            $("#Endereco_Estado").val("");
        }

        // Quando o campo cep perde o foco
        $("#Endereco_Cep").change(function () {

            // Nova variável "cep" somente com dígitos.
            var cep = $(this).val().replace(/\D/g, '');

            // Verifica se o campo CEP possui valor informado
            if (cep != "") {

                // Expressão regular para validar o CEP
                var validacep = /^[0-9]{8}$/;

                // Valida o formato do CEP
                if (validacep.test(cep)) {

                    //Preenche os campos com "..." enquanto consulta webservice
                    $("#Endereco_Logradouro").val("...");
                    $("#Endereco_Numero").val("...");
                    $("#Endereco_Complemento").val("...");
                    $("#Endereco_Bairro").val("...");
                    $("#Endereco_Cidade").val("...");
                    $("#Endereco_Estado").val("...");

                    // Consulta o webservice viacep.com.br/
                    $.getJSON("https://viacep.com.br/ws/" + cep + "/json/?callback=?",
                        function (dados) {

                            if (!("erro" in dados)) {
                                //Atualiza os campos com os valores da consulta.
                                $("#Endereco_Logradouro").val(dados.logradouro);
                                $("#Endereco_Numero").val("");
                                $("#Endereco_Complemento").val("");
                                $("#Endereco_Bairro").val(dados.bairro);
                                $("#Endereco_Cidade").val(dados.localidade);
                                $("#Endereco_Estado").val(dados.uf);
                            } // end if.
                            else {
                                // CEP pesquisado não foi encontrado
                                limpa_formulário_cep();
                                alert("CEP não encontrado.");
                            }
                        });
                } // end if
                else {
                    // cep é inválido
                    limpa_formulário_cep();
                    alert("Formato de CEP inválido.");
                }
            } // end if
            else {
                // cep sem valor, limpa formulário
                limpa_formulário_cep();
            }
        });
    });
}