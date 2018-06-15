﻿using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace NNGNET.Native
{
    public enum SocketOption
    {
        SocketName,

        Raw,

        Protocol,

        ProtocolName,

        Peer,

        PeerName,

        ReceiveBuffer,

        SendBuffer,

        ReceiveFileDescriptor,

        SendFileDescriptor,

        ReceiveTimeout,

        SendTimeout,

        LocalAddress,

        RemoteAddress,

        Url,

        MaxTTL,

        ReceiveMaximumSize,

        ReconnectTimeMin,

        ReconnectTimeMax,

        TcpNoDelay,

        TcpKeepAlive,

        TlsConfig,

        TlsAuthMode,

        TlsCertKeyFile,

        TlsCaFile,

        TlsServerName,

        TlsVerified,
    }

    /// <summary>
    ///     Name of options as used in nng
    /// </summary>
    [SuppressMessage("ReSharper", "InconsistentNaming", Justification = "Using native names of library")]
    public static class OptionNames
    {
        private static readonly Dictionary<SocketOption, string> _nameByEnumDictionary = new Dictionary<SocketOption, string>
        {
            {SocketOption.SocketName, NNG_OPT_SOCKNAME},
            {SocketOption.Raw, NNG_OPT_RAW},
            {SocketOption.Protocol, NNG_OPT_PROTO},
            {SocketOption.ProtocolName, NNG_OPT_PROTONAME},
            {SocketOption.Peer, NNG_OPT_PEER},
            {SocketOption.PeerName, NNG_OPT_PEERNAME},
            {SocketOption.ReceiveBuffer, NNG_OPT_RECVBUF},
            {SocketOption.SendBuffer, NNG_OPT_SENDBUF},
            {SocketOption.ReceiveFileDescriptor, NNG_OPT_RECVFD},
            {SocketOption.SendFileDescriptor, NNG_OPT_SENDFD},
            {SocketOption.ReceiveTimeout, NNG_OPT_RECVTIMEO},
            {SocketOption.SendTimeout, NNG_OPT_SENDTIMEO},
            {SocketOption.LocalAddress, NNG_OPT_LOCADDR},
            {SocketOption.RemoteAddress, NNG_OPT_REMADDR},
            {SocketOption.Url, NNG_OPT_URL},
            {SocketOption.MaxTTL, NNG_OPT_MAXTTL},
            {SocketOption.ReceiveMaximumSize, NNG_OPT_RECVMAXSZ},
            {SocketOption.ReconnectTimeMin, NNG_OPT_RECONNMINT},
            {SocketOption.ReconnectTimeMax, NNG_OPT_RECONNMAXT},
            {SocketOption.TcpNoDelay, NNG_OPT_TCP_NODELAY},
            {SocketOption.TcpKeepAlive, NNG_OPT_TCP_KEEPALIVE},
            {SocketOption.TlsConfig, NNG_OPT_TLS_CONFIG},
            {SocketOption.TlsAuthMode, NNG_OPT_TLS_AUTH_MODE},
            {SocketOption.TlsCertKeyFile, NNG_OPT_TLS_CERT_KEY_FILE},
            {SocketOption.TlsCaFile, NNG_OPT_TLS_CA_FILE},
            {SocketOption.TlsServerName, NNG_OPT_TLS_SERVER_NAME},
            {SocketOption.TlsVerified, NNG_OPT_TLS_VERIFIED},
        };

        private static readonly Dictionary<string, SocketOption> _enumByNameDictionary = new Dictionary<string, SocketOption>();

        /// <summary>
        ///     Initializes the <see cref="OptionNames"/> class.
        /// </summary>
        static OptionNames()
        {
            foreach (var kvp in _nameByEnumDictionary)
            {
                _enumByNameDictionary.Add(kvp.Value, kvp.Key);
            }
        }

        /// <summary>
        ///     Gets the option name specified by the given <paramref name="option"/>.
        /// </summary>
        /// <param name="option">The option.</param>
        /// <returns>
        ///     A string with the option name, which can be used for interfacing the native API.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        ///     <paramref name="option"/> is null.
        /// </exception>
        /// <exception cref="KeyNotFoundException">
        ///     The property is retrieved and <paramref name="option"/> does not exist in the collection.
        /// </exception>
        public static string GetNameByEnum(SocketOption option)
        {
            return _nameByEnumDictionary[option];
        }

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