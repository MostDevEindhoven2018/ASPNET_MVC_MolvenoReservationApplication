// Write your JavaScript code.

///// SCRIPT ONE STARTS
$("#date").datepicker(
    {
        dateFormat: "d-m-yy",
        changeYear: true
    }
);
//SCRIPT ONE ENDS

///// SCRIPT TWO STARTS
///// This lets the min value of calendar_date go to today so you cannot make a reservation on a day in the past
///// First makes a value for today in the same layout as the input for the DateTimePicker
var today = new Date();
var dd = today.getDate();
var mm = today.getMonth() + 1; // January is 0
var yyyy = today.getFullYear();

today = yyyy + '-' + mm + '-' + dd;

/// Changes the min attribute of calendar_Date to today
$('#date').datepicker('option', 'minDate', new Date(today));

//SCRIPT TWO ENDS

//SCRIPT THREE STARTS
// Script to not let you select a time (hour/minutes) in the past when today is selected in the DateTimePicker
// In calendar_Date input need: onchange="myFunction(),MyFunction2()      /// Changes the hours and minutes when the input value of DateTimePicker changes (e.g. when today is selected reset the comboboxes, cannot reserve in past (hour/minutes))
// In cmb_Time input need: onchange="MyFunction2()                        /// Changes the minutes when the input value of cmb_Time changes (e.g. for this hour cannot reserve in past (minutes))

// Need two new Date(); or else cannot use the step when the hour is almost over
var today2 = new Date();
var today3 = new Date();
var dd2 = today2.getDate();
var mm2 = today2.getMonth() + 1; // January is 0
var yy2 = today2.getFullYear();

// Comparison date for the date selected
today2Date = dd2 + '-' + mm2 + '-' + yy2;

//For TEST command these three lines
var todayHour2 = today2.getHours();
var todayHour3 = today3.getHours();
var todayMin2 = today2.getMinutes();

////// TEST ///
////var todayHour2 = 16;
////var todayHour3 = 16;
////var todayMin2 = 40;
////// TEST ///


// 12=[1]; 13=[2];...21=[10]
// Have to make number the same as the index of cmb_Time
todayHour2 = (todayHour2 - 11);

// Have to make the index one index higher because the hour is almost over; can only choose future hours
todayHour3 = (todayHour3 - 11) + 1;


// 00=[1]; 15=[2]; 30=[3]; 45=[4]
// Have to number the same as in the index of cmb_Minutes + 1, should only be able to choose future minutes
////// todayMin2 + 0.1: to not exactly have the index number of this quarter of an hour; can change this (5.1: cant reserve 5 minutes before the end of quarter of this hour)
////// todayMin / 15: get the indexnumber
////// (todayMin2 + 0.1) / 15) + 1 :  index of cmb_Minutes + 1; can change this to 2 (can't reserve for coming 15-30 min of this hour) 
todayMin2 = Math.ceil((todayMin2 + 0.1) / 15) + 1;

// Function for the comboxes hour
function myFunction() {

    // Reset first the disabled attributes of the comboboxes hour to false when the selected date is changed
    for (var k = 1; k < 11; k++) {
        document.getElementById("cmb_Time").options[k].disabled = false;
    }

    // Get the value of the selected date
    var date = document.getElementById("date").value;

    // Check if the selected date in datepicker is the same as today
    if (date === today2Date) {

        // Resets value in box because today was selected or else the combobox could show a value that cannot be selected for today
        document.getElementById("cmb_Time").value = null;

        // Check if the minutes in the time is above 45 minutes, which means the hour is almost over (or selected time you can't reserve anymore for this hour)
        if (todayMin2 > 4) {

            // For every index in cmb_Time, check if hour value is lower than the hour of the time now
            // If true, then disable the option for that hour
            // Don't have to disable the minutes because can only choose future hours
            for (var y = 1; y < 11; y++) {
                if (y < todayHour3) {
                    document.getElementById("cmb_Time").options[y].disabled = true;
                }
            }
        }
        // If the hour isn't almost done
        else {

            // Checks if you are reserving at 21h. If you are, it will disable all hours
            // Because cannot reserve anymore. Could make a reservation for 21h at 20.59 (if you have an increment of 0.1 to the todayMin2; if 5.1 can till 20.54)
            if (todayHour2 === 10) {
                for (var z = 1; z < 11; z++) {
                    document.getElementById("cmb_Time").options[z].disabled = true;
                }

            }

            // Don't have to make the index one index higher because the hour is isn't almost over
            // uses todayHour2 = (todayHour2 - 11);
            // For every hour (index) in the cmb_Time, check if it is lower than the hour of the time now
            // If true, then disable the option
            for (var i = 1; i < 11; i++) {
                if (i < todayHour2) {

                    document.getElementById("cmb_Time").options[i].disabled = true;

                }
            }
        }
    }
}

