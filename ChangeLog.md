# Changelog

All notable changes to this project will be documented in this file. See [standard-version](https://github.com/conventional-changelog/standard-version) for commit guidelines.

## 0.1.2 - 2023-12-13

### Feature
- Added Batching for running scripts now will include GO for MSSQL or similar statements for MYSQL or Postgres


## 0.1.1 - 2023-12-12
  
### Fixed
- **Identity Insert Issue Resolved**:
  - Fixed an issue where the Identity Insert feature wasn't functioning correctly during synchronization, causing database inconsistencies.


## 0.1.0 - 2023-12-12 (Reviesed)

### Changed
- **Package Name Change**:
  - Updated the package name to `DbSyncKit`
  - Users should now reference the package as `DbSyncKit` instead of `Sync`.


## 0.1.0 - 2023-12-11

### Added

- **Initial Release**
  - `DbSyncKit.Core`: Core functionality for data synchronization.
  - `DbSyncKit.DB`: Foundational package defining interfaces for database interactions.
  - `DbSyncKit.MSSQL`: Specialized package for Microsoft SQL Server databases.
