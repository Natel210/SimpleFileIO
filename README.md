# SimpleFileIO







## Third-Party

This library includes the following third-party components.</br>
It is designed as a single DLL to avoid external dependencies, with specific libraries configured to be publicly accessible for external use.

- **CsvHelper** (Version 33.0.1)
  - License
    - **Apache-2.0 OR MS-PL**
  - Copyright
    - © 2009–2024 Josh Close
  - Usage:
    - Fully integrated into the SimpleFileIO DLL and publicly accessible.
    - External consumers can use CsvHelper's functionality directly without additional dependencies.

- **ILRepack** (Version 2.0.36)
  - License
    - **Apache-2.0**
  - Copyright
    - © 2009–2024 Sadik Ali
    - © 2009–2024 contributors from the ILMerge project
  - Usage:
    - Used solely for merging assemblies during the build process.
    - This functionality is entirely private and not exposed to external consumers or required at runtime.


- **ILRepack.Lib.MSBuild.Task** (Version 2.0.34.2)
  - License
    - **MIT**
  - Copyright
    - © 2009–2024 contributors from the ILMerge project
  - Usage:
    - Utilized internally during the build process to assist in merging assemblies.
    - This library is strictly private and does not affect runtime behavior.