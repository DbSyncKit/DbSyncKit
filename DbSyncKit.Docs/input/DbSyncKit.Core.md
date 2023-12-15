﻿Title: DbSyncKit.Core
Order: 1
BreadcrumbTitle: Core
NavigationTitle: Core
ShowInSidebar: false
Xref: core
---

# Introduction

The [`DbSyncKit.Core`](xref:api-DbSyncKit.Core) package is a fundamental component of the Database Synchronization Library (`DbSyncKit`). It provides a versatile set of tools, utilities, and interfaces designed to streamline and simplify the process of data synchronization across diverse database systems. This library is built to ensure efficiency, consistency, and reliability in managing database synchronization tasks.

# Overview

The [`DbSyncKit.Core`](xref:api-DbSyncKit.Core) package encapsulates core functionalities essential for seamless data synchronization operations. It serves as the backbone of the entire `DbSyncKit` ecosystem, offering an extensible framework to cater to various database synchronization requirements.

## Key Features

- **Data Synchronization:** Provides robust mechanisms to synchronize data between different databases.
- **Query Generation:** Offers tools for generating database-specific SQL queries necessary for synchronization operations.
- **DI (Dependency Injection) Compatibility:** Designed to seamlessly integrate with DI containers, enabling effortless incorporation into diverse application architectures.
- **Error Handling:** Implements error-handling strategies to manage and log exceptions encountered during synchronization tasks.
- **Configurability:** Facilitates configuration for entities, tables, schemas, and key properties to adapt to various database structures.

# Getting Started

To incorporate [`DbSyncKit.Core`](xref:api-DbSyncKit.Core) into your project, follow these steps:

## Installation

Install the package via NuGet Package Manager or Package Manager Console:

```bash
PM> Install-Package DbSyncKit.Core
```

## Usage

1. **Dependency Injection Setup:**

   In your application's Startup class, register the required services:

   ```csharp
   services.AddScoped<QueryGenerator>();
   services.AddScoped<Synchronization>();
   ```

2. **Entity Configuration:**

   Configure entities with necessary attributes from the `DbSyncKit.DB.Attributes` namespace for synchronization:

   ```csharp
   using DbSyncKit.DB.Attributes;

   [TableName("YourEntityTable")]
   public class YourEntity
   {
       // Define properties and attributes
   }
   ```

3. **Performing Synchronization:**

   Utilize the `Synchronization` class for data synchronization operations:

   ```csharp
   using DbSyncKit.Core;

   // Instantiate Synchronization class
   Synchronization _sync = new Synchronization(new QueryGenerator());

   // Perform synchronization for a specific entity
   var entityData = _sync.SyncData<YourEntity>(sourceDatabase, destinationDatabase);
   ```

4. **Accessing Synchronization Results:**

   Retrieve synchronization results and associated SQL queries:

   ```csharp
   Console.WriteLine($"Added: {entityData.Added.Count} Edited: {entityData.Edited.Count} Deleted: {entityData.Deleted.Count}");
   var query = _sync.GetSqlQueryForSyncData(entityData);
   Console.WriteLine(query);
   ```

For more advanced usage, refer to the documentation and code examples available in the [Wiki](https://github.com/YourRepo/DbSyncKit.Core/wiki) or directly within the library's source code.

# Contribution Guidelines

Contributions to [`DbSyncKit.Core`](xref:api-DbSyncKit.Core) are welcome! To contribute, follow these steps:

1. Fork the repository.
2. Create a new branch for your changes.
3. Make necessary modifications in the codebase.
4. Commit your changes and create a Pull Request (PR) to the original repository.

# Support and Contact Information

For support, queries, or feedback regarding [`DbSyncKit.Core`](xref:api-DbSyncKit.Core), feel free to contact us:

- **GitHub Issues:** [Repository Issues](https://github.com/YourRepo/DbSyncKit.Core/issues)

# License Information

[`DbSyncKit.Core`](xref:api-DbSyncKit.Core) is licensed under the MIT License. For detailed information, refer to the [LICENSE](https://github.com/YourRepo/DbSyncKit.Core/blob/main/LICENSE) file in the repository.
