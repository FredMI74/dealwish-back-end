using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace dw_webservice.Repositories
{
    public interface IConfigRepository
    {
        Task<object> AtualizarConfigAsync(int id = 0, string codigo = "", string valor = "", long id_usuario_login = 0);
        Task<string> AtualizarValorConfigAsync(string codigo, string valor);
        Task<object> ConsultarConfigAsync(int id = 0, string codigo = "", string valor = "");
        Task<string> ConsultarValorConfigAsync(string codigo);
        IConfiguration GetConfiguration();
    }
}