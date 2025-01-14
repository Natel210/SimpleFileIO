# **SimpleFileIO**

---

## **Summary**

SimpleFileIO is a library designed to provide a unified interface for **record-based IO management**. It supports the following file types and features:

- **CSV**: Structured data storage in CSV format.
- **Appendable CSV (Log)**: Incremental logging directly into a CSV file.
- **Text**: General text file management.
- **Appendable Text (Log)**: Incremental logging into plain text files.

This library eliminates external dependencies by integrating required components, and **provides a single DLL that includes CsvHelper for advanced CSV management**. Through the use of managed objects and interfaces, SimpleFileIO ensures consistent and efficient file operations across different formats.



---

## **How to**


## **Third-Party Libraries**

This library integrates the following third-party components. It is designed as a single DLL to eliminate external dependencies, with certain libraries configured for public access.

### **1. CsvHelper**
- **Version**: 33.0.1
- **License**: Apache-2.0 OR MS-PL
- **Copyright**: © 2009–2024 Josh Close
- **Project URL**: [https://joshclose.github.io/CsvHelper](https://joshclose.github.io/CsvHelper)
- **Nuget**: [https://www.nuget.org/packages/CsvHelper](https://www.nuget.org/packages/CsvHelper)
- **Usage**:  
  - Fully integrated into the SimpleFileIO DLL.  
  - **Publicly accessible**: External consumers can directly use CsvHelper's functionality without additional dependencies.
  - **Modified**: **False**

---

### **2. ILRepack.Lib.MSBuild.Task**
- **Version**: 2.0.34.2
- **License**: MIT  
- **Copyright**:
  - © 2009–2024 contributors from the ILMerge project
- **Project URL**: [https://joshclose.github.io/CsvHelper](https://joshclose.github.io/CsvHelper)
- **Nuget**: [https://www.nuget.org/packages/CsvHelper](https://www.nuget.org/packages/CsvHelper)
- **Usage**:  
  - Utilized internally during the build process to assist in merging assemblies.  
  - **Private**: This library does not affect runtime behavior.
  - **Modified**: **False**

---

### **3. SimpleComposeActions**
- **Version**: N/A
- **License**: MIT  
- **Copyright**:
  - © 2024–2025 Natel210
- **Project URL**: [https://github.com/Natel210/SimpleComposeActions](https://github.com/Natel210/SimpleComposeActions)
- **Usage**:  
  - Used as a modular and reusable GitHub Actions workflow template.
  - Facilitates simplified CI/CD pipeline setup by enabling developers to compose workflows using pre-defined modular actions.
  - rovides pre-tested components to improve productivity and reduce workflow errors.
  
---

## **Key Features**

- Unified interface for multiple file types (CSV, Text, Logs).
- Integration of CsvHelper for advanced CSV processing.
- Single DLL distribution without external dependencies.

---

## **Notes**

- **Public Access**: CsvHelper is publicly accessible for external use.  
- **Private Components**: ILRepack.Lib.MSBuild.Task are solely used for internal processes and are not exposed to external consumers.

---