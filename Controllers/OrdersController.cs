using DoAn.Models;
using DoAn.Repositories.Order;
using DoAn.ViewModels.Orders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DoAn.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly IOrderRepositories _IOrderRepositories;
        public OrdersController(IOrderRepositories iorderRepositories)
        {
            _IOrderRepositories = iorderRepositories;
        }

        [Authorize(Roles = "admin")]
        [HttpGet("GetAllOrder")]
        public async Task<IActionResult> GetAllOrder([FromQuery]GetAllOrder getall)
        {
            return Ok(await _IOrderRepositories.getAllOrders(getall));
        }


        [HttpGet("GetDetailsOrder")]
        public async Task<IActionResult> GetDetailsOrder([FromQuery] int id)
        {
            return Ok(await _IOrderRepositories.getDetailtOrder(id));
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

        [Authorize(Roles = "admin")]
        [HttpPut("UpdateStatus")]

        public async Task<IActionResult> UpdateStatusOrder(int id, string status)
        {
            var result = await _IOrderRepositories.updateStatusOrder(id,status);
            if (result.IsSuccessed)
            {
                return Ok(result);
            }
            return BadRequest(result);

        }
        [Authorize(Roles = "admin")]
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
