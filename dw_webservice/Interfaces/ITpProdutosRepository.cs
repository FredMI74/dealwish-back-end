using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace dw_webservice.Repositories
{
    public interface ITpProdutosRepository
    {
        Task<object> AtualizarTpProdutoAsync(int id = 0, string descricao = "", int id_grp_prod = 0, int id_situacao = 0, string preenchimento = "", string icone = "", int ordem = 0, long id_usuario_login = 0);
        Task<object> ConsultarTpProdutoAsync(int id = 0, string descricao = "", int id_grp_prod = 0, int id_situacao = 0, string paginacao = "N", int max_id = 0, int pagina = 1);
        Task<object> ExcluirTpProdutoAsync(int id = 0, long id_usuario_login = 0);
        IConfiguration GetConfiguration();
        Task<object> IncluirTpProdutoAsync(string descricao = "", int id_grp_prod = 0, string preenchimento = "", string icone = "", int ordem = 0, long id_usuario_login = 0);
    }
}