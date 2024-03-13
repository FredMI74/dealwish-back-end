using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace dw_webservice.Repositories
{
    public interface IGrpProdutosRepository
    {
        Task<object> AtualizarGrpProdutoAsync(int id = 0, string descricao = "", int id_situacao = 0, string icone = "", int ordem = 0, long id_usuario_login = 0);
        Task<object> ConsultarGrpProdutoAsync(int id = 0, string descricao = "", int id_situacao = 0);
        Task<object> ConsultarTodosGrpProdutoAsync();
        Task<object> ConsultarUltimaAtualizacaoProdutosAsync();
        Task<object> ExcluirGrpProdutoAsync(int id = 0, long id_usuario_login = 0);
        IConfiguration GetConfiguration();
        Task<object> IncluirGrpProdutoAsync(string descricao = "", string icone = "", int ordem = 0, long id_usuario_login = 0);
    }
}