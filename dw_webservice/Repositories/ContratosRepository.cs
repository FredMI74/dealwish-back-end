﻿using Dapper;
using dw_webservice.Models;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Data;
using System.Threading.Tasks;

namespace dw_webservice.Repositories
{
    public class ContratosRepository : IContratosRepository
    {
        readonly IConfiguration _configuration;
        public ContratosRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IConfiguration GetConfiguration()
        {
            return _configuration;
        }

        readonly string query_empresas_inadimplentes = "(select id_empresa from dealwish.faturas where id_situacao = :id_situacao_fatura " +
                                       "group by id_empresa having count(id) >= :num_faturas_abertas_bloqueio_contrato)";
        public async Task<object> ConsultarContratoAsync(int id = 0, int id_empresa = 0, int id_plano = 0, int id_situacao = 0, int dia_vct = 0, string data_inicio = "", string data_bloqueio = "", string data_termino = "", string inadimplentes = "")
        {
            object result = null;
            Utils utils = new();

            ConfigRepository configRepository = new(_configuration);

            data_inicio = utils.FormatarDataZero(data_inicio);
            data_termino = utils.FormatarDataZero(data_termino);
            data_bloqueio = utils.FormatarDataZero(data_bloqueio);

            using NpgsqlConnection conn = new(Constantes.string_conexao);

            try
            {

                if (id == 0 && id_empresa == 0 && id_plano == 0 && id_situacao == 0 && dia_vct == 0 && string.IsNullOrWhiteSpace(data_inicio) &&
                    string.IsNullOrWhiteSpace(data_bloqueio) && string.IsNullOrWhiteSpace(data_termino) && string.IsNullOrWhiteSpace(inadimplentes))
                {
                    return (utils.FormataRetorno("", new { Erro = true, Mensagem = Constantes.MENSAGEM_CONSULTA }));
                }

                var dyParam = new DynamicParameters();

                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }

                if (conn.State == ConnectionState.Open)
                {
                    var query = "select c.id, c.id_empresa, e.razao_social, c.id_plano, p.descricao desc_plano, c.id_situacao, s.descricao desc_situacao, c.dia_vct, c.data_inicio, c.data_bloqueio, c.data_termino " +
                                "from dealwish.contratos c, dealwish.empresas e, dealwish.situacoes s, dealwish.planos p where " +
                                "c.id_plano = p.id and s.id = c.id_situacao and c.id_empresa = e.id and ( ";
                    var tem_param = false;

                    DateTime v_data_inicio = DateTime.Now;
                    if (!string.IsNullOrWhiteSpace(data_inicio))
                    {
                        try
                        {
                            v_data_inicio = DateTime.Parse(data_inicio);
                        }
                        catch (Exception)
                        {
                            return (utils.FormataRetorno("", new { Erro = true, Mensagem = "Data de início inválida." }));
                        }
                    }

                    DateTime v_data_bloqueio = DateTime.Now;
                    if (!string.IsNullOrWhiteSpace(data_bloqueio))
                    {
                        try
                        {
                            v_data_bloqueio = DateTime.Parse(data_bloqueio);
                        }
                        catch (Exception)
                        {
                            return (utils.FormataRetorno("", new { Erro = true, Mensagem = "Data de bloqueio inválida." }));
                        }
                    }

                    DateTime v_data_termino = DateTime.Now;
                    if (!string.IsNullOrWhiteSpace(data_termino))
                    {
                        try
                        {
                            v_data_termino = DateTime.Parse(data_termino);
                        }
                        catch (Exception)
                        {
                            return (utils.FormataRetorno("", new { Erro = true, Mensagem = "Data de término inválida." }));
                        }
                    }

                    if (dia_vct != 0 && (dia_vct < 1 || dia_vct > 20))
                    {
                        return (utils.FormataRetorno("", new { Erro = true, Mensagem = "Data de vencimento deve ser entre 1 e 20." }));
                    }

                    if (id != 0)
                    {
                        query += " c.id = :id";
                        dyParam.Add("id", id, DbType.Int32, ParameterDirection.Input);
                        tem_param = true;
                    }

                    if (id_empresa != 0)
                    {
                        query = query + (tem_param ? " and " : "") + " c.id_empresa = :id_empresa";
                        dyParam.Add("id_empresa", id_empresa, DbType.Int32, ParameterDirection.Input);
                        tem_param = true;
                    }

                    if (id_plano != 0)
                    {
                        query = query + (tem_param ? " and " : "") + " c.id_plano = :id_plano";
                        dyParam.Add("id_plano", id_plano, DbType.Int32, ParameterDirection.Input);
                        tem_param = true;
                    }

                    if (id_situacao != 0)
                    {
                        query = query + (tem_param ? " and " : "") + " c.id_situacao = :id_situacao";
                        dyParam.Add("id_situacao", id_situacao, DbType.Int32, ParameterDirection.Input);
                        tem_param = true;
                    }

                    if (dia_vct != 0)
                    {
                        query = query + (tem_param ? " and " : "") + " c.dia_vct = :dia_vct";
                        dyParam.Add("dia_vct", dia_vct, DbType.Int16, ParameterDirection.Input);
                        tem_param = true;
                    }


                    if (!string.IsNullOrWhiteSpace(data_inicio))
                    {
                        query = query + (tem_param ? " and " : "") + " c.data_inicio = :data_inicio";
                        dyParam.Add("data_inicio", v_data_inicio, DbType.Date, ParameterDirection.Input);
                        tem_param = true;
                    }

                    if (!string.IsNullOrWhiteSpace(data_bloqueio))
                    {
                        query = query + (tem_param ? " and " : "") + " c.data_bloqueio = :data_bloqueio";
                        dyParam.Add("data_bloqueio", v_data_bloqueio, DbType.Date, ParameterDirection.Input);
                        tem_param = true;
                    }

                    if (!string.IsNullOrWhiteSpace(data_termino))
                    {
                        query = query + (tem_param ? " and " : "") + " c.data_termino = :data_termino";
                        dyParam.Add("data_termino", v_data_termino, DbType.Date, ParameterDirection.Input);
                        tem_param = true;
                    }

                    if (!string.IsNullOrWhiteSpace(inadimplentes) && inadimplentes == Constantes.SIM)
                    {
                        query = query + (tem_param ? " and " : "") + " c.id_situacao = :id_situacao";
                        dyParam.Add("id_situacao", Constantes.ATIVO, DbType.Int32, ParameterDirection.Input);
                        tem_param = true;

                        query = query + (tem_param ? " and " : "") + "  c.id_empresa in " + query_empresas_inadimplentes;
                        dyParam.Add("id_situacao_fatura", Constantes.A_LIQUIDAR, DbType.Int32, ParameterDirection.Input);
                        dyParam.Add("num_faturas_abertas_bloqueio_contrato", int.Parse(await configRepository.ConsultarValorConfigAsync("num_faturas_abertas_bloqueio_contrato")), DbType.Int32, ParameterDirection.Input);
                    }

                    query += " )";

                    result = await SqlMapper.QueryAsync(conn, query, param: dyParam, commandType: CommandType.Text);

                }
            }
            catch (Exception ex)
            {
                return (utils.FormataRetorno("", new { Erro = true, Mensagem = ex.Message }));
            }

