# Meadow - A Modern IoT Hardware and Software Development Platform

## Abstract

Meadow is a microcontroller and software ecosystem based upon .NET Standard 2.1.  In contrast to traditional embedded development systems, Meadow allows hardware development using modern languages (C#, and F#) with Visual Studio (Windows and Mac) and VS Code (Windows, Mac and Linux) with full access to the .NET framework.

With Meadow, developers are able to leverage their current software development techniques and knowledge to develop hardware products using the latest .NET technologies.  The system is designed to be easy to use and to allow developers to focus on core product development.

In this presentation we will look at the extensive collection of libraries and demonstrate how they can be used to perform a variety of tasks including:

* Getting started with Meadow
* Connect to a sensor to Meadow and record data from the sensor
* Control hardware using Meadow
* Use Meadow to take the sensor readings and control hardware based upon the readings

Finally we will look at the road map and current plans to make the platform even more awesome.

## Presentation

[Developer South Coast 2022](./Meadow-DeveloperSouthCoast2022.pdf)

## Sample applications

Three demonstrations were given during the presentation, the sample code can be found in the following locations:

### [Getting Started](01-GettingStarted/Readme.md)

F# application that cycles through a range of colours using the on board LED.

### [TSL2591 Light Sensor](02-LightSensor/Readme.md)

The first part of this demonstration reads from a single sensor.  The second part of the demonstration reads from multiple sensors.

[TSL2591 light sensor](https://coolcomponents.co.uk/products/tsl2591-high-dynamic-range-digital-light-sensor-stemma-qt?_pos=5&_sid=cb5eb073a&_ss=r) that takes light readings and sends the results to the cloud - [io.adafruit.com](https://io.adafruit.com/).

A [BME280 Temperature, Humidity and Pressure Sensor](https://coolcomponents.co.uk/products/atmospheric-sensor-breakout-bme280?_pos=3&_sid=9ab60170b&_ss=r) is added to the board and both sensors will are read and the data sent to the console.

### Wilderness Labs

Creators of Meadow. Makers of Netduino. We power connected things.

<img src="../SocialMediaIcons/WWWIcon.png" height="32px" width="32px"> Developer Resources: [http://developer.wildernesslabs.co/](http://developer.wildernesslabs.co/)

<img src="../SocialMediaIcons/Twitter/Twitter-circle-blue.png" height="32px" width="32px"> Twitter: [https://twitter.com/wildernesslabs](https://twitter.com/wildernesslabs)

<img src="../SocialMediaIcons/GitHub/GitHub-Mark-32px.png" height="32px" width="32px"> GitHub: [https://github.com/Wildernesslabs](https://github.com/Wildernesslabs)

<img src="../SocialMediaIcons/Hackster/Hackster-Mark_RGB.png" height="32px" width="32px"> Hackster Projects: [https://www.hackster.io/WildernessLabs](https://www.hackster.io/WildernessLabs)

<img src="../SocialMediaIcons/Youtube/youtube_social_icon_white.png" height="32px" width="32px"> YouTube: [https://www.youtube.com/c/WildernessLabs](https://www.youtube.com/c/WildernessLabs)


### About the Speaker

Mark has been developing software since the days of the BBC Micro and has worked on a wide variety of platforms and technologies. He rediscovered his love of electronics after receiving a Netduino board as a present back in 2010.  Mark is currently working on the firmware for the Meadow boards and can be found in the depths of the C and C++ code controlling the STM32 and ESP32 microcontrollers.  He occasionally surfaces to add new features to the Meadow .NET libraries.

<img src="../SocialMediaIcons/Twitter/Twitter-circle-blue.png" height="32px" width="32px"> Twitter: [https://twitter.com/nevynuk](https://twitter.com/nevynuk)

<img src="../SocialMediaIcons/WWWIcon.png" height="32px" width="32px"> Blog: [https://blog.mark-stevens.co.uk/](https://blog.mark-stevens.co.uk/)

<img src="../SocialMediaIcons/Linkedin/LI-Logo.png" height="32px" width="32px"> Linkedin: [https://www.linkedin.com/in/mark-stevens-a188614/](https://www.linkedin.com/in/mark-stevens-a188614/)
