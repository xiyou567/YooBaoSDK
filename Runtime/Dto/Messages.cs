using System;
using System.Collections.Generic;

namespace YooBao.SDK
{
    // ============================================================
    // GET /v1/health
    // ============================================================

    [Serializable]
    public class HealthResponse
    {
        public string app;
        public int protocolVersion;
        public long uptimeMs;
        public int plugins;
    }

    // ============================================================
    // POST /v1/handshake
    // ============================================================

    [Serializable]
    public class HandshakeRequest
    {
        public int protocolVersion;
        public PluginManifest plugin;
    }

    [Serializable]
    public class HandshakeResponse
    {
        public int protocolVersion;
        public string server;
        public string sessionId;

        /// <summary>
        /// 采样间隔由服务端下发，而不是 Plugin 写死——
        /// 这样调参不用重新编译游戏。
        /// </summary>
        public int pollIntervalMs;

        public int heartbeatIntervalMs;
        public int maxBatchSize;

        /// <summary>
        /// 说话预算。Plugin 只上报观察，**说话决策权完全在 Client**——
        /// 这是防烧钱的核心红线。
        /// </summary>
        public SpeakPolicy speakPolicy;
    }

    [Serializable]
    public class SpeakPolicy
    {
        public int minIntervalMs;
        public int maxPerMinute;
        public int maxPerRun;
    }

    // ============================================================
    // POST /v1/events
    // ============================================================

    [Serializable]
    public class EventBatchRequest
    {
        public string sessionId;
        public long pluginSeq;
        public long sentAtMs;
        public List<GameEvent> events;
    }

    [Serializable]
    public class EventBatchResponse
    {
        public int accepted;
        public long ackedSeq;
        public long serverTimeMs;
        public Directives directives;
    }

    /// <summary>
    /// 服务端下发给 Plugin 的指令。
    /// 这是 Client → Plugin 方向的唯一通道：Client 是 HTTP 服务端，没法主动发起请求，
    /// 但可以把想说的话塞在 Plugin 那次 POST 的响应里。所以不需要 WebSocket。
    /// </summary>
    [Serializable]
    public class Directives
    {
        public int pollIntervalMs;
        public bool requestSnapshot;
        public bool mute;
    }

    // ============================================================
    // POST /v1/snapshot
    // ============================================================

    [Serializable]
    public class SnapshotRequest
    {
        public string sessionId;
        public long sentAtMs;
        public GameSnapshot state;
    }

    [Serializable]
    public class SnapshotResponse
    {
        public bool ok;
        public Directives directives;
    }

    // ============================================================
    // POST /v1/goodbye
    // ============================================================

    [Serializable]
    public class GoodbyeRequest
    {
        public string sessionId;
        public string reason;
    }

    // ============================================================
    // 统一错误信封
    // ============================================================

    [Serializable]
    public class ErrorEnvelope
    {
        public ErrorBody error;

        public ErrorEnvelope()
        {
        }

        public ErrorEnvelope(string code, string message, string hint = null)
        {
            error = new ErrorBody { code = code, message = message, hint = hint };
        }
    }

    [Serializable]
    public class ErrorBody
    {
        public string code;
        public string message;
        public string hint;
    }

    public static class ErrorCodes
    {
        public const string BadJson = "BAD_JSON";
        public const string BadRequest = "BAD_REQUEST";
        public const string Unauthorized = "UNAUTHORIZED";
        public const string ForbiddenOrigin = "FORBIDDEN_ORIGIN";
        public const string NotFound = "NOT_FOUND";
        public const string ProtocolVersionMismatch = "PROTOCOL_VERSION_MISMATCH";
        public const string StaleSession = "STALE_SESSION";
        public const string PayloadTooLarge = "PAYLOAD_TOO_LARGE";
        public const string RateLimited = "RATE_LIMITED";
        public const string Internal = "INTERNAL";
    }
}
