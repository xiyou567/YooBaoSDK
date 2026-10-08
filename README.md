# YooBao SDK

YooBao 平台（独立运行的 AI 游戏伙伴软件）与各个游戏 Plugin 之间的**唯一契约**。

## ⚠️ 三条不能违反的规则

1. **本目录不能移动、不能改名。**
   两个工程都通过 `"com.yoobao.sdk": "file:../../YooBaoSDK"` 引用它（相对各自的 `Packages/` 解析）。
   一旦移动或改名，两个工程都会打不开并报一堆难懂的错。

2. **改完这里，两个工程都要重新进一次 Play。**
   本地 UPM 包改动后 Unity 不一定会立即重编译。

3. **SDK 不依赖 Unity，也不依赖任何具体游戏。**
   `YooBao.SDK.asmdef` 设了 `noEngineReferences: true`，由编译器强制执行这条。
   所以这里看不到 `UnityEngine`，日志靠外部注入 `YooBaoJson.LogError`。

## 契约设计约束

- **禁止 `object` 作为值类型。** 变长字段用两个平行字典承载：
  `GameEvent.numData`（`Dictionary<string, double>`）+ `GameEvent.strData`（`Dictionary<string, string>`）。
  原因：`object` 作为泛型实参在 AOT/IL2CPP 下需要运行期类型分发，是已知的坑。
- **禁止多态、禁止 `TypeNameHandling`。** 事件类型靠 `name` 字符串区分，不靠 CLR 类型。
- **所有序列化只从 `YooBaoJson` 进出。** 万一将来要换序列化库，只改这一个文件。

## 目录

```
Runtime/
  YooBao.SDK.asmdef     name=YooBao.SDK, autoReferenced=true
  Protocol.cs           协议版本、端口、header 名、body 上限、runtime.json 路径
  EventNames.cs         事件名常量（两端共用同一份，消灭魔法字符串）
  Dto/                  GameEvent / GameSnapshot / PluginManifest / 各种消息体
  Json/YooBaoJson.cs    唯一的序列化入口
```
