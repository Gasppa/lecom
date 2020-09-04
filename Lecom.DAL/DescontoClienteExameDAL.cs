using InterSystems.Data.CacheClient;
using Lecom.DAL.Data;
using Lecom.Model;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Lecom.DAL
{
    public class DescontoClienteExameDAL
    {
        private readonly IConfiguration _config;
        public DescontoClienteExameDAL(IConfiguration config)
        {
            _config = config;
        }

        public async Task SalvarDescontoClienteExame(string codigoCliente, string descricaoConcessao, double valorDescontoFaturamento, double valorDescontoComissao)
        {

            string query = $"EXEC SPD_LECON_GRAVA_DESCONTO_CLIENTE {codigoCliente}, {descricaoConcessao}, {valorDescontoFaturamento}, {valorDescontoComissao}";

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
                    }
                }
                catch (SqlException e)
                {
                    Console.WriteLine((e.InnerException ?? e).Message);
                    throw;
                }
                finally
                {
                    conn.Close();
                }
            }
        }
    }
}
