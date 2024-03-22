<img src="Design/wildernesslabs-meadow-samples-banner.jpg"  alt="Meadow.ProjectLab, C#, iot" style="margin-bottom:10px" />

# Meadow.Samples

Sample applications for Meadow platforms, libraries and peripherals.

## Juego

[![Juego.Samples](/Design/wildernesslabs-meadow-juego-samples.jpg)](/Source/Juego.Samples/)

A collection of samples for the Wilderness Labs [Juego IoT Accelerator](https://github.com/WildernessLabs/Juego), an Open-source, Meadow-powered, multigame handheld console with DPads, speakers and a colored display.

### Contents
* [Getting Started](#getting-started)
* [Project Samples](#project-samples)
* [Support](#support)

### Getting Started

Before running any of the Juego samples below, make sure your board is running the latest OS version. We recomment going through our [Meadow OS Deployment](https://developer.wildernesslabs.co/Meadow/Getting_Started/Deploying_Meadow.OS/) guide. 

### Project Samples

<table>
    <tr>
        <td>
            <img src="Design/wildernesslabs-meadow-juego-getting-started.jpg" alt="juego, dotnet, meadow, dice, buttons"/><br/>
            Getting started with Juego</br>
            <a href="https://github.com/WildernessLabs/Juego/tree/main/Source/Juego_Demo">Source Code</a>
        </td>
        <td>
            <img src="Design/wildernesslabs-meadow-juego-froggit.jpg" alt="dotnet, meadow, juego, graphics, 2D, frogger"/><br/>
            Run/play frogger on a Juego</br>
            <a href="Source/Froggit/">Source Code</a>
        </td>
        <td>
            <img src="Design/wildernesslabs-meadow-juego-tetraminos.jpg" alt="dotnet, meadow, juego, graphics, 2D, tetris"/><br/>
            Run/play Tetraminoes on a Juego<br/>
            <a href="Source/Tetraminoes/">Source Code</a>
        </td>
    </tr>
    <tr>
        <td>
            <img src="Design/wildernesslabs-meadow-juego-span-four.jpg" alt="dotnet, meadow, juego, graphics, 2D, span 4"/><br/>
            Run/play a 2-player Span4</br>
            <a href="Source/Span4/">Source Code</a>
        </td>
        <td>
            <img src="Design/wildernesslabs-meadow-juego-eyeball.jpg" alt="dotnet, meadow, juego, graphics, 2D, eyeball"/><br/>
            Halloween Eye Ball animation</br>
            <a href="Source/Eyeball/">Source Code</a>
        </td>
        <td>
            <img src="Design/wildernesslabs-meadow-juego-snake.jpg" alt="dotnet, meadow, juego, graphics, 2D, snake"/><br/>
            Run/play snake on a Juego</br>
            <a href="Source/Snake/">Source Code</a>
        </td> 
    </tr>
    <tr>
        <td>
            <p>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</p>
        </td>
        <td>
            <p>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</p>
        </td>
        <td>
            <p>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</p>
        </td>
    </tr>
</table>

## Meadow.Core Samples

[![Meadow.Core.Samples](Design/wildernesslabs-meadow-core-samples.jpg)](/Source/Meadow.Core.Samples/)

* [Blinky (C#, F#, VB.NET)](./Source/Blinky) 
* [Bluetooth](./Source/Bluetooth/Bluetooth_Basics/)
* [Board Specific Samples](./Source/Board_Specific_Samples)
    * [F7 Feather V2](./Source/Board_Specific_Samples/F7_Micro)
    * [Core Computer Module](./Source/Board_Specific_Samples/CoreComputeBreakout)
* [IO Samples](./Source/IO)
    * [Analog Input Port](./Source/IO/AnalogInputPort)
    * [Bidirectional Port](./Source/IO/BiDirectonalPort)
    * [Counter](./Source/IO/Counter)
    * [Digital Input Port](./Source/IO/DigitalInputPort)
    * [Digital Input Port (Observable)](./Source/IO/DigitalInputPort_IObservable)
    * [GPIO Interrogation](./Source/IO/GpioInterrogation)
    * [Hello LED](./Source/IO/Hello_LED)
    * [Hello LED (F#)](./Source/IO/HelloLED_F%23)
    * [I2C](./Source/IO/I2C)
    * [PWM](./Source/IO/PWM)
    * [Serial Listener](./Source/IO/SerialListener)
    * [Serial Message Port](./Source/IO/SerialMessagePort)
    * [Serial Port](./Source/IO/SerialPort)
    * [Serial Port Echo](./Source/IO/SerialPort_Echo)
    * [SPI](./Source/IO/SPI)
* [Json Basics](./Source/Json_Basics)
* [Network](./Source/Network)
    * [Antenna Switching](./Source/Network/Antenna_Switching)
    * [HttpListener Basics](./Source/Network/HttpListener_Basics)
    * [WIFI Basics](./Source/Network/WiFi_Basics)
* [OS](./Source/OS)
    * [Battery Level](./Source/OS/BatteryLevel)
    * [BeginInvokeOnMainThread](./Source/OS/BeginInvokeOnMainThread)
    * [Config Files](./Source/OS/Config_Files)
    * [File System Basics](./Source/OS/FileSystem_Basics)
    * [Logging](./Source/OS/Logging)
    * [MCU Temperature](./Source/OS/McuTemp)
    * [Power Manager](./Source/OS/Power_Manager)
    * [Real Time Clock](./Source/OS/RealTimeClock)
    * [SDCard](./Source/OS/SDCard/CS)
    * [SQLite](./Source/OS/SQLite)
    * [Threading](./Source/OS/Threading)
    * [Threading.Tasks](./Source/OS/Threading.Tasks)
    * [Update](./Source/OS/Update)
    * [Watchdog](./Source/OS/Watchdog)
* [Walking Digital Outputs](./Source/Utilities/Walking_DigitalOutputs)


For peripheral samples, see the [Meadow.Foundation repo](https://github.com/wildernesslabs/Meadow.Foundation).

For project samples, see the [Meadow projects on Hackster.io](https://www.hackster.io/WildernessLabs/projects?sort=published).

---

[![Meadow.SBCs.Samples](Source/Meadow.SBCs.Samples/Design/wildernesslabs-meadow-sbcs-samples.jpg)](/Source/Meadow.SBCs.Samples/)

---

[![Meadow.Cloud.Samples](Source/Meadow.Cloud.Samples/Design/wildernesslabs-meadow-cloud-samples.jpg)](/Source/Meadow.Cloud.Samples/)

---

[![Meadow.Project.Samples](Source/Meadow.Project.Samples/Design/wildernesslabs-meadow-project-samples.jpg)](/Source/Meadow.Project.Samples/)

---

[![Meadow.Desktop.Samples](Source/Meadow.Desktop.Samples/Design/wildernesslabs-meadow-desktop-samples.jpg)](/Source/Meadow.Desktop.Samples/)

---

[![Meadow.ProjectLab.Samples](Source/Meadow.ProjectLab.Samples/Design/wildernesslabs-meadow-projectlab-samples.jpg)](/Source/Meadow.ProjectLab.Samples/)