---
title: FileManager
icon: Menu
route: /FileManager
---

# File Manager

The File Manager component allows users to manage files and folders in a hierarchical structure. It provides features such as creating, renaming, deleting, and moving files and folders, as well as uploading and downloading files.
You need to provide a <code>IFileProvider</code> implementation to the File Manager component to enable it to interact with the file system.
The <code>IFileProvider</code> interface defines methods for performing file operations such as retrieving file and folders, search files.

To be able to create, rename, delete, move and upload files and folders, you need to implement the <code>IFileMutationHandler</code> interface.
The idea of these two interfaces is to separate the read and write operations, so you can have different implementations for each of them if needed.
For example, you could have a read-only file provider that retrieves files from a database, and a separate mutation handler that allows users to upload files to a cloud storage service.

## Examples

{{ FileManagerExample }}

## API FileManager


