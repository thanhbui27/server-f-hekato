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
            if(create.users.id == Guid.Empty || string.IsNullOrEmpty(create.users.address) || string.IsNullOrEmpty(create.users.Email) || string.IsNullOrEmpty(create.users.fullName) || string.IsNullOrEmpty(create.users.PhoneNumber))
            {
                return new ApiErrorResult<bool>("Khổng thể để trống các trường thông tin");
            }else
            {
            var user = await _context.Users.FindAsync(create.users.id);
                user.fullName = create.users.fullName;
                user.PhoneNumber = create.users.PhoneNumber;
                user.Email = create.users.Email;
                user.address = create.users.address;
                user.CMND = create.users.CMND;
                await _context.SaveChangesAsync();
            }
            var orders = new Orders();

            orders.total = create.total;
            orders.Uid = create.users.id;
            orders.createAt = DateTime.Now;
            orders.payments = new Payment
            {
                amount = Convert.ToInt32(create.total),
                provider = create.typePay,
                createAt = DateTime.Now,
                status = "Đang xử lý"

            };
            List<OrderDetails> lOrderDetails = new List<OrderDetails>();

            foreach (ProductOrder p in create.ProductIds)
            {
                lOrderDetails.Add(new OrderDetails
                {
                    ProductId = p.productId,
                    quantity = p.quantity,
                    createAt = DateTime.Now,
                    OrderId = orders.OrderId
                });
            }

            orders.OrderDetails = lOrderDetails;

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
               payments = x.payments,
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
