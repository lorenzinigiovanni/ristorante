#include "ThermalPrinter.h"

#define ASCII_TAB '\t' // Horizontal tab
#define ASCII_LF  '\n' // Line feed
#define ASCII_FF  '\f' // Form feed
#define ASCII_CR  '\r' // Carriage return
#define ASCII_DC2  18  // Device control 2
#define ASCII_ESC  27  // Escape
#define ASCII_FS   28  // Field separator
#define ASCII_GS   29  // Group separator

#define BAUDRATE  19200
#define BYTE_TIME (((11L * 1000000L) + (BAUDRATE / 2)) / BAUDRATE)

ThermalPrinter::ThermalPrinter(Stream * s, uint8_t dtr) :
	stream(s), dtrPin(dtr) {
	dtrEnabled = false;
}

void ThermalPrinter::timeoutSet(unsigned long x)
{
	if (!dtrEnabled) resumeTime = micros() + x;
}

void ThermalPrinter::timeoutWait()
{
	if (dtrEnabled) {
		while (digitalRead(dtrPin) == HIGH);
	}
	else {
		while ((long)(micros() - resumeTime) < 0L);
	}
}

void ThermalPrinter::setTimes(unsigned long p, unsigned long f)
{
	dotPrintTime = p;
	dotFeedTime = f;
}

void ThermalPrinter::writeBytes(uint8_t a)
{
	timeoutWait();
	stream->write(a);
	timeoutSet(BYTE_TIME);
}

void ThermalPrinter::writeBytes(uint8_t a, uint8_t b)
{
	timeoutWait();
	stream->write(a);
	stream->write(b);
	timeoutSet(2 * BYTE_TIME);
}

void ThermalPrinter::writeBytes(uint8_t a, uint8_t b, uint8_t c)
{
	timeoutWait();
	stream->write(a);
	stream->write(b);
	stream->write(c);
	timeoutSet(3 * BYTE_TIME);
}

void ThermalPrinter::writeBytes(uint8_t a, uint8_t b, uint8_t c, uint8_t d)
{
	timeoutWait();
	stream->write(a);
	stream->write(b);
	stream->write(c);
	stream->write(d);
	timeoutSet(4 * BYTE_TIME);
}

size_t ThermalPrinter::write(uint8_t c)
{
	if (c != 0x13) { // Strip carriage returns
		timeoutWait();
		stream->write(c);
		unsigned long d = BYTE_TIME;
		if ((c == '\n') || (column == maxColumn)) { // If newline or wrap
			d += (prevByte == '\n') ?
				((charHeight + lineSpacing) * dotFeedTime) :             // Feed line
				((charHeight*dotPrintTime) + (lineSpacing*dotFeedTime)); // Text line
			column = 0;
			c = '\n'; // Treat wrap as newline on next pass
		}
		else {
			column++;
		}
		timeoutSet(d);
		prevByte = c;
	}

	return 1;
}

void ThermalPrinter::begin(uint8_t heatTime)
{
	timeoutSet(500000L);

	wake();
	reset();

	writeBytes(ASCII_FS, '.');

	writeBytes(ASCII_ESC, '7');   // Esc 7 (print settings)
	writeBytes(7, heatTime, 2); // Heating dots, heat time, heat interval

#define printDensity   10 // 100%
#define printBreakTime  2 // 500 uS

	writeBytes(ASCII_DC2, '#', (printBreakTime << 5) | printDensity);

	// Enable DTR pin if requested
	if (dtrPin < 255) {
		pinMode(dtrPin, INPUT_PULLUP);
		writeBytes(ASCII_GS, 'a', (1 << 5));
		dtrEnabled = true;
	}

	dotPrintTime = 30000;
	dotFeedTime = 2100;
	maxChunkHeight = 255;
}

void ThermalPrinter::reset()
{
	writeBytes(ASCII_ESC, '@'); // Init command
	prevByte = '\n';       // Treat as if prior line is blank
	column = 0;
	maxColumn = 32;
	charHeight = 24;
	lineSpacing = 6;
	barcodeHeight = 50;

	writeBytes(ASCII_ESC, 'D'); // Set tab stops...
	writeBytes(4, 8, 12, 16); // ...every 4 columns,
	writeBytes(20, 24, 28, 0); // 0 marks end-of-list.
}

