# Payment Gateway

A Payment Gateway API to interface between merchants and banks

# Prerequisites:

-   dotnet core SDK 3.1 or later (https://dotnet.microsoft.com/download/dotnet-core/3.1)
-   nodeJS latest LTS or later (https://nodejs.org/en/download/)

-   Ensure the local development certificate is trusted by running:
    `dotnet dev-certs https --trust`

# Run requirements:

-   Run the bank simulator by browsing to /PaymentGateway.BankSimulator and running:
    `dotnet build`
    `dotnet run`
-   Start up a redis instance, or clone the official redis docker image and start it:
    ``
-   Start up a SQL Server instance, or clone a SQL Server docker image and run:
    ``
-   Prepare the database by running the script on the database:
    ``
-   Populate the config files

# Run API Backend:

-   Browse to /PaymentGateway in your favourite command prompt, and run:
    `dotnet build`
    `dotnet run`
-   The application will start listening on `https://localhost:5001/`

# Make a request to the application:

-   Accepted payment:
    `http`
-   Declined Payment:
    `http `
-   Retrieve a payment:
    `http `

Payment ID: This is a unique identifier for your payment. I recommend using a Guid for this.

# Alternatively, use the API Client

-   reference the project PaymentGateway.Common (which would be a NuGet package), and use a PaymentGatewayApiClient

# How it works:

We take out a Distributed Lock on Redis, using ServiceStack.Redis (see note about this later), using the Payment ID as the key.
This should ensure that only one Thread, Process, or Machine can process a payment at once.
We then check the database for whether this payment has already completed (to prevent the same payment ID being used twice even after the lock has expired)
Next, we perform the payment with the bank, and log the payment result (with masked card number) to the database.

# Considerations:

-   ServiceStack.Redis and RedisLockEnhanced:
    During testing, it became apparent that ServiceStack.Redis's built in AcquireLock function doesn't distinguish between the timeout periods for
    lock acquisition and lock expiration - meaning that if you have a task which takes longer than the timeout parameter, the lock would be considered
    timed out, and another thread could grab the lock. Rather than guessing a timeout to use (particularly when the task requires communication with
    a database twice, and a third party - the bank - once), I decided to use the source of ServiceStack.Redis and make this distinction. There is a test
    covering this case:
    see RedisLockActionServiceTests.TryExecureLockAction_TwoConcurrentLocks_WithLongExecutionTime_ShortAcquireTime_ReturnsUnsuccessfulForOne
    if you change the RedisLockActionService using statement to:
    `using (redisClient.AcquireLock(lockAction.UniqueIdentifier, lockAction.MaxAge))`
    instead of AcquireLockEnhanced, then this test will fail.

-   Choice of data storage:
    I chose SQL Server in a docker container for the simplicity of it. This could relatively easily be switched with something like Azure Cosmos DB.

-   Crash or power failure recovery:
    If we wanted to add recovery from power failure, we could store the transaction in the DB before performing the payment with the bank, and use something like SQL server encryption.
    We would then need to add a background process to check these rows and post to a failsafe URL (for example). This then puts a lot of extra processing effort on to the client.

-   Authentication:
    If someone was somehow able to gain access to a PaymentID, they could then use this information to get details on a payment. If this was determined to be an issue, then we would need to add
    some authentication to each request, such that the original credentials used to make a payment would need to match those used to retrieve one.
