---
Title: Configuration
Order: 2
BreadcrumbTitle: Configuration
NavigationTitle: Configuration
ShowInSidebar: true
Xref: configuration
---

# Configuring DbSyncKit

DbSyncKit offers flexibility in configuration, allowing you to set up the application using Dependency Injection (DI) or create instances of classes manually.

## [Dependency Injection (DI)](xref:configuration/di-setup)

Learn how to configure DbSyncKit using Dependency Injection (DI). Services are registered in your application's DI container, providing an organized and extensible setup.

## [Manual Instance Creation](xref:configuration/manual-setup)

Prefer not to use DI? This guide walks you through the process of manually creating instances of required DbSyncKit classes. A straightforward setup for quick integration.

## [DataContract Configuration](xref:configuration/datacontract-configuration)

Configure the actual data contract for entities using attributes from the [DbSyncKit.DB.Attributes](xref:api-DbSyncKit.DB.Attributes) namespace. Customize synchronization behavior for each table.

Proceed to the [Usage Guide](xref:usage) to learn how to perform synchronization tasks with DbSyncKit.