void ThermalPrinter::setDefault()
{
	online();
	justify('L');
	inverseOff();
	doubleHeightOff();
	setLineHeight(30);
	boldOff();
	underlineOff();
	setBarcodeHeight(50);
	setSize('s');
	setCharset();
	setCodePage();
}

void ThermalPrinter::test()
{
	println(F("Hello World!"));
	feed(2);
}

void ThermalPrinter::testPage()
{
	writeBytes(ASCII_DC2, 'T');
	timeoutSet(
		dotPrintTime * 24 * 26 +      // 26 lines w/text (ea. 24 dots high)
		dotFeedTime * (6 * 26 + 30)); // 26 text lines (feed 6 dots) + blank line
}

void ThermalPrinter::setBarcodeHeight(uint8_t val)
{
	if (val < 1) val = 1;
	barcodeHeight = val;
	writeBytes(ASCII_GS, 'h', val);
}

void ThermalPrinter::setBarcodeWidht(uint8_t val)
{
	if (val < 1) val = 1;
	writeBytes(ASCII_GS, 'w', val);
}

void ThermalPrinter::printBarcode(char * text, uint8_t type)
{
	feed(1);
	writeBytes(ASCII_GS, 'k', type); // Barcode type (listed in .h file)

	int len = strlen(text);
	if (len > 255) len = 255;
	writeBytes(len);										// Write length byte
	for (uint8_t i = 0; i < len; i++) writeBytes(text[i]);	// Write string sans NUL

	timeoutSet((barcodeHeight + 40) * dotPrintTime);
	prevByte = '\n';
}

// === Character commands ===

#define	FONT_MASK		       (1 << 0)
#define INVERSE_MASK       (1 << 1)
#define UPDOWN_MASK        (1 << 2)
#define BOLD_MASK          (1 << 3)
#define DOUBLE_HEIGHT_MASK (1 << 4)
#define DOUBLE_WIDTH_MASK  (1 << 5)
#define STRIKE_MASK        (1 << 6)

void ThermalPrinter::setPrintMode(uint8_t mask)
{
	printMode |= mask;
	writePrintMode();
	charHeight = (printMode & DOUBLE_HEIGHT_MASK) ? 48 : 24;
	maxColumn = (printMode & DOUBLE_WIDTH_MASK) ? 16 : 32;
}

void ThermalPrinter::unsetPrintMode(uint8_t mask)
{
	printMode &= ~mask;
	writePrintMode();
	charHeight = (printMode & DOUBLE_HEIGHT_MASK) ? 48 : 24;
	maxColumn = (printMode & DOUBLE_WIDTH_MASK) ? 16 : 32;
}

void ThermalPrinter::writePrintMode()
{
	writeBytes(ASCII_ESC, '!', printMode);
}

void ThermalPrinter::normal()
{
	printMode = 0;
	writePrintMode();
}

void ThermalPrinter::fontA() {
	unsetPrintMode(FONT_MASK);
}

void ThermalPrinter::fontB() {
	setPrintMode(FONT_MASK);
}

void ThermalPrinter::inverseOn()
{
	writeBytes(ASCII_GS, 'B', 1);
}

void ThermalPrinter::inverseOff()
{
	writeBytes(ASCII_GS, 'B', 0);
}

void ThermalPrinter::upsideDownOn()
{
	setPrintMode(UPDOWN_MASK);
}

void ThermalPrinter::upsideDownOff()
{
	unsetPrintMode(UPDOWN_MASK);
}

void ThermalPrinter::doubleHeightOn()
{
	setPrintMode(DOUBLE_HEIGHT_MASK);
}

void ThermalPrinter::doubleHeightOff()
{
	unsetPrintMode(DOUBLE_HEIGHT_MASK);
}

void ThermalPrinter::doubleWidthOn()
{
	setPrintMode(DOUBLE_WIDTH_MASK);
}

