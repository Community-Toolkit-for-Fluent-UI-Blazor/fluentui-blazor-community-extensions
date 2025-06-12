export function copyToClipboard(dotNetReference, text) {
  if (window.isSecureContext) {
    navigator.clipboard.writeText(text).then(function () {
      dotNetReference.invokeMethodAsync("OnCopied");
    })
      .catch(function (error) {
        alert(error);
      })
  }
}
