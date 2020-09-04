using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using InterSystems.Data.CacheClient;
using Lecom.DAL.Data;
using Lecom.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Lecom.DAL
{
    public class RepresentanteDAL
    {
        private readonly IConfiguration _config;
        public RepresentanteDAL(IConfiguration config)
        {
            _config = config;
        }

        public async Task<List<Representante>> GetRepresentantes(string codigoCliente)
        {

            string query = @"
                                        SELECT 
                                            DADOSCLIENTE.CD_ENTIDADE AS CodigoCliente, 
	                                        ENTIDADES.NM_RAZAOSOC AS RazaoSocialCliente, 
	                                        EMPRESA_URA.CD_EMPRESA as CodigoUra, 
	                                        EMPRESA_URA.DS_RAZAOSOCIAL as RazaoSocialUra, 
	                                        ENTIDADE_REPRES.CD_ENTIDADE as CodigoRepresentante, 
	                                        ENTIDADE_REPRES.NM_RAZAOSOC as NomeRepresentante, 
	                                        ENTIDADE_SUPERVISOR.CD_ENTIDADE as CodigoSupervisor, 
	                                        ENTIDADE_SUPERVISOR.NM_RAZAOSOC as NomeSupervisor
                                        FROM
                                            DADOSCLIENTE
                                        INNER JOIN ENTIDADES
                                            ON ENTIDADES.CD_ENTIDADE = DADOSCLIENTE.CD_ENTIDADE

                                        LEFT JOIN DADOSREPRES REPRESENTANTE
                                            ON DADOSCLIENTE.CD_REPRES = REPRESENTANTE.CD_ENTIDADE

                                        LEFT JOIN ENTIDADES ENTIDADE_REPRES
                                            ON ENTIDADE_REPRES.CD_ENTIDADE = REPRESENTANTE.CD_ENTIDADE

                                        LEFT JOIN REPRESENTANTE_URA
                                            ON REPRESENTANTE.CD_ENTIDADE = REPRESENTANTE_URA.CD_ENTIDADE

                                        LEFT JOIN EMPRESAS EMPRESA_URA
                                            ON EMPRESA_URA.CD_EMPRESA = REPRESENTANTE_URA.CD_EMPRESA

                                        LEFT JOIN DADOSREPRES SUPERVISOR
                                            ON REPRESENTANTE.CD_SUPERVISOR = SUPERVISOR.CD_ENTIDADE

                                        LEFT JOIN ENTIDADES ENTIDADE_SUPERVISOR
                                            ON ENTIDADE_SUPERVISOR.CD_ENTIDADE = SUPERVISOR.CD_ENTIDADE

                                        WHERE " + 
                                            $"DADOSCLIENTE.CD_ENTIDADE = {codigoCliente}";

            List<Representante> dadosRepresentante = new List<Representante>();

            using (var context = new ApplicationDbContext(_config))
            {
                var conn = context.Database.GetDbConnection();
                try
                {
                    using (var command = context.Database.GetDbConnection().CreateCommand())
                    {
                        command.CommandText = query;
                        command.CommandTimeout = context.Database.GetCommandTimeout().Value;
                        await context.Database.OpenConnectionAsync();
                        using (var dataReader = command.ExecuteReader())
                        {
                            while (await dataReader.ReadAsync())
                            {
                                dadosRepresentante.Add(new Representante()
                                {
                                    codigoCliente = !DBNull.Value.Equals(dataReader["CodigoCliente"]) ? dataReader["CodigoCliente"].ToString() : null,
                                    razaoSocialCliente = !DBNull.Value.Equals(dataReader["RazaoSocialCliente"]) ? dataReader["RazaoSocialCliente"].ToString() : null,
                                    codigoUra = !DBNull.Value.Equals(dataReader["CodigoUra"]) ? dataReader["CodigoUra"].ToString() : null,
                                    razaoSocialUra = !DBNull.Value.Equals(dataReader["RazaoSocialUra"]) ? dataReader["RazaoSocialUra"].ToString() : null,
                                    codigoRepresentante = !DBNull.Value.Equals(dataReader["CodigoRepresentante"]) ? dataReader["CodigoRepresentante"].ToString() : null,
                                    nomeRepresentante = !DBNull.Value.Equals(dataReader["NomeRepresentante"]) ? dataReader["NomeRepresentante"].ToString() : null,
                                    codigoSupervisor = !DBNull.Value.Equals(dataReader["CodigoSupervisor"]) ? dataReader["CodigoSupervisor"].ToString() : null,
                                    nomeSupervisor = !DBNull.Value.Equals(dataReader["NomeSupervisor"]) ? dataReader["NomeSupervisor"].ToString() : null
                                });
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine((e.InnerException ?? e).Message);
                    throw;
                }
                finally
                {
                    conn.Close();
                }
            }
            return dadosRepresentante;
        }
    }
}