void ThermalPrinter::doubleWidthOff()
{
	unsetPrintMode(DOUBLE_WIDTH_MASK);
}

void ThermalPrinter::strikeOn()
{
	setPrintMode(STRIKE_MASK);
}

void ThermalPrinter::strikeOff()
{
	unsetPrintMode(STRIKE_MASK);
}

void ThermalPrinter::boldOn()
{
	setPrintMode(BOLD_MASK);
}

void ThermalPrinter::boldOff()
{
	unsetPrintMode(BOLD_MASK);
}

void ThermalPrinter::justify(char value)
{
	uint8_t pos = 0;

	switch (toupper(value)) {
	case 'L': pos = 0; break;
	case 'C': pos = 1; break;
	case 'R': pos = 2; break;
	}

	writeBytes(ASCII_ESC, 'a', pos);
}

void ThermalPrinter::feed(uint8_t x)
{
	writeBytes(ASCII_ESC, 'd', x);
	timeoutSet(dotFeedTime * charHeight);
	prevByte = '\n';
	column = 0;
}

void ThermalPrinter::feedRows(uint8_t rows)
{
	writeBytes(ASCII_ESC, 'J', rows);
	timeoutSet(rows * dotFeedTime);
	prevByte = '\n';
	column = 0;
}

void ThermalPrinter::flush()
{
	writeBytes(ASCII_FF);
}

void ThermalPrinter::setSize(char value)
{
	uint8_t size;

	switch (toupper(value)) {
	default:  // Small
		size = 0x00;
		charHeight = 24;
		maxColumn = 32;
		break;
	case 'M': // Medium
		size = 0x11;
		charHeight = 48;
		maxColumn = 32;
		break;
	case 'L': // Large
		size = 0x53;
		charHeight = 96;
		maxColumn = 16;
		break;
	}

	writeBytes(ASCII_GS, '!', size);
	prevByte = '\n'; // Setting the size adds a linefeed
}

// Underlines of different weights can be produced:
// 0 - no underline
// 1 - normal underline
// 2 - thick underline
void ThermalPrinter::underlineOn(uint8_t weight)
{
	if (weight > 2) weight = 2;
	writeBytes(ASCII_ESC, '-', weight);
}

void ThermalPrinter::underlineOff()
{
	writeBytes(ASCII_ESC, '-', 0);
}

void ThermalPrinter::printBitmap(int w, int h, const uint8_t * bitmap, bool fromProgMem)
{
	int rowBytes, rowBytesClipped, rowStart, chunkHeight, chunkHeightLimit,
		x, y, i;

	rowBytes = (w + 7) / 8; // Round up to next byte boundary
	rowBytesClipped = (rowBytes >= 48) ? 48 : rowBytes; // 384 pixels max width

														// Est. max rows to write at once, assuming 256 byte printer buffer.
	if (dtrEnabled) {
		chunkHeightLimit = 255; // Buffer doesn't matter, handshake!
	}
	else {
		chunkHeightLimit = 256 / rowBytesClipped;
		if (chunkHeightLimit > maxChunkHeight) chunkHeightLimit = maxChunkHeight;
		else if (chunkHeightLimit < 1)         chunkHeightLimit = 1;
	}

	for (i = rowStart = 0; rowStart < h; rowStart += chunkHeightLimit) {
		// Issue up to chunkHeightLimit rows at a time:
		chunkHeight = h - rowStart;
		if (chunkHeight > chunkHeightLimit) chunkHeight = chunkHeightLimit;

		writeBytes(ASCII_DC2, '*', chunkHeight, rowBytesClipped);

		for (y = 0; y < chunkHeight; y++) {
			for (x = 0; x < rowBytesClipped; x++, i++) {
				timeoutWait();
				stream->write(fromProgMem ? pgm_read_byte(bitmap + i) : *(bitmap + i));
			}
			i += rowBytes - rowBytesClipped;
		}
		timeoutSet(chunkHeight * dotPrintTime);
	}
	prevByte = '\n';
}

