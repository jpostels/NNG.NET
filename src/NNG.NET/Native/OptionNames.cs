using System.Diagnostics.CodeAnalysis;

namespace NNG.Native
{
    /// <summary>
    ///     Name of options as used in nng
    /// </summary>
    [SuppressMessage("ReSharper", "InconsistentNaming", Justification = "Using native names of library")]
    public static class OptionNames
    {
        public const string NNG_OPT_SOCKNAME = "socket-name";

        public const string NNG_OPT_RAW = "raw";

        public const string NNG_OPT_PROTO = "protocol";

        public const string NNG_OPT_PROTONAME = "protocol-name";

        public const string NNG_OPT_PEER = "peer";

        public const string NNG_OPT_PEERNAME = "peer-name";

        public const string NNG_OPT_RECVBUF = "recv-buffer";

        public const string NNG_OPT_SENDBUF = "send-buffer";

        public const string NNG_OPT_RECVFD = "recv-fd";

        public const string NNG_OPT_SENDFD = "send-fd";

        public const string NNG_OPT_RECVTIMEO = "recv-timeout";

        public const string NNG_OPT_SENDTIMEO = "send-timeout";

        public const string NNG_OPT_LOCADDR = "local-address";

        public const string NNG_OPT_REMADDR = "remote-address";

        public const string NNG_OPT_URL = "url";

        public const string NNG_OPT_MAXTTL = "ttl-max";

        public const string NNG_OPT_RECVMAXSZ = "recv-size-max";

        public const string NNG_OPT_RECONNMINT = "reconnect-time-min";

        public const string NNG_OPT_RECONNMAXT = "reconnect-time-max";

        #region TCP

        public const string NNG_OPT_TCP_NODELAY = "tcp-nodelay";

        public const string NNG_OPT_TCP_KEEPALIVE = "tcp-keepalive";

        #endregion

        #region TLS

        /// <summary>
        /// NNG_OPT_TLS_CONFIG is a pointer to an nng_tls_config object.  Generally
        /// this can used with endpoints, although once an endpoint is started, or
        /// once a configuration is used, the value becomes read-only. Note that
        /// when configuring the object, a hold is placed on the TLS configuration,
        /// using a reference count.  When retrieving the object, no such hold is
        /// placed, and so the caller must take care not to use the associated object
        /// after the endpoint it is associated with is closed.
        /// </summary>
        public const string NNG_OPT_TLS_CONFIG = "tls-config";

        /// <summary>
        /// NNG_OPT_TLS_AUTH_MODE is a write-only integer (int) option that specifies
        /// whether peer authentication is needed.  The option can take one of the
        /// values of NNG_TLS_AUTH_MODE_NONE, NNG_TLS_AUTH_MODE_OPTIONAL, or
        /// NNG_TLS_AUTH_MODE_REQUIRED.  The default is typically NNG_TLS_AUTH_MODE_NONE
        /// for listeners, and NNG_TLS_AUTH_MODE_REQUIRED for dialers. If set to
        /// REQUIRED, then connections will be rejected if the peer cannot be verified.
        /// If set to OPTIONAL, then a verification step takes place, but the connection
        /// is still permitted.  (The result can be checked with NNG_OPT_TLS_VERIFIED).
        /// </summary>
        public const string NNG_OPT_TLS_AUTH_MODE = "tls-authmode";

        /// <summary>
        /// NNG_OPT_TLS_CERT_KEY_FILE names a single file that contains a certificate
        /// and key identifying the endpoint.  This is a write-only value.  This can be
        /// set multiple times for times for different keys/certs corresponding to
        /// different algorithms on listeners, whereas dialers only support one.  The
        /// file must contain both cert and key as PEM blocks, and the key must
        /// not be encrypted.  (If more flexibility is needed, use the TLS configuration
        /// directly, via NNG_OPT_TLS_CONFIG.)
        /// </summary>
        public const string NNG_OPT_TLS_CERT_KEY_FILE = "tls-cert-key-file";

        /// <summary>
        /// NNG_OPT_TLS_CA_FILE names a single file that contains certificate(s) for a
        /// CA, and optionally CRLs, which are used to validate the peer's certificate.
        /// This is a write-only value, but multiple CAs can be loaded by setting this
        /// multiple times.
        /// </summary>
        public const string NNG_OPT_TLS_CA_FILE = "tls-ca-file";

        /// <summary>
        /// NNG_OPT_TLS_SERVER_NAME is a write-only string that can typically be
        /// set on dialers to check the CN of the server for a match.  This
        /// can also affect SNI (server name indication).  It usually has no effect
        /// on listeners.
        /// </summary>
        public const string NNG_OPT_TLS_SERVER_NAME = "tls-server-name";

        /// <summary>
        /// NNG_OPT_TLS_VERIFIED returns a single integer, indicating whether the peer
        /// has been verified (1) or not (0).  Typically this is read-only, and only
        /// available for pipes. This option may return incorrect results if peer
        /// authentication is disabled with `NNG_TLS_AUTH_MODE_NONE`.
        /// </summary>
        public const string NNG_OPT_TLS_VERIFIED = "tls-verified";

        #endregion

        // TODO: priorities, socket names, ipv4only
    }
}