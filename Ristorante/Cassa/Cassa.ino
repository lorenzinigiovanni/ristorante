//---------------------------librerie---------------------------------------------

#include <DS1307RTC.h>
#include <TimeLib.h>
#include <Time.h>
#include <LiquidCrystal_I2C.h>
#include <Wire.h>
#include <Ethernet.h>

//---------------------------definizione-pin--------------------------------------

LiquidCrystal_I2C lcd(0x27, 16, 2);

int buttonPin[20] = { 22, 24, 26, 28, 30, 32, 34, 36, 38, 23, 25, 27, 29, 31, 33, 35, 37, 39, 40, 41 };

int openDrawer = 43;

//---------------------------variabili-comunicazione-udp--------------------------

int cashRegisterNumber = 0;

byte mac[] = { 0x00, 0xAA, 0xBB, 0xCC, 0xDE, 0x02 };
IPAddress ip(192, 168, 1, 200 + cashRegisterNumber);

IPAddress server(192, 168, 1, 100);
const unsigned int port = 50200 + cashRegisterNumber;

String message = "";
char packetBuffer[999];

EthernetClient client;

//---------------------------variabili-per-ordini---------------------------------

int orderNumber = 1;

int platesNumber[18] = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
int totalPlatesNumber = 0;

float bill[18] = { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 };
float totalBill = 0.0;

//---------------------------variabili-piatti-default-----------------------------

String printerPlateDescription[] = {
	"Piatto 1",
	"Piatto 2",
	"Piatto 3",
	"Piatto 4",
	"Piatto 5",
	"Piatto 6",
	"Piatto 7",
	"Piatto 8",
	"Piatto 9",
	"Piatto 10",
	"Piatto 11",
	"Piatto 12",
	"Piatto 13",
	"Piatto 14",
	"Piatto 15",
	"Piatto 16",
	"Piatto 17",
	"Piatto 18"
};

String displayPlateDescription[] = {
	"Piatto 1        ",
	"Piatto 2        ",
	"Piatto 3        ",
	"Piatto 4        ",
	"Piatto 5        ",
	"Piatto 6        ",
	"Piatto 7        ",
	"Piatto 8        ",
	"Piatto 9        ",
	"Piatto 10       ",
	"Piatto 11       ",
	"Piatto 12       ",
	"Piatto 13       ",
	"Piatto 14       ",
	"Piatto 15       ",
	"Piatto 16       ",
	"Piatto 17       ",
	"Piatto 18       ",
};

float plateCost[18] = { 1.0, 2.0, 3.0, 4.0, 5.0, 6.0, 7.0, 8.0, 9.0, 10.0, 11.0, 12.0, 13.0, 14.0, 15.0, 16.0, 17.0, 18.0 };

//---------------------------intestazione-e-pie-di-pagina-------------------------

String heading = "LORENZINI GIOVANNI";
String footer = "BUON APPETITO!";


void setup()
{
	//---------------------------setup-comunicazione----------------------------------

	Ethernet.begin(mac, ip);

	//---------------------------setup-pin--------------------------------------------

	for (int i = 0; i < 20; i++) {
		pinMode(buttonPin[i], INPUT_PULLUP);
	}

	//---------------------------setup-display----------------------------------------

	lcd.init();
	lcd.backlight();

	lcd.setCursor(0, 0);
	lcd.print("    Starting    ");

	for (int i = 0; i <= 100; i++) {
		lcd_progress_bar(1, i, 0, 1000);
		delay(10);
	}

	//---------------------------setup-stampante--------------------------------------

	Serial1.begin(19600);			// inizializzazione seriale stampante

	Serial1.write(27);				// initialize the printer
	Serial1.write(64);

	Serial1.write(27);
	Serial1.write(55);
	Serial1.write(7);				// max printing dots
	Serial1.write(80);				// heating time
	Serial1.write(2);				// heating interval

	Serial1.write(18);				// printing density
	Serial1.write(35);
	Serial1.write(255);

	Serial1.write(27);				// tabella estesa europa
	Serial1.write(116);
	Serial1.write(19);

	Serial1.write(29);				// altezza codice a barre
	Serial1.write(104);
	Serial1.write(50);

	Serial1.write(29);				// larghezza codice a barre
	Serial1.write(119);
	Serial1.write(4);

	//---------------------------sincronizzazione-con-PC------------------------------

	getInfo();

	delay(1000);

	lcd.setCursor(0, 0);
	lcd.print("    Started     ");
	lcd.setCursor(0, 1);
	lcd.print("Ristorante v 2.0");

	delay(1000);

	lcd.setCursor(0, 1);
	lcd.print("Pronto per Nuovo");
	displayUpdate();
}


void loop()
{
	//---------------------------pulsanti-pasti---------------------------------------

	for (int i = 0; i < 18; i++)
		if (!digitalRead(buttonPin[i])) {
			platesNumber[i] ++;
			totalPlatesNumber++;
			displayUpdate();
			lcd.setCursor(0, 1);
			lcd.print(displayPlateDescription[i]);

			delay(200);
			while (!digitalRead(buttonPin[i])) {}
			delay(200);
		}

	//---------------------------pulsante-reset---------------------------------------

	if (!digitalRead(buttonPin[18])) {
		if (totalPlatesNumber > 0) {
			reset();
			displayUpdate();
			lcd.setCursor(0, 1);
			lcd.print("Conto Resettato ");
		}
		delay(200);
		while (!digitalRead(buttonPin[18])) {}
		delay(200);
	}

	//---------------------------pulsante-invio---------------------------------------

	if (!digitalRead(buttonPin[19])) {
		if (totalPlatesNumber > 0) {
			digitalWrite(openDrawer, HIGH);

			sender();

			for (int i = 0; i < 18; i++) {
				bill[i] = platesNumber[i] * plateCost[i];
				totalBill += bill[i];
			}

			lcd.setCursor(0, 0);
			lcd.print("     Totale     ");
			lcd.setCursor(5, 1);
			lcd.print(totalBill);

			print();

			reset();

			lcd.setCursor(0, 1);
			lcd.print("Pronto per Nuovo");
			displayUpdate();

			digitalWrite(openDrawer, LOW);
		}

		delay(200);
		while (!digitalRead(buttonPin[19])) {}
		delay(200);
	}
}


void reset() {
	memset(platesNumber, 0, sizeof(platesNumber));
	memset(bill, 0, sizeof(bill));

	totalPlatesNumber = 0;
	totalBill = 0.0;
}