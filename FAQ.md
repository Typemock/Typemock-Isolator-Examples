# 🧠 Typemock Isolator FAQ (.NET)

## 🔧 Common Errors & Setup

<details>
<summary><strong>Why do I get “Typemock is not enabled” when running tests?</strong></summary>

This means the Typemock runner or profiler isn’t active.

- Run `TMockRunner.exe` before executing tests.  
- Make sure **Visual Studio > Typemock Mocking** is turned **ON**.  
- If you’re using CI/CD, install Typemock or use the official build server Tasks for automatic deployment and license management.  
- Check that your license is active for the local machine or build agent.
</details>

---

<details>
<summary><strong>My tests fail after upgrading .NET. Is it supported?</strong></summary>

Yes — the latest Typemock Isolator versions fully support modern .NET and C#.  
👉 [Download the latest version](https://www.typemock.com/download-isolator/) for guaranteed compatibility.
</details>

---

## 🧱 Mocking & Best Practices

<details>
<summary><strong>How do I mock static methods, sealed classes, or constructors?</strong></summary>

Typemock lets you fake anything — even what other frameworks can’t:
```csharp
Isolate.WhenCalled(() => MyStaticClass.DoWork()).WillReturn(true);
var fake = Isolate.Fake.NextInstance<MyService>();
```
It works on statics, privates, constructors, events, and ref parameters — no code changes needed.  
👉 See more examples in the [Isolate API Reference](https://www.typemock.com/isolate/)
</details>

---

<details>
<summary><strong>How do I test private methods or fields?</strong></summary>

Invoke private methods:
```csharp
Isolate.Invoke.Method(instance, "PrivateMethod");
```
Set or get private fields:
```csharp
Isolate.SetField(instance, "fieldName", newValue);
var value = Isolate.GetField(instance, "fieldName");
```
Use sparingly — testing through public APIs is always cleaner.
</details>

---

<details>
<summary><strong>How can I unit test legacy code without refactoring it?</strong></summary>

That’s exactly why Typemock exists.  
It isolates tightly coupled dependencies (statics, singletons, sealed classes) using patented runtime technology.  
You can add tests without changing production code — and gradually refactor later for clarity.
</details>

---

<details>
<summary><strong>How do I mock async methods or Tasks?</strong></summary>

```csharp
Isolate.WhenCalled(() => dependency.GetDataAsync())
       .WillReturn(Task.FromResult(fakeData));
var result = await sut.DoWorkAsync();
```
Avoid blocking calls (`.Wait()`, `.Result`) — async/await is fully supported in Isolator 9.x and above.
</details>

---

<details>
<summary><strong>Can I mock DateTime.Now, Guid.NewGuid(), or Environment.MachineName?</strong></summary>

Yes — Typemock can fake framework statics for deterministic tests:
```csharp
Isolate.WhenCalled(() => DateTime.Now)
       .WillReturn(new DateTime(2025, 1, 1));
```
</details>

---

<details>
<summary><strong>How do I test member variables vs behavior?</strong></summary>

- Prefer **behavior verification** (`Isolate.Verify.WasCalled`) over direct field checks.  
- If you must, use `Isolate.SetField()` to manipulate internal state.  
Behavior-based testing is safer and cleaner for refactoring.
</details>

---

## ⚙️ Integration & Frameworks

<details>
<summary><strong>Which test frameworks are supported?</strong></summary>

MSTest, NUnit, and xUnit — all major .NET test runners integrate seamlessly with Typemock.
</details>

---

<details>
<summary><strong>How do I enable Typemock in CI/CD (Azure, GitHub Actions, Jenkins)?</strong></summary>

The best approach is to use Typemock’s **native add-ins or build tasks**, which automatically deploy Typemock on build agents and manage licenses.  

Alternatively:
```bash
TMockRunner.exe -deploy
# after tests complete
TMockRunner.exe -unregister   # introduced in v9.3.7
```
This ensures licenses are cleaned up properly between builds.
</details>

---

<details>
<summary><strong>Can I use Typemock in VS Code or Linux environments?</strong></summary>

Yes — Typemock supports .NET 6+ cross-platform projects.  
To use it in VS Code or Linux:
- Install the **Typemock NuGet package**.  
- Run tests via:
  ```bash
  tmockrunner dotnet test
  ```
Works on Windows, macOS, and most Linux distributions with .NET SDK 6+.
</details>

---

<details>
<summary><strong>How does Typemock integrate with Visual Studio?</strong></summary>

Typemock offers full Visual Studio integration, including:  
- **SmartRunner** suggestions for missing tests  
- Integration with MSTest, NUnit, and xUnit runners  
- Automatic profiler activation  
- Visual feedback on covered vs. uncovered code lines
</details>

---

## ⚡ Performance & Coverage

<details>
<summary><strong>Does Typemock slow down tests?</strong></summary>

Barely. Typemock introduces minimal overhead at runtime.  
To optimize performance:
- Disable verbose tracing and logs.  
- Use partial fakes instead of full ones.  
- Don't use coverage.
</details>

---

<details>
<summary><strong>How can I measure and improve test coverage with Typemock?</strong></summary>

Typemock includes its own coverage tool and integrates with:
- Microsoft Code Coverage  
- JetBrains dotCover  
- OpenCover  
- Coverlet  

Because Typemock lets you fake statics and privates, you can reach previously untestable paths — use this to close coverage gaps.  
Focus on **behavioral coverage**, not just line count.
</details>

---

<details>
<summary><strong>What patterns make Typemock tests maintainable?</strong></summary>

✅ Follow **Arrange-Act-Assert (AAA)**.  
✅ Mock only dependencies, not logic.  
✅ One assertion per test.  
✅ Use `Isolate.Verify` for behavioral validation.  
✅ Apply a **three-part naming convention**: `Method_State_ExpectedResult`.
</details>

---

## 🤖 AI Test Generation 

<details>
<summary><strong>Does Typemock support AI-based test suggestions?</strong></summary>

Yes — the new **AI Suggest** analyzes your code to suggest missing tests and coverage gaps.  
It automatically proposes tests for untested methods and helps maintain **behavioral equivalence** during refactoring.  
👉 Learn more in the [AI-Ready Isolator API](https://www.typemock.com/introducing-the-ai-ready-isolator-api/).
</details>

---

## ☁️ Licensing & Build Automation

<details>
<summary><strong>How do I manage Typemock licenses in dynamic build agents (Azure, Docker, CI)?</strong></summary>

Activate licenses automatically:
```bash
TMockRunner.exe -deploy -key <license>
```
After build completion, unregister:
```bash
TMockRunner.exe -unregister
```
This ensures floating licenses are released properly.  
Azure DevOps Tasks now include **auto-unregister** for ephemeral agents.
</details>

---

## 🧩 Advanced Scenarios

<details>
<summary><strong>How does Typemock actually intercept static and sealed calls?</strong></summary>

Typemock uses patented technology that hooks directly into the .NET runtime (Profiler API) to redirect calls to your fakes.  
No source changes, no re-compilation — pure runtime magic.
</details>

---

<details>
<summary><strong>Can I use Typemock to test database, file, or network dependencies?</strong></summary>

Yes — by mocking their APIs:
```csharp
Isolate.WhenCalled(() => File.ReadAllText("data.txt"))
       .WillReturn("mocked");
```
This avoids hitting real resources and makes tests fast and repeatable.
</details>

---

<details>
<summary><strong>How do I handle async tests that trigger external events?</strong></summary>

Combine Typemock’s fakes with async test support:
```csharp
Isolate.WhenCalled(() => _bus.PublishAsync(_)).IgnoreCall();
await sut.ExecuteAsync();
Isolate.Verify.WasCalledWithAnyArguments(() => _bus.PublishAsync(_));
```
Ensures async calls are verified deterministically — no race conditions.
</details>

---

## 🧭 Summary

Typemock Isolator empowers .NET developers to:

- ✅ Mock anything (static, sealed, private, constructors)  
- ✅ Test legacy code without refactoring  
- ✅ Integrate with any test framework or CI/CD pipeline  
- ✅ Run cross-platform (Windows, Linux, VS Code)  
- ✅ Use AI-based test insights via **Suggest**

---

### 📚 Related Links
- [Download Typemock Isolator](https://www.typemock.com/download-isolator/)  
- [Typemock Docs](https://www.typemock.com/docs/?book=Isolator&page=index.htm)  
- [AI-Ready Isolator API Blog](https://www.typemock.com/introducing-the-ai-ready-isolator-api/)  
- [Typemock Blog & Release Notes](https://www.typemock.com/blog/)  
