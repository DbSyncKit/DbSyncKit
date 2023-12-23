﻿Title: Generating SQL Queries
Order: 4
BreadcrumbTitle: Generating SQL Queries
NavigationTitle: Generating SQL Queries
ShowInSidebar: true
Xref: usage/generating-sql-queries
---

# Generating SQL Queries with DbSyncKit

In addition to synchronizing data, DbSyncKit allows you to generate SQL queries based on the changes detected during synchronization. This feature is valuable for understanding the exact SQL statements that will be executed in the destination database.

## Prerequisites

Before generating SQL queries, ensure that you have:

- Performed a synchronization operation using DbSyncKit (Refer to [Basic Synchronization](xref:usage/basic-synchronization)).
- Received a synchronization result ([`Result<T>`](xref:api-DbSyncKit.Core.DataContract.Result)) object.

## Generate SQL Queries

Once you have the synchronization result, you can generate SQL queries using the [`GetSqlQueryForSyncData`](xref:api-DbSyncKit.Core.Synchronization.GetSqlQueryForSyncData<T>) method. Replace `YourEntity` with the appropriate entity type.

```csharp
// Perform synchronization
Result<YourEntity> syncResult = Sync.SyncData<YourEntity>(SourceDatabase, DestinationDatabase);

// Generate SQL queries
string sqlQueries = Sync.GetSqlQueryForSyncData<YourEntity>(syncResult);

Console.WriteLine(sqlQueries);

```

The [`GetSqlQueryForSyncData`](xref:api-DbSyncKit.Core.Synchronization.GetSqlQueryForSyncData<T>) method takes the synchronization result as input and returns a list of SQL queries corresponding to the changes detected during synchronization.

## Understanding Generated SQL Queries

The generated SQL queries fall into three categories:

1. **Insert Queries**: SQL statements for adding new records to the destination database.

2. **Update Queries**: SQL statements for modifying existing records in the destination database.

3. **Delete Queries**: SQL statements for removing records from the destination database.

By examining these queries, you gain insights into the exact modifications that will be applied to the destination database.

Continue exploring other topics in the [Usage Guide](xref:usage).

## Query Generation Process Updates

### New Package: DbSyncKit.Templates

We're excited to introduce a new package called [`DbSyncKit.Templates`](xref:api-DbSyncKit.Templates). This package is dedicated to providing templates for all supported database providers.

### Templating Engine: Fluid

To facilitate the templating process, we've integrated the Fluid templating engine (https://github.com/sebastienros/fluid) into the [`DbSyncKit.Templates`](xref:api-DbSyncKit.Templates) package. Fluid provides a flexible and expressive way to define templates, enhancing the readability and maintainability of the code.

### Improved Readability and Flexibility

With the introduction of templating, the query generation process has seen improvements in terms of code readability and flexibility. The templating approach allows for clearer and more maintainable templates, ensuring a smooth and user-friendly experience.

Explore these updates as you continue to use DbSyncKit. If you have any questions or feedback, refer to the [Support and Contact](xref:support) section.
