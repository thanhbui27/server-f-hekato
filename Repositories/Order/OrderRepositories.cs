using AutoMapper;
using DoAn.EF;
using DoAn.Helpers.ApiResponse;
using DoAn.Models;
using DoAn.ViewModels.Orders;
using DoAn.ViewModels.Product;
using Microsoft.EntityFrameworkCore;

namespace DoAn.Repositories.Order
{
    public class OrderRepositories : IOrderRepositories
    {
        private readonly EFDbContext _context;
        private readonly IMapper _mapper;
        public OrderRepositories(EFDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<ApiResult<bool>> create(CreateOrders create)
        {
            var orders = new Orders();

            orders.total = create.total;
            orders.Uid = create.users.id;
            orders.createAt = DateTime.Now;

            List<OrderDetails> lOrderDetails = new List<OrderDetails>();

            foreach (ProductGetById p in create.ProductIds)
            {
                lOrderDetails.Add(new OrderDetails
                {
                    ProductId = p.ProductId,
                    quantity = p.quantity,
                    createAt = DateTime.Now,
                    OrderId = orders.OrderId
                });
            }

            orders.OrderDetails = lOrderDetails;

            //var orders = _mapper.Map<Orders>(create);

            //List<OrderDetails> pr = new List<OrderDetails>();
            //for (int i = 0; i < create.ProductIds.Count; i++)
            //{
            //    pr.Add(new OrderDetails
            //    {
            //        ProductId = create.ProductIds[i].ProductId,
            //        quantity = create.ProductIds[i].quantity,
            //        createAt = DateTime.Now,
            //        OrderId = orders.OrderId
            //    });
            //}
            //orders.OrderDetails = pr;
            //orders.createAt = DateTime.Now;

            _context.orders.Add(orders);

            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>
            {
                IsSuccessed = true,
                Message = "Đơn hàng đang được xử lý vui lòng đợi"
            };

        }

        public async Task<ApiResult<List<Orders>>> getOrderByUser(Guid Uid)
        {
           var order = await _context.orders.Select(x => new Orders
           {
               createAt = x.createAt,
               OrderId = x.OrderId,
               Uid = Uid,
               total = x.total,
               OrderDetails = x.OrderDetails.Where(o => o.OrderId == x.OrderId).ToList()
           }).Where(x => x.Uid == Uid).ToListAsync();

            return new ApiSuccessResult<List<Orders>>(order);
        }

        public async Task<ApiResult<bool>> remove(int OrderId)
        {
            _context.orders.Remove(new Orders { OrderId = OrderId });
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>
            {
                IsSuccessed = true,
                Message = "Xoá đơn hàng thành công"
            };
        }

    

    }
}
