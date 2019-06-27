//-------------------------------librerie---------------------------------------------

#include <RTClib.h>
#include <TimeLib.h>
#include <Time.h>
#include <LiquidCrystal_I2C.h>
#include <Wire.h>
#include <Ethernet.h> // W5100
//#include <Ethernet2.h> // W5500
#include "ThermalPrinter.h"
//#include "headingLogo.h" // Abilitazione logo testa scontrino
//#include "footerLogo.h"	// Abilitazione logo fondo scontrino

//-------------------------------define-----------------------------------------------

#define cashRegisterNumber 3  // RICORDATI DI MODIFICARLO PRIMA DI CARICARE DIO BOIAAAAA!!!!!

#define printingMode 0		// Mettere a 1 per stampa separata o a 0 per stampa scontrino unico

#define settingTime 0   // Mettere a 1 per impostare l'ora o a 0 per il funzionamento normale

#define platesNumber 18
#define buttonsNumber 20

#define bufferLenght 8
#define printerLenght 32
#define displayLenght 16
#define displayHeight 2

//-------------------------------tipi-------------------------------------------------

typedef char PrinterString[printerLenght + 1];
typedef char DisplayString[displayLenght + 1];
typedef char BufferString[bufferLenght + 1];

//-------------------------------strutture--------------------------------------------

struct Plate {
	PrinterString printerDescription;
	DisplayString displayDescription;

	int quantity;
	int effective;

	float cost;
	float bill;

	Plate() {
		quantity = 0;
		effective = 0;

		cost = 0.0;
		bill = 0.0;
	}
};

struct Button {
	int pin;
	bool oldState;

	Button(int x) {
		pin = x;
	}
};

//-------------------------------stampante--------------------------------------------

ThermalPrinter printer(&Serial1, 4);

//-------------------------------RTC--------------------------------------------------

RTC_DS3231 rtc;

//-------------------------------definizione-pin--------------------------------------

LiquidCrystal_I2C lcd(0x3F, displayLenght, displayHeight);

Button buttons[buttonsNumber] = { 41, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 22, 23, 24, 25, 26, 27, 28, 29, 30 };

//-------------------------------variabili-comunicazione-udp--------------------------

byte mac[] = { 0x00, 0xAA, 0xBB, 0xCC, 0xDE, 0x05 + cashRegisterNumber };
byte ip[] = { 192, 168, 1, 200 + cashRegisterNumber };

byte server[] = { 192, 168, 1, 40 };
const unsigned int port = 50200 + cashRegisterNumber;

EthernetClient client;

//-------------------------------variabili-per-ordini---------------------------------

int orderNumber = 0;
int totalPlatesNumber = 0;
float totalBill = 0.0;

//-------------------------------variabili-piatti-------------------------------------

Plate plates[platesNumber];

//-------------------------------intestazione-e-pie-di-pagina-------------------------

PrinterString heading;
PrinterString footer;

//////////////////////////////////////////////////////////////////////////////////////

void setup() {
	// Serial.begin(115200);

	//---------------------------setup-RTC--------------------------------------------
	
	rtc.begin();
#if settingTime
	rtc.adjust(DateTime(F(__DATE__), F(__TIME__)));
#endif

	//---------------------------setup-pin--------------------------------------------

	for (int i = 0; i < buttonsNumber; i++) {
		pinMode(buttons[i].pin, INPUT_PULLUP);
		buttons[i].oldState = digitalRead(buttons[i].pin);
	}

	//---------------------------setup-display----------------------------------------

	lcd.init();
	lcd.backlight();

	lcd.setCursor(0, 0);
	lcd.print(F("    Starting    "));

	for (int i = 0; i <= displayLenght * 5; i++) {
		lcdProgressBar(1, i, 0, displayLenght * 5);
		delay(10);
	}

	//---------------------------setup-stampante--------------------------------------

	Serial1.begin(19600);

	printer.begin();
	printer.setDefault();

	printer.setCharset(CHARSET_ITALY);
	printer.setCodePage(CODEPAGE_ISO_8859_15);

	printer.setBarcodeHeight(50);
	printer.setBarcodeWidht(4);

	//---------------------------setup-comunicazione----------------------------------

	if (!Ethernet.begin(mac)) {
		Ethernet.begin(mac, ip);
	}

	// Serial.println(Ethernet.localIP());

	//---------------------------sincronizzazione-con-PC------------------------------

	getInfo();

	lcd.setCursor(0, 0);
	lcd.print(F("    Started     "));
	lcd.setCursor(0, 1);
	lcd.print(F("Ristorante v 2.0"));
}

