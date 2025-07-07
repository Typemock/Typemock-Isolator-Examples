# Typemock Isolator .NET Examples

Welcome to the official example repository for [Typemock Isolator](https://www.typemock.com/isolator/), the most powerful mocking framework for .NET.

This repository contains real-world use cases and minimal examples that demonstrate how to use Typemock Isolator to write unit tests for hard-to-test code including static methods, private methods, sealed classes, legacy code, and more.

---

## 🚀 About Typemock Isolator

Typemock Isolator is a powerful and flexible mocking framework designed to **test legacy code without the need for refactoring**. It allows developers to isolate and control dependencies including the toughest ones like:

- Static methods
- Sealed classes
- Non-virtual methods
- Private methods
- Constructors

Isolator lets you create **tests before design**, giving you full control over the behavior of any .NET code.

📚 Learn more in our [full documentation](https://www.typemock.com/docs/?book=Isolator&page=introduction.htm)

---

## 📁 Folder Structure

```plaintext
Examples/
├── CSharp/                # Examples using the full .NET Framework (e.g., .NET 4.5+)
│   ├── Typemock.Examples.ThirdPartyProject/
│
└── CSharpCore/            # Examples targeting .NET Core / .NET 5+
    ├── Typemock.Examples.Core.ThirdPartyProject/
```

## Running Tests
```plaintext
🔍 How to Run the Examples
To run these examples successfully, follow the steps below:

✅ Step 1: Install Typemock Isolator
You must have Typemock Isolator installed on your machine with a valid license.

🔗 Installation Guide

Download the installer:
👉 https://www.typemock.com/download-isolator/

✅ Step 2: Activate Your Typemock License
You must activate your Typemock license before running tests.

🔗 Activation Guide

You can activate it using the Typemock Configuration tool or inside the Typemock Extension menu inside VisualStudio.

✅ Step 3: Open and Run Tests in Visual Studio
Clone this repository:

git clone https://github.com/typemock/Typemock-Isolator-Examples.git
cd Typemock-Isolator-Examples

Open one of the solution files:

For .NET Framework:
CSharp/TypemockExamples.sln

For .NET Core / .NET 5+:
CSharpCore/TypemockExamples.Core.sln

Build the solution.

Run the tests using Test Explorer or Typemock Smart Runner in Visual Studio.
![image](https://github.com/user-attachments/assets/ed912349-dcdb-4169-91ac-031d3ec70544)

✅ Optional Step: Run Tests from the Command Line
You can also run tests using CLI tools with Typemock Isolator enabled:

▶️ dotnet test (for .NET Core / .NET 5+)

%Installation_Dir%/BuildScripts/TMockRunner.exe dotnet test test.dll
Typemock will automatically integrate with the test run as long as it is installed and licensed on the machine.

▶️ vstest.console.exe (for .NET Framework)

%Installation_Dir%/BuildScripts/TMockRunner.exe "C:\Program Files\Microsoft Visual Studio\2022\Enterprise\Common7\IDE\Extensions\TestPlatform\vstest.console.exe" test.dll

TMockRunner.exe is Typemock’s wrapper to inject mocks into external runners.

Adjust the path to vstest.console.exe as needed based on your Visual Studio version.

More info:
🔗 Running Tests with TMockRunner

```