function MyFunction2() {

    // First reset disabling of the options in combobox minutes when the date is changed and the hour is changed; does not delete value shown in the combobox minutes
    for (var x = 1; x < 5; x++) {
        document.getElementById("cmb_Minutes").options[x].disabled = false;
    }

    // Get the index of the selected hour that is chosen in the combobox
    var selectedHour = document.getElementById("cmb_Time").selectedIndex;
    // If the selected hour is 21h, reset value combobox minutes to 00 and disable other options. Cannot make a reservation for later than 21
    if (selectedHour === 10) {

        // Resets value of combobox value automatically to 00 when 21h was selected
        document.getElementById("cmb_Minutes").value = document.getElementById("cmb_Minutes").options[1].value;
        // Disables 15, 30 and 45 minutes in combobox for minutes
        document.getElementById("cmb_Minutes").options[1].disabled = false;
        document.getElementById("cmb_Minutes").options[2].disabled = true;
        document.getElementById("cmb_Minutes").options[3].disabled = true;
        document.getElementById("cmb_Minutes").options[4].disabled = true;
    }

    // Get the value of the selected date (calendar input)
    var date2 = document.getElementById("date").value;

    if (date2 === today2Date) {

        // For every every index in cmb_Minutes, check if it is lower than index of the minutes now
        // If true, disable the option
        if (selectedHour === todayHour2) {

            for (var j = 1; j < 5; j++) {

                if (j < todayMin2) {

                    // Resets value in box because todays hour was selected or else the combobox could show a value that cannot be selected for today
                    document.getElementById("cmb_Minutes").value = null;

                    document.getElementById("cmb_Minutes").options[j].disabled = true;
                }
            }                     

        }
    }

}

function MyFunction3() {

    // Get the value of the selected date (calendar input)
    var date2 = document.getElementById("date").value;

    if (date2 === today2Date) {

        // Resets value in box because today was selected or else the combobox could show a value that cannot be selected for today
        document.getElementById("cmb_Minutes").value = null;

        // Disables combobox minutes after 21h
        if (todayHour2 > 9) {
            // Disables 00, 15, 30 and 45 minutes in combobox for minutes
            for (var b = 1; b < 5; b++) {
                document.getElementById("cmb_Minutes").options[b].disabled = true;
            }
        }
    }
}


////////+++++++++++++++++++++++++++++++++++++++++++++++++++++
//////SCRIPT THREE ENDS

//SCRIPT FOUR STARTS
///jQuery function that inputs the phonenumber and email and changes required to empty/false for one of them when the other one has a value

jQuery(function ($) {

    var $inputs = $('input[id=phone],input[id=email]');
    $inputs.on('input', function () {
        $inputs.not(this).prop('required', !$(this).val().length);

    });
});

//SCRIPT FOUR ENDS



//PERHAPS NECESSARY CODE:
//@* Use this when inputboxes are aligned to the right *@
//                @* <div class="form-group">
//    <label asp-for="_resArrivingTime.Minute" for="cmb_Minutes" class="control-label"></label>
//    <select asp-for="_resArrivingTime.Minute" class="form-control-static comboboxMinutes" type="number" max="10" min="0" required tabindex="3" id="cmb_Minutes" name="cmb_Minutes">
//        <option disabled selected hidden></option>  Added this option so when you start you don't see 12:00, but you cannot select it (hidden)
//                            <option value="Minutes00">00</option>
//        <option value="Minutes15">15</option>
//        <option value="Minutes30">30</option>
//        <option value="Minutes45">45</option>
//    </select>
//    <span asp-validation-for="_resArrivingTime.Minute" class="text-danger"></span>

//    <label asp-for="_resArrivingTime.Hour" for="cmb_Time" class="control-label">Time:</label>
//    <select asp-for="_resArrivingTime.Hour" class="form-control-static combobox" type="number" max="10" min="0" required tabindex="2" onchange="MyFunction2()" id="cmb_Time" name="cmb_Time">
//        <option disabled selected hidden></option>  Added this option so when you start you don't see 12:00, but you cannot select it (hidden)
//                            <option value="Hour12">12</option>
//        <option value="Hour1">13</option>
//        <option value="Hour2">14</option>
//        <option value="Hour3">15</option>
//        <option value="Hour4">16</option>
//        <option value="Hour5">17</option>
//        <option value="Hour6">18</option>
//        <option value="Hour7">19</option>
//        <option value="Hour8">20</option>
//        <option value="Hour9">21</option>
//    </select>
//    <span asp-validation-for="_resArrivingTime.Hour" class="text-danger"></span>

//</div> *@
