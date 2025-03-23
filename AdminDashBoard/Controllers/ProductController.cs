using AdminDashBoard.Helper;
using AdminDashBoard.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Talabat.Core;
using Talabat.Core.Entities;

namespace AdminDashBoard.Controllers
{
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index()
        {
            var products = await _unitOfWork.Repository<Product>().GetAllAsync();
            var mappedProduct = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductViewModel>>(products);
            return View(mappedProduct);

        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(ProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.Image != null)
                {
                    model.PictureUrl = PictureSetting.UploadFile(model.Image, "Products");
                }
                else
                {
                    model.PictureUrl = "Images/Products/sb-react1.png";
                }
                var mapped = _mapper.Map<ProductViewModel, Product>(model);
                await _unitOfWork.Repository<Product>().AddAsync(mapped);
                await _unitOfWork.CompleteAsync();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var product = await _unitOfWork.Repository<Product>().GetByIdAsync(id);
            var mapped = _mapper.Map<Product, ProductViewModel>(product);
            return View(mapped);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.Image != null)
                {     
                if(model.PictureUrl != null)
                    {
                        PictureSetting.DeleteFile(model.PictureUrl, "Products");
                        model.PictureUrl = PictureSetting.UploadFile(model.Image, "Products");
                    }
                }

               var mapped = _mapper.Map<ProductViewModel,Product >(model);
                _unitOfWork.Repository<Product>().Update(mapped);
              var result=  await _unitOfWork.CompleteAsync();

                if (result > 0) 
                { 
                 return RedirectToAction("Index");
                }

            }
            return View(model);

        }
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _unitOfWork.Repository<Product>().GetByIdAsync(id);

            var mapped = _mapper.Map<Product, ProductViewModel>(product);
            return View(mapped);
        }


        [HttpPost]
        public async Task<IActionResult> Delete(ProductViewModel model)
        {
            try
            {
                var product = await _unitOfWork.Repository<Product>().GetByIdAsync(model.Id);
                if (product.PictureUrl != null)
                {
                    PictureSetting.DeleteFile(product.PictureUrl, "Products");
                }

                _unitOfWork.Repository<Product>().Delete(product);
                await _unitOfWork.CompleteAsync();

                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return View(model);
            }
        }

    }

}
