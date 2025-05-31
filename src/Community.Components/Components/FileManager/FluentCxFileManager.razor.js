const files = [];

export function initializeStream(filename) {
  const file = files[filename];

  if (!file) {
    files[filename] = {
      content: []
    }
  }
}

export function stream(filename, content) {
  const file = files[filename];

  if (file) {
    let currentContent = file.content;
    file.content = currentContent.concat(content);
  }
}

export function downloadStream(filename, target, type) {
  const file = files[filename];

  if (file) {
    let totalLength = 0;

    for (var i = 0; i < file.content.length; ++i) {
      totalLength += file.content[i].length;
    }

    // Create a new Int8Array with the total length
    const mergedArray = new Int8Array(totalLength);
    let currentLength = 0;

    for (var i = 0; i < file.content.length; ++i) {
      mergedArray.set(file.content[i], currentLength);
      currentLength += file.content[i].length;
    }

    openFile(filename, mergedArray, true, target, type)
  }
}

function destroyStream(filename) {
  const file = files[filename];

  if (file) {
    file.content = [];
    let index = files.indexOf(filename);
    files.splice(index, 1);
  }
}

export function openFile(filename, content, download, target, type) {
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
  destroyStream(filename);
}
