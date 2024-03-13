using dw_webservice.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace dw_webservice.Repositories
{
    public interface IFaturasRepository
    {
        Task<object> AtualizarFaturaAsync(int id = 0, int mes = 0, int ano = 0, string nosso_numero = "", double valor = 0, string data_vct = "", string data_pg = "", double multa = 0, double juros = 0, double valor_pg = 0, int qtd_ofertas = -1, int id_situacao = 0, long id_usuario_login = 0);
        Task<string> BaixarFaturaAsync(TransacaoRetornoBoleto boleto, long id_usuario_login = 0);
        object ConsultarFatura(out bool erro_exportar, long id_usuario_login = 0, int id = 0, int id_empresa = 0, string razao_social = "", int mes = 0, int ano = 0, string nosso_numero = "", int id_situacao = 0, string abertas = "", string exportar = "N");
        Task<object> ConsultarFaturasAbertasAsync(long id_usuario_login);
        Task<object> ConsultarIndicadoresAsync();
        Task<object> EfetivarFaturasAbertasAsync(long id_usuario_login);
        Task<object> ExcluirFaturaAsync(int id = 0, long id_usuario_login = 0);
        IConfiguration GetConfiguration();
        Task<object> IncluirFaturaAsync(int id_empresa = 0, int mes = 0, int ano = 0, string nosso_numero = "", double valor = 0, string data_vct = "", int qtd_ofertas = -1, int id_situacao = 0, long id_usuario_login = 0);
        Task<object> ProcessarRetornoBoletoAsync(IFormFile file = null, long id_usuario_login = 0);
        Task<object> ProcessarRetornoNFAsync(IFormFile file = null, long id_usuario_login = 0);
        Task<string> RegistrarNFFaturaAsync(DetalheRetornoNF nf, long id_usuario_login);
    }
}