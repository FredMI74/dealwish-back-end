using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace dw_webservice.Repositories
{
    public interface IPermissoesRepository
    {
        Task<object> ConsultarPermissaoAsync(int id = 0, string descricao = "", string codigo = "");
        IConfiguration GetConfiguration();
    }
}