void ThermalPrinter::printBitmap(int w, int h, Stream * fromStream)
{
	int rowBytes, rowBytesClipped, rowStart, chunkHeight, chunkHeightLimit,
		x, y, i, c;

	rowBytes = (w + 7) / 8; // Round up to next byte boundary
	rowBytesClipped = (rowBytes >= 48) ? 48 : rowBytes; // 384 pixels max width

														// Est. max rows to write at once, assuming 256 byte printer buffer.
	if (dtrEnabled) {
		chunkHeightLimit = 255; // Buffer doesn't matter, handshake!
	}
	else {
		chunkHeightLimit = 256 / rowBytesClipped;
		if (chunkHeightLimit > maxChunkHeight) chunkHeightLimit = maxChunkHeight;
		else if (chunkHeightLimit < 1)         chunkHeightLimit = 1;
	}

	for (rowStart = 0; rowStart < h; rowStart += chunkHeightLimit) {
		// Issue up to chunkHeightLimit rows at a time:
		chunkHeight = h - rowStart;
		if (chunkHeight > chunkHeightLimit) chunkHeight = chunkHeightLimit;

		writeBytes(ASCII_DC2, '*', chunkHeight, rowBytesClipped);

		for (y = 0; y < chunkHeight; y++) {
			for (x = 0; x < rowBytesClipped; x++) {
				while ((c = fromStream->read()) < 0);
				timeoutWait();
				stream->write((uint8_t)c);
			}
			for (i = rowBytes - rowBytesClipped; i>0; i--) {
				while ((c = fromStream->read()) < 0);
			}
		}
		timeoutSet(chunkHeight * dotPrintTime);
	}
	prevByte = '\n';
}

void ThermalPrinter::printBitmap(Stream * fromStream)
{
	uint8_t  tmp;
	uint16_t width, height;

	tmp = fromStream->read();
	width = (fromStream->read() << 8) + tmp;

	tmp = fromStream->read();
	height = (fromStream->read() << 8) + tmp;

	printBitmap(width, height, fromStream);
}

void ThermalPrinter::offline()
{
	writeBytes(ASCII_ESC, '=', 0);
}

void ThermalPrinter::online()
{
	writeBytes(ASCII_ESC, '=', 1);
}

void ThermalPrinter::sleep()
{
	sleepAfter(1); // Can't be 0, that means 'don't sleep'
}

void ThermalPrinter::sleepAfter(uint16_t seconds)
{
	writeBytes(ASCII_ESC, '8', seconds, seconds >> 8);
}

void ThermalPrinter::wake()
{
	timeoutSet(0);   // Reset timeout counter
	writeBytes(255); // Wake
	delay(50);
	writeBytes(ASCII_ESC, '8', 0, 0); // Sleep off (important!)
}

bool ThermalPrinter::hasPaper()
{
	writeBytes(ASCII_ESC, 'v', 0);

	int status = -1;
	for (uint8_t i = 0; i<10; i++) {
		if (stream->available()) {
			status = stream->read();
			break;
		}
		delay(100);
	}

	return !(status & 0b00000100);
}

void ThermalPrinter::setLineHeight(int val)
{
	if (val < 24) val = 24;
	lineSpacing = val - 24;

	// The printer doesn't take into account the current text height
	// when setting line height, making this more akin to inter-line
	// spacing.  Default line spacing is 30 (char height of 24, line
	// spacing of 6).
	writeBytes(ASCII_ESC, '3', val);
}

void ThermalPrinter::setMaxChunkHeight(int val)
{
	maxChunkHeight = val;
}

void ThermalPrinter::setCharset(uint8_t val)
{
	if (val > 15) val = 15;
	writeBytes(ASCII_ESC, 'R', val);
}

void ThermalPrinter::setCodePage(uint8_t val)
{
	if (val > 47) val = 47;
	writeBytes(ASCII_ESC, 't', val);
}

void ThermalPrinter::tab()
{
	writeBytes(ASCII_TAB);
	column = (column + 4) & 0b11111100;
}

void ThermalPrinter::setCharSpacing(int spacing)
{
	writeBytes(ASCII_ESC, ' ', spacing);
}
