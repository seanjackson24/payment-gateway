using System.Threading;
using System.Threading.Tasks;

namespace PaymentGateway.Services
{
	public interface IPaymentRepository
	{
		Task<bool> PaymentExists(string paymentId, CancellationToken cancellationToken);
		Task Insert(Payment payment, CancellationToken cancellationToken);
		Task<Payment> GetPaymentById(string paymentId);
	}
	public class PaymentRepository : IPaymentRepository
	{
		private readonly PaymentGatewayDbContext _context;

		public PaymentRepository(PaymentGatewayDbContext context)
		{
			this._context = context;
		}

		public async Task<Payment> GetPaymentById(string paymentId)
		{
			return await _context.Payments.FindAsync(paymentId);
		}

		public async Task Insert(Payment payment, CancellationToken cancellationToken)
		{
			await _context.Payments.AddAsync(payment, cancellationToken);
			await _context.SaveChangesAsync(cancellationToken);
		}

		public async Task<bool> PaymentExists(string paymentId, CancellationToken cancellationToken)
		{
			var payment = await _context.Payments.FindAsync(keyValues: new[] { paymentId }, cancellationToken: cancellationToken);
			return payment != null;
		}
	}
}