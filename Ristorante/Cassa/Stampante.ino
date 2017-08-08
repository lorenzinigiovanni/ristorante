void print() {
	tmElements_t actualTime;

	Serial1.write(27);				// testo centrato
	Serial1.write(97);
	Serial1.write(49);

	Serial1.write(29);				// dimensione testo grande
	Serial1.write(33);
	Serial1.write(83);

	Serial1.write("N");
	Serial1.write(248);
	Serial1.print(orderNumber);
	Serial1.write(10);

	Serial1.write(29);				// dimensione testo normale
	Serial1.write(33);
	Serial1.write(byte(0));

	Serial1.print(heading);
	Serial1.print("\n\n");

	Serial1.write(27);				// testo a sinistra
	Serial1.write(97);
	Serial1.write(48);

	for (int i = 0; i < 18; i++) {
		if (platesNumber[i] > 0) {
			Serial1.print(platesNumber[i]);
			Serial1.write(" ");
			Serial1.print(printerPlateDescription[i]);
			Serial1.write(27);		// posizione testo personalizzata
			Serial1.write(36);
			Serial1.write(55);
			Serial1.write(1);
			if (bill[i] < 100.0) {
				Serial1.print(" ");
			}
			if (bill[i] < 10.0) {
				Serial1.print(" ");
			}
			Serial1.print(bill[i]);
			Serial1.write(10);
		}
		delay(50);
	}
	delay(50);

	Serial1.print("\nTOTALE");

	Serial1.write(27);				// posizione testo personalizzata
	Serial1.write(36);
	Serial1.write(20);
	Serial1.write(1);
	Serial1.print(" ");
	if (totalBill < 100.0) {
		Serial1.print(" ");
	}
	if (totalBill < 10.0) {
		Serial1.print(" ");
	}
	Serial1.write(213);				// simbolo €
	Serial1.print(" ");
	Serial1.print(totalBill);

	Serial1.write(27);				// testo centrato
	Serial1.write(97);
	Serial1.write(49);

	if (RTC.read(actualTime)) {
		Serial1.write(10);
		printDigits(actualTime.Day);
		Serial1.print("/");
		printDigits(actualTime.Month);
		Serial1.print("/");
		printDigits(tmYearToCalendar(actualTime.Year));
		Serial1.print("             ");
		printDigits(actualTime.Hour);
		Serial1.print(":");
		printDigits(actualTime.Minute);
		Serial1.print(":");
		printDigits(actualTime.Second);
	}

	Serial1.print("\n\n");

	delay(500);
	printBarcode(orderNumber);
	delay(500);

	Serial1.print(footer);
	Serial1.print("\n\nDeveloped by Lorenzini Giovanni\n\n\n");
}


void printBarcode(int n) {
	Serial1.write(29);
	Serial1.write(107);
	Serial1.write(73);

	char barCode[12];

	for (int i = 0; i < 12; i++)
		barCode[i] = 47;			// riempo l'array di caratteri non numerici

	itoa(n, barCode, 10);

	int empty = 0;
	for (int i = 0; i < 12; i++)
		if (barCode[i] == 47)
			empty += 1;				// calcolo il numero di caratteri non numerici

	int m = 12 - empty;

	if (m < 2) {
		Serial1.write(2);
		Serial1.write(48);			// aggiungo uno zero (CODE128 minimo 2 caratteri)
	}
	else {
		Serial1.write(m);
	}

	for (int i = 0; i < m; i++)
		Serial1.write(barCode[i]);	// stampo solo i caratteri numerici
}


void printDigits(int digits) {
	if (digits < 10)
		Serial1.print('0');
	Serial1.print(digits);
}