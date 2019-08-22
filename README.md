# crypto-wallet-monitor

## Scope

**Implement a feature to alert (email or SMS) on suspicious/anomalous activity such as trades made 20% larger than average order**

Done. I have made this a configurable as well and included a threshold of when the average should start.

**Identify all orders that are greater than predefined amount.**

Done. I have made the predefined amount configurable.

**Identity all outgoing transfers where purpose of transaction is not mentioned or purpose mentioned is other than i.e. Fund Transfer, Home Maintenance, Bill Payments, Salary, Bonus, Commission, Purchase, Inter Bank Transfer.**

Done. I have made the purposes configurable.

**Store orders that were alerted for later reporting/auditing**

Done. All transactions are stored. When a transaction is suspicious, there is a flag that will be set to true.

## Included

- Testing (Unit & Integration)
- User registration/authentication
- Coding best practices
- Logging
- Exception handling
- Consider quality attributes where possible (Maintainable, Scalable, Available, Extensible, Reusable)
- Docker
- Asp.net Core
- Relational DB Server
- Entity Framework Core
- Web API

## Code

The code structure is as follows

- wallet-ui
- Wallet
  - Wallet.Api
  - Wallet.Database

## Compilation instructions

### wallet-ui

Within a container (hosted at http://localhost:4201):

```
npm run docker
```

Standard (hosted at http://localhost:4200):

```
npm start
```

### Wallet.Api

Standard

_Remember to set the port in the config file to 5000 as well as the port to your SQL instance._

### Wallet.Database

Simply publish the project to your SQL Server.

_NOTE! In order for the API container to use the database, you need to enable TCP and browsing capabilities on your SQL Server as well as add the required user and password._
