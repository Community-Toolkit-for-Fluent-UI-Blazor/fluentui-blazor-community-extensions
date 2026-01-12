import { DotNet } from "../../d-ts/Microsoft.JSInterop";

export namespace FluentUI.Blazor.Community.DeviceDetector {
  interface DeviceInfo {
    userAgent: string,
    browser: string,
    operatingsystem: string,
    isMobile: boolean,
    hasTouch: boolean,
    orientation: string
  }
  export function Initialize(dotNetHelper: DotNet.DotNetObject) {
    function OrientationChanged(e: Event): void {
      const orientation: string = getOrientation();
      dotNetHelper.invokeMethodAsync("OrientationChanged", orientation);
    }

    window.screen.orientation.addEventListener('change', OrientationChanged);

    return {
      dispose: () => {
        window.screen.orientation.removeEventListener('change', OrientationChanged);
      }
    }
  }

  export function GetDeviceInfo(): DeviceInfo {
    const userAgent: string = navigator.userAgent;
    const operatingSystem: string = getOperatingSystem(userAgent);

    const deviceInfo: DeviceInfo = {
      userAgent: userAgent,
      browser: getBrowser(userAgent),
      operatingsystem: operatingSystem,
      isMobile: operatingSystem === "iOS" || operatingSystem === "Android",
      hasTouch: 'ontouchstart' in document.documentElement,
      orientation: getOrientation()
    };

    return deviceInfo;
  }

  function getBrowser(userAgent: string): string {
    /* Internet Explorer is not supported by Blazor. Therefore we don't need to check for it. */
    if (/Edg\/\d+/.test(userAgent)) return 'Edge';
    if (/Edge\/\d+/.test(userAgent)) return 'Edge';
    if (/Firefox\W\d/.test(userAgent)) return 'Firefox';
    if (/Chrom(e|ium)\W\d|CriOS\W\d/.test(userAgent)) return 'Chrome';
    if (/\bSafari\W\d/.test(userAgent)) return 'Safari';
    if (/\bOpera\W\d/.test(userAgent)) return 'Opera';
    if (/\bOPR\W\d/i.test(userAgent)) return 'Opera';
    return 'Undefined';
  }
  function getOperatingSystem(userAgent: string): string {
    // Windows user agents include 'Windows' or 'Windows NT'
    if (/Windows/.test(userAgent) || /Windows NT/.test(userAgent)) return "Windows";

    // Android devices
    if (/Android/i.test(userAgent)) return 'Android';

    // iOS devices: iPhone, iPad, iPod
    if (/iPhone|iPad|iPod/i.test(userAgent)) return 'iOS';

    // iPadOS 13+ may report as Macintosh but supports touch points
    if (/Macintosh/i.test(userAgent) && typeof navigator !== 'undefined' && (navigator as any).maxTouchPoints > 1) return 'iOS';

    // macOS / macOS-like (including iPadOS which may report 'Macintosh')
    if (/Macintosh|Mac OS X|Mac\sOS|Mac/.test(userAgent)) return "Mac";

    // Treat Linux, X11, ChromeOS as 'Linux' for the simplified output
    if (/Linux|X11|CrOS/.test(userAgent)) return "Linux";

    return "Undefined";
  }
  function getOrientation(): string {
    const orientation: string = window.screen.orientation.type;

    if (orientation === "portrait-primary") {
      return "Portrait";
    } else if (orientation === "portrait-secondary") {
      return "PortraitReversed";
    } else if (orientation === "landscape-primary") {
      return "Landscape";
    } else if (orientation === "landscape-secondary") {
      return "LandscapeReversed";
    }

    return "Unknown";
  }
}
