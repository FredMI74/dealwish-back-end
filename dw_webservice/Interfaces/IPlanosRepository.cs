using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace dw_webservice.Repositories
{
    public interface IPlanosRepository
    {
        Task<object> AtualizarPlanoAsync(int id = 0, string descricao = "", int qtd_ofertas = 0, float valor_mensal = 0, float valor_oferta = 0, string visualizacao = "", long id_usuario_login = 0);
        Task<object> ConsultarPlanoAsync(int id = 0, string descricao = "");
        Task<object> ExcluirPlanoAsync(int id = 0, long id_usuario_login = 0);
        IConfiguration GetConfiguration();
        Task<object> IncluirPlanoAsync(string descricao = "", int qtd_ofertas = 0, float valor_mensal = 0, float valor_oferta = 0, string visualizacao = "", long id_usuario_login = 0);
    }
}