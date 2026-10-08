using System;

namespace YooBao.SDK
{
    /// <summary>
    /// 某一瞬间的完整游戏状态。
    ///
    /// 事件回答「刚刚发生了什么」，快照回答「现在整体什么情况」——
    /// LLM 需要两者才能说对话。
    ///
    /// 约定：所有字段都必须能从游戏里零反射地读到。读不到就留默认值，
    /// 绝不允许因为某个字段读不到而让整条链路失败。
    /// </summary>
    [Serializable]
    public class GameSnapshot
    {
        /// <summary>当前场景名。用于判断是否在对局中。</summary>
        public string scene;

        /// <summary>是否处于一局进行中的状态（已进入对局场景且玩家对象存在）。</summary>
        public bool inRun;

        public float hp;
        public float maxHp;
        public float hpRegen;

        public float exp;
        public int money;
        public int level;
        public int needExp;
        public int killCount;

        /// <summary>从 0 开始的波次下标。展示给用户时记得 +1。</summary>
        public int waveIndex;

        /// <summary>总波数。为 0 表示关卡数据还没加载好，此时不要产出波次事件。</summary>
        public int totalWaves;

        public bool waveActive;
        public float waveCountdown;

        public bool shopping;
        public bool choosingUpgrade;
        public bool dead;

        /// <summary>游戏是否被暂停（商店 / 升级 / 结算面板都会把 Time.timeScale 置 0）。</summary>
        public bool paused;

        /// <summary>血量比例，maxHp 为 0 时返回 0 而不是 NaN。</summary>
        public double HpRatio
        {
            get { return maxHp > 0f ? (double)hp / maxHp : 0d; }
        }

        /// <summary>展示给用户看的波次号（从 1 开始）。</summary>
        public int WaveNumber
        {
            get { return waveIndex + 1; }
        }
    }
}
