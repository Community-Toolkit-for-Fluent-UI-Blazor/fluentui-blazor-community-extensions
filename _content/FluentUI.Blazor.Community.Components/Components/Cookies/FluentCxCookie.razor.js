window.dataLayer = window.dataLayer || [];
function gtag() { dataLayer.push(arguments) };

const injectGAScript = (measurementId) => {
  // Load the Google tag manager script dynamically
  const script = document.createElement('script');
  script.async = true;
  script.src = `https://www.googletagmanager.com/gtag/js?id=${measurementId}`;
  document.head.appendChild(script);

  // Initialize GA4 once the script loads
  script.onload = () => {
    gtag('js', new Date());
    gtag('config', measurementId);
    console.log('Google Analytics 4 initialized successfully');
  };

  script.onerror = () => {
    console.error('Failed to load Google Analytics 4');
  };
};

export function deleteCookiePolicy() {
  localStorage.removeItem("cookie-policy");
}

export function getCookiePolicy() {
  const cookiePolicy = localStorage.getItem("cookie-policy")

  return JSON.parse(cookiePolicy)
}

export function setCookiePolicy(value) {
  localStorage.setItem("cookie-policy", JSON.stringify(value))
}

export function initializeGoogleAnalytics(googleAnalyticsId) {
  injectGAScript(googleAnalyticsId);
}
