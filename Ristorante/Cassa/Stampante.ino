void printOrder() {
	PrinterString buffer = { '\0' };

	printer.justify('C');
	printer.setSize('L');
	printer.print(F("#"));
	printer.println(orderNumber);

	printer.setSize('S');

#ifdef _headingLogo_h_
	printer.feed();
	printer.printBitmap(headingLogo_width, headingLogo_height, headingLogo_data);
	printer.feed();
#endif

	printer.println(heading);

	printer.feed();

	printer.justify('L');

	for (int i = 0; i < platesNumber; i++) {
		if (plates[i].effective > 0) {
			snprintf(buffer, sizeof(PrinterString), "%-2d %-22s %3d.%02d", plates[i].effective, plates[i].printerDescription, (int)plates[i].bill, (int)(plates[i].bill * 100) % 100);
			buffer[sizeof(PrinterString) - 1] = '\0';
			printer.println(buffer);
			memset(buffer, '\0', sizeof(PrinterString));
		}
	}

	printer.feed();

	snprintf(buffer, sizeof(PrinterString), "TOTALE %22d.%02d", (int)totalBill, (int)(totalBill * 100.0) % 100);
  buffer[sizeof(PrinterString) - 1] = '\0';
	printer.println(buffer);
	memset(buffer, '\0', sizeof(PrinterString));

	printer.feed();

	DateTime now = rtc.now();

	snprintf(buffer, sizeof(PrinterString), "%02d/%02d/%04d              %02d:%02d:%02d", now.day(), now.month(), now.year(), now.hour(), now.minute(), now.second());
	buffer[sizeof(PrinterString) - 1] = '\0';
	printer.println(buffer);
	memset(buffer, '\0', sizeof(PrinterString));

	printer.justify('C');

	snprintf(buffer, sizeof(PrinterString), "%d", orderNumber);
  buffer[sizeof(PrinterString) - 1] = '\0';
	printer.printBarcode(buffer, CODE128);

	printer.println(footer);

#ifdef _footerLogo_h_
	printer.feed();
	printer.printBitmap(footerLogo_width, footerLogo_height, footerLogo_data);
	printer.feed();
#endif

	printer.fontB();
	printer.println(F("Developed by Lorenzini Giovanni"));
	printer.fontA();

	printer.feed(2);
}

//////////////////////////////////////////////////////////////////////////////////////

/*
void printPlate(int plate) {

	// CUT PAPER

	PrinterString buffer = { '\0' };

	printer.justify('C');
	printer.setSize('L');
	printer.print(F("#"));
	printer.println(orderNumber);

	printer.setSize('M');

	printer.feed();

	printer.justify('L');

	sprintf(buffer, "%s", plates[plate].printerDescription);
	printer.println(buffer);

	printer.feed();

	printer.justify('C');

	sprintf(buffer, "%d", orderNumber);
	printer.printBarcode(buffer, CODE128);

	// CUT PAPER

	printer.feed(2);
}
*/
//////////////////////////////////////////////////////////////////////////////////////

/*
void printBill() {

	// CUT PAPER

	PrinterString buffer = { '\0' };

	printer.justify('C');
	printer.setSize('S');

#ifdef _headingLogo_h_
	printer.feed();
	printer.printBitmap(headingLogo_width, headingLogo_height, headingLogo_data);
	printer.feed();
#endif

	printer.println(heading);

	printer.feed();

	printer.justify('L');

	sprintf(buffer, "TOTALE %22d.%02d", (int)totalBill, (int)(totalBill * 100.0) % 100);
	printer.println(buffer);
	memset(buffer, '\0', sizeof(PrinterString));

	printer.feed();

	printer.justify('C');
	printer.println(footer);

#ifdef _footerLogo_h_
	printer.feed();
	printer.printBitmap(footerLogo_width, footerLogo_height, footerLogo_data);
	printer.feed();
#endif

	printer.fontB();
	printer.println(F("Developed by Lorenzini Giovanni"));
	printer.fontA();

	// CUT PAPER

	printer.feed(2);
}
*/
