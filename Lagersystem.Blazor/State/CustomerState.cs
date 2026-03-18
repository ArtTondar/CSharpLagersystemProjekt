using Lagersystem.Blazor.Models.Dtos;
using Lagersystem.Blazor.Services.Abstractions;

namespace Lagersystem.Blazor.State;

public class CustomerState
{
    private readonly ICustomerService _customerService;

    public IReadOnlyList<CustomerDto> Customers { get; private set; } = new List<CustomerDto>();


    public CustomerDto? SelectedCustomer { get; private set; }

    public bool IsLoading { get; private set; }

    public CustomerState(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    public async Task LoadCustomersAsync()
    {
        IsLoading = true;

        try
        {
            Customers = await _customerService.GetCustomersAsync();
        }
        finally
        {
            IsLoading = false;
        }
    }

    public async Task LoadCustomerByIdAsync(Guid id)
    {
        IsLoading = true;

        try
        {
            SelectedCustomer = await _customerService.GetCustomerByIdAsync(id);
        }
        finally
        {
            IsLoading = false;
        }
    }

    public void ClearSelectedCustomer()
    {
        SelectedCustomer = null;
    }
}