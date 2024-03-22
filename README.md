<img src="Design/wildernesslabs-meadow-samples-banner.jpg"  alt="Meadow.ProjectLab, C#, iot" style="margin-bottom:10px" />

# Meadow.Samples

Sample applications for Meadow platforms, libraries and peripherals.

## Juego

[![Juego.Samples](/Design/wildernesslabs-meadow-juego-samples.jpg)](/Source/Juego.Samples/)

A collection of samples for the Wilderness Labs [Juego IoT Accelerator](https://github.com/WildernessLabs/Juego), an Open-source, Meadow-powered, multigame handheld console with DPads, speakers and a colored display.

### Juego Samples

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

## Meadow.Core

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

## Single-Board-Computers

[![Meadow.SBCs.Samples](Design/wildernesslabs-meadow-sbcs-samples.jpg)](/Source/Meadow.SBCs.Samples/)

Public project samples for [Single-Board-Computers (SBCs)](https://developer.wildernesslabs.co/Meadow/Getting_Started/SBCs/). Click on any of these sample project to learn how they work and run them on a Raspberry Pi, SeeedStudio reTerminal and/or Nvidia Jetson Nano.

### Single-Board-Computer Samples

<table>
    <tr>
        <td>
            <img src="Design/wildernesslabs-meadow-linux-blinky.jpg"/><br/>
            Getting started with a Blinky app on a Raspberry Pi</br>
            <a href="Source/Blinky/">Source Code</a>
        </td>
        <td>
            <img src="Design/wildernesslabs-meadow-linux-character-display.jpg"/><br/>
            Using a 20x4 LCD Character Display on a Raspberry Pi</br>
            <a href="Source/CharacterDisplaySample/">Source Code</a>
        </td>
        <td>
            <img src="Design/wildernesslabs-meadow-linux-st7789.jpg"/><br/>
            Using MicroGraphics on a ST7789 display on a Raspberry Pi</br>
            <a href="Source/ST7789_Sample/">Source Code</a>
        </td>
    </tr>
    <tr>
        <td>
            <img src="Design/wildernesslabs-meadow-linux-wifiweather.jpg"/><br/>
            Build a weather widget using MicroLayout on a Raspberry Pi</br>
            <a href="Source/WifiWeather/">Source Code</a>
        </td>
        <td>
            <img src="Design/template-blue.png"/><br/>
            Using a BME280 atmospheric sensor on a Raspberry Pi</br>
            <a href="Source/Bme280_Sample/">Source Code</a>
        </td>
        <td>
            <img src="Design/template-orange.png"/><br/>
            Working with push button events on a Rapsberry Pi</br>
            <a href="Source/Linux/WifiWeather/">Source Code</a>
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

## Meadow.Cloud

[![Meadow.Cloud.Samples](Design/wildernesslabs-meadow-cloud-samples.jpg)](/Source/Meadow.Cloud.Samples/)

Meadow.Cloud provides secure, Over-the-Air (OtA) updates, which enable you to push a new version of a Meadow application to a device in the field over the network. Before running any of the project samples below, make sure to go through the [Meadow.Cloud basics](https://developer.wildernesslabs.co/Meadow/Meadow.Cloud/) guides showing you how to provision your device, how to download and apply an update from Meadow, and make/publish a package. 

## Meadow.Cloud Samples

<table>
    </tr>
        <tr>
        <td>
            <img src="Design/wildernesslabs-meadow-cloud-begginer.jpg" alt="iot, dotnet, meadow, led, dice, buttons"/><br/>
            Send an over-the-air update to change colors on an RGB LED</br>
            <a href="Source/RgbLedUpdateSample/">Source Code</a>
        </td>
        <td>
            <img src="Design/wildernesslabs-meadow-cloud-log.jpg" alt="iot, dotnet, meadow, game, memory"/><br/>
            Send diagnostics logs from Meadow to Meadow.Cloud</br>
            <a href="Source/CloudLogging/">Source Code</a>
        </td>
        <td>
            <img src="Design/wildernesslabs-meadow-cloud-health-metrics.jpg" alt="iot, dotnet, meadow, morse-code, graphics"/><br/>
            Check your Meadow's Health Metrics on Meadow.Cloud</br>
            <a href="Source/HealthMetricsMonitoring/">Source Code</a>
        </td>
    </tr>
    <tr>
        <td>
            <a href="Source/Meadow.Cloud_Logging/"><img src="Design/wildernesslabs-meadow-cloud-projectlab-logging.jpg"/></a><br/>
            Send environmental data to Meadow.Cloud using Log Event</br>
            <a href="Source/Meadow.Cloud_Logging/">Source Code</a>
        </td>
        <td>
            <a href="Source/Meadow.Cloud_OTA/"><img src="Design/wildernesslabs-meadow-cloud-projectlab-ota-update.jpg"/></a><br/>
            Use Meadow.Cloud to push Over-the-air Updates<br/>
            <a href="Source/Meadow.Cloud_OTA/">Source Code</a>
        </td>
        <!--<td>
            <a href="Source/Meadow.Cloud_Client/"><img src="Design/wildernesslabs-projectlab-meadow-cloud-client.png"/></a><br/>
            Get log event data from Meadow.Cloud using its client API<br/>
            <a href="Source/Meadow.Cloud_Client/">Source Code</a>
        </td>-->
        <td>
            <a href="Source/Meadow.Cloud_Command/"><img src="Design/wildernesslabs-meadow-cloud-projectlab-relay-command.png"/></a><br/>
            Use Meadow.Cloud commands to control a four channel relay</br>
            <a href="Source/Meadow.Cloud_Command/">Source Code</a>
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

[![Meadow.Project.Samples](Source/Meadow.Project.Samples/Design/wildernesslabs-meadow-project-samples.jpg)](/Source/Meadow.Project.Samples/)

---

[![Meadow.Desktop.Samples](Source/Meadow.Desktop.Samples/Design/wildernesslabs-meadow-desktop-samples.jpg)](/Source/Meadow.Desktop.Samples/)

---

[![Meadow.ProjectLab.Samples](Source/Meadow.ProjectLab.Samples/Design/wildernesslabs-meadow-projectlab-samples.jpg)](/Source/Meadow.ProjectLab.Samples/)