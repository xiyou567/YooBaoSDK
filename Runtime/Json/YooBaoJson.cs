using System;
using System.Globalization;
using Newtonsoft.Json;

namespace YooBao.SDK
{
    /// <summary>
    /// SDK 唯一的序列化出入口。
    ///
    /// 为什么所有序列化都必须走这里：
    /// 两端各自 new 一个 JsonSerializerSettings 的话，只要有一边的配置不同
    /// （比如驼峰命名、数字格式、null 处理），就会出现「一边发得出去、另一边解不开」
    /// 这种极难定位的问题。收在一个文件里，对称性由构造保证。
    /// 万一将来要换掉 Newtonsoft（例如打包后发现 AOT 有问题），也只改这一个文件。
    /// </summary>
    public static class YooBaoJson
    {
        /// <summary>
        /// SDK 不引用 UnityEngine（asmdef 设了 noEngineReferences），
        /// 所以日志能力由宿主注入。不注入也不会崩，只是错误会被静默吞掉。
        /// </summary>
        public static Action<string> LogWarning;
        public static Action<string> LogError;

        private static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            // null 字段直接省略，事件负载能小一大截。
            NullValueHandling = NullValueHandling.Ignore,

            // 反序列化时遇到不认识的字段直接忽略。
            // 这样以后给 DTO 加字段，老版本的一端不会因为多出来的字段而失败。
            MissingMemberHandling = MissingMemberHandling.Ignore,

            // 关掉日期自动识别：否则形如 "2026-09-26" 的字符串会被当成 DateTime，
            // 之后再想读成数字就会抛异常。
            DateParseHandling = DateParseHandling.None,

            // 数字一律用不变文化格式化。中文 Windows 上小数点也是 "."，
            // 但这个习惯一旦不写死，换个区域设置就会出诡异问题。
            Culture = CultureInfo.InvariantCulture,

            Formatting = Formatting.None
        };

        public static string Serialize(object value)
        {
            if (value == null)
            {
                return "null";
            }

            return JsonConvert.SerializeObject(value, Settings);
        }

        public static T Deserialize<T>(string json) where T : class
        {
            if (string.IsNullOrEmpty(json))
            {
                return null;
            }

            return JsonConvert.DeserializeObject<T>(json, Settings);
        }

        /// <summary>
        /// 不抛异常的反序列化。网络层用它：一个坏包不该让整个服务端挂掉。
        /// </summary>
        public static bool TryDeserialize<T>(string json, out T value, out string error) where T : class
        {
            value = null;
            error = null;

            if (string.IsNullOrEmpty(json))
            {
                error = "请求体为空";
                return false;
            }

            try
            {
                value = JsonConvert.DeserializeObject<T>(json, Settings);
            }
            catch (Exception e)
            {
                error = e.Message;
                if (LogWarning != null)
                {
                    LogWarning("JSON 解析失败：" + e.Message);
                }

                return false;
            }

            if (value == null)
            {
                error = "解析结果为空（可能是 null 字面量）";
                return false;
            }

            return true;
        }
    }
}
