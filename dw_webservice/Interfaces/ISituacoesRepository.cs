using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace dw_webservice.Repositories
{
    public interface ISituacoesRepository
    {
        Task<object> ConsultarSituacaoAsync(int id = 0, string descricao = "", string contratos = "", string usuarios = "", string desejos = "", string faturas = "", string produtos = "", string ofertas = "", string origem = "");
        IConfiguration GetConfiguration();
    }
}