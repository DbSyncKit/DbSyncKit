﻿Title: Manual Setup
Order: 3
BreadcrumbTitle: Manual Setup
NavigationTitle: Manual Setup
ShowInSidebar: true
Xref: configuration/manual-setup
---

# Manual Setup

DbSyncKit can be configured manually without using Dependency Injection (DI). This guide outlines the steps to set up DbSyncKit manually in your application.

## 1. Query Generator Setup (Optional)

### For MSSQL
API Reference: [QueryGenerator](xref:api-DbSyncKit.MSSQL.QueryGenerator)

```csharp
IQueryGenerator queryGenerator = new QueryGenerator();
```

### For MySQL
API Reference: [QueryGenerator](xref:api-DbSyncKit.MySQL.QueryGenerator)

```csharp
IQueryGenerator queryGenerator = new QueryGenerator();
```

## 2. Synchronization Setup

To set up the synchronization process manually, you'll need to create instances of DbSyncKit components.

API Reference: [Synchronization](xref:api-DbSyncKit.Core.Synchronization)

```csharp
// manual synchronization setup
Synchronization Sync = new Synchronization();
```

Or With QueryGenerator

```csharp
Synchronization Sync = new Synchronization(destinationQueryGenerator);
```

Where `destinationQueryGenerator` is an instance of [`IQueryGenerator`](xref:api-DbSyncKit.DB.Interface.IQueryGenerator)

## 3. Database Configuration

Configure your source and destination databases using DbSyncKit's [IDatabase](xref:api-DbSyncKit.DB.Interface.IDatabase) interface.

### For MSSQL
 Api Ref: [Connection](xref:api-DbSyncKit.MSSQL.Connection)
```csharp
// MSSQL manual database configuration
IDatabase SourceDatabase = new Connection("(localdb)\\MSSQLLocalDB", "SourceChinook", true);
IDatabase DestinationDatabase = new Connection("(localdb)\\MSSQLLocalDB", "DestinationChinook", true);
```

### For MySQL
Api Ref: [Connection](xref:api-DbSyncKit.MySQL.Connection)
```csharp
// MySQL manual database configuration
IDatabase SourceDatabase = new Connection("localhost", 3306, "SourceChinook", "root", "");
IDatabase DestinationDatabase = new Connection("localhost", 3306, "DestinationChinook", "root", "");
```

Replace connection strings and other details according to your actual configurations.

## 4. Start Synchronization

Once everything is set up, you can start the synchronization process.

Proceed to the [Usage Guide](xref:usage) to learn how to perform synchronization tasks with DbSyncKit.

## Note
Manual setup provides more control but may require additional configuration compared to Dependency Injection (DI).
