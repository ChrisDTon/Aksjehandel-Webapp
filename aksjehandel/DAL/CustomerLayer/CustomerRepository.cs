using aksjehandel.Model;
using aksjehandel;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using aksjehandel.Controllers;

namespace aksjehandel.DAL
{
    public class CustomerRepository: ICustomerRepository
    {
        private readonly Context _db;
        private readonly ILogger<CustomerRepository> _logger;

        public CustomerRepository(Context db, ILogger<CustomerRepository> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<Customer> Get(int userId)
        {
            try
            {
                var customerObject = await _db.Customers.FindAsync(userId);

                if (customerObject == null)
                {   
                    // Failed to find customer
                    return null;
                }

                var customer = new Customer { id = customerObject.id, firstname = customerObject.firstname, lastname = customerObject.lastname, email = customerObject.email };

                return customer;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                return null;
            }
        }

        public async Task<int?> Login(LoginCustomer customer)
        {
            try
            {
                Customers? customerFound = await _db.Customers.FirstOrDefaultAsync(b => b.email == customer.email);

                if (customerFound == null)
                {
                    // Customer not found.
                    return null;
                }

                byte[] hash = Helpers.LagHash(customer.password, customerFound.salt);
                bool ok = hash.SequenceEqual(customerFound.password_hash);

                if (!ok)
                {
                    // Password does not match hash / is incorrect
                    return null;
                }
           
                return customerFound.id;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                return null;
            }
        }

        public async Task<bool> Register (CreateCustomer customer)
        {
            try
            {
                Customers emailUsed = await _db.Customers.FirstOrDefaultAsync(b => b.email == customer.email);

                if (emailUsed != null)
                {
                    return false;
                }

                Customers newCustomer = new Customers();

                byte[] salt = Helpers.LagSalt();
                byte[] hash = Helpers.LagHash(customer.password, salt);

                newCustomer.email = customer.email;
                newCustomer.firstname = customer.firstname;
                newCustomer.lastname = customer.lastname;
                newCustomer.password_hash = hash;
                newCustomer.salt = salt;

                _db.Customers.Add(newCustomer);
                await _db.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                return false;
            }
        }

        public async Task<bool> Update(UpdateCustomer updateCustomer, int userId)
        {
            try
            {
                var customerObject = await _db.Customers.FindAsync(userId);

                if (customerObject == null)
                {

                    return false;
                }

                var emailUsed = await _db.Customers.FirstOrDefaultAsync(c => c.email == updateCustomer.email && c.id != userId);

                if (emailUsed != null)
                {
                    return false;
                }

                customerObject.email = updateCustomer.email;
                customerObject.firstname = updateCustomer.firstname;
                customerObject.lastname = updateCustomer.lastname;

                await _db.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                return false;
            }
        }

        public async Task<bool> ChangePassword(string password, int userId)
        {
            try
            {
                var customerObject = await _db.Customers.FindAsync(userId);

                if (customerObject == null)
                {

                    return false;
                }

                byte[] salt = Helpers.LagSalt();
                byte[] hash = Helpers.LagHash(password, salt);

                customerObject.salt = salt;
                customerObject.password_hash = hash;

                await _db.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                return false;
            }
        }

    }
}
