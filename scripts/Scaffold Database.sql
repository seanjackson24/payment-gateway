IF NOT EXISTS (select * from sysdatabases where Name = 'PaymentGateway')
BEGIN
create database PaymentGateway
END
GO

use PaymentGateway
IF NOT EXISTS (select * from INFORMATION_SCHEMA.TABLES where Name = "Payment")
BEGIN
	create table Payment (
		PaymentId nvarchar(100) not null primary key,
		MaskedCardNumber nvarchar(20) not null,
		PaymentAmountInCents int not null,
		PaymentStatus int not null,
		BankReference nvarchar(100) not null,
		CardExpiryDate nvarchar(5) not null,
		TimestampUtc datetime2 not null,

	)
END
GO