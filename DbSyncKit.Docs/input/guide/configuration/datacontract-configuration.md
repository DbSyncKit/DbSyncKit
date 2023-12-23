Title: DataContract Configuration
Order: 4
BreadcrumbTitle: DataContract Configuration
NavigationTitle: DataContract Configuration
ShowInSidebar: true
Xref: configuration/datacontract-configuration
---

# DataContract Configuration

To configure the actual data contract for entities, you can use attributes from the [DbSyncKit.DB.Attributes](xref:api-DbSyncKit.DB.Attributes) namespace. These attributes allow you to customize the behavior of synchronization for each table.

## Entity Configuration Example (Album Entity):

To configure entities like the `Album` entity, apply attributes from the [DbSyncKit.DB.Attributes](xref:api-DbSyncKit.DB.Attributes) namespace to the entity class.

```csharp
using DbSyncKit.DB.Attributes;
using DbSyncKit.DB.Extensions;
using DbSyncKit.DB.Utils;
using System.Data;

[TableName("Album")]
public class Album : DataContractUtility<Album>
{
    [KeyProperty]
    public int AlbumId { get; set; }
    public string Title { get; set; }
    public int ArtistId { get; set; }

    public Album(DataRow albumInfo)
    {
        // Initialization code for Album properties from DataRow
    }
}
```

### Available Attributes for Configuration:

- [TableName](xref:api-DbSyncKit.DB.Attributes.TableNameAttribute): Specifies the name of the table.
- [TableSchema](xref:api-DbSyncKit.DB.Attributes.TableSchemaAttribute): Specifies the schema of the table.
- [KeyProperty](xref:api-DbSyncKit.DB.Attributes.KeyPropertyAttribute): Defines a property as a key property.
- [ExcludedProperty](xref:api-DbSyncKit.DB.Attributes.ExcludedPropertyAttribute): Excludes a property from specific operations.
- [GenerateInsertWithID](xref:api-DbSyncKit.DB.Attributes.GenerateInsertWithIDAttribute): Controls the inclusion of the ID property in the insert query generation, allowing fine-tuning of insertion behavior.
  - [GenerateWithID](xref:api-DbSyncKit.DB.Attributes.GenerateInsertWithIDAttribute.GenerateWithID): Determines whether the ID property should be included in the insert query generation. Possible values are `true` (to include the ID property) or `false` (to exclude the ID property).
  - [IncludeIdentityInsert](xref:api-DbSyncKit.DB.Attributes.GenerateInsertWithIDAttribute.IncludeIdentityInsert): Indicates whether to include database-specific SQL statements during insert query generation affecting identity insert behavior. The default value is `true`.
