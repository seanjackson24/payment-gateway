# Payment Gateway

A Payment Gateway API to interface between merchants and banks

# Prerequisites:

-   dotnet core SDK 3.1 or later (https://dotnet.microsoft.com/download/dotnet-core/3.1)
-   nodeJS latest LTS or later (https://nodejs.org/en/download/)

-   Ensure the local development certificate is trusted by running:
    > dotnet dev-certs https --trust

# Run requirements:

-   Run the bank simulator by browsing to /PaymentGateway.BankSimulator and running:

    > dotnet build

    > dotnet run

    This will launch the project on https://localhost:5003

-   Start up a redis instance, or clone the official redis docker image and start it:

    > docker run --name paymentgateway.redis -p 6379:6379 -d redis

    > docker start paymentgateway.redis

-   Start up a SQL Server instance, or clone a SQL Server docker image and run:

    > docker run --name paymentgateway.database -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=yourStrong!Password' -p 1433:1433 -d mcr.microsoft.com/mssql/server:2017-latest

    > docker start paymentgateway.database

-   Prepare the database by running the script on the database:
    `scripts/Scaffold Database.sql`
    You can either connect to the database using something like SQL Server Management Studio, or use sqlcmd directly on the container:
    > docker exec -it paymentgateway.database /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P yourStrong!Password
-   Populate the config files:
    > "ConnectionStrings": { "PaymentGatewayDatabase": "\<your connection string>", "Redis": "\<your redis connection string>" }, "BankSimulator.Url": "\<Root URL of bank simulator>/BankPayment"
-   Remember to check if the bank simulator is running on http or https!

# Run API Backend:

-   Browse to /PaymentGateway in your favourite command prompt, and run:

    > dotnet build

    > dotnet run

-   The application will start listening on `https://localhost:5001/`

# Make a request to the application:

As an example using the command-line utility [httpie] (https://httpie.org/), you can simulate the below samples by running the commands:

-   Accepted payment:

    > http --verify=no --timeout=300 PUT https://localhost:5001/Payment PaymentId=10411bb3-d53d-440e-974c-ae65f4de559d CardNumber=4111111111111111 ExpiryDate=0222 CVV=123 PaymentAmountInCents:=23.0 CurrencyCode=GBP

    You should get back a response such as:

    > { "paymentId": "10411bb3-d53d-440e-974c-ae65f4de559d", "status": "Accepted" }

-   Declined Payment:

    > http --verify=no --timeout=300 PUT https://localhost:5001/Payment PaymentId=4cf50653-bdfd-4af1-8825-e4b2dc57640b CardNumber=5105105105105100 ExpiryDate=0222 CVV=123 PaymentAmountInCents:=23 CurrencyCode=GBP

    This should then return a response such as:

    > { "paymentId": "4cf50653-bdfd-4af1-8825-e4b2dc57640b", "status": "Declined" }

-   A payment which already exists will give you an http 409, with the problem details:
    > { "PaymentId": [ "A payment with this unique ID already exists" ] }
-   If you manage to send two payments with the same ID at the same time, one will get a Status of Processing, and the results of this query should be discarded.
-   Retrieve a payment:
    > http --verify=no --timeout=300 GET https://localhost:5001/PaymentRetrieval PaymentId=4cf50653-bdfd-4af1-8825-e4b2dc57640b

## Payment Request Fields:

-   PaymentId: This is a unique identifier for your payment. I recommend using a Guid for this.
-   CardNumber: The bank simulator will accept 4111111111111111, and decline all other valid credit card numbers.
-   ExpiryDate: A string of length 4.
-   CVV: A string of length 3.
-   PaymentAmountInCents: A positive integer, equal to the lowest possible unit of the Currency you are paying in.
-   CurrencyCode: The three-letter, ISO4217 representation of the currency in which you are paying. For example, GBP, NZD, JPY

# Alternatively, use the API Client

-   reference the project PaymentGateway.Common (which would be a NuGet package), and use a PaymentGatewayApiClient

# docker

From the root, in you can build the Bank Simulator by running:

> docker build --pull --rm -f "BankSimulator.Dockerfile" -t paymentgateway.banksimulator:latest "."

> docker run -d -p 5003:80 --name PaymentGateway.BankSimulator paymentgateway.banksimulator:latest

> docker start PaymentGateway.BankSimulator

Similarly, you can also build the main API:

> docker build --pull --rm -f "Dockerfile" -t paymentgateway:latest "."

> docker run -d -p 5001:80 --name PaymentGateway.API paymentgateway:latest

> docker start PaymentGateway.API

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

    ```csharp
    using (redisClient.AcquireLock(lockAction.UniqueIdentifier, lockAction.MaxAge))
    ```

    instead of AcquireLockEnhanced, then this test will fail.

-   Choice of data storage:
    I chose SQL Server in a docker container for the simplicity of it. This could relatively easily be switched with something like Azure Cosmos DB.

-   Crash or power failure recovery:
    If we wanted to add recovery from power failure, we could store the transaction in the DB before performing the payment with the bank, and use something like SQL server encryption.
    We would then need to add a background process to check these rows and post to a failsafe URL (for example). This then puts a lot of extra processing effort on to the client.

-   Authentication:
    If someone was somehow able to gain access to a PaymentID, they could then use this information to get details on a payment. If this was determined to be an issue, then we would need to add some authentication to each request, such that the original credentials used to make a payment would need to match those used to retrieve one.