//////////////////////////////////////////////////////////////////////////////////////

void loop() {
	//---------------------------pulsante-reset---------------------------------------

	if (edge(&buttons[0])) {
		if (totalPlatesNumber > 0) {
			reset();
			displayUpdate(-1);
		}
	}

	//---------------------------pulsante-invio---------------------------------------

	if (edge(&buttons[1])) {

		if (totalPlatesNumber > 0) {
			sendOrder();

			if (orderNumber == 0) {
				lcd.setCursor(0, 0);
				lcd.print(F("     Ordine     "));
				lcd.setCursor(0, 1);
				lcd.print(F("   non Valido   "));
				delay(1000);
			}
			else {
				for (int i = 0; i < platesNumber; i++) {
					plates[i].bill = plates[i].effective * plates[i].cost;
					totalBill += plates[i].bill;
				}

				DisplayString buffer;
				snprintf(buffer, sizeof(DisplayString), "Totale %6d.%02d", (unsigned)totalBill, (unsigned)(totalBill * 100.0) % 100);
        buffer[sizeof(DisplayString) - 1] = '\0';

				lcd.setCursor(0, 0);
				lcd.print(buffer);

				if (!printer.hasPaper()) {
					lcd.setCursor(0, 0);
					lcd.print(F(" Inserire Carta "));

					while (!printer.hasPaper()) {
						delay(1);
					}

					lcd.setCursor(0, 0);
					lcd.print(F(" Stampa  Ordine "));

					delay(2000);
				}

				printOrder();

       delay(1000);

				if (!printer.hasPaper()) {
					lcd.setCursor(0, 0);
					lcd.print(F(" Inserire Carta "));

					while (!printer.hasPaper()) {
						delay(1);
					}

					lcd.setCursor(0, 0);
					lcd.print(F("Ristampa  Ordine"));

					delay(5000);

					printOrder();
				}
			}

			bool notAvailable = false;

			for (int i = 0; i < platesNumber; i++) {
				if (plates[i].quantity != plates[i].effective) {
					notAvailable = true;
				}
			}
			
			if (notAvailable) {
				lcd.setCursor(0, 0);
				lcd.print(F(" ALCUNI  PIATTI "));
				lcd.setCursor(0, 1);
				lcd.print(F("NON  DISPONIBILI"));
				delay(1000);
			}

			reset();
			displayUpdate(-2);
		}
	}

	//---------------------------pulsanti-pasti---------------------------------------

	for (int i = 2; i < buttonsNumber; i++) {
		if (edge(&buttons[i])) {
			plates[i-2].quantity++;
			totalPlatesNumber++;
			displayUpdate(i-2);
			delay(250);
		}
	}
}

//////////////////////////////////////////////////////////////////////////////////////

void reset() {
	for (int i = 0; i < platesNumber; i++) {
		plates[i].bill = 0.0;
		plates[i].quantity = 0;
		plates[i].effective = 0;
	}

	totalPlatesNumber = 0;
	totalBill = 0.0;
}

//////////////////////////////////////////////////////////////////////////////////////

bool edge(Button *button) {
	bool state = digitalRead(button->pin);

	if (button->oldState && !state) {
		button->oldState = state;
		return true;
	}
	else {
		button->oldState = state;
		return false;
	}
}
