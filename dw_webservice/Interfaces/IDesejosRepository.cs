using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace dw_webservice.Repositories
{
    public interface IDesejosRepository
    {
        Task<object> AtualizarDesejoAsync(long id = 0, string descricao = "", long id_usuario = 0, int id_tipo_produto = 0, int id_situacao = 0);
        Task<object> AtualizarSituacaoDesejoAsync(long id = 0, int id_situacao = 0);
        Task<object> ConsultarDesejoAsync(long id_usuario_login = 0, long id = 0, string descricao = "", long id_usuario = 0, int id_tipo_produto = 0, int id_situacao = 0, string oferta = "", int id_empresa_oferta = 0, string uf = "", int id_cidade = 0, string paginacao = "N", int max_id = 0, int pagina = 1, string exportar = "N");
        Task<object> ExcluirDesejoAsync(long id = 0);
        IConfiguration GetConfiguration();
        Task<object> IncluirDesejoAsync(string descricao = "", long id_usuario = 0, int id_tipo_produto = 0, int id_situacao = 0);
    }
}