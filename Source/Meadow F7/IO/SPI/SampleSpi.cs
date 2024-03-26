using Meadow;
using Meadow.Hardware;
using Meadow.Units;
using System;

namespace SPI;

public class SampleSpi : ISpiCommunications
{
    public IDigitalOutputPort ChipSelect => throw new NotImplementedException();

    public ISpiBus Bus => throw new NotImplementedException();

    public Frequency BusSpeed
    {
        get => throw new NotImplementedException();
        set => throw new NotImplementedException();
    }

    public SpiClockConfiguration.Mode BusMode
    {
        get => throw new NotImplementedException();
        set => throw new NotImplementedException();
    }

    Frequency ISpiCommunications.BusSpeed
    {
        get => throw new NotImplementedException();
        set => throw new NotImplementedException();
    }

    public SampleSpi(ISpiBus bus, IDigitalOutputPort chipSelect)
    {
    }

    public void Exchange(Span<byte> writeBuffer, Span<byte> readBuffer, DuplexType duplex = DuplexType.Half)
    {
        throw new NotImplementedException();
    }

    public void Read(Span<byte> readBuffer)
    {
        throw new NotImplementedException();
    }

    public void ReadRegister(byte address, Span<byte> readBuffer)
    {
        throw new NotImplementedException();
    }

    public byte ReadRegister(byte address)
    {
        throw new NotImplementedException();
    }

    public ushort ReadRegisterAsUShort(byte address, ByteOrder order = ByteOrder.LittleEndian)
    {
        throw new NotImplementedException();
    }

    public void Write(byte value)
    {
        throw new NotImplementedException();
    }

    public void Write(Span<byte> writeBuffer)
    {
        throw new NotImplementedException();
    }

    public void WriteRegister(byte address, byte value)
    {
        throw new NotImplementedException();
    }

    public void WriteRegister(byte address, Span<byte> writeBuffer, ByteOrder order = ByteOrder.LittleEndian)
    {
        throw new NotImplementedException();
    }

    public void WriteRegister(byte address, ushort value, ByteOrder order = ByteOrder.LittleEndian)
    {
        throw new NotImplementedException();
    }

    public void WriteRegister(byte address, uint value, ByteOrder order = ByteOrder.LittleEndian)
    {
        throw new NotImplementedException();
    }

    public void WriteRegister(byte address, ulong value, ByteOrder order = ByteOrder.LittleEndian)
    {
        throw new NotImplementedException();
    }
}