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
            return Ok(await _IOrderRepositories.getOrderByUser(uid));
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateOrders(CreateOrders create)
        {
            return Ok(await _IOrderRepositories.create(create));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _IOrderRepositories.remove(id));
        }
    }
}
