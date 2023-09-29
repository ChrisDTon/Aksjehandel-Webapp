using aksjehandel.Model;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace aksjehandel.DAL;

public class CreateCustomer
{
    [RegularExpression(@"^[a-zA-ZæøåÆØÅ ]{1,35}$")]
    public string firstname { get; set; }
    [RegularExpression(@"^[a-zA-ZæøåÆØÅ ]{1,35}$")]
    public string lastname { get; set; }
    [RegularExpression(@"^[a-z0-9][-a-z0-9._]+@([-a-z0-9]+.)+[a-z]{2,5}$")]
    public string email { get; set; }
    
    public string password { get; set; }
}

public class UpdateCustomer
{
    [RegularExpression(@"^[a-zA-ZæøåÆØÅ ]{1,35}$")]
    public string firstname { get; set; }
    [RegularExpression(@"^[a-zA-ZæøåÆØÅ ]{1,35}$")]
    public string lastname { get; set; }
    [RegularExpression(@"^[a-z0-9][-a-z0-9._]+@([-a-z0-9]+.)+[a-z]{2,5}$")]
    public string email { get; set; }
}

public class LoginCustomer
{
    public string email { get; set; }
    public string password { get; set; }
}

public class ChangePassword
{
    public string password { get; set; }
}

public interface ICustomerRepository
{
    Task<Customer> Get(int userId);
    Task<int?> Login(LoginCustomer customer);
    Task<bool> Register(CreateCustomer customer);
    Task<bool> Update(UpdateCustomer customer, int userId);
    Task<bool> ChangePassword (string password, int userId);
}
