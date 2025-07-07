# Typemock-Isolator-Examples
A collection of practical examples demonstrating how to use Typemock Isolator for .NET to isolate and test hard-to-mock code like static methods, sealed classes, constructors, and legacy code without refactoring.


# Typemock Isolator .NET Examples

Welcome to the official example repository for [Typemock Isolator](https://www.typemock.com/isolator/), the most powerful mocking framework for .NET.

This repository contains real-world use cases and minimal examples that demonstrate how to use Typemock Isolator to write unit tests for hard-to-test code — including static methods, private methods, sealed classes, legacy code, and more.

---

## 🚀 About Typemock Isolator

Typemock Isolator is a powerful and flexible mocking framework designed to **test legacy code without the need for refactoring**. It allows developers to isolate and control dependencies — including the toughest ones like:

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
