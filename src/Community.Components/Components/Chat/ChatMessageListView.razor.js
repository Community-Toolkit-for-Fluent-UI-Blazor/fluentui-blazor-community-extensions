export function copyToClipboard(dotNetReference, text) {
  if (window.isSecureContext) {
    navigator.clipboard.writeText(text).then(function () {
      dotNetReference.invokeMethodAsync("OnCopied");
    })
      .catch(function (error) {
        dotNetReference.invokeMethodAsync("OnCopyError");
        console.log(error);
      })
  }
}

export function scrollToBottom() {

}
