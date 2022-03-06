namespace CSI5112BackEndApi.Models;

public class CSI5112BackEndDataBaseSettings {
    public string ConnectionString { get; set; } = null!;

    public string DatabaseName { get; set; } = null!;

    public string CustomersCollectionName { get; set; } = null!;

    public string ProductsCollectionName { get; set; } = null!;

    public string CartItemsCollectionName { get; set; } = null!;

    public string MerchantsCollectionName { get; set; } = null!;

    public string SalesOrdersCollectionName { get; set; } = null!;

    public string ShippingAddressCollectionName { get; set; } = null!;

    public string QuestionsCollectionName { get; set; } = null!;

    public string AnswersCollectionName { get; set; } = null!;
}