namespace API.Repositories.DbAccess
{
    public class ProductAccess
    {
        //CRUD methods for Product entity

        //GetOneById -- this will be used to get a single product by its unique identifier, which is essential for displaying product details or editing a product in the frontend.

        //GetAll -- this will be used to retrieve a list of all products, which is necessary for displaying products in a catalog or inventory list on the frontend.
        //GetAllBySize -- this will allow the frontend to filter products based on their size, which can be a common requirement for users looking for specific product dimensions.
        //GetAllByWarehouse -- this will enable the frontend to display products based on their storage location, which can be useful for inventory management or for customers looking for products available in a specific warehouse.
        //GetAllByUnitStatus -- this will allow the frontend to filter products based on their availability status, which is crucial for providing accurate information to customers about whether a product is in stock, out of stock, or discontinued.
        // add whatever else is needed for the frontend to display products in the way it needs to

        //Create -- this will be used to add new products to the database, which is essential for inventory management and for keeping the product catalog up to date.

        //Update -- this will be used to modify existing product details, which is necessary for maintaining accurate and current product information in the database, such as updating prices, descriptions, or stock levels.

        //Delete -- this will be used to remove products from the database, which is important for managing the product catalog and ensuring that outdated or discontinued products are no longer available for customers to view or purchase.
    }
}
