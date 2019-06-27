void displayUpdate(int i) {
	DisplayString buffer = { '\0' };
	DisplayString mask = "Quantita %7d";

	if (i == -2) {
		lcd.setCursor(0, 0);
		snprintf(buffer, sizeof(DisplayString), mask, 0);
    buffer[sizeof(DisplayString) - 1] = '\0';
		lcd.print(buffer);

		lcd.setCursor(0, 1);
		lcd.print(F("Pronto per Nuovo"));
		
	}
	else if (i == -1) {
		lcd.setCursor(0, 0);
		snprintf(buffer, sizeof(DisplayString), mask, 0);
    buffer[sizeof(DisplayString) - 1] = '\0';
		lcd.print(buffer);

		lcd.setCursor(0, 1);
		lcd.print(F("Conto Resettato "));
	}
	else {
		lcd.setCursor(0, 0);
		snprintf(buffer, sizeof(DisplayString), mask, plates[i].quantity);
    buffer[sizeof(DisplayString) - 1] = '\0';
		lcd.print(buffer);

		lcd.setCursor(0, 1);
		snprintf(buffer, sizeof(DisplayString), "%-16s", plates[i].displayDescription);
    buffer[sizeof(DisplayString) - 1] = '\0';
		lcd.print(buffer);
	}
}

//////////////////////////////////////////////////////////////////////////////////////

void lcdProgressBar(int row, int var, int minVal, int maxVal) {
	int block = map(var, minVal, maxVal, 0, displayLenght);
	int line = map(var, minVal, maxVal, 0, displayLenght * 5);
	int bar = (line - (block * 5));

	byte bar1[8] = { 0x10, 0x10, 0x10, 0x10, 0x10, 0x10, 0x10, 0x10 };
	byte bar2[8] = { 0x18, 0x18, 0x18, 0x18, 0x18, 0x18, 0x18, 0x18 };
	byte bar3[8] = { 0x1C, 0x1C, 0x1C, 0x1C, 0x1C, 0x1C, 0x1C, 0x1C };
	byte bar4[8] = { 0x1E, 0x1E, 0x1E, 0x1E, 0x1E, 0x1E, 0x1E, 0x1E };
	byte bar5[8] = { 0x1F, 0x1F, 0x1F, 0x1F, 0x1F, 0x1F, 0x1F, 0x1F };

	lcd.createChar(1, bar1);
	lcd.createChar(2, bar2);
	lcd.createChar(3, bar3);
	lcd.createChar(4, bar4);
	lcd.createChar(5, bar5);

	for (int x = 0; x < block; x++)	{
		lcd.setCursor(x, row);
		lcd.write(1023);
	}

	lcd.setCursor(block, row);

	if (bar != 0) {
		lcd.write(bar);
	}

	if (block == 0 && line == 0) {
		lcd.write(1022);
	}

	for (int x = displayLenght; x > block; x--) {
		lcd.setCursor(x, row);
		lcd.write(1022);
	}
}
