using ArchiSteamFarm.Core;
using ArchiSteamFarm.NLog;
using System.Reflection;

namespace AdapterDemoPlugin;

internal static class Utils
{
    /// <summary>
    /// 获取版本号
    /// </summary>
    internal static Version MyVersion => Assembly.GetExecutingAssembly().GetName().Version ?? new Version("0.0.0.0");

    /// <summary>
    /// 日志
    /// </summary>
    internal static ArchiLogger ASFLogger => ASF.ArchiLogger;
}
