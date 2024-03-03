Imports System.ComponentModel
Imports System.IO
Imports System.Net.WebRequestMethods
Imports System.Threading
Imports Meadow
Imports Meadow.Devices
Imports Meadow.Foundation
Imports Meadow.Foundation.Leds
Imports Meadow.Gateways.Bluetooth
Imports Meadow.Peripherals.Leds

Public Class MeadowApp
    'Change F7FeatherV2 to F7FeatherV1 for V1.x boards'
    Inherits App(Of F7FeatherV2)

    Dim bleTreeDefinition As Definition
    Dim onOffCharacteristic As CharacteristicBool

    Public Overrides Function Initialize() As Task
        Console.WriteLine("Initialize hardware...")

        ' initialize the bluetooth defnition tree
        Console.WriteLine("Starting the BLE server.")
        bleTreeDefinition = GetDefinition()
        Device.BluetoothAdapter.StartBluetoothServer(bleTreeDefinition)

        ' wire up some notifications on set
        For Each Characteristic In bleTreeDefinition.Services(0).Characteristics
            AddHandler Characteristic.ValueSet,
                Sub(c As ICharacteristic, d As Object)
                    Console.WriteLine($"HEY, I JUST GOT THIS BLE DATA for Characteristic '{c.Name}' of type {d.GetType().Name}: {d}")
                End Sub
        Next

        ' addressing individual characteristics
        AddHandler onOffCharacteristic.ValueSet,
            Sub(c, d)
                Console.WriteLine($"{c.Name}: {d}")
            End Sub

        Console.WriteLine("Hardware initialized.")

        Return Task.CompletedTask
    End Function

    Protected Function GetDefinition() As Definition
        onOffCharacteristic = New CharacteristicBool(
            "On_Off",
            Guid.NewGuid().ToString(),
            CharacteristicPermission.Read Or CharacteristicPermission.Write,
            CharacteristicProperty.Read Or CharacteristicProperty.Write)

        Dim Service = New Service(
                 "ServiceA",
                 253,
                 onOffCharacteristic,
                 New CharacteristicBool(
                     "My Bool",
                     uuid:="017e99d6-8a61-11eb-8dcd-0242ac1300aa",
                     permissions:=CharacteristicPermission.Read,
                     properties:=CharacteristicProperty.Read
                     ),
                 New CharacteristicInt32(
                     "My Number",
                     uuid:="017e99d6-8a61-11eb-8dcd-0242ac1300bb",
                     permissions:=CharacteristicPermission.Write Or CharacteristicPermission.Read,
                     properties:=CharacteristicProperty.Write Or CharacteristicProperty.Read
                     ),
                 New CharacteristicString(
                     "My Text",
                     uuid:="017e99d6-8a61-11eb-8dcd-0242ac1300cc",
                     maxLength:=20,
                     permissions:=CharacteristicPermission.Write Or CharacteristicPermission.Read,
                     properties:=CharacteristicProperty.Write Or CharacteristicProperty.Read
                 )
            )

        Return New Definition("MY MEADOW F7", Service)
    End Function
End Class