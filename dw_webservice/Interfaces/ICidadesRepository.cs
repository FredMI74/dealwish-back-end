using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace dw_webservice.Repositories
{
    public interface ICidadesRepository
    {
        Task<object> ConsultarCidadeAsync(int id = 0, string nome = "", string nome_exato = "", string uf = "");
        IConfiguration GetConfiguration();
    }
}