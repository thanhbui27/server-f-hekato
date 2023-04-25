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
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;
using System.Xml;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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

                    List<ProductInCategory> pr = new List<ProductInCategory>();
                    for (int i = 0; i < create.Categories.Count; i++)
                    {
                        pr.Add(new ProductInCategory
                        {
                            CategoryId = create.Categories[i],
                            ProductId = productMapper.ProductId,
                        });
                    }
                    productMapper.GetsProductInCategories = pr;
                    _context.products.Add(productMapper);
                    await _context.SaveChangesAsync();
                    return new ApiSuccessResult<bool>();
                }
            }
            return new ApiErrorResult<bool>();
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
            var product = _context.products.Where(x => x.ProductId == id.ProductId).FirstOrDefault();
            if (product != null)
            {
                _context.products.Remove(product);
                await _context.SaveChangesAsync();
                return new ApiSuccessResult<bool>();
            }

            return new ApiErrorResult<bool>();
        }

        public async Task<PagedResult<ProductGetAll>> GetAll(GetProductRequestPagi request)
        {

            var query = (from p in _context.products
                         join pi in _context.ProductImage on p.ProductId equals pi.ProductId
                         join pic in _context.GetsProductInCategory on pi.ProductId equals pic.ProductId into picc
                         from pics in picc.DefaultIfEmpty()
                         join c in _context.categories on pics.CategoryId equals c.CategoryId
                         select new { p, pi, c });

            int totalRow = await _context.products.CountAsync();

            if (!string.IsNullOrEmpty(request.q))
                query = query.Where(x => x.p.ProductName.Contains(request.q));



            var data = await query
               .Select(x => new ProductGetAll()
               {
                   ProductId = x.p.ProductId,
                   ProductName = x.p.ProductName,
                   PriceOld = x.p.PriceOld,
                   PriceNew = x.p.PriceNew,
                   Image_Url = x.pi.url_image,
                   dateAdd = x.p.dateAdd,
                   ProductDescription = x.p.ProductDescription,
                   ShortDetails = x.p.ShortDetails,
                   Categories = new List<CategoryGetAll>()
                  {
                      new CategoryGetAll()
                      {
                          CategoryId = x.c.CategoryId,
                          CategoryName = x.c.CategoryName,
                      }
                  }

               }).ToListAsync();

            data = data.GroupBy(p => p.ProductId)
            .Select(g => new ProductGetAll
            {
                ProductId = g.Key,
                ProductName = g.First().ProductName,
                Image_Url = g.First().Image_Url,
                PriceNew = g.First().PriceNew,
                PriceOld = g.First().PriceOld,
                ShortDetails = g.First().ShortDetails,
                ProductDescription = g.First().ProductDescription,
                dateAdd = g.First().dateAdd,
                Categories = g.SelectMany(p => p.Categories).ToList()
            }).Skip((request.PageIndex - 1) * request.PageSize)
             .Take(request.PageSize).ToList();


            var pagedResult = new PagedResult<ProductGetAll>()
            {
                TotalRecords = totalRow,
                PageSize = request.PageSize,
                PageIndex = request.PageIndex,
                Items = data
            };
            return pagedResult;
        }



        public async Task<ApiResult<bool>> update(ProductUpdate update)
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

                if (update.dateAdd != null)
                {
                    product.dateAdd = update.dateAdd;

                }

                if (update.ProductDescription != null)
                {
                    product.ProductDescription = update.ProductDescription;

                }

                if (update.ShortDetails != null)
                {
                    product.ShortDetails = update.ShortDetails;

                }
                            
                if(update.CategoryId.Count > 0)
                {
                    var existingId = product.GetsProductInCategories.Select(x => x.ProductId).ToList();
                    var SelectId = update.CategoryId.ToList();

                    var toAdd = SelectId.Except(existingId).ToList();
                    var toRemove = existingId.Except(SelectId).ToList();

                    product.GetsProductInCategories = product.GetsProductInCategories.Where(x => !toRemove.Contains(x.CategoryId)).ToList();

                    foreach (var item in toAdd)
                    {
                        product.GetsProductInCategories.Add(new ProductInCategory
                        {
                            CategoryId = item
                        });
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
                _context.products.Update(product);
                await _context.SaveChangesAsync();
                return new ApiSuccessResult<bool>(true);
            }
            return new ApiErrorResult<bool>();
        }

        public async Task<ApiResult<ProductGetById>> GetById(int id)
        {
            if (id != null)
            {
                var product = _context.products
                 .Where(p => p.ProductId == id)
                 .Select(p => new ProductGetById
                 {
                     ProductId = p.ProductId,
                     ProductName= p.ProductName,
                     List_image = p.GetsProductImage.Select(pi => new GetProductImage
                     {
                         Id = pi.Id,
                         url_image = pi.url_image,
                         timeAdd = pi.timeAdd.ToString("yyyy/MM/dd")
                     }).ToList(),

                     PriceNew = p.PriceNew,
                     PriceOld = p.PriceOld,
                     ShortDetails = p.ShortDetails,
                     ProductDescription = p.ProductDescription,
                     dateAdd = p.dateAdd,
                     Categories = p.GetsProductInCategories
                         .Select(pc => pc.GetCategory)
                         .Select(c => new CategoryGetAll
                         {
                             CategoryId=c.CategoryId,
                             CategoryName = c.CategoryName
                         }).ToList()
                 })
                 .FirstOrDefault();

                var mappterData = _mapper.Map<ProductGetById>(product);

                return new ApiSuccessResult<ProductGetById>(mappterData);
            }
            return new ApiErrorResult<ProductGetById>();
        }
        
        public async Task<ApiResult<bool>> RemoveCategory(ProductRemoveCatgory rm)
        {
            if (rm != null)
            {
                var produc = _mapper.Map<ProductInCategory>(rm);
                _context.GetsProductInCategory.Remove(produc);
                await _context.SaveChangesAsync();

                return new ApiSuccessResult<bool>();

            }
            return new ApiErrorResult<bool>();

        }

        public async Task<ApiResult<List<GetProductImage>>> UploadImage(int id, List<IFormFile> files)
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
            var listImage = _context.ProductImage.Where(p => p.ProductId== id).ToList();

            var mapperImageList = _mapper.Map<List<GetProductImage>>(listImage);

            return new  ApiSuccessResult<List<GetProductImage>>(mapperImageList);
        }

        public async Task<ApiResult<List<GetProductImage>>> GetAllImageById(int id)
        {
            var listImage = _context.ProductImage.Where(p => p.ProductId == id).ToList();

            var mapperImageList = _mapper.Map<List<GetProductImage>>(listImage);

            return new ApiSuccessResult<List<GetProductImage>>(mapperImageList);
        }
    }
}


