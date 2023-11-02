# ASFEnhanceAdapterDemoPlugin

![GitHub Repo stars](https://img.shields.io/github/stars/chr233/ASFEnhanceAdapterDemoPlugin?logo=github)
![GitHub last commit](https://img.shields.io/github/last-commit/chr233/ASFEnhanceAdapterDemoPlugin?logo=github)
[![License](https://img.shields.io/github/license/chr233/ASFEnhanceAdapterDemoPlugin?logo=apache)](https://github.com/chr233/ASFEnhanceAdapterDemoPlugin/blob/master/license)

[![Bilibili](https://img.shields.io/badge/bilibili-Chr__-00A2D8.svg?logo=bilibili)](https://space.bilibili.com/5805394)
[![Steam](https://img.shields.io/badge/steam-Chr__-1B2838.svg?logo=steam)](https://steamcommunity.com/id/Chr_)

[![Steam](https://img.shields.io/badge/steam-donate-1B2838.svg?logo=steam)](https://steamcommunity.com/tradeoffer/new/?partner=221260487&token=xgqMgL-i)
[![爱发电](https://img.shields.io/badge/爱发电-chr__-ea4aaa.svg?logo=github-sponsors)](https://afdian.net/@chr233)

> 本插件为 ASFEnhance Adapter 示例插件, 无实际功能, 仅用于演示如何接入 ASFEnhance

## 安装方式

### ASFEnhance 联动

> 推荐搭配 [ASFEnhance](https://github.com/chr233/ASFEnhance) 使用, 可以通过 ASFEnhance 实现插件更新管理和禁用特定命令等功能

## 插件指令说明

| 命令                          | 缩写  | 权限            | 说明                                    |
| ----------------------------- | ----- | --------------- | --------------------------------------- |
| `ASFEnhanceAdapterDemoPlugin` | `ADP` | `FamilySharing` | 查看 ASFEnhanceAdapterDemoPlugin 的版本 |

## 接入 ASFEnhance 步骤

1. 将 `AdapterBridge.cs` 添加到项目
2. 在 `OnLoaded` 事件中向 `ASFEnhance` 注册插件, 代码如下

   ```cshape
    var flag = BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
    var handler = typeof(AdapterDemoPlugin).GetMethod(nameof(ResponseCommand), flag);

    const string pluginId = nameof(AdapterDemoPlugin); //插件标识符
    const string cmdPrefix = "ADP"; //插件命令前缀
    const string repoName = "chr233/ASFEnhanceAdapterDemoPlugin"; //自动更新仓库名称 比如 ASFEnhance 或 chr233/ASFEnhance (用户默认为chr233), 不需要自动更新可以设为 null

    var registered = AdapterBridge.InitAdapter(Name, pluginId, cmdPrefix, repoName, handler);
   ```

   注册成功时返回 `true`, 否则返回 `false`, 注册成功后无需在 `OnBotCommand` 中处理命令, 会由 `ASFEnhance` 通过反射调用命令响应函数.
