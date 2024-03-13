using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace dw_webservice.Repositories
{
    public interface IEmpresasRepository
    {
        Task<object> AtualizarEmpresaAsync(int id = 0, string fantasia = "", string razao_social = "", string cnpj = "", string insc_est = "", string url = "", string email_com = "", string email_sac = "", string fone_com = "", string fone_sac = "", string endereco = "", string numero = "", string complemento = "", string bairro = "", string cep = "", string endereco_cob = "", string numero_cob = "", string complemento_cob = "", string bairro_cob = "", string cep_cob = "", int id_cidade = 0, int id_cidade_cob = 0, string logo = "", int id_qualificacao = 0, long id_usuario_login = 0);
        Task<object> ConsultarEmpresaAsync(int id = 0, string fantasia = "", string razao_social = "", string cnpj = "", string insc_est = "", string url = "", string email_com = "", string email_sac = "", string fone_com = "", string fone_sac = "", string endereco = "", string numero = "", string complemento = "", string bairro = "", string cep = "", string endereco_cob = "", string numero_cob = "", string complemento_cob = "", string bairro_cob = "", string cep_cob = "", int id_cidade = 0, int id_cidade_cob = 0, string uf = "");
        Task<object> ConsultarQualificacaoAsync();
        Task<object> ExcluirEmpresaAsync(int id = 0, long id_usuario_login = 0);
        IConfiguration GetConfiguration();
        Task<object> IncluirEmpresaAsync(string fantasia = "", string razao_social = "", string cnpj = "", string insc_est = "", string url = "", string email_com = "", string email_sac = "", string fone_com = "", string fone_sac = "", string endereco = "", string numero = "", string complemento = "", string bairro = "", string cep = "", string endereco_cob = "", string numero_cob = "", string complemento_cob = "", string bairro_cob = "", string cep_cob = "", int id_cidade = 0, int id_cidade_cob = 0, string logo = "", int id_qualificacao = 0, long id_usuario_login = 0);
    }
}