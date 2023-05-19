using AutoMapper;
using DoAn.EF;
using DoAn.Helpers.ApiResponse;
using DoAn.Helpers.Pagination;
using DoAn.Models;
using DoAn.Repositories.StorageService;
using DoAn.Repositories.StorageService.StorageService;
using DoAn.ViewModels.Category;
using DoAn.ViewModels.Product;
using DoAn.ViewModels.ProductImage;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;
using System.Xml;

namespace DoAn.Repositories.Products
{
    public class ProductRepositories : IProductRepositories
    {
        public readonly EFDbContext _context;
        public readonly IMapper _mapper;
        private readonly IStorageService _storageService;
        private const string USER_CONTENT_FOLDER_NAME = "Images";
        private readonly ILogger<ProductRepositories> _logger;
        public ProductRepositories(EFDbContext context, IMapper mapper, IStorageService storageService, ILogger<ProductRepositories> logger)
        {
            _context = context;
            _mapper = mapper;
            _storageService = storageService;
            _logger = logger;
        }
        public async Task<ApiResult<bool>> create(ProductCreate create)
        {
            try
            {
                var productMapper = _mapper.Map<Product>(create);
                if (productMapper != null)
                {
                    if (productMapper.Image_Url != null)
                    {
                        productMapper.Image_Url = await this.SaveFile(create.Image_Url);
                        productMapper.GetsProductImage = new List<ProductImage>()
                        {
                            new ProductImage()
                            {
                                url_image = productMapper.Image_Url,
                                timeAdd = DateTime.Now
                            }
                        };
                        productMapper.productAction = new ProductActions
                        {
                            SpecialOffer = false,
                            NewArrival = false,
                            Featured= false,
                            BestSeller = false,
                        };
                        List<ProductInCategory> pr = new List<ProductInCategory>();
                        for (int i = 0; i < create.Categories.Count; i++)
                        {
                            pr.Add(new ProductInCategory
                            {
                                CategoryId = create.Categories[i],
                                ProductId = productMapper.ProductId,
                            });
                        }
                        _logger.LogInformation(productMapper.ProductId.ToString());
                        productMapper.dateAdd = DateTime.Now;
                        productMapper.GetsProductInCategories = pr;
                        _context.products.Add(productMapper);
                        await _context.SaveChangesAsync();
                        return new ApiSuccessResult<bool>
                        {
                            IsSuccessed = true,
                            Message = "Thêm sản phẩm thành công"
                        };
                    }
                }
            }catch(SqlException ex)
            {
                _logger.LogInformation("An error occurred while saving changes: " + ex.Message);           
            }
            return new ApiErrorResult<bool>
            {
                IsSuccessed = false,
                Message = "Thêm sản phẩm thất bại"
            };
        }

        private async Task<string> SaveFile(IFormFile file)
        {
            var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(originalFileName)}";
            await _storageService.SaveFileAsync(file.OpenReadStream(), fileName);
            return "/" + USER_CONTENT_FOLDER_NAME + "/" + fileName;
        }


