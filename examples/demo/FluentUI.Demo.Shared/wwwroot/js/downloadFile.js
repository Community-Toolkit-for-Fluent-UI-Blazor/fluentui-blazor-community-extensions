export function downloadFile(filename, content, download, target, type) {
  // Create the URL
  const file = new File([content], filename, { type: type });
  const exportUrl = URL.createObjectURL(file);

  // Create the <a> element and click on it
  const a = document.createElement("a");
  document.body.appendChild(a);
  a.href = exportUrl;

  if (download) {
    a.download = filename;
  }

  a.target = target;
  a.click();

  URL.revokeObjectURL(exportUrl);
}
