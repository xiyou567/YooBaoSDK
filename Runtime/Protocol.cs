using System;
using System.IO;

namespace YooBao.SDK
{
    /// <summary>
    /// 协议的唯一致数源。Client 和 Plugin 都从这里取值，不允许各自写死。
    /// </summary>
    public static class Protocol
    {
        /// <summary>写进 /v1/health 响应用来自证身份，Plugin 靠它确认端口后面真的是 YooBao。</summary>
        public const string AppName = "YooBao";

        /// <summary>协议主版本。不兼容时握手返回 409，两端都要显式提示用户。</summary>
        public const int Version = 1;

        public const int DefaultPort = 17870;
        public const int PortScanCount = 10;

        /// <summary>请求体上限。超过直接 413，防止本机进程灌爆内存。</summary>
        public const int MaxBodyBytes = 256 * 1024;

        public const string TokenHeader = "X-YooBao-Token";
        public const string SessionHeader = "X-YooBao-Session";

        public const string RouteHealth = "/v1/health";
        public const string RouteHandshake = "/v1/handshake";
        public const string RouteEvents = "/v1/events";
        public const string RouteSnapshot = "/v1/snapshot";
        public const string RouteGoodbye = "/v1/goodbye";

        public const string RuntimeInfoFolderName = "YooBao";
        public const string RuntimeInfoFileName = "runtime.json";

        /// <summary>测试用：直接指定 runtime.json 的所在目录，绕过 %LOCALAPPDATA%。</summary>
        public const string RuntimeInfoDirEnvVar = "YOOBAO_RUNTIME_INFO_DIR";

        public static bool IsCompatible(int other)
        {
            return other == Version;
        }

        /// <summary>
        /// 故意用 SpecialFolder.LocalApplicationData，而不是 Application.persistentDataPath：
        /// 后者的路径里带着 productName，两个工程的值不一样，Plugin 会找不到 Client 写的文件。
        /// </summary>
        public static string RuntimeInfoDirectory
        {
            get
            {
                string overridden = Environment.GetEnvironmentVariable(RuntimeInfoDirEnvVar);
                if (!string.IsNullOrEmpty(overridden))
                {
                    return overridden;
                }

                string localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                return Path.Combine(localAppData, RuntimeInfoFolderName);
            }
        }

        public static string RuntimeInfoPath
        {
            get { return Path.Combine(RuntimeInfoDirectory, RuntimeInfoFileName); }
        }

        public static long NowMs()
        {
            return DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        }
    }
}
