using DBConfirm.Core.Runners.Abstract;
using DBConfirm.Core.Templates;
using System.Threading.Tasks;

namespace Sample.Nuget.Northwind.SQLServer.MSTest.Tests.Templates.Complex
{
    public class CompleteOrderForCustomerTemplate : BaseComplexTemplate
    {
        public CustomersTemplate CustomersTemplate { get; set; } = new CustomersTemplate();
        public ProductsTemplate ProductsTemplate { get; set; } = new ProductsTemplate();
        public OrdersTemplate OrdersTemplate { get; set; } = new OrdersTemplate();
        public Order_DetailsTemplate Order_DetailsTemplate { get; set; } = new Order_DetailsTemplate();

        public override async Task InsertAsync(ITestRunner testRunner)
        {
            if (!CustomersTemplate.IsInserted)
            {
                await testRunner.InsertTemplateAsync(CustomersTemplate);
            }

            if (!ProductsTemplate.IsInserted)
            {
                await testRunner.InsertTemplateAsync(ProductsTemplate);
            }

            if (!OrdersTemplate.IsInserted)
            {
                OrdersTemplate.WithCustomerID((string)CustomersTemplate.MergedData["CustomerID"]);
                await testRunner.InsertTemplateAsync(OrdersTemplate);
            }

            if (!Order_DetailsTemplate.IsInserted)
            {
                Order_DetailsTemplate.WithOrderID(OrdersTemplate.Identity).WithProductID(ProductsTemplate.Identity);
                await testRunner.InsertTemplateAsync(Order_DetailsTemplate);
            }
        }
    }
}
