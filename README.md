# DbSyncKit: Database Synchronization Library

The Sync library is a comprehensive suite of packages designed to simplify and streamline the process of data synchronization across multiple database platforms. It offers a range of tools, utilities, and interfaces to facilitate efficient and consistent data synchronization operations.

## Introduction

Welcome to the Sync Library documentation! This comprehensive guide offers insights into the functionalities and usage of the Sync Library, a collection of packages designed to streamline and simplify the process of data synchronization across diverse database systems.

### Overview of the Sync Library

The Sync Library comprises four primary packages: `DbSyncKit.Core`, `DbSyncKit.DB`, `DbSyncKit.MSSQL`, and `DbSyncKit.MySQL`, each serving distinct roles in enabling efficient data synchronization.

### Package Descriptions

- **DbSyncKit.Core:** This package forms the core functionality of the Sync Library, providing a robust set of tools and utilities for data synchronization tasks. It aims to optimize synchronization processes for efficiency and speed.

- **DbSyncKit.DB:** As a foundational package, DbSyncKit.DB defines a set of interfaces and contracts to establish a consistent interaction layer across various database systems. It ensures a unified approach to database operations for seamless integration.

- **DbSyncKit.MSSQL:** This specialized package offers tailored functionalities specifically for Microsoft SQL Server databases. It includes tools for query generation, connection management, and error handling optimized for MSSQL environments.

- **DbSyncKit.MySQL:** This specialized package offers tailored functionalities specifically for Microsoft SQL Server databases. It includes tools for query generation, connection management, and error handling optimized for My SQL environments.
  
- **Future Packages: DbSyncKit.PostgreSQL:** These upcoming packages are dedicated to providing similar synchronization capabilities for PostgreSQL databases. Although currently under development, they aim to align with the principles and features of the existing Sync Library packages.

Continue exploring this documentation to learn about installation procedures, usage examples, advanced configurations, FAQs, and more to leverage the Sync Library effectively in your projects.

### Configuration and Setup

#### .NET Core Dependency Injection (DI)

To incorporate the synchronization functionalities provided by DbSyncKit.Core into your application, you can establish Dependency Injection (DI) within your project's Startup class.

1. **Dependency Injection Setup:**

    Open your `Startup.cs` file and configure the DI container to register the `Synchronization` and `QueryGenerator` services:

    ```csharp
    services.AddScoped<QueryGenerator>();
    services.AddScoped<Synchronization>();
    ```

    Utilizing `services.AddScoped` registers these services, allowing them to be injected into your application components as needed.

2. **Usage Example:**

    After registering in the DI container, you can inject these services into your classes or controllers:

    ```csharp
    using DbSyncKit.Core;

    private readonly Synchronization _sync;

    public YourServiceOrController(Synchronization sync)
    {
        _sync = sync;
    }
    
    ```

    Incorporating these services via DI enables seamless integration of DbSyncKit.Core's synchronization features within your application architecture.

### Usage Guide

#### Basic Synchronization

To perform basic data synchronization using the DbSyncKit.Core package, follow these steps:

1. **Instantiate Synchronization:**

    ```csharp
    using DbSyncKit.Core;

    // Instantiate the Synchronization class
    Synchronization _sync = new Synchronization(new QueryGenerator());
    ```

    or with DI

    ```csharp
    using DbSyncKit.Core;

    private readonly Synchronization _sync;

    public YourServiceOrController(Synchronization sync)
    {
        _sync = sync;
    }
    
    ```


    This creates an instance of the `Synchronization` class with a `QueryGenerator` necessary for generating SQL queries.

2. **Create Database Instances:**

    ```csharp
    using DbSyncKit.Core;

    // Instantiate source database connection
    IDatabase sourceDatabase = new Connection("(localdb)\\MSSQLLocalDB", "SourceChinook", true);

    // Instantiate destination database connection
    IDatabase destinationDatabase = new Connection("(localdb)\\MSSQLLocalDB", "DestinationChinook", true);
    ```

    Replace `"(localdb)\\MSSQLLocalDB"` with your specific server address, and `"SourceChinook"` and `"DestinationChinook"` with the names of your source and destination databases, respectively. The last parameter `true` indicates the usage of integrated security for authentication. Adjust the parameters based on your authentication requirements.

