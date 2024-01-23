using backloja.DTOS;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace backloja.Controllers
{

    [Route("[controller]")]
    [ApiController]

    public class RegisterController : ControllerBase
    {

        private readonly IConfiguration _config;
        public RegisterController(IConfiguration config)
        {
            _config = config;
        }

        [HttpPost]
        public IActionResult RegisterUsers([FromBody] RegisterUser register)
        {
            using (var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                try
                {
                    // Verificar se o email já existe
                    var checarEmail = "SELECT * FROM cadastrados WHERE email = @Email";
                    var emailExistente = connection.QueryFirstOrDefault<Register>(checarEmail, new { Email = register.email });

                    if (emailExistente != null)
                    {
                        return BadRequest(" Email já cadastrado, use outro email não cadastrado e válido.");
                    }
                    if (emailExistente == null)
                    {
                        var insertQuery = "INSERT INTO cadastrados (nome, email, senha) VALUES (@Nome, @Email, @Senha)";
                        connection.Execute(insertQuery, new { Nome = register.nome, Email = register.email, Senha = register.senha });

                        // Confirma se os dados foram inseridos
                        var selectQuery = "SELECT nome, email, senha FROM cadastrados WHERE email = @Email AND senha = @Senha";
                        var usuario = connection.QueryFirstOrDefault<Register>(selectQuery, new { Email = register.email, Senha = register.senha });

                        return Ok(usuario);
                    }

                    return BadRequest("Falha ao obter dados do usuário recém cadastrados");

                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }

            }
        }
    }
}
