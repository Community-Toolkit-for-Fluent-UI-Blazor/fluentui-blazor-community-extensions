window.getBlobUrl = (contentType, byteArray) => {
  const blob = new Blob([new Uint8Array(byteArray)], { type: contentType });
  return URL.createObjectURL(blob);
};
