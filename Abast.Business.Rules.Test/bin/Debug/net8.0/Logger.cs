using log4net;
using log4net.Config;
using log4net.Core;

namespace Abast.Utils.Logger
{
    using log4net;

    public static class Logger
    {
        private static readonly ILog log = LogManager.GetLogger("logger");

        public static void LogInfo(string message)
        {
            log.Info(message);
        }

        public static void LogError(string message)
        {
            log.Error(message);
        }

        public static void LogDebug(string message)
        {
            log.Debug(message);
        }
        // Agrega más métodos según sea necesario para otros niveles de log.
    }

}
