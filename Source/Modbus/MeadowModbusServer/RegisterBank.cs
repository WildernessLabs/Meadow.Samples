using System;

namespace MeadowModbusServer;

internal class RegisterBank
{
    private byte[] registers;

    public RegisterBank(int registerCount)
    {
        registers = new byte[registerCount * 2];
    }

    public void SetRegisters(Registers register, float value)
    {
        var offset = (int)register;

        // all registers are ushorts, so split the incoming data
        var bytes = BitConverter.GetBytes(value);
        Buffer.BlockCopy(bytes, 0, registers, offset * 2, bytes.Length);
    }

    public void SetRegisters(Registers register, int value)
    {
        var offset = (int)register;

        // all registers are ushorts, so split the incoming data
        var bytes = BitConverter.GetBytes(value);
        Buffer.BlockCopy(bytes, 0, registers, offset * 2, bytes.Length);
    }

    public ushort[]? GetRegisters(int startRegister, int count)
    {
        var end = (int)Registers.TotalLength;
        if (startRegister >= end) return null;
        if (end - startRegister < count) return null;

        var result = new ushort[count];

        Buffer.BlockCopy(registers, startRegister * 2, result, 0, count * 2);

        return result;
    }

    public enum Registers : ushort
    {
        Temperature = 0,
        Humidity = 2,
        AirPressure = 4,
        TotalLength = 6
    }
}