using backloja.DTOS;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Linq.Expressions;

namespace backloja.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {

        private readonly IConfiguration _configuration;
        public ProductController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        [Route("GetProductsById")]
        public IActionResult GetProductsById(int id)
        {
            try
            {
                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {

                    string query = "select nome, quantidade, descricao, caminho FROM produtos where id = @id";
                    var produtos = connection.QueryFirstOrDefault<Produtos>(query, new { id = id });
                    if (produtos != null)
                    
                        return Ok(produtos);
                    
                    else
                    
                        return NotFound();
                    
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Houve um erro ao obter os produtos : {ex}");
            }


        }

        [HttpGet]
        [Route("GetAllProducts")]
        public IActionResult GetAllProducts()
        {
            try
            {
                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    string query = "select * from produtos";
                    var produtos = connection.Query<Produtos>(query).ToList();
                    if(produtos != null)
                    
                        return Ok(produtos);
                    
                    else
                    
                        return NotFound();
                    
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }






    }

}

