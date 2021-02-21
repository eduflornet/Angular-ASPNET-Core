using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Angular_ASPNET_Core.Data;
using Angular_ASPNET_Core.Data.Entities;
using Angular_ASPNET_Core.Infrastructure;
using Angular_ASPNET_Core.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Angular_ASPNET_Core.Apis
{
    [Route("api/customers")]
    public class CustomersApiController : Controller
    {
        ICustomersRepository _CustomersRepository;
        ILogger _Logger;
        private readonly IMapper _Mapper;

        public CustomersApiController(ICustomersRepository customersRepo, ILoggerFactory loggerFactory, IMapper mapper) {
            _CustomersRepository = customersRepo;
            _Logger = loggerFactory.CreateLogger(nameof(CustomersApiController));
            _Mapper = mapper;
        }

        // GET api/customers
        [HttpGet]
        [NoCache]
        [ProducesResponseType(typeof(List<Customer>), 200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        public async Task<ActionResult> Customers()
        {
            try
            {
                var customers = await _CustomersRepository.GetCustomersAsync();
                return Ok(_Mapper.Map<CustomerModel[]>(customers));
            }
            catch (Exception exp)
            {
                _Logger.LogError(exp.Message);
                return BadRequest(new ApiResponse { Status = false });
            }
        }

        // GET api/customers/page/10/10
        [HttpGet("page/{skip}/{take}")]
        [NoCache]
        [ProducesResponseType(typeof(List<Customer>), 200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        public async Task<ActionResult> CustomersPage(int skip, int take)
        {
            try
            {
                var pagingResult = await _CustomersRepository.GetCustomersPageAsync(skip, take);
                Response.Headers.Add("X-InlineCount", pagingResult.TotalRecords.ToString());
                return Ok(pagingResult.Records);
            }
            catch (Exception exp)
            {
                _Logger.LogError(exp.Message);
                return BadRequest(new ApiResponse { Status = false });
            }
        }

        // GET api/customers/5
        [HttpGet("{id}", Name = "GetCustomerRoute")]
        [NoCache]
        [ProducesResponseType(typeof(Customer), 200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        public async Task<ActionResult> Customers(int id)
        {
            try
            {
                var customer = await _CustomersRepository.GetCustomerAsync(id);
                return Ok(_Mapper.Map<CustomerModel[]>(customer));
            }
            catch (Exception exp)
            {
                _Logger.LogError(exp.Message);
                return BadRequest(new ApiResponse { Status = false });
            }
        }

        // POST api/customers
        [HttpPost]
        // [ValidateAntiForgeryToken]
        [ProducesResponseType(typeof(ApiResponse), 201)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        public async Task<ActionResult> CreateCustomer([FromBody]CustomerModel customer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse { Status = false, ModelState = ModelState });
            }

            try
            {
                var customerEntity = _Mapper.Map<Customer>(customer);
                var newCustomer = await _CustomersRepository.InsertCustomerAsync(customerEntity);
                var newCustomerModel = _Mapper.Map<CustomerModel>(newCustomer);
                if (newCustomerModel == null)
                {
                    return BadRequest(new ApiResponse { Status = false });
                }
                return CreatedAtRoute("GetCustomerRoute", new { id = newCustomerModel.Id },
                        new ApiResponse { Status = true, Customer = newCustomerModel });
            }
            catch (Exception exp)
            {
                _Logger.LogError(exp.Message);
                return BadRequest(new ApiResponse { Status = false });
            }
        }

        // PUT api/customers/5
        [HttpPut("{id}")]
        // [ValidateAntiForgeryToken]
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        public async Task<ActionResult> UpdateCustomer(int id, [FromBody]CustomerModel customer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse { Status = false, ModelState = ModelState });
            }

            try
            {
                var customerEntity = _Mapper.Map<Customer>(customer);
                var status = await _CustomersRepository.UpdateCustomerAsync(customerEntity);
                if (!status)
                {
                    return BadRequest(new ApiResponse { Status = false });
                }
                return Ok(new ApiResponse { Status = true, Customer = customer });
            }
            catch (Exception exp)
            {
                _Logger.LogError(exp.Message);
                return BadRequest(new ApiResponse { Status = false });
            }
        }

        // DELETE api/customers/5
        [HttpDelete("{id}")]
        // [ValidateAntiForgeryToken]
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        public async Task<ActionResult> DeleteCustomer(int id)
        {
            try
            {
                var status = await _CustomersRepository.DeleteCustomerAsync(id);
                if (!status)
                {
                    return BadRequest(new ApiResponse { Status = false });
                }
                return Ok(new ApiResponse { Status = true });
            }
            catch (Exception exp)
            {
                _Logger.LogError(exp.Message);
                return BadRequest(new ApiResponse { Status = false });
            }
        }

    }
}
