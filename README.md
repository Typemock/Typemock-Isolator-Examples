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

---
## Running Tests

🔍 How to Run the Examples
To run these examples successfully, follow the steps below:

✅ Step 1: Install Typemock Isolator
You must have Typemock Isolator installed on your machine with a valid license.

🔗 [Installation Guide](https://www.typemock.com/docs/?book=Isolator&page=HtmlDocs/installingtypemockisolatorclient.htm)  

Download the installer:
👉 [Download Isolator](https://www.typemock.com/download-isolator/)

✅ Step 2: Activate Your Typemock License
You must activate your Typemock license before running tests.

🔗 [Activation Guide](https://www.typemock.com/docs/?book=Isolator&page=HtmlDocs/step1activatingyourtypemocklicense.htm)

You can activate it using the Typemock Configuration tool or inside the Typemock Extension menu inside Visual Studio.

✅ Step 3: Open and Run Tests in Visual Studio
Clone this repository:
```plaintext
git clone https://github.com/typemock/Typemock-Isolator-Examples.git
cd Typemock-Isolator-Examples
```
Open one of the solution files:

For .NET Framework:
CSharp/Typemock.Examples.sln

For .NET Core / .NET 5+:
CSharpCore/Typemock.Examples.Core.sln

Build the solution.

Run the tests using Test Explorer or Typemock Smart Runner in Visual Studio.


✅ Optional Step: Run Tests from the Command Line
You can also run tests using CLI tools with Typemock Isolator enabled:


▶️ dotnet test
```plaintext
%Installation_Dir%/BuildScripts/TMockRunner.exe dotnet test path/to/test.dll
```
Typemock will automatically integrate with the test run as long as it is installed and licensed on the machine.

▶️ vstest.console.exe (for .NET Framework)
```plaintext
%Installation_Dir%/BuildScripts/TMockRunner.exe "C:\Program Files\Microsoft Visual Studio\2022\Enterprise\Common7\IDE\Extensions\TestPlatform\vstest.console.exe" test.dll
```
TMockRunner.exe is Typemock’s wrapper to inject mocks into external runners.

Adjust the path to vstest.console.exe as needed based on your Visual Studio version.

More info:
🔗 [Running Tests with TMockRunner](https://www.typemock.com/docs/?book=Isolator&page=HtmlDocs%2Frunningtestsintmockrunner.htm)

---
