using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace dw_webservice.Repositories
{
    public interface IContratosRepository
    {
        Task<object> AtualizarContratoAsync(int id = 0, int id_empresa = 0, int id_plano = 0, int id_situacao = 0, int dia_vct = 0, string data_inicio = "", string data_bloqueio = "", string data_termino = "", long id_usuario_login = 0);
        Task<object> BloquearContratosInadimplentesAsync(long id_usuario_login = 0);
        Task<object> ConsultarContratoAsync(int id = 0, int id_empresa = 0, int id_plano = 0, int id_situacao = 0, int dia_vct = 0, string data_inicio = "", string data_bloqueio = "", string data_termino = "", string inadimplentes = "");
        Task<object> DesbloquearContratoAsync(int id = 0, long id_usuario_login = 0);
        Task<object> ExcluirContratoAsync(int id = 0, long id_usuario_login = 0);
        IConfiguration GetConfiguration();
        Task<object> IncluirContratoAsync(int id_empresa = 0, int id_plano = 0, int id_situacao = 0, int dia_vct = 0, string data_inicio = "", long id_usuario_login = 0);
    }
}