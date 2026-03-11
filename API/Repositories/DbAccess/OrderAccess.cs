namespace API.Repositories.DbAccess
{
    public class OrderAccess
    {
        //CRUD methods for Order entity

        //GetOneById -- this will be used to get a single order by its unique identifier, which is essential for displaying order details or editing an order in the frontend.

        //GetAll -- this will be used to retrieve a list of all orders, which is necessary for displaying orders in an order management system on the frontend.
        //GetAllByCustomerId -- this will allow the frontend to filter orders based on the customer who placed them, which can be useful for customers to view their order history or for administrators to manage orders by customer.
        //GetAllByOrderDate -- this will enable the frontend to display orders based on the date they were placed, which can be useful for tracking recent orders or for generating reports based on order dates.
        // add whatever else is needed for the frontend to display orders in the way it needs to

        //Create -- this will be used to add new orders to the database, which is essential for processing customer purchases and keeping track of sales.

        //Update -- this will be used to modify existing order details, which is necessary for maintaining accurate and current order information in the database, such as updating order status, shipping information, or payment details.

        //Delete -- this will be used to remove orders from the database, which is important for managing the order records and ensuring that canceled or erroneous orders are no longer available for customers or administrators to view.
    }
}
