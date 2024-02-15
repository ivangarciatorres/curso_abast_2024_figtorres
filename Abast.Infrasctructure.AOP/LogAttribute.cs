using Abast.Utils.Logger;
using MethodBoundaryAspect.Fody.Attributes;
using System;

public sealed class LogAttribute : OnMethodBoundaryAspect
{


    public override void OnEntry(MethodExecutionArgs args)
    {

        Console.WriteLine("OnEntry: " + args.Method.DeclaringType.FullName + "");
        Logger.LogDebug("OnEntry:");
        Logger.LogInfo("OnEntry:");
    }

    public override void OnExit(MethodExecutionArgs args)
    {
        Console.WriteLine("OnExit: " + args + "");
        Logger.LogInfo("OnExit:");
    }

    public override void OnException(MethodExecutionArgs args)
    {
        Console.WriteLine("OnException: " + args + "");
        Logger.LogError("OnException:");
    }
}