            return (utils.FormataRetorno(result, new { Erro = false, Mensagem = "" }));
        }

        public async Task<object> IncluirContratoAsync(int id_empresa = 0, int id_plano = 0, int id_situacao = 0, int dia_vct = 0, string data_inicio = "", long id_usuario_login = 0)
        {
            object result = null;
            Utils utils = new();

            data_inicio = utils.FormatarDataZero(data_inicio);

            using NpgsqlConnection conn = new(Constantes.string_conexao);

            try
            {
                if (id_empresa == 0 || id_plano == 0 || id_situacao == 0 || dia_vct == 0 || string.IsNullOrWhiteSpace(data_inicio))
                {
                    return (utils.FormataRetorno("", new { Erro = true, Mensagem = Constantes.MENSAGEM_INCLUSAO_ATUALIZACAO + "Cód. Empresa, Cód. Plano, Cód. Situação, Dia Vct. e Data Início." }));
                }

                DateTime v_data_inicio = DateTime.Now;
                try
                {
                    v_data_inicio = DateTime.Parse(data_inicio);
                }
                catch (Exception)
                {
                    return (utils.FormataRetorno("", new { Erro = true, Mensagem = "Data de início inválida." }));
                }

                if (dia_vct < 1 || dia_vct > 20)
                {
                    return (utils.FormataRetorno("", new { Erro = true, Mensagem = "Data de vencimento deve ser entre 1 e 20." }));
                }

                var dyParam = new DynamicParameters();

                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }

                if (conn.State == ConnectionState.Open)
                {

                    if (await ExisteContratoAtvBloqAsync(conn, id_empresa) && id_situacao == Constantes.ATIVO)
                    {
                        return (utils.FormataRetorno("", new { Erro = true, Mensagem = "A empresa possui um contrato ativo ou bloqueado." }));
                    }

                    var query = "insert into dealwish.contratos (id_empresa, id_plano, id_situacao, dia_vct, data_inicio, id_usuario_log) " +
                                "values (:id_empresa, :id_plano, :id_situacao, :dia_vct, :data_inicio, :id_usuario_login) returning id";

                    dyParam.Add("id", null, DbType.Int32, ParameterDirection.Output);
                    dyParam.Add("id_empresa", id_empresa, DbType.Int32, ParameterDirection.Input);
                    dyParam.Add("id_plano", id_plano, DbType.Int32, ParameterDirection.Input);
                    dyParam.Add("id_situacao", id_situacao, DbType.Int32, ParameterDirection.Input);
                    dyParam.Add("dia_vct", dia_vct, DbType.Int16, ParameterDirection.Input);
                    dyParam.Add("data_inicio", v_data_inicio, DbType.Date, ParameterDirection.Input);
                    dyParam.Add("id_usuario_login", id_usuario_login, DbType.Int64, ParameterDirection.Input);

                    await SqlMapper.ExecuteAsync(conn, query, dyParam, null, null, CommandType.Text);
                    result = new { ID = dyParam.Get<int>("id") };
                }
            }
            catch (Exception ex)
            {
                return (utils.FormataRetorno("", new { Erro = true, Mensagem = ex.Message }));
            }

