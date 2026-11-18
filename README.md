# Pure.DI 2.5.8 generates an undefined `overriddenBoolean`

Pure.DI 2.5.8 emits C# code that references `overriddenBoolean` without declaring it. The resulting compiler error is CS0103. Pure.DI 2.5.6 reports the unresolved dependency as DIE000 for the same input.

## Steps to reproduce

This project targets .NET 10 and has no dependencies other than Pure.DI. From the repository root, run:

```powershell
dotnet build Repro.csproj -p:PureDIVersion=2.5.8
dotnet build Repro.csproj -p:PureDIVersion=2.5.6
```

## Actual result

With Pure.DI 2.5.8, the build fails with:

```text
Composition.g.cs(117,35): error CS0103: The name 'overriddenBoolean' does not exist in the current context
```

The generated code includes this reference without an `overriddenBoolean` declaration in scope:

```csharp
return new global::FileSystem(overriddenBoolean, _singletonNotify323d);
```

With Pure.DI 2.5.6, the same source instead fails with:

```text
Composition.cs(19,33): error DIE000: Unable to resolve "bool" in FileSystem(bool Flag<--bool, Notify Notify<--Notify).
```

## Expected result

The direct `Root<FileSystem>()` has an unresolved `bool` constructor parameter. Pure.DI should report that dependency error without generating C# that references an undeclared variable.

Both versions are expected to reject this input. The regression demonstrated here is the generated CS0103 diagnostic in 2.5.8, not a change from a successful build to a failed build.

The generated source can be inspected under `obj/generated` after building. The results above were observed with .NET SDK 11.0.100 targeting `net10.0`.
