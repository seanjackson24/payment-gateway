using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Converters;
using PaymentGateway.Domain.Factories;
using PaymentGateway.Domain.Services;
using PaymentGateway.Domain.TestBank;
using ServiceStack.Redis;

namespace PaymentGateway
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddControllers()
				.AddNewtonsoftJson(opts => opts.SerializerSettings.Converters.Add(new StringEnumConverter()));
			services.AddSingleton<IRedisClientsManager>(c => new RedisManagerPool(Configuration.GetConnectionString("Redis")));
			services.AddHttpClient<TestBankHttpClient>();
			services.AddDbContext<PaymentGatewayDbContext>(options =>
				options.UseSqlServer(Configuration.GetConnectionString("PaymentGatewayDatabase"),
				sqlServerOptionsAction: sqlOptions =>
				{
					sqlOptions.EnableRetryOnFailure(
					maxRetryCount: 10,
					maxRetryDelay: TimeSpan.FromSeconds(10),
					errorNumbersToAdd: null);
				})
			);

			services.AddSingleton<ITimeProvider, TimeProvider>();

			services.AddTransient<ICardMaskingService, CardMaskingService>();
			services.AddTransient<ILockActionService, RedisLockActionService>();
			services.AddTransient<IBankFactory, BankFactory>();
			services.AddTransient<IPaymentRepository, PaymentRepository>();
			services.AddTransient<IPaymentResponseFactory, PaymentResponseFactory>();
			services.AddTransient<IPaymentActionService, PaymentActionService>();
			services.AddTransient<IPaymentService, PaymentService>();
			services.AddTransient<IBankService, BankService>();
			services.AddTransient<IBank, TestBank>();
			services.AddTransient<TestBank>();

			services.AddTransient<IPaymentRetrievalService, PaymentRetrievalService>();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/error");
			}

			app.UseHttpsRedirection();

			app.UseRouting();

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}
