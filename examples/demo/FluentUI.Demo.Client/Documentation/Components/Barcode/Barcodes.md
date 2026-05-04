---
title: Barcodes
icon: BarcodeScanner
route: /Barcodes
---

# Barcodes

The barcode component allows you to generate barcodes in your application.
It supports various types of barcodes, such as QR codes, Code 128, and more...
You can customize the appearance of the barcode, including colors, size, and error correction level.
By the end, there will be around 80 symbologies supported, we expose a few of them as examples.
Only free documented symbologies are supported, see the table at the end of this page to know which ones are supported.

## Examples

### Code128
{{ Code128BarcodeExample }}

### GGS1-128
{{ GS1_128BarcodeExample }}

### Code39
{{ Code39BarcodeExample }}

### Code93
{{ Code93BarcodeExample }}

### Upc-A
{{ UpcABarcodeExample }}

### Upc-E
{{ UpcEBarcodeExample }}

### Ean-13
{{ Ean13BarcodeExample }}

### Ean-8
{{ Ean8BarcodeExample }}

### Itf-14
{{ Itf14BarcodeExample}}

### Codabar
{{ CodabarBarcodeExample}}

### QR Code
{{ QRCodeExample }}

### PDF417
{{ Pdf417BarcodeExample }}

## API Barcode

{{ API Type=Barcode }}


## Table of Symbologies

| Symbol | Meaning |
| --- | --- |
| 🟩 | Implemented & tested |
| 🟦 | Implemented but not yet tested |
| 🟥 | Proprietary / paid documentation |
| ⬜ | Not implemented yet (free spec available) |

### 1D Linear Barcodes

| Symbology | Status |
| --- | --- |
| EAN‑13 | 🟩 |
| EAN‑8 | 🟩 |
| UPC‑A | 🟩 |
| UPC‑E | 🟩 |
| ITF‑14 | 🟩 |
| GS1‑128 (UCC/EAN‑128) | ⬜ |
| Interleaved 2 of 5 (ITF) | ⬜ |
| Standard 2 of 5 | ⬜ |
| Industrial 2 of 5 | ⬜ |
| Matrix 2 of 5 | ⬜ |
| IATA 2 of 5 | ⬜ |
| Airline 2 of 5 | ⬜ |
| COOP 2 of 5 | ⬜ |
| Deutsche Post 2 of 5 | ⬜ |
| China Post 2 of 5 | ⬜ |
| MSI | ⬜ |
| MSI‑10 | ⬜ |
| MSI‑11 | ⬜ |
| MSI‑1010 | ⬜ |
| MSI‑1110 | ⬜ |
| Plessey | ⬜ |
| Anker Plessey | ⬜ |
| UK Plessey | ⬜ |
| Codabar | 🟩 |
| Code 11 | ⬜ |
| Code 25 | ⬜ |
| Code 39 | 🟩 |
| Code 39 Extended | 🟩 |
| Code 93 | 🟩 |
| Code 93 Extended | 🟩 |
| Code 128 | 🟩 |
| Code 128A | 🟩 |
| Code 128B | 🟩 |
| Code 128C | 🟩 |
| LOGMARS | ⬜ |
| BC412 | ⬜ |
| Channel Code | ⬜ |

### Stacked Linear Barcodes

| Symbology | Status |
| --- | --- |
| Codablock A | ⬜ |
| Codablock F | ⬜ |
| Code 16K | ⬜ |
| Code 49 | ⬜ |
| Compact PDF417 | 🟩 |
| PDF417 | 🟩 |
| MicroPDF417 | ⬜ |

### Postal Barcodes

| Symbology | Status |
| --- | --- |
| POSTNET | ⬜ |
| PLANET | ⬜ |
| USPS Intelligent Mail Barcode (IMB) | ⬜ |
| USPS FIM | ⬜ |
| USPS Sack Label | ⬜ |
| USPS Tray Label | ⬜ |
| USPS Container Label | ⬜ |
| KIX (Netherlands Post) | ⬜ |
| RM4SCC (Royal Mail) | ⬜ |
| Australia Post 4‑State | ⬜ |
| Japan Post | ⬜ |
| Singapore Post | ⬜ |
| Korea Post | ⬜ |

### Pharmaceutical / Industrial Barcodes

| Symbology | Status |
| --- | --- |
| PZN (Pharma Zentral Nummer) | ⬜ |
| SISAC | ⬜ |
| Pharmacode (Laetus) | ⬜ |
| Pharmacode 2‑track | ⬜ |
| Code 32 (Italian Pharmacode) | ⬜ |
| LabelCode V | ⬜ |
| LabelCode 2 | ⬜ |
| LabelCode 5 | ⬜ |

### 2D Matrix Codes

| Symbology | Status |
| --- | --- |
| QR Code | 🟩 |
| Micro QR | 🟥 |
| rMQR (Rectangular Micro QR) | 🟥 |
| SQRC | 🟥 |
| FrameQR | 🟥 |
| Data Matrix ECC200 | ⬜ |
| Data Matrix ECC000‑140 | ⬜ |
| GS1 DataMatrix | ⬜ |
| Aztec Code | ⬜ |
| Aztec Rune | ⬜ |
| MaxiCode | ⬜ |
| DotCode | ⬜ |
| Code One | ⬜ |
| Han Xin Code | ⬜ |
| Grid Matrix | ⬜ |

### Color 2D Codes (Proprietary)

| Symbology | Status |
| --- | --- |
| Ultracode | 🟥 |
| Vericode | 🟥 |
| Snowflake Code | 🟥 |
| HueCode | 🟥 |
| JAB Code | 🟥 |
| ColorCode | 🟥 |
| High Capacity Color Barcode (HCCB) | 🟥 |
| ShotCode | 🟥 |
| SPARQCode | 🟥 |
| BeeTagg | 🟥 |
| mCode | 🟥 |
| CyberCode | 🟥 |

### GS1 DataBar Family

| Symbology | Status |
| --- | --- |
| GS1 DataBar Omnidirectional | ⬜ |
| GS1 DataBar Truncated | ⬜ |
| GS1 DataBar Stacked | ⬜ |
| GS1 DataBar Stacked Omnidirectional | ⬜ |
| GS1 DataBar Limited | ⬜ |
| GS1 DataBar Expanded | ⬜ |
| GS1 DataBar Expanded Stacked | ⬜ |
