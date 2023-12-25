﻿Title: DbSyncKit.DB
Order: 2
BreadcrumbTitle: DbSyncKit.DB
NavigationTitle: DbSyncKit.DB
ShowInSidebar: true
Xref: packages/DbSyncKit.DB
---

# DbSyncKit.DB

DbSyncKit.DB is a package that extends DbSyncKit.Core, providing database-specific implementations and features for various database providers. It includes classes and utilities tailored to support synchronization operations on specific databases such as MSSQL, MySQL, PostgreSQL, and more.

## Supported Database Providers

- **Microsoft SQL Server (MSSQL):** DbSyncKit.MSSQL package
- **MySQL:** DbSyncKit.MySQL package
- **PostgreSQL:** DbSyncKit.PostgreSQL package
- **(Additional Providers):** Future extensions may include support for other database providers.

## Installation

Choose the appropriate `DbSyncKit.DB` package based on your target database provider. For example, to install the MSSQL-specific package, use the following NuGet Package Manager command:

```bash
dotnet add package DbSyncKit.MSSQL
```

Replace `MSSQL` with `MySQL`, `PostgreSQL`, or the relevant provider when installing a different package.

## Documentation

Explore the detailed API documentation for `DbSyncKit.DB` to understand the classes, methods, and features specific to each supported database provider:

- [DbSyncKit.DB API Documentation](xref:api-DbSyncKit.DB)

Refer to the [Setup Guide](xref:configuration) for configuring `DbSyncKit.DB` with your database connections.

Continue exploring other topics in the [Usage Guide](xref:usage).
