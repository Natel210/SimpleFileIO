# SimpleFileIO

## Summary

SimpleFileIO is a library designed to provide a unified interface for record-based IO management. It supports the following file types and features:

- CSV: Structured data storage in CSV format.
- Appendable CSV (Log): Incremental logging directly into a CSV file.
- Text: General text file management.
- Appendable Text (Log): Incremental logging into plain text files.

This library eliminates unnecessary dependencies while maintaining flexibility and ease of use. CsvHelper is used for advanced CSV management.

## Key Features

- Unified interface for multiple file types (CSV, Text, Logs).
- Modular approach.

## How to

## Notes

## Third-Party Libraries

### CsvHelper
- Version: 33.0.1
- License: Apache-2.0 OR MS-PL
- Copyright: © 2009–2024 Josh Close
- Project URL: [CsvHelper Project](https://joshclose.github.io/CsvHelper)
- Nuget: [CsvHelper NuGet](https://www.nuget.org/packages/CsvHelper)
- Usage:  
  - Used for advanced CSV parsing and serialization.
  
### SimpleComposeActions
- Version: N/A
- License: MIT  
- Copyright: © 2024–2025 Natel210
- Project URL: [SimpleComposeActions](https://github.com/Natel210/SimpleComposeActions)
- Usage:  
  - Used as a modular and reusable GitHub Actions workflow template.
  - Facilitates simplified CI/CD pipeline setup by enabling developers to compose workflows using pre-defined modular actions.
  - Provides pre-tested components to improve productivity and reduce workflow errors.