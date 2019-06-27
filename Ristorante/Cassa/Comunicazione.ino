void sendOrder() {
	lcd.setCursor(0, 0);
	lcd.print(F("  Invio Ordine  "));
	lcd.setCursor(0, 1);
	lcd.print(F("    in Corso    "));

	if (client.connect(server, port)) {
		client.print(F("ORDER"));
		for (int i = 0; i < platesNumber; i++) {
			client.print(plates[i].quantity);
			client.print(F(","));
		}
		client.println();

		client.flush();

		while (client.connected() && !client.available()) {
			delay(1);
		}

		BufferString buffer = { '\0' };
		for (int i = 0; i < platesNumber; i++) {
			client.readBytesUntil(',', buffer, sizeof(BufferString));
			buffer[sizeof(BufferString) - 1] = '\0';
			plates[i].effective = atoi(buffer);      
			memset(buffer, '\0', sizeof(BufferString));
		}

		client.readBytesUntil(',', buffer, sizeof(BufferString));
    buffer[sizeof(BufferString) - 1] = '\0';
		orderNumber = atoi(buffer);

		do {
			client.flush();
			client.stop();
		} while (client.connected());

		lcd.setCursor(0, 0);
		lcd.print(F("  Invio Ordine  "));
		lcd.setCursor(0, 1);
		lcd.print(F("       OK       "));
	}
	else {
		orderNumber = 0;
	}
}

//////////////////////////////////////////////////////////////////////////////////////

/*
int sendPlate(int plate) {
	int toReturn = -1;

	if (client.connect(server, port)) {
		client.print("ORDER");
		for (int i = 0; i < platesNumber; i++) {
			if (i == plate)
				client.print("1");
			else
				client.print("0");
			client.print(",");
		}
		client.println();

		client.flush();

		while (client.connected() && !client.available()) {
			delay(1);
		}

		BufferString buffer = { '\0' };
		for (int i = 0; i < platesNumber; i++) {
			client.readBytesUntil(',', buffer, sizeof(BufferString));
			if (i == plate)
				toReturn = atoi(buffer);
			memset(buffer, '\0', sizeof(BufferString));
		}

		client.readBytesUntil(',', buffer, sizeof(BufferString));
		orderNumber = atoi(buffer);

		do {
			client.flush();
			client.stop();
		} while (client.connected());
	}

	return toReturn;
}
*/

//////////////////////////////////////////////////////////////////////////////////////

void getInfo() {
	lcd.setCursor(0, 0);
	lcd.print(F("Sincronizzazione"));
	lcd.setCursor(0, 1);
	lcd.print(F("  Informazioni  "));

	if (client.connect(server, port)) {
		client.println(F("INFO"));

		client.flush();

		while (client.connected() && !client.available()) {
			delay(1);
		}

		for (int i = 0; i < platesNumber; i++) {
			client.readBytesUntil(',', plates[i].printerDescription, sizeof(PrinterString));
			plates[i].printerDescription[sizeof(PrinterString) - 1] = '\0';
		}

		for (int i = 0; i < platesNumber; i++) {
			client.readBytesUntil(',', plates[i].displayDescription, sizeof(DisplayString));
      plates[i].displayDescription[sizeof(DisplayString) - 1] = '\0';
		}

		client.readBytesUntil(',', heading, sizeof(PrinterString));
    heading[sizeof(PrinterString) - 1] = '\0';
    
		client.readBytesUntil(',', footer, sizeof(PrinterString));
    footer[sizeof(PrinterString) - 1] = '\0';

		BufferString buffer = { '\0' };
		for (int i = 0; i < platesNumber; i++) {
			client.readBytesUntil(',', buffer, sizeof(BufferString));
      buffer[sizeof(BufferString) - 1] = '\0';
			plates[i].cost = atof(buffer);
			memset(buffer, '\0', sizeof(BufferString));
		}

		do {
			client.flush();
			client.stop();
		} while (client.connected());

		lcd.setCursor(0, 0);
		lcd.print(F("  Informazioni  "));
		lcd.setCursor(0, 1);
		lcd.print(F("       OK       "));
	}
}
