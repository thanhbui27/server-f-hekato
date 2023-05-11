using DoAn.Models;
using DoAn.Repositories.Order;
using DoAn.ViewModels.Orders;
using Microsoft.AspNetCore.Mvc;

namespace DoAn.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class OrdersController : Controller
    {
        private readonly IOrderRepositories _IOrderRepositories;
        public OrdersController(IOrderRepositories iorderRepositories)
        {
            _IOrderRepositories = iorderRepositories;
        }

        [HttpGet("GetOrderById")]
        public async Task<IActionResult> GetOrderById(Guid uid)
        {
            var result = await _IOrderRepositories.getOrderByUser(uid);
            if (result.IsSuccessed)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateOrders(CreateOrders create)
        {
            var result = await _IOrderRepositories.create(create);
            if (result.IsSuccessed)
            {
                return Ok(result);
            }
            return BadRequest(result);
     
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _IOrderRepositories.remove(id);
            if (result.IsSuccessed)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