            return (utils.FormataRetorno(result, new { Erro = false, Mensagem = "" }));
        }

        public async Task<object> ExcluirContratoAsync(int id = 0, long id_usuario_login = 0)
        {
            object result = null;
            IDbTransaction transaction = null;
            Utils utils = new();

            using NpgsqlConnection conn = new(Constantes.string_conexao);

            try
            {
                if (id == 0)
                {
                    return (utils.FormataRetorno("", new { Erro = true, Mensagem = Constantes.MENSAGEM_INCLUSAO_ATUALIZACAO + "Código." }));
                }

                var dyParam = new DynamicParameters();

                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }

                if (await SituacaoAtualContratoAsync(conn, id) == Constantes.BLOQUEADO)
                {
                    return (utils.FormataRetorno("", new { Erro = true, Mensagem = "Um contrato BLOQUEADO não pode ser excluído." }));
                }

                if (conn.State == ConnectionState.Open)
                {
                    transaction = conn.BeginTransaction();

                    var query = "update dealwish.contratos set id_usuario_log = :id_usuario_login where id = :id ";

                    dyParam.Add("id", id, DbType.Int32, ParameterDirection.Input);
                    dyParam.Add("id_usuario_login", id_usuario_login, DbType.Int64, ParameterDirection.Input);
                    var linhasafetadas = await SqlMapper.ExecuteAsync(conn, query, dyParam, transaction, null, CommandType.Text);

                    query = "delete from dealwish.contratos where id = :id ";

                    linhasafetadas += await SqlMapper.ExecuteAsync(conn, query, dyParam, transaction, null, CommandType.Text);

                    transaction.Commit();

                    result = new { linhasafetadas };
                }
            }
            catch (Exception ex)
            {
                transaction.Rollback();

                return (utils.FormataRetorno("", new { Erro = true, Mensagem = ex.Message }));
            }

            return (utils.FormataRetorno(result, new { Erro = false, Mensagem = "" }));
        }

        public async Task<object> AtualizarContratoAsync(int id = 0, int id_empresa = 0, int id_plano = 0, int id_situacao = 0, int dia_vct = 0, string data_inicio = "", string data_bloqueio = "", string data_termino = "", long id_usuario_login = 0)
        {
            object result = null;
            IDbTransaction transaction = null;
            Utils utils = new();

            UsuariosRepository usuariosRepository = new(_configuration);

            data_inicio = utils.FormatarDataZero(data_inicio);
            data_termino = utils.FormatarDataZero(data_termino);
            data_bloqueio = utils.FormatarDataZero(data_bloqueio);

            using NpgsqlConnection conn = new(Constantes.string_conexao);

            try
            {
                if (id == 0 || id_empresa == 0 || id_plano == 0 || id_situacao == 0 || dia_vct == 0 || string.IsNullOrWhiteSpace(data_inicio))
                {
                    return (utils.FormataRetorno("", new { Erro = true, Mensagem = Constantes.MENSAGEM_INCLUSAO_ATUALIZACAO + "Código, Cód. Empresa, Cód. Plano, Cód. Situação, Dia Vct. e Data Início. Opcionalmente Data de Bloqueio e/ou Data de Término." }));
                }


                DateTime v_data_inicio = DateTime.Now;
                try
                {
                    v_data_inicio = DateTime.Parse(data_inicio);
                }
                catch (Exception)
                {
                    return (utils.FormataRetorno("", new { Erro = true, Mensagem = "Data de início inválida." }));
                }

                var lista_permissoes = await usuariosRepository.ConsultarListaPermUsuarioAsync(id_usuario_login);
                if (!lista_permissoes.Contains("bkf") && !lista_permissoes.Contains("tin") && (await SituacaoAtualContratoAsync(conn, id) == Constantes.BLOQUEADO))
                {
                    return (utils.FormataRetorno("", new { Erro = true, Mensagem = "Sem permissão para alterar contrato BLOQUEADO." }));
                }

                DateTime v_data_termino = DateTime.Now;
                if (!string.IsNullOrWhiteSpace(data_termino))
                {
                    try
                    {
                        v_data_termino = DateTime.Parse(data_termino);
                    }
                    catch (Exception)
                    {
                        return (utils.FormataRetorno("", new { Erro = true, Mensagem = "Data de término inválida." }));
                    }
                }

                DateTime v_data_bloqueio = DateTime.Now;
                if (!string.IsNullOrWhiteSpace(data_bloqueio))
                {
                    try
                    {
                        v_data_bloqueio = DateTime.Parse(data_bloqueio);
                    }
                    catch (Exception)
                    {
                        return (utils.FormataRetorno("", new { Erro = true, Mensagem = "Data de bloqueio inválida." }));
                    }
                }

                if (dia_vct < 1 || dia_vct > 20)
                {
                    return (utils.FormataRetorno("", new { Erro = true, Mensagem = "Data de vencimento deve ser entre 1 e 20." }));
                }

                var dyParam = new DynamicParameters();

                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }

                if (conn.State == ConnectionState.Open)
                {

                    if (await ExisteContratoAtvBloqAsync(conn, id_empresa, id) && id_situacao == Constantes.ATIVO)
                    {
                        return (utils.FormataRetorno("", new { Erro = true, Mensagem = "A empresa possui um contrato ativo ou bloqueado." }));
                    }

                    transaction = conn.BeginTransaction();

                    var query = "update dealwish.contratos set id_empresa =:id_empresa, id_plano =:id_plano, id_situacao =:id_situacao, id_usuario_log = :id_usuario_login, " +
                                "dia_vct =:dia_vct, data_inicio = :data_inicio, data_bloqueio = :data_bloqueio, data_termino = :data_termino where id  = :id ";

                    dyParam.Add("id", id, DbType.Int32, ParameterDirection.Input);
                    dyParam.Add("id_empresa", id_empresa, DbType.Int32, ParameterDirection.Input);
                    dyParam.Add("id_plano", id_plano, DbType.Int32, ParameterDirection.Input);
                    dyParam.Add("id_situacao", !string.IsNullOrWhiteSpace(data_termino) ? Constantes.ENCERRADO : id_situacao, DbType.Int32, ParameterDirection.Input);
                    dyParam.Add("dia_vct", dia_vct, DbType.Int16, ParameterDirection.Input);
                    dyParam.Add("data_inicio", v_data_inicio, DbType.Date, ParameterDirection.Input);
                    dyParam.Add("id_usuario_login", id_usuario_login, DbType.Int64, ParameterDirection.Input);

                    if (!string.IsNullOrWhiteSpace(data_termino))
                    {
                        dyParam.Add("data_termino", v_data_termino, DbType.Date, ParameterDirection.Input);
                    }
                    else
                    {
                        dyParam.Add("data_termino", null, DbType.Date, ParameterDirection.Input);
                    }
                    if (!string.IsNullOrWhiteSpace(data_bloqueio))
                    {
                        dyParam.Add("data_bloqueio", v_data_termino, DbType.Date, ParameterDirection.Input);
                    }
                    else
                    {
                        dyParam.Add("data_bloqueio", null, DbType.Date, ParameterDirection.Input);
                    }

                    var linhasafetadas = await SqlMapper.ExecuteAsync(conn, query, dyParam, transaction, null, CommandType.Text);

                    if (!string.IsNullOrWhiteSpace(data_termino))
                    {
                        query = "update dealwish.usuarios set id_situacao = :id_nova_situacao_usuario, id_usuario_log = :id_usuario_login " +
                                "where id_situacao != :id_nova_situacao_usuario and " +
                                "id_empresa = (select id_empresa from dealwish.contratos where id  = :id)";
                        dyParam.Add("id_nova_situacao_usuario", Constantes.INATIVO, DbType.Int32, ParameterDirection.Input);
                        linhasafetadas += await SqlMapper.ExecuteAsync(conn, query, dyParam, transaction, null, CommandType.Text);
                    }

                    transaction.Commit();

                    result = new { linhasafetadas };
                }
            }
            catch (Exception ex)
            {
                if (transaction != null)
                {
                    transaction.Rollback();
                }

                return (utils.FormataRetorno("", new { Erro = true, Mensagem = ex.Message }));
            }

            return (utils.FormataRetorno(result, new { Erro = false, Mensagem = "" }));
        }

        public async Task<object> BloquearContratosInadimplentesAsync(long id_usuario_login = 0)
        {
            object result = null;
            IDbTransaction transaction = null;
            Utils utils = new();
            ConfigRepository configRepository = new(_configuration);

            using NpgsqlConnection conn = new(Constantes.string_conexao);

            try
            {
                var dyParam = new DynamicParameters();

                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }

                if (conn.State == ConnectionState.Open)
                {
                    transaction = conn.BeginTransaction();

                    dyParam.Add("id_situacao_fatura", Constantes.A_LIQUIDAR, DbType.Int32, ParameterDirection.Input);
                    dyParam.Add("num_faturas_abertas_bloqueio_contrato", int.Parse(await configRepository.ConsultarValorConfigAsync("num_faturas_abertas_bloqueio_contrato")), DbType.Int32, ParameterDirection.Input);

                    // Verifica quantidade de contratos a serem bloqueados
                    var query = "select count(1) qtd from dealwish.contratos where id_situacao = :id_situacao_contrato and id_empresa in " + query_empresas_inadimplentes;
                    dyParam.Add("id_situacao_contrato", Constantes.ATIVO, DbType.Int32, ParameterDirection.Input);

                    var contratos = await SqlMapper.QuerySingleAsync(conn, query, param: dyParam, commandType: CommandType.Text);
                    bool nao_pode_bloquear = contratos.qtd == 0;

                    if (nao_pode_bloquear)
                    {
                        return (utils.FormataRetorno("", new { Erro = true, Mensagem = "Nenhum contrato a ser bloqueado." }));
                    }

                    query = "update dealwish.contratos set id_situacao = :id_nova_situacao_contrato, data_bloqueio = current_date, id_usuario_log = :id_usuario_login " +
                            "where id_situacao = :id_situacao_contrato and id_empresa in " + query_empresas_inadimplentes;
                    dyParam.Add("id_nova_situacao_contrato", Constantes.BLOQUEADO, DbType.Int32, ParameterDirection.Input);
                    dyParam.Add("id_usuario_login", id_usuario_login, DbType.Int64, ParameterDirection.Input);

                    var linhasafetadas = await SqlMapper.ExecuteAsync(conn, query, dyParam, transaction, null, CommandType.Text);

                    query = "update dealwish.usuarios set id_situacao = :id_nova_situacao_contrato, id_usuario_log = :id_usuario_login " +
                            "where id_situacao = :id_situacao_contrato and " +
                            "id_empresa in " + query_empresas_inadimplentes;
                    linhasafetadas += await SqlMapper.ExecuteAsync(conn, query, dyParam, transaction, null, CommandType.Text);

                    transaction.Commit();

                    result = new { linhasafetadas };

                }
            }
            catch (Exception ex)
            {
                transaction.Rollback();

                return (utils.FormataRetorno("", new { Erro = true, Mensagem = ex.Message }));
            }

            return (utils.FormataRetorno(result, new { Erro = false, Mensagem = "" }));
        }

        public async Task<object> DesbloquearContratoAsync(int id = 0, long id_usuario_login = 0)
        {
            object result = null;
            IDbTransaction transaction = null;
            Utils utils = new();

            using NpgsqlConnection conn = new(Constantes.string_conexao);

            try
            {
                if (id == 0)
                {
                    return (utils.FormataRetorno("", new { Erro = true, Mensagem = Constantes.MENSAGEM_INCLUSAO_ATUALIZACAO + "Código." }));
                }

                var dyParam = new DynamicParameters();

                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }

                if (conn.State == ConnectionState.Open)
                {
                    transaction = conn.BeginTransaction();

                    var query = "update dealwish.contratos set id_situacao = :id_nova_situacao_contrato, data_bloqueio = null, id_usuario_log = :id_usuario_login  " +
                                "where id_situacao = :id_situacao_contrato and id = :id";
                    dyParam.Add("id_situacao_contrato", Constantes.BLOQUEADO, DbType.Int32, ParameterDirection.Input);
                    dyParam.Add("id_nova_situacao_contrato", Constantes.ATIVO, DbType.Int32, ParameterDirection.Input);
                    dyParam.Add("id", id, DbType.Int32, ParameterDirection.Input);
                    dyParam.Add("id_usuario_login", id_usuario_login, DbType.Int64, ParameterDirection.Input);

                    var linhasafetadas = await SqlMapper.ExecuteAsync(conn, query, dyParam, transaction, null, CommandType.Text);

                    query = "update dealwish.usuarios set id_situacao = :id_nova_situacao_contrato, id_usuario_log = :id_usuario_login " +
                            "where id_situacao = :id_situacao_contrato and " +
                            "id_empresa = (select id_empresa from dealwish.contratos where id  = :id)";

                    linhasafetadas += await SqlMapper.ExecuteAsync(conn, query, dyParam, transaction, null, CommandType.Text);

                    transaction.Commit();

                    result = new { linhasafetadas };
                }
            }
            catch (Exception ex)
            {
                transaction.Rollback();

                return (utils.FormataRetorno("", new { Erro = true, Mensagem = ex.Message }));
            }

            return (utils.FormataRetorno(result, new { Erro = false, Mensagem = "" }));
        }

        private static async Task<bool> ExisteContratoAtvBloqAsync(IDbConnection conn, int id_empresa, int id_contrato = 0)
        {
            var dyParam = new DynamicParameters();

            var query = "select count(1) qtd from dealwish.contratos where (id_situacao = :id_situacao_contrato_ativo or id_situacao = :id_situacao_contrato_bloq)" +
                " and id_empresa = :id_empresa and id <> :id";

            dyParam.Add("id_situacao_contrato_ativo", Constantes.ATIVO, DbType.Int32, ParameterDirection.Input);
            dyParam.Add("id_situacao_contrato_bloq", Constantes.BLOQUEADO, DbType.Int32, ParameterDirection.Input);
            dyParam.Add("id_situacao_contrato_bloq", Constantes.BLOQUEADO, DbType.Int32, ParameterDirection.Input);
            dyParam.Add("id_empresa", id_empresa, DbType.Int32, ParameterDirection.Input);
            dyParam.Add("id", id_contrato, DbType.Int32, ParameterDirection.Input);

            var contratos = await SqlMapper.QuerySingleAsync(conn, query, param: dyParam, commandType: CommandType.Text);

            return contratos.qtd > 0;
        }

        private static async Task<int> SituacaoAtualContratoAsync(IDbConnection conn, int id_contrato = 0)
        {
            var dyParam = new DynamicParameters();

            var query = "select id_situacao from dealwish.contratos where id = :id";

            dyParam.Add("id", id_contrato, DbType.Int32, ParameterDirection.Input);

            var contratos = await SqlMapper.QuerySingleAsync(conn, query, param: dyParam, commandType: CommandType.Text);

            return contratos.id_situacao;
        }

    }
}