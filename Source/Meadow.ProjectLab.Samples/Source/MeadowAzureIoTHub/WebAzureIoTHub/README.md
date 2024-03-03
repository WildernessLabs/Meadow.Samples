# Visualize Azure IoT Hub data on a local web app

This is a tweaked sample project from [Visualize real-time sensor data from your Azure IoT hub in a web application](https://learn.microsoft.com/en-us/azure/iot-hub/iot-hub-live-data-visualization-in-web-apps)

### Run locally

1. To pass parameters to the website, you may use environment variables or parameters.
    - Open a command prompt or PowerShell terminal and set the environment variables **IotHubConnectionString** and **EventHubConsumerGroup**.

        > Syntax for Windows command prompt is `set key=value`, PowerShell is `$env:key="value"`, and Linux shell is `export key="value"`.

    - Or, if you are debugging with [VS Code](https://code.visualstudio.com/docs/nodejs/nodejs-debugging), you can edit the launch.json file and add these values in the env property.

        ```json
        "env": {
            "NODE_ENV": "local",
            "IotHubConnectionString": "IOT_HUB_CONNECTION_STRING",
            "EventHubConsumerGroup": "EVENT_HUB_CONSUMER_GROUP"
        }
        ```

1. In the same directory as package.json, run `npm install` to download and install referenced packages.

1. Run the website one of the following ways:
    - From the command-line (with environment variables set), use `npm start`
    - In VS Code, press F5 to start debugging

1. Watch for console output from the website.

1. If you are debugging, you may set breakpoints in any of the server-side scripts and step through the code to watch the code work.

1. Open a browser to <http://localhost:3000>.
