﻿Title: Cross-Database Migration
Order: 4
BreadcrumbTitle: Cross-Database Migration
NavigationTitle: Cross-Database Migration
ShowInSidebar: true
Xref: usage/cross-database-migration
---

# Cross-Database Migration with DbSyncKit

DbSyncKit allows you to perform cross-database migration, syncing data between different database providers. In this guide, we'll walk through a basic example of migrating data from MySQL to MSSQL.

## Setup

Let's start by setting up the necessary variables

```csharp
// Cross-Database Migration Setup
IDatabase Source = new DbSyncKit.MySQL.Connection("localhost", 3306, "SourceChinook", "root", "");
IDatabase Destination = new DbSyncKit.MSSQL.Connection("(localdb)\\MSSQLLocalDB", "DestinationChinook", true);
Synchronization Sync = new Synchronization();
```

## Perform Cross-Database Migration

Now that we have the variables set up, let's perform the cross-database migration. Replace `YourEntity` with the appropriate entity you want to synchronize.

```csharp
// Perform Cross-Database Migration
Result<YourEntity> migrationResult = Sync.SyncData<YourEntity>(Source, Destination);
```

The migration result provides detailed information about added, edited, and deleted records, along with the change type and data counts.

## Generate SQL Query

To generate the SQL query corresponding to the migration, use the following code:

```csharp
// Generate SQL Query for Cross-Database Migration
var query = Sync.GetSqlQueryForSyncData(migrationResult);
```


### Importatant:

Please note that **SQL queries generated** will be based on the **destination [IDatabase](xref:api-DbSyncKit.DB.Interface.IDatabase)** or [**IQueryGenerator**](xref:api-DbSyncKit.DB.Interface.IQueryGenerator) provided to the [Synchronization](xref:api-DbSyncKit.Core.Synchronization) instance

Proceed to the [Usage Guide](xref:usage) for more comprehensive guidance on using DbSyncKit with different scenarios.