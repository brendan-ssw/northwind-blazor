using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Northwind.Ef;
using NorthwindBlazor.Shared;
using NorthwindBlazor.Shared.Customer;

namespace NorthwindApplication.Customer
{
    

    public class GetCustomersHandler : IRequestHandler<GetCustomers, IEnumerable<CustomerModel>>
    {

        private readonly NorthwindContext _dbCtx;

        public GetCustomersHandler(NorthwindContext dbCtx)
        {
            _dbCtx = dbCtx;
        }

        public async Task<IEnumerable<CustomerModel>> Handle(GetCustomers request, CancellationToken cancellationToken)
        {
            var query =  _dbCtx.Customers.Select(c => new CustomerModel()
            {
                Address = c.Address,
                City = c.City,
                CompanyName = c.CompanyName,
                ContactName = c.ContactName,
                CustomerId = c.CustomerId
            })
            .OrderBy(c => c.CompanyName)
            .AsQueryable();

            if (!string.IsNullOrEmpty(request.SearchText))
            {
                query = query.Where(c => c.CompanyName.ToLower().StartsWith(request.SearchText.ToLower()));
            }
                
            return await query.ToListAsync(cancellationToken);
        }
    }
    
}