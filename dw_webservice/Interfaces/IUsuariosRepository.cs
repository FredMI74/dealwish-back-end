using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace dw_webservice.Repositories
{
    public interface IUsuariosRepository
    {
        Task<object> AtualizarUsuarioAsync(long id = 0, string email = "", string nome = "", string data_nasc = "", string cpf = "", string aplicativo = "", string retaguarda = "", string empresa = "", int id_situacao = 0, int id_cidade_ap = 0, int id_empresa = 0, long id_usuario_login = 0);
        Task<object> ConsultarGrpPermissoesUsuarioAsync(long id_usuario = 0);
        Task<string> ConsultarListaPermUsuarioAsync(long id_usuario = 0);
        Task<object> ConsultarPermissoesUsuarioAsync(long id_usuario = 0);
        Task<object> ConsultarTermoServicoAsync();
        Task<object> ConsultarUsuarioAsync(long id = 0, string email = "", string nome = "", string cpf = "", string aplicativo = "", string retaguarda = "", string empresa = "", int id_situacao = 0, int id_cidade_ap = 0, string uf = "", int id_empresa = 0, string origem = "");
        Task<object> ExcluirGrpPermissaoUsuarioAsync(long id = 0, long id_usuario_login = 0);
        Task<object> ExcluirUsuarioAsync(long id = 0, long id_usuario_login = 0, string origem = "");
        IConfiguration GetConfiguration();
        Task<object> IncluirGrpPermissaoUsuarioAsync(long id_usuario = 0, int id_grp_permissao = 0, long id_usuario_login = 0);
        Task<object> IncluirUsuarioAsync(string email = "", string senha1 = "", string senha2 = "", string nome = "", string data_nasc = "", string cpf = "", string aplicativo = "", string retaguarda = "", string empresa = "", int id_situacao = 0, int id_cidade_ap = 0, int id_empresa = 0, long id_usuario_login = 0);
        Task<object> LoginUsuarioAsync(string email = "", string senha = "", string origem = "", string token_app = "");
        Task<object> ReiniciarSenhaAsync(string email = "", string cpf = "", long id_usuario_login = 0);
        Task<object> TrocarSenhaAsync(string email = "", string senha_atual = "", string senha_nova = "", string senha_nova_conf = "", long id_usuario_login = 0);
    }
}