﻿using AutoMapper;
using DoAn.EF;
using DoAn.Helpers.ApiResponse;
using DoAn.Helpers.Pagination;
using DoAn.Models;
using DoAn.Repositories.Email;
using DoAn.Repositories.Users;
using DoAn.ViewModels.Orders;
using DoAn.ViewModels.Product;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace DoAn.Repositories.Order
{
    public class OrderRepositories : IOrderRepositories
    {
        private readonly EFDbContext _context;
        private readonly IMapper _mapper;
        private readonly IEmailRepositories _emailRepositories;
        private readonly IUserRepositories _userRepositories;

        public OrderRepositories(EFDbContext context, IMapper mapper, IEmailRepositories emailRepositories, IUserRepositories userRepositories)
        {
            _context = context;
            _mapper = mapper;
            _emailRepositories = emailRepositories;
            _userRepositories = userRepositories;
        }
        public async Task<ApiResult<int>> create(CreateOrders create)
        {
            try
            {

                if(create.users.id == Guid.Empty || string.IsNullOrEmpty(create.users.address) || string.IsNullOrEmpty(create.users.Email) || string.IsNullOrEmpty(create.users.fullName) || string.IsNullOrEmpty(create.users.PhoneNumber))
                {
                    return new ApiErrorResult<int>("Khổng thể để trống các trường thông tin");
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
                    status = "pending"

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
                return new ApiSuccessResult<int>
                {
                    IsSuccessed = true,
                    Message = "Đơn hàng đang được xử lý vui lòng đợi",
                    Data = orders.payments.paymentId
                };
            }
            catch (Exception ex)
            {
                return new ApiErrorResult<int>
                {
                    IsSuccessed = false,
                    Message = ex.Message
                };
            }

        }

        public async Task<PagedResult<Orders>> getAllOrders(GetAllOrder getall)
        {
            try
            {
                var order = await _context.orders.Select(x => new Orders
                {
                    createAt = x.createAt,
                    OrderId = x.OrderId,
                    Uid = x.Uid,
                    payments = x.payments,
                    total = x.total,
                    users = x.users,
                    OrderDetails = x.OrderDetails.Select(od => new OrderDetails
                    {
                        createAt= od.createAt,
                        OrderId = od.OrderId,
                        Id= od.Id,
                        products = od.products,
                        quantity = od.quantity ,
               
                    }).Where(o => o.OrderId == x.OrderId).ToList()
                }).OrderByDescending(x => x.createAt).ToListAsync();

                if (!string.IsNullOrEmpty(getall.q))
                {
                    order = order.Where(x => x.users.fullName.Contains(getall.q)).ToList();
                }

                int totalRow = await _context.orders.CountAsync();

                order = order.Skip((getall.PageIndex - 1) * getall.PageSize)
                .Take(getall.PageSize).ToList();

                return new PagedResult<Orders>
                {
                    TotalRecords = totalRow,
                    PageSize = getall.PageSize,
                    PageIndex = getall.PageIndex,
                    Items = order
                };
            }
            catch (Exception ex)
            {
                return new PagedResult<Orders>
                {
                    TotalRecords = 0,
                    PageSize = 0,
                    PageIndex = 0,
                    Items = new List<Orders>()
                };
            }
        }

        public async Task<ApiResult<Orders>> getDetailtOrder(int id)
        {
            try
            {

                var order = _context.orders.Select(x => new Orders
                {
                    createAt = x.createAt,
                    OrderId = x.OrderId,
                    Uid = x.Uid,
                    payments = x.payments,
                    total = x.total,
                    users = x.users,
                    OrderDetails = x.OrderDetails.Select(od => new OrderDetails
                    {
                        createAt = od.createAt,
                        OrderId = od.OrderId,
                        Id = od.Id,
                        products = od.products,
                        quantity = od.quantity,

                    }).Where(o => o.OrderId == x.OrderId).ToList()
                }).FirstOrDefault(x => x.OrderId == id);

                if (order != null)
                {
                    return new ApiSuccessResult<Orders>(order);
                }

                return new ApiErrorResult<Orders>("Đơn hàng không tồn tại");
            }
            catch (Exception ex)
            {
                return new ApiErrorResult<Orders>
                {
                    IsSuccessed = false,
                    Message = ex.Message
                };
            }

        }

        public async Task<ApiResult<List<Orders>>> getOrderByUser(Guid Uid)
        {
            try
            {
                var order = await _context.orders.Select(x => new Orders
                {
                    createAt = x.createAt,
                    OrderId = x.OrderId,
                    Uid = x.Uid,
                    payments = x.payments,
                    total = x.total,
                    OrderDetails = x.OrderDetails.Where(o => o.OrderId == x.OrderId).ToList()
                }).Where(x => x.Uid == Uid).OrderByDescending(x => x.createAt).ToListAsync();

                return new ApiSuccessResult<List<Orders>>(order);
            }
            catch (Exception ex)
            {
                return new ApiErrorResult<List<Orders>>
                {
                    IsSuccessed = false,
                    Message = ex.Message
                };
            }

        }



        public async Task<ApiResult<bool>> remove(int OrderId)
        {
            try
            {
                _context.orders.Remove(new Orders { OrderId = OrderId });
                await _context.SaveChangesAsync();
                return new ApiSuccessResult<bool>
                {
                    IsSuccessed = true,
                    Message = "Xoá đơn hàng thành công"
                };
            }
            catch (Exception ex)
            {
                return new ApiErrorResult<bool>
                {
                    IsSuccessed = false,
                    Message = ex.Message
                };
            }

        }

        public async Task<ApiResult<bool>> updateStatusOrder(int id, string status)
        {
            try
            {
                var payment = _context.payments.FirstOrDefault(x => x.paymentId == id);
                var user = _context.Users.Where(item => item.orders.Where(o => o.payments.paymentId == id).Any()).FirstOrDefault();
                if (payment == null)
                {
                    return new ApiErrorResult<bool>("Không thể tìm thấy đơn hàng");
                }
                payment.status = status;
                //if(status == "order_confirmed")
                //{
                //    _emailRepositories.SendEmail(user.Email, "Cảm ơn bạn đã đạt hàng từ shop", "Sản phẩm sẽ được giao đến tận tay cho bạn trong thời gian sớm nhất. Một lần nữa cảm ơn bạn đã tin tưởng shop");

                //}
                //else if(status != "order_confirmed" || status != "pending")
                //{
                //    _emailRepositories.SendEmail(user.Email, "Đơn hàng bị huỷ", "Sản phẩm của bạn đã hết hàng hoặc có lỗi không mong muốn trong quá trình đặt hàng, chùng tôi rất tiết khi phải thông báo với bạn điều này. Một lần nữa cảm ơn bạn đã tin tưởng shop");
                //}
                await _context.SaveChangesAsync();
                return new ApiSuccessResult<bool>
                {
                    Message = "Cập nhật trạng thái đơn hàng thành công"
                };
            } catch (Exception ex)
            {
                return new ApiErrorResult<bool>
                {
                    IsSuccessed = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<ApiResult<bool>> updateTransCode(int id, string transCode)
        {
            try
            {
                var payment = _context.payments.FirstOrDefault(x => x.paymentId == id);
                if (payment == null)
                {
                    return new ApiErrorResult<bool>("Không thể tìm thấy đơn hàng");
                }
                payment.transactionCode = transCode;
                await _context.SaveChangesAsync();
                return new ApiSuccessResult<bool>
                {
                    Message = "Cập nhật trạng thái đơn hàng thành công"
                };
            }
            catch (Exception ex)
            {
                return new ApiErrorResult<bool>
                {
                    IsSuccessed = false,
                    Message = ex.Message
                };
            }
        }
    }
}
