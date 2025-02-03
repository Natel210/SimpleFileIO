## **SimpleFileIO**

---

## **Summary**

SimpleFileIO is a library designed to provide a unified interface for **record-based IO management**. It supports the following file types and features:

- **CSV**: Structured data storage in CSV format.
- **Appendable CSV (Log)**: Incremental logging directly into a CSV file.
- **Text**: General text file management.
- **Appendable Text (Log)**: Incremental logging into plain text files.

This library eliminates unnecessary dependencies while maintaining flexibility and ease of use. CsvHelper is used for advanced CSV management, but it is **not embedded** in the DLL, allowing developers to manage dependencies separately.

---

## **How to**

## **Third-Party Libraries**

This library relies on the following third-party components but does not embed them, allowing users to manage dependencies externally.

### **1. CsvHelper**
- **Version**: 33.0.1
- **License**: Apache-2.0 OR MS-PL
- **Copyright**: © 2009–2024 Josh Close
- **Project URL**: [CsvHelper Project](https://joshclose.github.io/CsvHelper)
- **Nuget**: [CsvHelper NuGet](https://www.nuget.org/packages/CsvHelper)
- **Usage**:  
  - Used for advanced CSV parsing and serialization.
  - **Not embedded**: Developers must install CsvHelper separately via NuGet.
  
### **2. SimpleComposeActions**
- **Version**: N/A
- **License**: MIT  
- **Copyright**:
  - © 2024–2025 Natel210
- **Project URL**: [SimpleComposeActions](https://github.com/Natel210/SimpleComposeActions)
- **Usage**:  
  - Used as a modular and reusable GitHub Actions workflow template.
  - Facilitates simplified CI/CD pipeline setup by enabling developers to compose workflows using pre-defined modular actions.
  - Provides pre-tested components to improve productivity and reduce workflow errors.
  
---

## **Key Features**

- Unified interface for multiple file types (CSV, Text, Logs).
- CsvHelper integration for advanced CSV processing (requires external installation).
- Modular approach with minimal external dependencies.

---

## **Notes**

- **CsvHelper is not embedded**: Developers must install it via NuGet.
- **No ILRepack usage**: The library does not merge external dependencies into a single DLL.

