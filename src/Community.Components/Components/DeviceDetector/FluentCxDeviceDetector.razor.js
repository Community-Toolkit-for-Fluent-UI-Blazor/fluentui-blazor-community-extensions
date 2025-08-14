export function getDeviceInfo() {
  var
    userAgent = navigator.userAgent,
    browser = /Edg\/\d+/.test(userAgent) ? 'Edge' : /Edge\/\d+/.test(userAgent) ? 'Edge' : /MSIE 9/.test(userAgent) ? 'InternetExplorer9' : /MSIE 10/.test(userAgent) ? 'InternetExplorer10' : /MSIE 11/.test(userAgent) ? 'InternetExplorer11' : /MSIE\s\d/.test(userAgent) ? 'InternetExplorerUnknown' : /rv\:11/.test(userAgent) ? 'InternetExplorer11' : /Firefox\W\d/.test(userAgent) ? 'Firefox' : /Chrom(e|ium)\W\d|CriOS\W\d/.test(userAgent) ? 'Chrome' : /\bSafari\W\d/.test(userAgent) ? 'Safari' : /\bOpera\W\d/.test(userAgent) ? 'Opera' : /\bOPR\W\d/i.test(userAgent) ? 'Opera' : typeof MSPointerEvent !== 'Undefined' ? 'InternetExplorerUnknown' : 'Undefined',
    operatingsystem = /Windows NT 10/.test(userAgent) ? "Windows10" : /Windows NT 6\.0/.test(userAgent) ? "WindowsVista" : /Windows NT 6\.1/.test(userAgent) ? "Windows7" : /Windows NT 6\.\d/.test(userAgent) ? "Windows8" : /Windows NT 5\.1/.test(userAgent) ? "WindowsXp" : /Windows NT [1-5]\./.test(userAgent) ? "WindowsNT" : /Mac/.test(userAgent) ? "Mac" : /Linux/.test(userAgent) ? "Linux" : /X11/.test(userAgent) ? "Nix" : "Undefined",
    touch = 'ontouchstart' in document.documentElement,
    mobile = /IEMobile|Windows Phone|Lumia/i.test(userAgent) ? 'WindowsPhone' : /iPhone/.test(userAgent) ? 'IPhone' : /iPad/.test(userAgent) ? 'IPad' : /iPod/.test(userAgent) ? "IPod" : /Android/.test(userAgent) ? 'Android' : /BlackBerry|PlayBook|BB10/.test(userAgent) ? 'BlackBerry' : /Mobile Safari/.test(userAgent) ? 'Safari' : /webOS|Mobile|Tablet|Opera Mini|\bCrMo\/|Opera Mobi/i.test(userAgent) ? "UnknownMobileDevice" : "NotMobileDevice",
    isTablet = /Tablet|iPad/i.test(userAgent);

  const value = {
    userAgent,
    browser,
    operatingsystem,
    touch,
    mobile,
    isTablet
  }

  return value;
}

export function getDeviceOrientation(reference) {
  function updateOrientation(e) {
    const orientation = e === undefined ? window.screen.orientation.type : e.target.type;
    const value = orientation === 'portrait-primary' ? 'Portrait' :
                  orientation === 'portrait-secondary' ? 'PortraitReversed' :
                  orientation === 'landscape-primary' ? 'Landscape' :
                  'LandscapeReversed'

    reference.invokeMethodAsync("ChangeOrientation", value);
  }

  window.screen.orientation.addEventListener('change', (e) => updateOrientation(e));
  updateOrientation();
}
