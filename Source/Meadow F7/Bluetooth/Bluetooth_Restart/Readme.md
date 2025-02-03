# Bluetooth Restart Example

This example demonstrates how to stop the Bluetooth service and start a new service.

## Description

The application sets up two service definitions:

* MeadowF7A
* MeadowF7B

Each service has a single characteristic with read and write permissions.  Both services perform the same way depending upon the value written to the characteristic.

* 0 - End the application
* 1 - Switch the service
* Anything else - Set the characteristic value, this can then be read back