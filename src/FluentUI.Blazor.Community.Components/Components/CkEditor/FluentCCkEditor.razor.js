export function setup(id, dotNetReference) {

  class MyUploadAdapter {
    constructor(loader) {
      this.loader = loader;
      this.chunkSize = 100000;
    }

    upload() {
      return new Promise((resolve, reject) => {
        this.loader.file
          .then(file => {
            const fileSize = file.size;
            const fileName = crypto.randomUUID();
            let offset = 0;

            const readChunk = () => {
              const reader = new FileReader();

              reader.onload = () => {
                const arrayBuffer = reader.result;
                const byteArray = new Uint8Array(arrayBuffer);
                const chunk = byteArray.slice(0, this.chunkSize);
                offset += chunk.length;

                // Call the C# method with the chunk and file size
                dotNetReference.invokeMethodAsync("UploadChunkAsync", chunk, fileSize, fileName)
                  .then((imagePath) => {
                    if (offset < fileSize) {
                      // If there are more chunks to send, read the next chunk
                      readChunk();
                    } else {
                      // Once upload is complete, set the image source
                      resolve({
                        default: imagePath,
                      });
                    }
                  })
                  .catch(error => {
                    console.log(error);
                    reject(error);
                  });
              };

              reader.onerror = error => {
                console.log(error);
                reject(error);
              };

              const slice = file.slice(offset, offset + this.chunkSize);
              reader.readAsArrayBuffer(slice);
            };

            readChunk();
          })
          .catch(error => {
            console.log(error);
            reject(error)
          });
      });
    }
    abort() {

    }
  }


  ClassicEditor
    .create(document.querySelector('#ckeditor-' + id), {
      language: 'en',
      licenseKey: '',
    })
    .then(editor => {
      window.editor = editor;

      editor.plugins.get('FileRepository').createUploadAdapter = (loader) => {
        window.CKEditorAdapter = new MyUploadAdapter(loader);
        return window.CKEditorAdapter;
      };
      editor.model.document.on('change:data', () => {
        let data = editor.getData();

        const el = document.createElement('div');
        el.innerHTML = data;
        if (el.innerText.trim() == '')
          data = "";

        var editorContent = editor.getData();

        if (editorContent.length == 0) {
          dotNetReference.invokeMethodAsync('EditorDataChanged', "", true);
        } else {
          // Split content into chunks (e.g., 1 KB each)
          const chunkSize = 100000;
          for (let i = 0; i < editorContent.length; i += chunkSize) {
            const chunk = editorContent.slice(i, i + chunkSize);
            // Call JavaScript function to send each chunk
            var isLastTransfer = editorContent.length < i + chunkSize;
            const myObject = {
              contentLength: editorContent.length,
              i: i,
              chunkSize: chunkSize,
              isLastTransfer: isLastTransfer,
              chunk: chunk
            };
            console.table(myObject);

            dotNetReference.invokeMethodAsync('EditorDataChanged', chunk, isLastTransfer);
          }

        }

      });
    })
    .catch(error => {
      console.error('Oops, something went wrong!');
      console.error('Please, report the following error on https://github.com/ckeditor/ckeditor5/issues with the build id and the error stack trace:');
      console.warn('Build id: n48v085f21si-dqhb6duovqri');
      console.error(error);
    });
}

export function update(id, data) {
  var editors = document.querySelectorAll('.ck-editor__editable');
  for (var i = 0; i < editors.length; i++) {
    if (editors[i].ckeditorInstance != null && editors[i].ckeditorInstance.sourceElement.id == "ckeditor-" + id) {
      editors[i].ckeditorInstance.setData(data);
    }
  }
}

export function destroy(id) {
  var editors = document.querySelectorAll('.ck-editor__editable');
  for (var i = 0; i < editors.length; i++) {
    if (editors[i].ckeditorInstance != null && editors[i].ckeditorInstance.sourceElement.id == "ckeditor-" + id) {
      editors[i].ckeditorInstance.destroy();
    }
  }
}
