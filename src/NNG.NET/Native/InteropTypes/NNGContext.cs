using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace NNGNET.Native.InteropTypes
{
    [SuppressMessage("ReSharper", "InconsistentNaming", Justification = "Using native name")]
    [StructLayout(LayoutKind.Sequential)]
    public readonly struct NNGContext : IEquatable<NNGContext>, IDisposable
    {
        public readonly uint Id;

        /// <inheritdoc />
        public bool Equals(NNGContext other)
        {
            return Id == other.Id;
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (obj is null)
            {
                return false;
            }

            return obj is NNGContext context && Equals(context);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return (int) Id;
        }

        public static bool operator ==(NNGContext left, NNGContext right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(NNGContext left, NNGContext right)
        {
            return !left.Equals(right);
        }

        public static NNGContext Open(NNGSocket socket) => NNG.OpenContext(socket);

        /// <inheritdoc />
        public void Dispose()
        {
            NNG.CloseContext(this);
        }

        public int GetId()
        {
            return NNG.GetContextId(this);
        }

        public void Receive(NNGAIO aio)
        {
            NNG.ReceiveContext(this, aio);
        }

        public void Send(NNGAIO aio)
        {
            NNG.SendContext(this, aio);
        }

        public Span<byte> GetContextOption(string optionName)
        {
            return NNG.GetContextOption(this, optionName);
        }

        public bool GetContextOptionBool(string optionName)
        {
            return NNG.GetContextOptionBool(this, optionName);
        }

        public int GetContextOptionInt32(string optionName)
        {
            return NNG.GetContextOptionInt32(this, optionName);
        }

        public TimeSpan GetContextOptionTimeSpan(string optionName)
        {
            return NNG.GetContextOptionTimeSpan(this, optionName);
        }

        public IntPtr GetContextOptionPointer(string optionName)
        {
            return NNG.GetContextOptionPointer(this, optionName);
        }

        public void SetContextOption(string optionName, Span<byte> value)
        {
            NNG.SetContextOption(this, optionName, value);
        }

        public void SetContextOption(string optionName, bool value)
        {
            NNG.SetContextOption(this, optionName, value);
        }

        public void SetContextOption(string optionName, int value)
        {
            NNG.SetContextOption(this, optionName, value);
        }

        public void SetContextOption(string optionName, TimeSpan value)
        {
            NNG.SetContextOption(this, optionName, value);
        }

        public void SetContextOption(string optionName, UIntPtr value)
        {
            NNG.SetContextOption(this, optionName, value);
        }
    }
}