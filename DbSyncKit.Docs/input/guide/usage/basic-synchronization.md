﻿Title: Basic Synchronization
Order: 1
BreadcrumbTitle: Basic Synchronization
NavigationTitle: Basic Synchronization
ShowInSidebar: true
Xref: usage/basic-synchronization
---

# Performing Basic Synchronization with DbSyncKit

In this guide, we'll walk through the process of performing a basic data synchronization operation using DbSyncKit.

## Prerequisites

Before we start, ensure that you have the following:

- [DbSyncKit.Core](xref:packages/DbSyncKit.Core) package installed
- [DbSyncKit.DB](xref:packages/DbSyncKit.DB) package installed
- Appropriate database provider package installed (e.g., [DbSyncKit.MSSQL](xref:packages/DbSyncKit.MSSQL), [DbSyncKit.MySQL](xref:packages/DbSyncKit.MySQL))

## Setup

To begin, let's set up the necessary variables. For detailed setup instructions, refer to the [Setup Guide](xref:configuration).

Here's a brief summary of the variables:

- `IDatabase SourceDatabase`: Represents the source database connection.
- `IDatabase DestinationDatabase`: Represents the destination database connection.
- `Synchronization Sync`: Represents the DbSyncKit synchronization instance.

For detailed instructions on setting up these variables, refer to the [Setup Guide](xref:configuration).

## Perform Synchronization

Now that we have the variables set up, let's perform the synchronization. Replace `YourEntity` with the appropriate entity you want to synchronize.

```csharp
// Perform synchronization
Result<YourEntity> syncResult = Sync.SyncData<YourEntity>(SourceDatabase, DestinationDatabase);
```

The synchronization result provides detailed information about added, edited, and deleted records, along with the change type and data counts.

For more details on interpreting the synchronization result, refer to the [Synchronization Results Guide](xref:usage/synchronization-results).

Continue exploring other topics in the [Usage Guide](xref:usage).
