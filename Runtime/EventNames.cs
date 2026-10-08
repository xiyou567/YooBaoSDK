namespace YooBao.SDK
{
    /// <summary>
    /// 事件名常量。Plugin 发什么、Client 认什么，全走这里，不写魔法字符串。
    /// 新增事件时只加常量，不要改已有常量的字面值（会破坏兼容性）。
    /// </summary>
    public static class EventNames
    {
        // ---- 对局生命周期 ----
        public const string GameStarted = "GAME_STARTED";
        public const string GameOver = "GAME_OVER";

        // ---- 玩家 ----
        public const string PlayerDamaged = "PLAYER_DAMAGED";
        public const string PlayerHealed = "PLAYER_HEALED";
        public const string PlayerLowHp = "PLAYER_LOW_HP";
        public const string PlayerLevelUp = "PLAYER_LEVEL_UP";
        public const string PlayerDied = "PLAYER_DIED";

        // ---- 战斗 / 资源 ----
        public const string EnemyKilled = "ENEMY_KILLED";
        public const string MoneyChanged = "MONEY_CHANGED";

        // ---- 波次 ----
        public const string WaveStarted = "WAVE_STARTED";
        public const string WaveCompleted = "WAVE_COMPLETED";

        // ---- 商店 ----
        public const string ShopOpened = "SHOP_OPENED";
        public const string ShopClosed = "SHOP_CLOSED";
        public const string ItemPurchased = "ITEM_PURCHASED";
    }
}
