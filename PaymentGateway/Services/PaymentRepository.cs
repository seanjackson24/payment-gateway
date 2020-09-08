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
		public async Task<Payment> GetPaymentById(string paymentId)
		{
			using (var context = new PaymentGatewayDbContext())
			{
				return await context.Payments.FindAsync(paymentId);
			}
		}

		public async Task Insert(Payment payment, CancellationToken cancellationToken)
		{
			using (var context = new PaymentGatewayDbContext())
			{
				await context.Payments.AddAsync(payment, cancellationToken);
				await context.SaveChangesAsync(cancellationToken);
			}
		}

		public async Task<bool> PaymentExists(string paymentId, CancellationToken cancellationToken)
		{
			using (var context = new PaymentGatewayDbContext())
			{
				var payment = await context.Payments.FindAsync(paymentId, cancellationToken);
				return payment != null;
			}
		}
	}
}