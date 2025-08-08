# Copilot Instructions- 
- Limit responses to one sentence especially for explanations.
- Projects in this repository should use the latest C# features.

  
# Team Best Practices
- always create files using file-scoped namespaces.
- When doing auth, always use .NET 8/9 idioms & auth state is optional/should be set to false by default
- Prefer inline lambdas over full method bodies in C#.
- Prefer async and await over synchronous code.
- Never use CSS inline styles. Always use a CSS file.
- When creating Web API projects, prefer minimal APIs project.

## Creating service classes
- Services should be placed in the 'Services' folder.
- All services must have an interface.
- Interfaces should be placed in the 'Interfaces' folder.

## Testing Guidelines
- use xUnit for unit tests
- use FluentAssertions version 7.2 for assertions
- Use Moq for mocking
- The unit tests project unclude GlobalUsings as follows:
  ```csharp
  global using FluentAssertions;
  global using Moq;
  global using Xunit;
  ```
- When creating unit tests, there is no need to include the following namespaces
  ```
  using FluentAssertions;
  using Moq;
  using Xunit;
  ```



