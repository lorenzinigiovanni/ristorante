bool getTime(const char *str) {
	int Hour, Min, Sec;

	if (sscanf(str, "%d:%d:%d", &Hour, &Min, &Sec) != 3) {
		return false;
	}

	time.Hour = Hour;
	time.Minute = Min;
	time.Second = Sec;

	return true;
}


bool getDate(const char *str) {
	char *monthName[12] = { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
	char Month[12];
	int Day, Year;
	uint8_t monthIndex;

	if (sscanf(str, "%s %d %d", Month, &Day, &Year) != 3) {
		return false;
	}

	for (monthIndex = 0; monthIndex < 12; monthIndex++) {
		if (strcmp(Month, monthName[monthIndex]) == 0) {
			break;
		}
	}

	if (monthIndex >= 12) {
		return false;
	}

	time.Day = Day;
	time.Month = monthIndex + 1;
	time.Year = CalendarYrToTm(Year);

	return true;
}
