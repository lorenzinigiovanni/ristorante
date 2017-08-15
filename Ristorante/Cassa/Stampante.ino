void printOrder() {
	char buffer[50];

	printer.justify('C');
	printer.setSize('L');
	printer.print("#");
	printer.println(orderNumber);

	printer.setSize('S');
	printer.println(heading);

	printer.feed();

	printer.justify('L');

	for (int i = 0; i < 18; i++) {
		if (platesNumber[i] > 0) {
			sprintf(buffer, "%-2d %-22.22s %3d.%02d", platesNumber[i], printerPlateDescription[i].c_str(), (int)bill[i], (int)(bill[i] * 100) % 100);
			printer.println(buffer);
		}
	}

	printer.feed();

	sprintf(buffer, "TOTALE %22d.%02d", (int)totalBill, (int)(totalBill * 100.0) % 100);
	printer.println(buffer);

	if (RTC.read(time)) {
		printer.feed();
		sprintf(buffer, "%02d/%02d/%04d              %02d:%02d:%02d", time.Day, time.Month, tmYearToCalendar(time.Year), time.Hour, time.Minute, time.Second);
		printer.println(buffer);
	}

	printer.justify('C');

	sprintf(buffer, "%d", orderNumber);
	printer.printBarcode(buffer, CODE128);

	printer.println(footer);

	printer.fontB();
	printer.println("Developed by Lorenzini Giovanni");
	printer.fontA();

	printer.feed(2);
}