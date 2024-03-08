<img src="Design/wildernesslabs-meadow-desktop-samples.jpg" style="margin-bottom:10px" />

# Meadow.Desktop.Samples

Public project samples for [Meadow.Windows](http://developer.wildernesslabs.co/Meadow/Getting_Started/Getting_Started_Meadow.Desktop/Getting_Started_Windows/) and [Meadow.Linux](http://developer.wildernesslabs.co/Meadow/Getting_Started/Getting_Started_Meadow.Desktop/Getting_Started_Linux/). Click on any of these sample project to learn how they work and run them straight from your Windows machine or Linux device.

## Contents
- [Windows](#windows)
    - [Pre-requisites](#pre-requisites)
    - [Meadow.Windows Samples](#meadowwindows-samples)
- [Linux](#linux)
    - [Meadow Linux Samples](#meadowlinux-samples)
    - [Linux embedded pinout diagrams](#linux-embedded-pinout-diagrams)
        - [Raspberry Pi 4](#raspberry-pi-4)
- [Support](#support)

## Windows

### Pre-Requisites

To run these samples, make sure:

1. Your development environment is [properly configured](https://developer.wildernesslabs.co/Meadow/Getting_Started/Getting_Started_Meadow.Desktop/) to run Meadow apps on your dev machine.
2. [Optional] If the sample you wish to run uses a physical peripheral or sensor, add the native library (`libmpsse.dll`) of the FT232H IO Expander depending on your CPU's architecture ([Win32 or x64](/Support%20Files/Windows/FT232H%20Native%20Library/))  to that project and set the **Copy to Output Directory** to `Copy if newer` or `Copy always`

<p align="center">
    <img src="Design/build-action.png" style="width:50%" />
</p>

Also check the pinout to make sure to connect the peripheral or sensor on the right pins:

<p align="center">
    <img src="Design/pinout-ft232h.png" style="width:50%" />
</p>

3. Rebuild and right-click the project and click on **Set as Startup Project** 
4. Click **Debug** to run see a Meadow App running on Windows!

### Meadow.Windows Samples

<table>
    <tr>
        <td>
            <img src="Design/wildernesslabs-meadow-windows-winforms.jpg"/><br/>
            Build HMI Screens using Meadow.WinForms</br>
            <a href="https://www.hackster.io/wilderness-labs/run-meadow-micrographics-on-winforms-directly-from-your-pc-db875b">Hackster</a> | <a href="Source/WinForms/">Source Code</a>
        </td>
        <td>
            <img src="Design/wildernesslabs-meadow-windows-maui.png"/><br/>
            Build hardware apps using Meadow in a MAUI app</br>
            <a href="https://www.hackster.io/wilderness-labs/run-meadow-within-a-maui-windows-application-196d8d">Hackster</a> | <a href="Source/MauiMeadow/">Source Code</a>
        </td>
        <td>
            <img src="Design/wildernesslabs-meadow-windows-avalonia.png"/><br/>
            Build hardware apps using Meadow in an Avalonia app</br>
            <a href="https://www.hackster.io/wilderness-labs/run-meadow-within-an-avalonia-application-68371e">Hackster</a> | <a href="Source/AvaloniaMeadow/">Source Code</a>
        </td>
    </tr>
    <tr>
        <td>
            <img src="Design/wildernesslabs-meadow-windows-blinky.png"/><br/>
            Running Blinky app with an FT232H IO Expander</br>
            <a href="https://www.hackster.io/wilderness-labs/run-meadow-apps-directly-from-your-pc-using-meadow-windows-dab4bf">Hackster</a> | <a href="Source/Blinky/">Source Code</a>
        </td>
        <td>
            <img src="Design/wildernesslabs-meadow-windows-characterdisplay.png"/><br/>
            Using a Character Display with an FT232H IO Expander</br>
            <a href="https://www.hackster.io/wilderness-labs/control-an-lcd-display-with-your-pc-using-meadow-windows-186c6d">Hackster</a> | <a href="Source/CharacterDisplaySample/">Source Code</a>
        </td>
        <td>
            <img src="Design/wildernesslabs-meadow-windows-graphics.png"/><br/>
            Show weather data on a display with an FT232H IO Expander</br>
            <a href="https://www.hackster.io/wilderness-labs/build-this-weather-widget-running-directly-from-your-pc-57c69f">Hackster</a> | <a href="Source/WifiWeather/">Source Code</a>
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

## Support

Having trouble running these samples? 
* File an [issue](https://github.com/WildernessLabs/Meadow.Desktop.Samples/issues) with a repro case to investigate, and/or
* Join our [public Slack](http://slackinvite.wildernesslabs.co/), where we have an awesome community helping, sharing and building amazing things using Meadow.