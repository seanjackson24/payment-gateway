IF NOT EXISTS (select * from INFORMATION_SCHEMA.DATABASES where Name = 'PaymentGateway')
BEGIN
create database PaymentGateway
END
GO

