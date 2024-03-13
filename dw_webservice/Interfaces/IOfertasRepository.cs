using dw_webservice.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace dw_webservice.Repositories
{
    public interface IOfertasRepository
    {
        Task<object> AtualizarLidaOfertaAsync(long id = 0, long id_usuario_login = 0);
        Task<object> AtualizarLikeUnlikeOfertaAsync(long id = 0, string like_unlike = "", long id_usuario_login = 0);
        Task<object> AtualizarOfertaAsync(long id = 0, long id_desejo = 0, int id_empresa = 0, string validade = "", double valor = 0, string url = "", string descricao = "", long id_usuario_login = 0);
        Task<object> AtualizarSituacaoOfertaAsync(long id = 0, int id_situacao = 0, int id_empresa = 0, long id_usuario_login = 0);
        Task<object> ConsultarOfertaAsync(long id = 0, int id_fatura = 0, long id_desejo = 0, int id_empresa = 0, int id_situacao = 0, string validade = "", double valor = 0, string url = "", string descricao = "", string origem = "", string data_ini = "", string data_fim = "", string paginacao = "N", int max_id = 0, int pagina = 1);
        Task<object> ExcluirOfertaAsync(int id = 0, long id_usuario_login = 0);
        IConfiguration GetConfiguration();
        Task<NovoRegistro> IncluirOfertaAsync(long id_desejo = 0, int id_empresa = 0, string validade = "", double valor = 0, string url = "", string descricao = "", string destaque = "", long id_usuario = 0);
        Task<object> IncluirOfertaLoteAsync(IFormFile file = null, int id_empresa = 0, long id_usuario_login = 0);
    }
}