# FCXSIGN Binary File Format Specification

## Overview

`FCXSIGN` is a compact, deterministic, cryptographically verifiable binary format designed to store:

- A rendered signature image (PNG)
- A structured document payload (JSON/SVG/Binary)
- A cryptographic proof block
- Optional AES encryption
- Optional QES (Qualified Electronic Signature) metadata

The format is stable, versioned, and suitable for long‑term archival and verification.

---

# 1. File Layout

FCXSIGN files exist in two forms:

## 1.1 Clear (Unencrypted) Format

+----------------------+---------------------------+
| HEADER (16 bytes)    | Always unencrypted        |
+----------------------+---------------------------+
| INDEX (48 bytes)     | Offsets & lengths         |
+----------------------+---------------------------+
| IMAGE                | PNG bytes                 |
+----------------------+---------------------------+
| DOCUMENT             | JSON/SVG/Binary payload   |
+----------------------+---------------------------+
| PROOF                | JSON proof block          |
+----------------------+---------------------------+


## 1.2 AES‑Encrypted Format

+----------------------+---------------------------+
| HEADER (16 bytes)    | Always unencrypted        |
+----------------------+---------------------------+
| IV (16 bytes)        | AES initialization vector |
+----------------------+---------------------------+
| CIPHERTEXT           | Encrypted block           |
+----------------------+---------------------------+
| HMAC (32 bytes)      | Authentication tag        |
+----------------------+---------------------------+


The ciphertext contains the **entire clear FCXSIGN file**, including:

HEADER + INDEX + IMAGE + DOCUMENT + PROOF


---

# 2. Header (16 bytes)

| Offset | Size | Description |
|--------|------|-------------|
| 0      | 8    | Magic number `FCXSIGN\0` (0x004E474953584346) |
| 8      | 1    | Version major |
| 9      | 1    | Version minor |
| 10     | 1    | Flags |
| 11     | 5    | Reserved (zero) |

## 2.1 Flags

Bitmask:


Examples:

| Meaning | Flags |
|---------|--------|
| Clear, unsigned | `0x00` |
| Clear, signed | `0x02` |
| Clear, QES | `0x06` |
| AES encrypted, signed | `0x03` |
| AES encrypted, QES | `0x07` |

---

# 3. Index (48 bytes)

Present in clear files and inside the AES ciphertext.

| Offset | Size | Field |
|--------|------|--------|
| 0      | 8    | ImageOffset |
| 8      | 8    | ImageLength |
| 16     | 8    | DocumentOffset |
| 24     | 8    | DocumentLength |
| 32     | 8    | ProofOffset |
| 40     | 8    | ProofLength |

Offsets are absolute positions within the **clear file**.

---

# 4. Sections

## 4.1 Image Section

- Raw PNG bytes
- No additional compression

## 4.2 Document Section

- JSON, SVG, or binary
- Format determined by the rendering engine

## 4.3 Proof Section

A JSON object of type:

```json
{
  "UserData": { ... },
  "PayloadHash": "base64",
  "DocumentHash": "base64",
  "ImageHash": "base64",
  "GlobalHash": "base64",
  "DigitalSignature": "base64 or null"
}
```

Field meanings :

- PayloadHash: SHA‑256 hash of the structured payload
- DocumentHash: SHA‑256 hash of the document section
- ImageHash: SHA‑256 hash of the PNG image
- GlobalHash: SHA‑256 hash of the entire clear FCXSIGN file
- DigitalSignature: Signature over GlobalHash (AES/QES)

---


# 5. Cryptographic Model

## 5.1 Hashing

All hashes use SHA‑256.

Global Hash
For clear files:
GlobalHash = SHA256(clearFileBytes)

For AES files:
GlobalHash = SHA256(clearFileBytes)  
(not the encrypted file)

---

## 5.2 Signature

If present:

DigitalSignature = Sign(GlobalHash)

---

## 5.3 AES Encryption

AES mode:

AES‑CBC

HMAC‑SHA256 for authentication

IV is always 16 bytes

HMAC is always 32 bytes

Encrypted block layout:

[IV | CIPHERTEXT | HMAC]

---

# 6. Validation Rules

A valid FCXSIGN file must satisfy:

Magic number matches

Header flags are consistent

Section hashes match their content

GlobalHash matches the clear file

Signature is valid if present

QES flag requires QES validation

AES block must decrypt successfully

---

# 7. QES (Qualified Electronic Signature)

If the QES flag is set:

The file must contain a valid signature

A QES validation routine must be executed

Additional domain‑specific checks may be applied

The FCXSIGN reader automatically triggers the QES executor if provided.

---

# 8. Versioning

The format is versioned using:

VersionMajor

VersionMinor

Breaking changes increment the major version.
Backward‑compatible changes increment the minor version.

---

# 9. Summary

FCXSIGN provides:

Deterministic binary layout

Strong cryptographic guarantees

Optional AES encryption

Optional QES compliance

Clear separation between header, index, and sections

Long‑term stability for archival and verification

This specification defines the complete structure and validation rules for all FCXSIGN files.

---
