using System;
using System.Collections.Generic;

namespace YooBao.SDK
{
    /// <summary>
    /// 一条游戏事件。
    ///
    /// 负载为什么是「两个平行字典」而不是 Dictionary&lt;string, object&gt;：
    /// object 作为泛型实参在 AOT/IL2CPP 下需要运行期类型分发，是已知的坑。
    /// 拆成 numData / strData 之后，契约里再没有 object，也不需要多态。
    /// </summary>
    [Serializable]
    public class GameEvent
    {
        /// <summary>Plugin 侧单调递增。Client 靠它去重、断线补发，同一 session 内不允许回退。</summary>
        public long seq;

        /// <summary>取值见 <see cref="EventNames"/>。</summary>
        public string name;

        public long tsMs;

        /// <summary>数值负载，可以为 null（序列化时会被省略）。</summary>
        public Dictionary<string, double> numData;

        /// <summary>字符串负载，可以为 null。</summary>
        public Dictionary<string, string> strData;

        public GameEvent()
        {
        }

        public GameEvent(string name, long seq, long tsMs)
        {
            this.name = name;
            this.seq = seq;
            this.tsMs = tsMs;
        }

        public GameEvent Num(string key, double value)
        {
            if (numData == null)
            {
                numData = new Dictionary<string, double>();
            }

            numData[key] = value;
            return this;
        }

        public GameEvent Str(string key, string value)
        {
            if (strData == null)
            {
                strData = new Dictionary<string, string>();
            }

            strData[key] = value;
            return this;
        }

        public double GetNum(string key, double fallback = 0d)
        {
            double value;
            if (numData != null && numData.TryGetValue(key, out value))
            {
                return value;
            }

            return fallback;
        }

        public string GetStr(string key, string fallback = null)
        {
            string value;
            if (strData != null && strData.TryGetValue(key, out value))
            {
                return value;
            }

            return fallback;
        }

        public override string ToString()
        {
            return "#" + seq + " " + name;
        }
    }
}
