// ThermalPrinter.h

#ifndef _THERMALPRINTER_h
#define _THERMALPRINTER_h

#include "Arduino.h"

#define UPC_A   65
#define UPC_E   66
#define EAN13   67
#define EAN8    68
#define CODE39  69
#define ITF     70
#define CODABAR 71
#define CODE93  72
#define CODE128 73

#define CHARSET_USA           0
#define CHARSET_FRANCE        1
#define CHARSET_GERMANY       2
#define CHARSET_UK            3
#define CHARSET_DENMARK1      4
#define CHARSET_SWEDEN        5
#define CHARSET_ITALY         6
#define CHARSET_SPAIN1        7
#define CHARSET_JAPAN         8
#define CHARSET_NORWAY        9
#define CHARSET_DENMARK2     10
#define CHARSET_SPAIN2       11
#define CHARSET_LATINAMERICA 12
#define CHARSET_KOREA        13
#define CHARSET_SLOVENIA     14
#define CHARSET_CROATIA      14
#define CHARSET_CHINA        15

#define CODEPAGE_CP437        0 
#define CODEPAGE_KATAKANA     1
#define CODEPAGE_CP850        2
#define CODEPAGE_CP860        3
#define CODEPAGE_CP863        4
#define CODEPAGE_CP865        5
#define CODEPAGE_WCP1251      6
#define CODEPAGE_CP866        7
#define CODEPAGE_MIK          8
#define CODEPAGE_CP755        9
#define CODEPAGE_IRAN        10
#define CODEPAGE_CP862       15
#define CODEPAGE_WCP1252     16
#define CODEPAGE_WCP1253     17
#define CODEPAGE_CP852       18
#define CODEPAGE_CP858       19
#define CODEPAGE_IRAN2       20
#define CODEPAGE_LATVIAN     21
#define CODEPAGE_CP864       22
#define CODEPAGE_ISO_8859_1  23
#define CODEPAGE_CP737       24
#define CODEPAGE_WCP1257     25
#define CODEPAGE_THAI        26
#define CODEPAGE_CP720       27
#define CODEPAGE_CP855       28
#define CODEPAGE_CP857       29
#define CODEPAGE_WCP1250     30
#define CODEPAGE_CP775       31
#define CODEPAGE_WCP1254     32
#define CODEPAGE_WCP1255     33
#define CODEPAGE_WCP1256     34
#define CODEPAGE_WCP1258     35
#define CODEPAGE_ISO_8859_2  36
#define CODEPAGE_ISO_8859_3  37
#define CODEPAGE_ISO_8859_4  38
#define CODEPAGE_ISO_8859_5  39
#define CODEPAGE_ISO_8859_6  40
#define CODEPAGE_ISO_8859_7  41
#define CODEPAGE_ISO_8859_8  42
#define CODEPAGE_ISO_8859_9  43
#define CODEPAGE_ISO_8859_15 44
#define CODEPAGE_THAI2       45
#define CODEPAGE_CP856       46
#define CODEPAGE_CP874       47

class ThermalPrinter : public Print
{
public:
	ThermalPrinter(Stream *s = &Serial, uint8_t dtr = 255);

	size_t
		write(uint8_t c);
	void
		begin(uint8_t heatTime = 120),
		boldOff(),
		boldOn(),
		doubleHeightOff(),
		doubleHeightOn(),
		doubleWidthOff(),
		doubleWidthOn(),
		feed(uint8_t x = 1),
		feedRows(uint8_t),
		fontA(),
		fontB(),
		flush(),
		inverseOff(),
		inverseOn(),
		justify(char value),
		offline(),
		online(),
		printBarcode(char *text, uint8_t type),
		printBitmap(int w, int h, const uint8_t *bitmap, bool fromProgMem = true),
		printBitmap(int w, int h, Stream *fromStream),
		printBitmap(Stream *fromStream),
		normal(),
		reset(),
		setBarcodeHeight(uint8_t val = 50),
		setBarcodeWidht(uint8_t val = 3),
		setCharSpacing(int spacing = 0),
		setCharset(uint8_t val = 0),
		setCodePage(uint8_t val = 0),
		setDefault(),
		setLineHeight(int val = 30),
		setMaxChunkHeight(int val = 256),
		setSize(char value),
		setTimes(unsigned long, unsigned long),
		sleep(),
		sleepAfter(uint16_t seconds),
		strikeOff(),
		strikeOn(),
		tab(),
		test(),
		testPage(),
		timeoutSet(unsigned long),
		timeoutWait(),
		underlineOff(),
		underlineOn(uint8_t weight = 1),
		upsideDownOff(),
		upsideDownOn(),
		wake();
	bool
		hasPaper();

private:

	Stream
		*stream;
	uint8_t
		printMode,
		prevByte,      // Last character issued to printer
		column,        // Last horizontal column printed
		maxColumn,     // Page width (output 'wraps' at this point)
		charHeight,    // Height of characters, in 'dots'
		lineSpacing,   // Inter-line spacing (not line height), in dots
		barcodeHeight, // Barcode height in dots, not including text
		maxChunkHeight,
		dtrPin;        // DTR handshaking pin (experimental)
	boolean
		dtrEnabled;    // True if DTR pin set & printer initialized
	unsigned long
		resumeTime,    // Wait until micros() exceeds this before sending byte
		dotPrintTime,  // Time to print a single dot line, in microseconds
		dotFeedTime;   // Time to feed a single dot line, in microseconds
	void
		writeBytes(uint8_t a),
		writeBytes(uint8_t a, uint8_t b),
		writeBytes(uint8_t a, uint8_t b, uint8_t c),
		writeBytes(uint8_t a, uint8_t b, uint8_t c, uint8_t d),
		setPrintMode(uint8_t mask),
		unsetPrintMode(uint8_t mask),
		writePrintMode();
};

#endif
