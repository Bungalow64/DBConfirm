using DBConfirm.Core.Runners.Abstract;
using DBConfirm.Core.Templates;
using System.Threading.Tasks;

namespace Sample.NUnit.Northwind.Tests.Templates.Complex;

public class CompleteOrderForCustomerTemplate : BaseComplexTemplate
{
    public CustomersTemplate CustomersTemplate { get; set; } = [];
    public ProductsTemplate ProductsTemplate { get; set; } = [];
    public OrdersTemplate OrdersTemplate { get; set; } = [];
    public Order_DetailsTemplate Order_DetailsTemplate { get; set; } = [];

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
