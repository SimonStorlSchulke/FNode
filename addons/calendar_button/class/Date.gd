class_name Date

var day : int: set = set_day
var month : int: set = set_month
var year : int: set = set_year

func _init(day : int = Time.get_datetime_dict_from_system()["day"], 
		month : int = Time.get_datetime_dict_from_system()["month"], 
		year : int = Time.get_datetime_dict_from_system()["year"]):
	self.day = day
	self.month = month
	self.year = year

# Supported Date Formats:
# DD : Two digit day of month
# MM : Two digit month
# YY : Two digit year
# YYYY : Four digit year
func date(date_format = "DD-MM-YY") -> String:
	if("DD".is_subsequence_of(date_format)):
		date_format = date_format.replace("DD", str(day()).pad_zeros(2))
	if("MM".is_subsequence_of(date_format)):
		date_format = date_format.replace("MM", str(month()).pad_zeros(2))
	if("YYYY".is_subsequence_of(date_format)):
		date_format = date_format.replace("YYYY", str(year()))
	elif("YY".is_subsequence_of(date_format)):
		date_format = date_format.replace("YY", str(year()).substr(2,3))
	return date_format

func day() -> int:
	return day

func month() -> int:
	return month

func year() -> int:
	return year

func set_day(_day : int):
	day = _day

func set_month(_month : int):
	month = _month

func set_year(_year : int):
	year = _year

func change_to_prev_month():
	var selected_month = month()
	selected_month -= 1
	if(selected_month < 1):
		set_month(12)
		set_year(year() - 1)
	else:
		set_month(selected_month)

func change_to_next_month():
	var selected_month = month()
	selected_month += 1
	if(selected_month > 12):
		set_month(1)
		set_year(year() + 1)
	else:
		set_month(selected_month)

func change_to_prev_year():
	set_year(year() - 1)

func change_to_next_year():
	set_year(year() + 1)