3. **Entity Configuration Example (Album Entity):**

    To configure entities like the `Album` entity, apply attributes from the `DbSyncKit.DB.Attributes` namespace to the entity class.

    ```csharp
    using DbSyncKit.DB.Attributes;
    using DbSyncKit.DB.Extensions;
    using DbSyncKit.DB.Utils;
    using System.Data;

    [TableName("Album"), ]
    public class Album : DataContractUtility<Album>
    {
        [KeyProperty(isPrimaryKey: true)]
        public int AlbumId { get; set; }
        public string Title { get; set; }
        public int ArtistId { get; set; }

        public Album(DataRow albumInfo)
        {
            // Initialization code for Album properties from DataRow
        }
    }
    ```

    - `[TableName]`: Specifies the name of the table corresponding to this entity in the database.
    - `[TableSchema]`: Specifies the schema of the table.
    - `[KeyProperty]`: Defines a property as the primary key for the entity.

4. **Available Attributes for Configuration:**

    - `[TableName]`: Specifies the name of the table.
    - `[TableSchema]`: Specifies the schema of the table.
    - `[KeyProperty]`: Defines a property as a key property.
    - `[ExcludedProperty]`: Excludes a property from specific operations.
    - `[GenerateInsertWithID]`: Controls the inclusion of the ID property in the insert query generation, allowing fine-tuning of insertion behavior.
      - `GenerateWithID`: Determines whether the ID property should be included in the insert query generation. Possible values are `true` (to include the ID property) or `false` (to exclude the ID property).
      - `IncludeIdentityInsert`: Indicates whether to include database-specific SQL statements during insert query generation affecting identity insert behavior. Default value is `true`.

5. **Perform Synchronization for Specific Entity:**

    ```csharp
    // Example: Synchronize data for a specific entity
    var entityData = _sync.SyncData<YourEntity>(sourceDatabase, destinationDatabase);
    ```

    Replace `<YourEntity>` with the specific entity type you want to synchronize. `sourceDatabase` and `destinationDatabase` should be instances of the `IDatabase` interface representing your source and destination databases.

6. **Access Synchronization Results:**

    ```csharp
    // Example: Access synchronization results for added, edited, and deleted records
    Console.WriteLine($"Added: {entityData.Added.Count} Edited: {entityData.Edited.Count} Deleted: {entityData.Deleted.Count}");
    Console.WriteLine($"Total Source Data: {entityData.SourceDataCount}");
    Console.WriteLine($"Total Destination Data: {entityData.DestinaionDataCount}");

    // Retrieve SQL query generated during synchronization
    var query = sync.GetSqlQueryForSyncData(entityData);
    Console.WriteLine(query);
    ```

    This snippet showcases how to access statistics related to added, edited, and deleted records, as well as the total counts of source and destination data. Additionally, it retrieves the SQL query generated during synchronization.

7. **Repeat for Other Entities:**

    Repeat the synchronization process (steps 3-4) for other entity types as needed by replacing `<YourEntity>` with the desired entity type.

8. **Handle Errors:**

    Ensure to handle any exceptions that might occur during the synchronization process using appropriate error handling mechanisms.

Great! Here's the revised information:

### Contribution Guidelines

Contributions to the DbSyncKit.Core, DbSyncKit.DB, and DbSyncKit.MSSQL packages are welcome! To contribute, follow these steps:

1. **Fork the Repository:**
   Fork the repository to your GitHub account.

2. **Clone the Repository:**
   Clone the forked repository to your local machine:
   ```
   git clone https://github.com/RohitM-IN/DbSyncKit.git
   ```

3. **Create a Branch:**
   Create a new branch for your changes:
   ```
   git checkout -b feature/your-feature-name
   ```

4. **Make Changes:**
   Make necessary changes in the codebase.

5. **Commit Changes:**
   Commit your changes and provide descriptive commit messages:
   ```
   git commit -m "Add your descriptive message here"
   ```

6. **Push Changes:**
   Push your changes to your forked repository:
   ```
   git push origin feature/your-feature-name
   ```

7. **Create Pull Request:**
   Create a Pull Request (PR) from your forked repository to the original repository.

### Support and Contact Information

To contribute to DbSyncKit.Core, DbSyncKit.DB, DbSyncKit.MSSQL, or DbSyncKit.MySQL packages:

- **Email:** support@rohit-mahajan.in
- **GitHub Issues:** [Repository Issues](https://github.com/RohitM-IN/DBSync/issues)

### License Information

DbSyncKit packages are licensed under the MIT License. For detailed information, refer to the [LICENSE](https://github.com/RohitM-IN/DBSync/blob/main/LICENSE) file in the repository.
