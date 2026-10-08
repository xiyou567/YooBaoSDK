using System;

namespace YooBao.SDK
{
    /// <summary>
    /// 插件的自我介绍。握手时发给 Client，用于版本兼容性检查和未来的插件管理界面。
    /// </summary>
    [Serializable]
    public class PluginManifest
    {
        /// <summary>反向域名风格的唯一标识，例如 com.yoobao.plugin.brotato。</summary>
        public string id;

        public string name;
        public string version;

        /// <summary>游戏标识，例如 brotato。Client 用它决定调用哪套游戏知识。</summary>
        public string gameId;

        public string gameVersion;
        public string unityVersion;

        /// <summary>能力声明，例如 snapshot.basic / events.diff。为将来的功能协商预留。</summary>
        public string[] capabilities;
    }
}
