# Developer Developer Developer!

This directory contains the slides and sample code for the Developer Day presentation given online 27th November 2021.

## Abstract

Meadow - A Modern Development Ecosystem for Internet of Things

Embedded computing and Internet of Things (IoT) is heading towards a new epoch with more products being controlled by microcontrollers and often talking to the cloud.  Traditional development in these environments requires the use of C/C++ and specialist tools in order to take a product from concept to final product.

Enter Meadow, a microcontroller and software ecosystem based upon .NET Standard 2.1.  Meadow allows product development using modern languages (C#, and F#) with Visual Studio (Windows and Mac) and VS Code with full access to the .NET framework.

With Meadow, developers are able to leverage their current software development techniques and knowledge to develop hardware products using the latest .NET technologies.  The ecosystem is designed to be easy to use and to allow developers to focus on core product development.

We will learn how to:

* Connect to a sensor to Meadow and record data from the sensor
* Control hardware using Meadow
* Use Meadow to take the sensor readings and control hardware based upon the readings

Finally we will learn what the future holds for Meadow, the road map and current plans to make the platform even more awesome.

## Slide Deck

The slides are available in [PDF format](MeadowEcosystemForIoTDevelopment.pdf).

## Sample applications

Three demonstrations were given during the presentation, the sample code can be found in the following locations:

### [Getting Started](01-GettingStarted/Readme.md)

F# application that cycles through a range of colours using the on board LED.

### [TSL2591 Light Sensor](02-LightSensor/Readme.md)

[TSL2591 light sensor](https://coolcomponents.co.uk/products/tsl2591-high-dynamic-range-digital-light-sensor-stemma-qt?_pos=5&_sid=cb5eb073a&_ss=r) that takes light readings and sends the results to the cloud - [io.adafruit.com](https://io.adafruit.com/).

### [Mars Rover](03-MarsRover/Readme.md)

Rover project using the [EMOZNY Mecanum 4 wheel drive rover](https://www.amazon.co.uk/gp/product/B084TNLFYB/ref=ppx_yo_dt_b_asin_title_o01_s00?ie=UTF8&psc=1).  The rover is controlled using Bluetooth and also integrates the [VL53L0X distance sensor](https://coolcomponents.co.uk/products/vl53l0x-time-of-flight-distance-sensor-carrier-with-voltage-regulator-200cm-max?_pos=1&_sid=84ed08d55&_ss=r) for collision detection.

## Social Media and Further information

[Developer documentation](http://developer.wildernesslabs.co/)

[Github](https://github.com/wildernesslabs)

[Store](https://store.wildernesslabs.co/)

[Twitter](https://twitter.com/wildernesslabs)

[Instagram](https://www.instagram.com/wildernesslabs)

[YouTube](https://www.youtube.com/c/WildernessLabs)

[Hackster](https://www.hackster.io/WildernessLabs)
