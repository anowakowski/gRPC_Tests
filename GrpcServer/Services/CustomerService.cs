using Grpc.Core;

namespace GrpcServer.Services
{
    public class CustomerService : Customer.CustomerBase
    {
        private readonly ILogger<GreeterService> _logger;
        public CustomerService(ILogger<GreeterService> logger)
        {
            _logger = logger;
        }

        public override Task<CustomerModel> GetCustomerInfo(CustomerLookupModel request, ServerCallContext context)
        {
            CustomerModel output = new CustomerModel();

            if (request.UserId == 1)
            {
                output.FirstName = "Tom";
                output.LastName = "Bee";
            }

            return Task.FromResult(output);
        }

        public override async Task GetNewCustomers(NewCustomerRequest request, IServerStreamWriter<CustomerModel> responseStream, ServerCallContext context)
        {
            List<CustomerModel> customers = new List<CustomerModel>
            {
                new CustomerModel()
                {
                    FirstName = "Tim",
                    LastName = "Bee"
                },
                new CustomerModel()
                {
                    FirstName = "Tedy",
                    LastName = "Bee2"
                }
            };

            foreach(var cust in customers)
            {
                await responseStream.WriteAsync(cust);
            }
        }
    }
}
