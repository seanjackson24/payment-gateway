using System.Threading;
using System.Threading.Tasks;
using PaymentGateway.Domain.DomainModels;

namespace PaymentGateway.Domain.Services
{
	public interface IPaymentRepository
	{
		Task<bool> PaymentExists(string paymentId, CancellationToken cancellationToken);
		Task Insert(Payment payment);
		Task<Payment> GetPaymentById(string paymentId);
	}
	public class PaymentRepository : IPaymentRepository
	{
		private readonly PaymentGatewayDbContext _context;

		public PaymentRepository(PaymentGatewayDbContext context)
		{
			_context = context;
		}

		public async Task<Payment> GetPaymentById(string paymentId)
		{
			return await _context.Payments.FindAsync(paymentId);
		}

		public async Task Insert(Payment payment)
		{
			await _context.Payments.AddAsync(payment);
			await _context.SaveChangesAsync();
		}

		public async Task<bool> PaymentExists(string paymentId, CancellationToken cancellationToken)
		{
			var payment = await _context.Payments.FindAsync(keyValues: new[] { paymentId }, cancellationToken: cancellationToken);
			return payment != null;
		}
	}
}