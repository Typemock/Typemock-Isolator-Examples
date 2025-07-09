# Typemock Isolator .NET Examples

Welcome to the official example repository for [Typemock Isolator](https://www.typemock.com/isolator-product-page/?utm_source=github&utm_medium=readme&utm_campaign=isolator_examples), the most powerful mocking framework for .NET.

This repository showcases official example projects for Typemock Isolator, the most powerful .NET mocking framework. It includes real-world and minimal examples that demonstrate how to unit test hard-to-mock .NET code, including static methods, sealed classes, legacy systems, and private members using Typemock’s advanced mocking engine.

---

## 🚀 About Typemock Isolator

Typemock Isolator is a powerful and flexible mocking framework designed to **test legacy code without the need for refactoring**. It allows developers to isolate and control dependencies, including the toughest ones like:

- Static methods
- Sealed classes
- Non-virtual methods
- Private methods
- Constructors

Isolator lets you create **tests before design**, giving you full control over the behavior of any .NET code.

📚 Learn more in the online [documentation](https://www.typemock.com/docs/?book=Isolator&page=HtmlDocs%2Funderstandingtypemockisolator.htm)

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
## Running Tests in Visual Studio

🔍 How to Run the Examples
To run these examples successfully, follow the steps below:

✅ Step 1: Install Typemock Isolator

Download the installer:

👉 [Download Isolator](https://www.typemock.com/download-isolator/?utm_source=github&utm_medium=readme&utm_campaign=isolator_examples)

🔗 [Installation Guide](https://www.typemock.com/docs/?book=Isolator&page=HtmlDocs/installingtypemockisolatorclient.htm)  


✅ Step 2:
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

**Build the solution.**

✅ Step 3: Open and Run Tests in Visual Studio
Run the tests using Test Explorer or Typemock Smart Runner in Visual Studio.

**Typemock Test Navigator**:

![image](https://github.com/user-attachments/assets/3464e6fc-916e-40e4-a9ce-f0ae6723b96a)

**Visual Studio Test Explorer**:

![image](https://github.com/user-attachments/assets/4d86828f-0c48-4dc5-b0b4-a49fe501b918)

---
## Advanced option: Running Tests from Command Line:

▶️ dotnet test
```plaintext
%Installation_Dir%/BuildScripts/TMockRunner.exe dotnet test path/to/test.dll
```

See more:
🔗 [Running Tests with TMockRunner](https://www.typemock.com/docs/?book=Isolator&page=HtmlDocs%2Frunningtestsintmockrunner.htm)

---
