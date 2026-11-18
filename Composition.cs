using System;
using Pure.DI;

internal partial class Composition
{
    private static void Setup() =>
        DI.Setup(nameof(Composition))
            .Hint(Hint.ThreadSafe, "Off")
            .DefaultLifetime(Lifetime.Singleton)
            .Bind<Dialogs>().To<Dialogs>()
            .Bind<Notify>().To<Notify>()
            .Root<Service>("Service")
            .Root<Notify>()
            .Root<Dialogs>()
            .Root<FileSystem>();
}

internal record Service(Lazy<Dialogs> Dialogs, Func<bool, FileSystem> Create);
internal record FileSystem(bool Flag, Notify Notify);
internal record Notify(Lazy<Dialogs> Dialogs);
internal record Dialogs(Func<string, Dialog> Create);
internal record Dialog(string Name, Func<bool, FileSystem> Create);

