import { DotNet } from "../../d-ts/Microsoft.JSInterop";

export namespace FluentUI.Blazor.Community.CookieConsent {
  export interface CookiePolicyEntry {
    name: string;
    isActive: boolean;
  }

  export interface CookiePolicyEntries {
    items: CookiePolicyEntry[];
  }

  const GA = { dataLayer: [] as any[], gtag: (...args: any[]) => GA.dataLayer.push(args) };
  const STORAGE_KEY = "cookie-policy";

  GA.dataLayer = GA.dataLayer || [];

  function injectGAScript(measurementId: string): void {
    const script = document.createElement("script");
    script.async = true;
    script.src = `https://www.googletagmanager.com/gtag/js?id=${measurementId}`;
    document.head.appendChild(script);

    script.onload = () => {
      GA.gtag("js", new Date());
      GA.gtag("config", measurementId);
      console.log("Google Analytics 4 initialized successfully");
    };

    script.onerror = () => {
      console.error("Failed to load Google Analytics 4");
    };
  }

  export function initializeGoogleAnalytics(measurementId: string): void {
    injectGAScript(measurementId);
  }

  export function deleteCookiePolicy(): void {
    localStorage.removeItem(STORAGE_KEY);
  }

  export function getCookiePolicy(): CookiePolicyEntry[] | null {
    const raw = localStorage.getItem(STORAGE_KEY);
    if (!raw) return null;

    try {
      return JSON.parse(raw) as CookiePolicyEntry[];
    } catch {
      return null;
    }
  }

  export function setCookiePolicy(value: CookiePolicyEntry[]): void {
    localStorage.setItem(STORAGE_KEY, JSON.stringify(value));
  }
}
