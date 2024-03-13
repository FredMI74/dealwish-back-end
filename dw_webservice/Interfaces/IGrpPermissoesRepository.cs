using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace dw_webservice.Repositories
{
    public interface IGrpPermissoesRepository
    {
        Task<object> ConsultarGrpPermissaoAsync(int id = 0, long id_usuario_login = 0, string descricao = "", string origem = "");
        Task<object> ConsultarPermissaoGrupoAsync(int id_grp_permissao = 0);
        IConfiguration GetConfiguration();
    }
}