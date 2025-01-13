# **SimpleFileIO**

---

## **Summary**

SimpleFileIO is a library designed to provide a unified interface for **record-based IO management**. It supports the following file types and features:

- **CSV**: Structured data storage in CSV format.
- **Appendable CSV (Log)**: Incremental logging directly into a CSV file.
- **Text**: General text file management.
- **Appendable Text (Log)**: Incremental logging into plain text files.

Through the use of managed objects and interfaces, the library ensures consistent and efficient file operations across different formats.

---

## **How to**



## **GitHub Actions**

The GitHub workflow for this library was created based on [SimpleComposeActions](https://github.com/Natel210/SimpleComposeActions), a reusable and modular GitHub Actions template.

---

## **Third-Party Libraries**

This library integrates the following third-party components. It is designed as a single DLL to eliminate external dependencies, with certain libraries configured for public access.

### **1. CsvHelper**
- **Version**: 33.0.1
- **License**: Apache-2.0 OR MS-PL
- **Copyright**: © 2009–2024 Josh Close  
- **Usage**:  
  - Fully integrated into the SimpleFileIO DLL.  
  - **Publicly accessible**: External consumers can directly use CsvHelper's functionality without additional dependencies.

---

### **2. ILRepack**
- **Version**: 2.0.36
- **License**: Apache-2.0  
- **Copyright**:
  - © 2009–2024 Sadik Ali  
  - © 2009–2024 contributors from the ILMerge project  
- **Usage**:  
  - Exclusively used for **merging assemblies** during the build process.  
  - **Private**: This functionality is not exposed to external consumers or required at runtime.

---

### **3. ILRepack.Lib.MSBuild.Task**
- **Version**: 2.0.34.2
- **License**: MIT  
- **Copyright**:
  - © 2009–2024 contributors from the ILMerge project  
- **Usage**:  
  - Utilized internally during the build process to assist in merging assemblies.  
  - **Private**: This library does not affect runtime behavior.

---

## **Key Features**

_Optional: Add a list of the key features of the library here if necessary._

---

## **Notes**

- **Public Access**: CsvHelper is publicly accessible for external use.  
- **Private Components**: ILRepack and ILRepack.Lib.MSBuild.Task are solely used for internal processes and are not exposed to external consumers.

---