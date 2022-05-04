using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using PriceParser.Core;
using PriceParser.Core.DTO;
using PriceParser.Core.Interfaces;
using PriceParser.Data.Entities;
using PriceParser.DataAccess;
using PriceParser.Domain.CQS;
using PriceParser.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PriceParser.Domain.Tests
{
    [TestFixture]
    public class ProductServiceTests
    {
        private ProductService _productService;
        private Mock<ILogger<ProductService>> _loggerMock;
        private IMapper _mapper;
        private Mock<IUnitOfWork> _unitOfWork;

        [SetUp]
        public void Setup()
        {
            _loggerMock = new Mock<ILogger<ProductService>>();
            if (_mapper == null)
            {
                var mappingConfig = new MapperConfiguration(mc =>
                {
                    mc.AddProfile(new ProductMapper());
                    mc.AddProfile(new UserReviewsMapper());
                    mc.AddProfile(new ProductFromSitesMapper());
                });
                IMapper mapper = mappingConfig.CreateMapper();
                _mapper = mapper;
            }
            _unitOfWork = new Mock<IUnitOfWork>();
            _productService = new ProductService(_unitOfWork.Object, _mapper, _loggerMock.Object);
        }

        [Test]
        public async Task GetAllProductsAsync_ReturnsData()
        {
            var result = new List<Product>()
                {
                    new Product(),
                    new Product(),
                    new Product()
                }.AsQueryable();

            _unitOfWork.Setup(x => x.Products.Get(It.IsAny<Expression<Func<Product, bool>>?>(), 
                It.IsAny<Func<IQueryable<Product>, IOrderedQueryable<Product>>?>(),
                It.IsAny<Expression<Func<Product, object>>[]>())).ReturnsAsync(result);

            var products = await _productService.GetAllProductsAsync();
            
            Assert.AreEqual(products.Count(), 3);
        }

        [Test]
        public async Task UpdateAggregatedPricesDataAsync_PricesAggregateCorrectly()
        {
            var productEntity = new Product() { Id = Guid.Parse("473a7283-1237-49d8-107b-08da14b6dec1") };
            var productFromSiteEntity1 = new ProductFromSites() { Id = Guid.Parse("c8985f03-2977-4726-106a-08da14b6dec1"), ProductId = productEntity.Id, product = productEntity };
            var productFromSiteEntity2 = new ProductFromSites() { Id = Guid.Parse("1130f775-cc1f-4d1c-1091-08da14b6dec1"), ProductId = productEntity.Id, product = productEntity };

            var prices = new List<ProductPrice>()
            { 
                new ProductPrice() { Id = Guid.NewGuid(), ParseDate = DateTime.Now.AddDays(-1), 
                    ProductFromSiteId = productFromSiteEntity1.Id, ProductFromSite = productFromSiteEntity1, FullPrice = 10},
                new ProductPrice() { Id = Guid.NewGuid(), ParseDate = DateTime.Now, 
                    ProductFromSiteId = productFromSiteEntity1.Id, ProductFromSite = productFromSiteEntity1, FullPrice = 15},
                new ProductPrice() { Id = Guid.NewGuid(), ParseDate = DateTime.Now.AddDays(-1), 
                    ProductFromSiteId = productFromSiteEntity2.Id, ProductFromSite = productFromSiteEntity2, FullPrice = 12},
                new ProductPrice() { Id = Guid.NewGuid(), ParseDate = DateTime.Now, 
                    ProductFromSiteId = productFromSiteEntity2.Id, ProductFromSite = productFromSiteEntity2, FullPrice = 17}
            };

            _unitOfWork.Setup(x => x.Products.GetQueryable())
                .ReturnsAsync(new List<Product>() { productEntity }.AsQueryable());
            _unitOfWork.Setup(x => x.ProductsFromSites.GetQueryable())
                .ReturnsAsync(new List<ProductFromSites>() { productFromSiteEntity1, productFromSiteEntity2 }.AsQueryable());
            _unitOfWork.Setup(x => x.ProductPricesHistory.GetQueryable())
                .ReturnsAsync(prices.AsQueryable());
            _unitOfWork.Setup(x => x.Products.FindBy(It.IsAny<Expression<Func<Product, bool>>?>(), It.IsAny<Expression<Func<Product, object>>[]>()))
                .ReturnsAsync( productEntity );

            _unitOfWork.Setup(x => x.Products.Update(It.IsAny<Product>())).Callback((Product s) => productEntity = s);

            _productService = new ProductService(_unitOfWork.Object, _mapper, _loggerMock.Object);

            await _productService.UpdateAggregatedPricesDataAsync(productEntity.Id);

            Assert.AreEqual(productEntity.Id, productEntity.Id);
            Assert.AreEqual(productEntity.AveragePriceNow, 16);
            Assert.AreEqual(productEntity.AveragePriceOverall, 13.5);
            Assert.AreEqual(productEntity.BestPriceNow, 15);
            Assert.AreEqual(productEntity.BestPriceOverall, 10);
        }

        [Test]
        public async Task UpdateAggregatedPricesDataAsync_WrongProductId_PricesAggregateDoesntThrowException() //Failing test
        {
            var productEntity = new Product() { Id = Guid.Parse("473a7283-1237-49d8-107b-08da14b6dec1") };
            var productFromSiteEntity1 = new ProductFromSites() { Id = Guid.Parse("c8985f03-2977-4726-106a-08da14b6dec1"), ProductId = productEntity.Id, product = productEntity };
            var productFromSiteEntity2 = new ProductFromSites() { Id = Guid.Parse("1130f775-cc1f-4d1c-1091-08da14b6dec1"), ProductId = productEntity.Id, product = productEntity };

            var prices = new List<ProductPrice>()
            { 
                new ProductPrice() { Id = Guid.NewGuid(), ParseDate = DateTime.Now.AddDays(-1), 
                    ProductFromSiteId = productFromSiteEntity1.Id, ProductFromSite = productFromSiteEntity1, FullPrice = 10},
                new ProductPrice() { Id = Guid.NewGuid(), ParseDate = DateTime.Now, 
                    ProductFromSiteId = productFromSiteEntity1.Id, ProductFromSite = productFromSiteEntity1, FullPrice = 15},
                new ProductPrice() { Id = Guid.NewGuid(), ParseDate = DateTime.Now.AddDays(-1), 
                    ProductFromSiteId = productFromSiteEntity2.Id, ProductFromSite = productFromSiteEntity2, FullPrice = 12},
                new ProductPrice() { Id = Guid.NewGuid(), ParseDate = DateTime.Now, 
                    ProductFromSiteId = productFromSiteEntity2.Id, ProductFromSite = productFromSiteEntity2, FullPrice = 17}
            };

            _unitOfWork.Setup(x => x.Products.GetQueryable())
                .ReturnsAsync(new List<Product>() { productEntity }.AsQueryable());
            _unitOfWork.Setup(x => x.ProductsFromSites.GetQueryable())
                .ReturnsAsync(new List<ProductFromSites>() { productFromSiteEntity1, productFromSiteEntity2 }.AsQueryable());
            _unitOfWork.Setup(x => x.ProductPricesHistory.GetQueryable())
                .ReturnsAsync(prices.AsQueryable());
            _unitOfWork.Setup(x => x.Products.FindBy(It.IsAny<Expression<Func<Product, bool>>?>(), It.IsAny<Expression<Func<Product, object>>[]>()))
                .ReturnsAsync( productEntity );

            _unitOfWork.Setup(x => x.Products.Update(It.IsAny<Product>())).Callback((Product s) => productEntity = s);

            _productService = new ProductService(_unitOfWork.Object, _mapper, _loggerMock.Object);

            Assert.ThrowsAsync<NullReferenceException>(async () => await _productService.UpdateAggregatedPricesDataAsync(Guid.Empty));
        }

    }
}