        public async Task<ApiResult<bool>> delete(ProductDelete id)
        {
            try
            {

                var product = _context.products.Where(x => x.ProductId == id.ProductId).FirstOrDefault();
                if (product != null)
                {
                    _context.products.Remove(product);
                    await _context.SaveChangesAsync();
                    return new ApiSuccessResult<bool>
                    {
                        IsSuccessed = true,
                        Message = "Xoá sản phẩm thành công"
                    };
                }

                return new ApiErrorResult<bool>
                {
                    IsSuccessed = false,
                    Message = "Sản phẩm không tồn tại"
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

        public async Task<PagedResult<GetProductByPa>> GetAll(GetProductRequestPagi request)
        {
            try
            {

                var query = _context.products
                    .Select(p => new GetProductByPa
                    {
                        ProductId = p.ProductId,
                        ProductName = p.ProductName,
                        List_image = p.GetsProductImage.Select(pi => new GetProductImage
                        {
                            Id = pi.Id,
                            url_image = pi.url_image,
                            timeAdd = pi.timeAdd.ToString("yyyy/MM/dd")
                        }).ToList(),
                        productAction = p.productAction,
                        quantity = p.quantity,
                        PriceNew = p.PriceNew,
                        PriceOld = p.PriceOld,
                        ShortDetails = p.ShortDetails,
                        ProductDescription = p.ProductDescription,
                        dateAdd = p.dateAdd,
                        Categories = p.GetsProductInCategories
                            .Select(pc => pc.GetCategory)
                            .Select(c => new CategoryGetAll
                            {
                                CategoryId = c.CategoryId,
                                CategoryName = c.CategoryName
                            }).ToList()
                    });
                int totalRow = await _context.products.CountAsync();

                if (!string.IsNullOrEmpty(request.q))
                    query = query.Where(x => x.ProductName.Contains(request.q));

                var data = query.Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize).ToList();

                var pagedResult = new PagedResult<GetProductByPa>()
                {
                    TotalRecords = totalRow,
                    PageSize = request.PageSize,
                    PageIndex = request.PageIndex,
                    Items = data
                };
                return pagedResult;
            }
            catch (Exception ex)
            {
                return new PagedResult<GetProductByPa>
                {
                    TotalRecords = 0,
                    PageSize = request.PageSize,
                    PageIndex = request.PageIndex,
                    Items = new List<GetProductByPa>()
                };
            }
        }



        public async Task<ApiResult<bool>> update(ProductUpdate update)
        {
            try
            {
                var product = await _context.products.Include(p => p.GetsProductInCategories).ThenInclude(c => c.GetCategory).FirstOrDefaultAsync(d => d.ProductId == update.ProductId);

                if (product != null)
                {
                    if (update.ProductName != null)
                    {
                        product.ProductName = update.ProductName;

                    }
                    if (update.PriceOld != null)
                    {
                        product.PriceOld = (decimal)update.PriceOld;

                    }
                    if (update.PriceNew != null)
                    {
                        product.PriceNew = (decimal)update.PriceNew;

                    }

                    if (update.quantity != null)
                    {
                        product.quantity = (int)update.quantity;
                    }


                    if (update.ProductDescription != null)
                    {
                        product.ProductDescription = update.ProductDescription;

                    }

                    if (update.ShortDetails != null)
                    {
                        product.ShortDetails = update.ShortDetails;

                    }

                    if (update.CategoryId.Count > 0)
                    {

                        var existingId = product.GetsProductInCategories.Select(x => x.ProductId).ToList();
                        var SelectId = update.CategoryId.ToList();

                        var toAdd = SelectId.Except(existingId).ToList();
                        var toRemove = existingId.Except(SelectId).ToList();

                        product.GetsProductInCategories = product.GetsProductInCategories.Where(x => !toRemove.Contains(x.CategoryId)).ToList();

                        foreach (var item in toAdd)
                        {

                            if (product.GetsProductInCategories.Where(x => x.CategoryId == item).ToList().Count == 0)
                            {
                                product.GetsProductInCategories.Add(new ProductInCategory
                                {
                                    CategoryId = item
                                });

                            }
                            else
                            {

                                return new ApiErrorResult<bool>
                                {
                                    IsSuccessed = false,
                                    Message = "Không thể cập nhật category cho sản phẩm"
                                };
                            }

                        }

                    }

                    if (update.Image_Url != null)
                    {
                        var path = await this.SaveFile(update.Image_Url);
                        product.Image_Url = path;

                        var productImage = await _context.ProductImage.FirstOrDefaultAsync(x => x.ProductId == product.ProductId);
                        if (productImage != null)
                        {
                            productImage.url_image = path;
                            productImage.timeAdd = DateTime.Now;
                            _context.ProductImage.Update(productImage);
                            await _context.SaveChangesAsync();
                        }
                    }
                    product.dateAdd = DateTime.Now;
                    _context.products.Update(product);
                    await _context.SaveChangesAsync();
                    return new ApiSuccessResult<bool>
                    {
                        IsSuccessed = true,
                        Message = "Cập nhật sản phẩm thành công"
                    };
                }
                return new ApiErrorResult<bool>
                {
                    IsSuccessed = false,
                    Message = "Cập nhật sản phẩm thất bại"
                }; 
            }catch(Exception ex)
            {
                return new ApiErrorResult<bool>
                {
                    IsSuccessed = false,
                    Message = ex.Message
                };
            }

          
        }

        public async Task<ApiResult<ProductGetById>> GetById(int id)
        {
            try
            {
                if (id != null)
                {
                    var product = _context.products
                     .Where(p => p.ProductId == id)
                     .Select(p => new ProductGetById
                     {
                         ProductId = p.ProductId,
                         ProductName = p.ProductName,
                         List_image = p.GetsProductImage.Select(pi => new GetProductImage
                         {
                             Id = pi.Id,
                             url_image = pi.url_image,
                             timeAdd = pi.timeAdd.ToString("yyyy/MM/dd")
                         }).ToList(),
                         quantity = p.quantity,
                         PriceNew = p.PriceNew,
                         PriceOld = p.PriceOld,
                         ShortDetails = p.ShortDetails,
                         ProductDescription = p.ProductDescription,
                         dateAdd = p.dateAdd,
                         Categories = p.GetsProductInCategories
                             .Select(pc => pc.GetCategory)
                             .Select(c => new CategoryGetAll
                             {
                                 CategoryId = c.CategoryId,
                                 CategoryName = c.CategoryName
                             }).ToList()
                     })
                     .FirstOrDefault();

                    var mappterData = _mapper.Map<ProductGetById>(product);

                    return new ApiSuccessResult<ProductGetById>(mappterData);
                }
                return new ApiErrorResult<ProductGetById>();
            }catch(Exception ex)
            {
                return new ApiErrorResult<ProductGetById>
                {
                    IsSuccessed= false,
                    Message=ex.Message,
                };
            }
           
        }
        
        public async Task<ApiResult<bool>> RemoveCategory(ProductRemoveCatgory rm)
        {
            try
            {
                if (rm != null)
                {
                    var produc = _mapper.Map<ProductInCategory>(rm);
                    _context.GetsProductInCategory.Remove(produc);
                    await _context.SaveChangesAsync();

                    return new ApiSuccessResult<bool>
                    {
                        IsSuccessed = true,
                        Message = "Xoá category của sản phẩm thành công"
                    };

                }
                return new ApiErrorResult<bool>
                {
                    IsSuccessed = false,
                    Message = "Xoá category của sản phẩm thất bại"
                };


            }catch(Exception ex)
            {
                return new ApiErrorResult<bool>
                {
                    IsSuccessed = false,
                    Message = ex.Message
                };
            }


        }

        public async Task<ApiResult<List<GetProductImage>>> UploadImage(int id, List<IFormFile> files)
        {
            try
            {
                foreach (var file in files)
                {
                    if (file.Length > 0)
                    {
                        _context.ProductImage.Add(new ProductImage
                        {
                            ProductId = id,
                            url_image = await this.SaveFile(file),
                            timeAdd = DateTime.Now

                        });
                        await _context.SaveChangesAsync();
                    }
                }
                var listImage = _context.ProductImage.Where(p => p.ProductId == id).ToList();

                var mapperImageList = _mapper.Map<List<GetProductImage>>(listImage);

                return new ApiSuccessResult<List<GetProductImage>>(mapperImageList);
            }catch(Exception ex)
            {
                return new ApiErrorResult<List<GetProductImage>>
                {
                    IsSuccessed = false,
                    Message = ex.Message
                };
            }
           
        }

        public async Task<ApiResult<List<GetProductImage>>> GetAllImageById(int id)
        {
            try
            {
                var listImage = _context.ProductImage.Select(x => new ProductImage
                {
                    ProductId = x.ProductId,
                    url_image = x.url_image,
                    Id = id,
                    timeAdd = x.timeAdd
                }).Where(p => p.ProductId == id).ToList();

                var mapperImageList = _mapper.Map<List<GetProductImage>>(listImage);

                return new ApiSuccessResult<List<GetProductImage>>(mapperImageList);
            }catch(Exception ex)
            {
                return new ApiErrorResult<List<GetProductImage>>
                {
                    IsSuccessed = false,
                    Message = ex.Message
                };
            }
          
        }

        public async Task<ApiResult<List<GetProductByPa>>> GetProductFeature()
        {
            try
            {
                var product = await _context.products
                .Select(p => new GetProductByPa
                {
                    ProductId = p.ProductId,
                    ProductName = p.ProductName,
                    List_image = p.GetsProductImage.Select(pi => new GetProductImage
                    {
                        Id = pi.Id,
                        url_image = pi.url_image,
                        timeAdd = pi.timeAdd.ToString("yyyy/MM/dd")
                    }).ToList(),
                    productAction = p.productAction,
                    quantity = p.quantity,
                    PriceNew = p.PriceNew,
                    PriceOld = p.PriceOld,
                    ShortDetails = p.ShortDetails,
                    ProductDescription = p.ProductDescription,
                    dateAdd = p.dateAdd,
                    Categories = p.GetsProductInCategories
                        .Select(pc => pc.GetCategory)
                        .Select(c => new CategoryGetAll
                        {
                            CategoryId = c.CategoryId,
                            CategoryName = c.CategoryName
                        }).ToList()
                }).Where(x => x.productAction.Featured == true)
                .ToListAsync();

                return new ApiSuccessResult<List<GetProductByPa>>(product);
            }catch(Exception ex)
            {
                return new ApiErrorResult<List<GetProductByPa>>
                {
                    IsSuccessed = false,
                    Message = ex.Message
                };
            }
          
        }

        public async Task<ApiResult<List<GetProductByPa>>> GetProductBestSeller()
        {
            try
            {
                var product = await _context.products
                                .Select(p => new GetProductByPa
                                {
                                    ProductId = p.ProductId,
                                    ProductName = p.ProductName,
                                    List_image = p.GetsProductImage.Select(pi => new GetProductImage
                                    {
                                        Id = pi.Id,
                                        url_image = pi.url_image,
                                        timeAdd = pi.timeAdd.ToString("yyyy/MM/dd")
                                    }).ToList(),
                                    productAction = p.productAction,
                                    quantity = p.quantity,
                                    PriceNew = p.PriceNew,
                                    PriceOld = p.PriceOld,
                                    ShortDetails = p.ShortDetails,
                                    ProductDescription = p.ProductDescription,
                                    dateAdd = p.dateAdd,
                                    Categories = p.GetsProductInCategories
                                        .Select(pc => pc.GetCategory)
                                        .Select(c => new CategoryGetAll
                                        {
                                            CategoryId = c.CategoryId,
                                            CategoryName = c.CategoryName
                                        }).ToList()
                                }).Where(x => x.productAction.BestSeller == true)
                                .ToListAsync();

                return new ApiSuccessResult<List<GetProductByPa>>(product);
            }
            catch (Exception ex)
            {
                return new ApiErrorResult<List<GetProductByPa>>
                {
                    IsSuccessed = false,
                    Message = ex.Message
                };
            }

        }

        public async Task<ApiResult<List<GetProductByPa>>> GetProductSpecialOffer()
        {
            try
            {

            var product = await _context.products
                  .Select(p => new GetProductByPa
                  {
                      ProductId = p.ProductId,
                      ProductName = p.ProductName,
                      List_image = p.GetsProductImage.Select(pi => new GetProductImage
                      {
                          Id = pi.Id,
                          url_image = pi.url_image,
                          timeAdd = pi.timeAdd.ToString("yyyy/MM/dd")
                      }).ToList(),
                      productAction = p.productAction,
                      quantity = p.quantity,
                      PriceNew = p.PriceNew,
                      PriceOld = p.PriceOld,
                      ShortDetails = p.ShortDetails,
                      ProductDescription = p.ProductDescription,
                      dateAdd = p.dateAdd,
                      Categories = p.GetsProductInCategories
                          .Select(pc => pc.GetCategory)
                          .Select(c => new CategoryGetAll
                          {
                              CategoryId = c.CategoryId,
                              CategoryName = c.CategoryName
                          }).ToList()
                  }).Where(x => x.productAction.SpecialOffer == true)
                  .ToListAsync();

            return new ApiSuccessResult<List<GetProductByPa>>(product);
            }
            catch (Exception ex)
            {
                return new ApiErrorResult<List<GetProductByPa>>
                {
                    IsSuccessed = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<ApiResult<List<GetProductByPa>>> GetProductNewArrival()
        {
            try
            {

            var product = await _context.products
                 .Select(p => new GetProductByPa
                 {
                     ProductId = p.ProductId,
                     ProductName = p.ProductName,
                     List_image = p.GetsProductImage.Select(pi => new GetProductImage
                     {
                         Id = pi.Id,
                         url_image = pi.url_image,
                         timeAdd = pi.timeAdd.ToString("yyyy/MM/dd")
                     }).ToList(),
                     productAction = p.productAction,
                     quantity = p.quantity,
                     PriceNew = p.PriceNew,
                     PriceOld = p.PriceOld,
                     ShortDetails = p.ShortDetails,
                     ProductDescription = p.ProductDescription,
                     dateAdd = p.dateAdd,
                     Categories = p.GetsProductInCategories
                         .Select(pc => pc.GetCategory)
                         .Select(c => new CategoryGetAll
                         {
                             CategoryId = c.CategoryId,
                             CategoryName = c.CategoryName
                         }).ToList()
                 }).Where(x => x.productAction.NewArrival == true)
                 .ToListAsync();

            return new ApiSuccessResult<List<GetProductByPa>>(product);
            }
            catch (Exception ex)
            {
                return new ApiErrorResult<List<GetProductByPa>>
                {
                    IsSuccessed = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<ApiResult<List<GetProductByPa>>> GetProductTrending()
        {
            try
            {

            var product = await _context.products
                .Select(p => new GetProductByPa
                {
                    ProductId = p.ProductId,
                    ProductName = p.ProductName,
                    List_image = p.GetsProductImage.Select(pi => new GetProductImage
                    {
                        Id = pi.Id,
                        url_image = pi.url_image,
                        timeAdd = pi.timeAdd.ToString("yyyy/MM/dd")
                    }).ToList(),
                    productAction = p.productAction,
                    quantity = p.quantity,
                    PriceNew = p.PriceNew,
                    PriceOld = p.PriceOld,
                    ShortDetails = p.ShortDetails,
                    ProductDescription = p.ProductDescription,
                    dateAdd = p.dateAdd,
                    Categories = p.GetsProductInCategories
                        .Select(pc => pc.GetCategory)
                        .Select(c => new CategoryGetAll
                        {
                            CategoryId = c.CategoryId,
                            CategoryName = c.CategoryName
                        }).ToList()
                }).Where(x => x.productAction.trending == true)
                .ToListAsync();

            return new ApiSuccessResult<List<GetProductByPa>>(product);
            }
            catch (Exception ex)
            {
                return new ApiErrorResult<List<GetProductByPa>>
                {
                    IsSuccessed = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<ApiResult<List<GetProductByPa>>> GetProductTrendSmall()
        {
            try
            {

            var product = await _context.products
                .Select(p => new GetProductByPa
                {
                    ProductId = p.ProductId,
                    ProductName = p.ProductName,
                    List_image = p.GetsProductImage.Select(pi => new GetProductImage
                    {
                        Id = pi.Id,
                        url_image = pi.url_image,
                        timeAdd = pi.timeAdd.ToString("yyyy/MM/dd")
                    }).ToList(),
                    productAction = p.productAction,
                    quantity = p.quantity,
                    PriceNew = p.PriceNew,
                    PriceOld = p.PriceOld,
                    ShortDetails = p.ShortDetails,
                    ProductDescription = p.ProductDescription,
                    dateAdd = p.dateAdd,
                    Categories = p.GetsProductInCategories
                        .Select(pc => pc.GetCategory)
                        .Select(c => new CategoryGetAll
                        {
                            CategoryId = c.CategoryId,
                            CategoryName = c.CategoryName
                        }).ToList()
                }).Where(x => x.productAction.trendSmall == true).OrderByDescending(x => x.ProductId)
                .ToListAsync();

            return new ApiSuccessResult<List<GetProductByPa>>(product);
            }
            catch (Exception ex)
            {
                return new ApiErrorResult<List<GetProductByPa>>
                {
                    IsSuccessed = false,
                    Message = ex.Message
                };
            }
        }
    }
}


