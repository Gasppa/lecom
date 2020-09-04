using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using InterSystems.Data.CacheClient;
using Lecom.Model;
using Lecom.DAL.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

namespace Lecom.DAL
{
    public class ExameDAL
    {
        private readonly IConfiguration _config;
        public ExameDAL(IConfiguration config)
        {
            _config = config;
        }

        public async Task<List<Exame>> GetDadosExames(string codigoCliente, string codigoExame, string dataBase)
        {

            string query = $"SELECT * FROM FN_RET_LECON_DADOSFATURAMENTO({codigoCliente}, {codigoExame}, {dataBase})";

            List<Exame> dadosExames = new List<Exame>();

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
                                dadosExames.Add(new Exame()
                                {
                                    codigoCliente = !DBNull.Value.Equals(dataReader["CD_CLIENTE"]) ? dataReader["CD_CLIENTE"].ToString() : null,
                                    codigoExame = !DBNull.Value.Equals(dataReader["CD_EXAME"]) ? dataReader["CD_EXAME"].ToString() : null,
                                    precoAcordadoExame = Convert.ToDouble(dataReader["VR_ACORDADO_EXAME"]),
                                    precoFaturadoExame = Convert.ToDouble(dataReader["VR_PRECO_FATURADO"]),
                                    comissaoTotal = Convert.ToDouble(dataReader["VR_COMISSAO_TOTAL"]),
                                    comissaoFaturada = Convert.ToDouble(dataReader["VR_COMISSAO_FATURADA"])
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
            return dadosExames;
        }
    }
}
