using AutoMapper;
using MCComputers.Entities;
using MCComputers.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCComputers.Repositories.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Invoice Mappings
            CreateMap<Invoice, InvoiceResponseModel>()
                .ForMember(dest => dest.Items,
                    opt => opt.MapFrom(src => src.InvoiceItems));

            CreateMap<InvoiceCreateModel, Invoice>()
                .ForMember(dest => dest.InvoiceItems,
                    opt => opt.Ignore()) // Manually map invoice items
                .ForMember(dest => dest.TransactionDate,
                    opt => opt.MapFrom(_ => DateTime.UtcNow));

            // Invoice Item Mappings
            CreateMap<InvoiceItem, InvoiceItemResponseModel>()
                .ForMember(dest => dest.ProductName,
                    opt => opt.MapFrom(src => src.Product.Name));

            CreateMap<InvoiceItemModel, InvoiceItem>()
                .ForMember(dest => dest.LineTotal,
                    opt => opt.MapFrom((src, dest, _, context) =>
                    {
                        var product = context.Items.OfType<Product>().FirstOrDefault(p => p.Id == src.ProductId);
                        return product != null ? product.Price * src.Quantity : 0;
                    }));

            // Product Mappings
            CreateMap<Product, ProductModel>();
            CreateMap<ProductModel, Product>();

            // Customer Mappings
            CreateMap<Customer, CustomerModel>();
            CreateMap<CustomerModel, Customer>();

        }
    }
}
