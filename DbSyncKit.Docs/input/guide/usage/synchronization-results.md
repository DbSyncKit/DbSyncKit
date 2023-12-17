﻿Title: Synchronization Results
Order: 3
BreadcrumbTitle: Synchronization Results
NavigationTitle: Synchronization Results
ShowInSidebar: true
Xref: usage/synchronization-results
---

# Understanding Synchronization Results

After performing a synchronization operation with DbSyncKit, you receive a [`Result<T>`](xref:api-DbSyncKit.Core.DataContract.Result) object that provides detailed information about the changes made during the process.

## Result<T> Properties

### 1. [Added](xref:api-DbSyncKit.Core.DataContract.Result.Added)

Represents the list of entities that were added during synchronization. Each entity corresponds to a data record added to the destination database.

### 2. [Deleted](xref:api-DbSyncKit.Core.DataContract.Result.Deleted)

Represents the list of entities that were deleted during synchronization. Each entity corresponds to a data record deleted from the destination database.

### 3. [Edited](xref:api-DbSyncKit.Core.DataContract.Result.Edited)

Represents the list of entities that were edited during synchronization. Each item in the list is a tuple containing the edited entity and a dictionary of updated properties. The dictionary provides information about the properties that were modified and their new values.

### 4. [SourceDataCount](xref:api-DbSyncKit.Core.DataContract.Result.SourceDataCount)

Represents the count of data records in the source database before synchronization.

### 5. [DestinaionDataCount](xref:api-DbSyncKit.Core.DataContract.Result.DestinaionDataCount)

Represents the count of data records in the destination database before synchronization.

### 6. [ResultChangeType](xref:api-DbSyncKit.Core.DataContract.Result.ResultChangeType)

Indicates the type of change represented by the synchronization result. Possible values are [ChangeType.Add](xref:api-DbSyncKit.Core.Enum.ChangeType.Add), [ChangeType.Edit](xref:api-DbSyncKit.Core.Enum.ChangeType.Edit), [ChangeType.Delete](xref:api-DbSyncKit.Core.Enum.ChangeType.Delete).

## Example Usage

Here's an example of how to use the synchronization result:

```csharp
// Perform synchronization
Result<YourEntity> syncResult = Sync.SyncData<YourEntity>(SourceDatabase, DestinationDatabase);

// Accessing synchronization result properties & getting its count
Console.WriteLine($"Added: {syncResult.Added.Count} Deleted: {syncResult.Deleted.Count}");
Console.WriteLine($"Total Source Data: {syncResult.SourceDataCount}");
Console.WriteLine($"Total Destination Data: {syncResult.DestinaionDataCount}");
```

Understanding the synchronization result is crucial for analyzing the impact of the synchronization operation and making informed decisions based on the changes detected.

Continue exploring other topics in the [Usage Guide](xref:usage).