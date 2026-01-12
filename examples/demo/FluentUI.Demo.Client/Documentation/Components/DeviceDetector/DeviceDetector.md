---
title: DeviceDetector
route: /DeviceDetector
order: 0000
category: 10|Layout
icon: Regular.PhoneDesktop
---

# DeviceDetector

The Device Detector component provides insights into the system on which the application is running. It allows you to retrieve information such as:

- Operating system (Windows, macOS, Linux, Android, iOS)
- Device orientation (Portrait/Landscape)
- Browser information (Chrome, Egde, Firefox ...)

## Examples

The Device Detector processes device information through the `DeviceInfoUpdated` event callback. We recommend defining a single `FluentCxDeviceDetector` in your `MainLayout` and injecting `DeviceInfoState` into any component that needs to adapt its behavior based on these properties.

### Default

{{ DeviceDetectorDefault }}
