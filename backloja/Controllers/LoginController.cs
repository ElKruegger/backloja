using backloja.DTOS;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;

namespace backloja.Controllers
{


    [Route("[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {

        private readonly IConfiguration _configuration;
        public LoginController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        public IActionResult GetLoginUsers([FromBody]LoginUser login)
        {
            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
               
                var query = "select id, email, senha from cadastrados where email = @email and senha = @senha";


                // Validação com banco de dados :
                var usuarios = connection.Query<LoginUser>(query, new {email = login.email, senha = login.senha }).FirstOrDefault();

                if (usuarios != null)
                    return Ok(usuarios);
                else
                    return BadRequest();

               

            }

        }
    }
}
