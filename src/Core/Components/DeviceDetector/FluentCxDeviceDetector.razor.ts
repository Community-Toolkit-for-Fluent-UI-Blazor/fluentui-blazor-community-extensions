import { DotNet } from "../../d-ts/Microsoft.JSInterop";

export namespace FluentUI.Blazor.Community.DeviceDetector {
  interface DeviceInfo {
    userAgent: string,
    browser: string,
    operatingsystem: string,
    isMobile: boolean,
    hasTouch: boolean
  }
  export function GetDeviceInfo(): DeviceInfo {
    const userAgent: string = navigator.userAgent;
    const operatingSystem: string = getOperatingSystem(userAgent);

    const deviceInfo: DeviceInfo = {
      userAgent: userAgent,
      browser: getBrowser(userAgent),
      operatingsystem: operatingSystem,
      isMobile: operatingSystem === "iOS" || operatingSystem === "Android",
      hasTouch: 'ontouchstart' in document.documentElement
    };

    return deviceInfo;
  }

  export function getDeviceOrientation(dotNetHelper: DotNet.DotNetObject) {
    //function updateOrientation(e) {
    //  const orientation = e === undefined ? window.screen.orientation.type : e.target.type;
    //  const value = orientation === 'portrait-primary' ? 'Portrait' :
    //    orientation === 'portrait-secondary' ? 'PortraitReversed' :
    //      orientation === 'landscape-primary' ? 'Landscape' :
    //        'LandscapeReversed'

    //  dotNetHelper.invokeMethodAsync("ChangeOrientation", value);
    //}

    //window.screen.orientation.addEventListener('change', (e) => updateOrientation(e));
    //updateOrientation();
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
}
