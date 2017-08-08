void sender() {
	lcd.setCursor(0, 0);
	lcd.print("  Invio ordine  ");
	lcd.setCursor(0, 1);
	lcd.print("    in corso    ");

	client.connect(server, port);

	message = "ORDER";
	for (int i = 0; i < 18; i++) {
		message += String(platesNumber[i]);
		message += ",";
	}

	client.print(message);
	client.flush();

	message = "";

	while (!client.available()) {
		delay(1);
	}

	int i = 0;

	while (client.available()) {
		char c = client.read();
		if (c == 10)
			break;
		packetBuffer[i] = c;
		i++;
	}

	String received = String(packetBuffer);

	for (int i = 0; i < 18; i++) {
		int posComma = received.indexOf(',');
		platesNumber[i] = received.substring(0, posComma).toInt();
		received.remove(0, posComma + 1);
	}

	int posComma = received.indexOf(',');
	orderNumber = received.substring(0, posComma).toInt();

	memset(packetBuffer, 0, sizeof(packetBuffer));

	client.stop();

	lcd.clear();
	lcd.setCursor(0, 0);
	lcd.print(" Ordine Inviato ");
	lcd.setCursor(0, 1);
	lcd.print(orderNumber);
}


void getInfo() {
	lcd.setCursor(0, 0);
	lcd.print("Sincronizzazione");
	lcd.setCursor(0, 1);
	lcd.print("  Informazioni  ");

	client.connect(server, port);

	client.print("INFO");
	client.flush();

	while (!client.available()) {
		delay(1);
	}

	int i = 0;

	while (client.available()) {
		char c = client.read();
		if (c == 10)
			break;
		packetBuffer[i] = c;
		i++;
	}

	String received = String(packetBuffer);

	int posSemicolon = received.indexOf(';');
	int posAt = received.indexOf('@');
	int posDollar = received.indexOf('$');
	int posHash = received.indexOf('#');
		
	heading = received.substring(posAt + 1, posDollar);
	footer = received.substring(posDollar + 1, posHash);
		
	String descrizioneStampante = received.substring(0, posSemicolon);
	for (int i = 0; i < 18; i++) {
		int posComma = descrizioneStampante.indexOf(',');
		printerPlateDescription[i] = descrizioneStampante.substring(0, posComma);
		descrizioneStampante.remove(0, posComma + 1);
	}

	String descrizioneDisplay = received.substring(posSemicolon + 1, posAt);
	for (int i = 0; i < 18; i++) {
		int posComma = descrizioneDisplay.indexOf(',');
		displayPlateDescription[i] = descrizioneDisplay.substring(0, posComma);
		descrizioneDisplay.remove(0, posComma + 1);
	}

	String prezzi = received.substring(posHash + 1);
	for (int i = 0; i < 18; i++) {
		int posComma = prezzi.indexOf(',');
		plateCost[i] = prezzi.substring(0, posComma).toFloat();
		prezzi.remove(0, posComma + 1);
	}

	memset(packetBuffer, 0, sizeof(packetBuffer));

	client.stop();

	lcd.setCursor(0, 0);
	lcd.print("  Informazioni  ");
	lcd.setCursor(0, 1);
	lcd.print("       OK       ");
}