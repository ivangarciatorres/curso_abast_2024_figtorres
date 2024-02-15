using Abast.Utils.Logger;
using MethodBoundaryAspect.Fody.Attributes;
using Newtonsoft.Json;
using System;

public sealed class LogDetail : OnMethodBoundaryAspect
{
    log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    public override void OnEntry(MethodExecutionArgs args)
    {

        Console.WriteLine("OnEntry: " + args.Method.DeclaringType.FullName + "");
        Console.WriteLine("OnEntry Args: " + JsonConvert.SerializeObject(args.Arguments) + "");
        log.Debug("OnEntry:");
        log.Debug("OnEntry Args: " + JsonConvert.SerializeObject(args.Arguments) + "");
    }

    public override void OnExit(MethodExecutionArgs args)
    {
        Console.WriteLine("OnExit: " + args + "");
        this.log.Debug("OnExit:");
    }

    public override void OnException(MethodExecutionArgs args)
    {
        Console.WriteLine("OnException: " + args + "");
        this.log.Error("OnException:");
    }
}