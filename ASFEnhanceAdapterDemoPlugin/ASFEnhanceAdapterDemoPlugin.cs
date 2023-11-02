using ArchiSteamFarm.Plugins.Interfaces;
using ArchiSteamFarm.Steam;
using System.ComponentModel;
using System.Composition;
using System.Reflection;

namespace AdapterDemoPlugin;

[Export(typeof(IPlugin))]
internal sealed class AdapterDemoPlugin : IBotCommand2
{
    public string Name => "Adapter Demo Plugin";
    public Version Version => Utils.MyVersion;

    private bool ASFEBridge;

    /// <summary>
    /// 插件加载事件
    /// </summary>
    /// <returns></returns>
    public Task OnLoaded()
    {
        Utils.ASFLogger.LogGenericInfo("插件作者 Chr_, 联系方式 chr@chrxw.com");
        Utils.ASFLogger.LogGenericInfo("爱发电: https://afdian.net/@chr233");

        var flag = BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
        var handler = typeof(AdapterDemoPlugin).GetMethod(nameof(ResponseCommand), flag);

        const string pluginId = nameof(AdapterDemoPlugin); //插件标识符
        const string cmdPrefix = "ADP"; //插件命令前缀
        const string repoName = "chr233/ASFEnhanceAdapterDemoPlugin"; //自动更新仓库名称 比如 ASFEnhance 或 chr233/ASFEnhance (用户默认为chr233), 不需要自动更新可以设为 null

        ASFEBridge = AdapterBridge.InitAdapter(Name, pluginId, cmdPrefix, repoName, handler);

        if (ASFEBridge)
        {
            Utils.ASFLogger.LogGenericDebug("ASFEBridge 注册成功");
        }
        else
        {
            Utils.ASFLogger.LogGenericInfo("ASFEBridge 注册失败");
            Utils.ASFLogger.LogGenericWarning("推荐安装 ASFEnhance, 支持自动更新插件");
        }

        return Task.CompletedTask;
    }

    /// <summary>
    /// 获取插件信息
    /// </summary>
    private static string? PluginInfo => string.Format("{0} {1}", nameof(AdapterDemoPlugin), Utils.MyVersion);

    /// <summary>
    /// 处理命令
    /// </summary>
    /// <param name="access"></param>
    /// <param name="cmd"></param>
    /// <param name="args"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    private static Task<string?>? ResponseCommand(EAccess access, string cmd, string[] args)
    {
        var argLength = args.Length;
        return argLength switch
        {
            0 => throw new InvalidOperationException(nameof(args)),
            1 => cmd switch //不带参数
            {
                //Plugin Info
                "ADAPTERDEMOPLUGIN" or
                "ADP" when access >= EAccess.FamilySharing =>
                    Task.FromResult(PluginInfo),

                _ => null,
            },
            _ => cmd switch //带参数
            {
                _ => null,
            },
        };
    }

    /// <summary>
    /// 处理命令事件
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="access"></param>
    /// <param name="message"></param>
    /// <param name="args"></param>
    /// <param name="steamId"></param>
    /// <returns></returns>
    /// <exception cref="InvalidEnumArgumentException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public async Task<string?> OnBotCommand(Bot bot, EAccess access, string message, string[] args, ulong steamId = 0)
    {
        if (ASFEBridge)
        {
            return null;
        }

        if (!Enum.IsDefined(access))
        {
            throw new InvalidEnumArgumentException(nameof(access), (int)access, typeof(EAccess));
        }

        try
        {
            var cmd = args[0].ToUpperInvariant();

            if (cmd.StartsWith("DEMO."))
            {
                cmd = cmd[5..];
            }

            var task = ResponseCommand(access, cmd, args);
            if (task != null)
            {
                return await task.ConfigureAwait(false);
            }
            else
            {
                return null;
            }
        }
        catch (Exception ex)
        {
            _ = Task.Run(async () =>
            {
                await Task.Delay(500).ConfigureAwait(false);
                Utils.ASFLogger.LogGenericException(ex);
            }).ConfigureAwait(false);

            return ex.StackTrace;
        }
    }
}
