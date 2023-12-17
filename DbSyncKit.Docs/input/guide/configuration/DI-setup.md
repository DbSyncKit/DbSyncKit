﻿Title: Dependency Injection Setup
Order: 2
BreadcrumbTitle: DI Setup
NavigationTitle: DI Setup
ShowInSidebar: true
Xref: configuration/di-setup
---

# Dependency Injection (DI) Setup

DbSyncKit can be configured using Dependency Injection (DI) to manage the application's services more efficiently. This guide outlines the steps to set up DbSyncKit with DI in your application.

## Note:

This portion is still in development for now check the [manual class implementation](xref:configuration/manual-setup)

<!---

## 1. Service Registration

The first step is to register the necessary services in your application's DI container. Add the following registrations to your DI setup:

```csharp
// Add DbSyncKit services
services.AddScoped<Synchronization>();
services.AddScoped<IQueryGenerator, YourQueryGeneratorImplementation>();
services.AddScoped<IDatabase, YourDatabaseConnectionImplementation>();
// Add other services as needed
```

Ensure you replace `YourQueryGeneratorImplementation` and `YourDatabaseConnectionImplementation` with your actual implementations.

## 2. Database Configuration

Configure your source and destination databases using DbSyncKit's `IDatabase` interface. Here's an example using MSSQL:

```csharp
// Configure source and destination databases
IDatabase sourceDatabase = new YourDatabaseConnectionImplementation("source_connection_string");
IDatabase destinationDatabase = new YourDatabaseConnectionImplementation("destination_connection_string");
```

Replace `"source_connection_string"` and `"destination_connection_string"` with your actual connection strings.

## 3. Synchronization Setup

Finally, set up the synchronization process by injecting the configured services into your application components.

```csharp
// Example of setting up synchronization
ISynchronization synchronization = new Synchronization(
    serviceProvider.GetService<IQueryGenerator>(),
    sourceDatabase,
    destinationDatabase
);
```

Now you have DbSyncKit configured with Dependency Injection in your application.

Proceed to the [Usage Guide](xref:usage) to learn how to perform synchronization tasks with DbSyncKit.

-->