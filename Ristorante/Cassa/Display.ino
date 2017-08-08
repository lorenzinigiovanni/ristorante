void displayUpdate() {
	lcd.setCursor(0, 0);
	lcd.print("Piatto ");
	lcd.print(totalPlatesNumber);
	lcd.print("         ");
}


void lcd_progress_bar(int row, int var, int minVal, int maxVal) {
	int block = map(var, minVal, maxVal, 0, 16);
	int line = map(var, minVal, maxVal, 0, 80);
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

	for (int x = 0; x < block; x++)
	{
		lcd.setCursor(x, row);
		lcd.write(1023);
	}

	lcd.setCursor(block, row);
	if (bar != 0) lcd.write(bar);
	if (block == 0 && line == 0) lcd.write(1022);

	for (int x = 16; x > block; x--)
	{
		lcd.setCursor(x, row);
		lcd.write(1022);
	}
}