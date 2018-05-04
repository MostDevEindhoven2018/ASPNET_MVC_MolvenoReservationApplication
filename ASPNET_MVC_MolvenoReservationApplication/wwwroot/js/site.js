// Write your JavaScript code.

//SCRIPT ONE STARTS
/// This lets the min value of calendat_date go to today so you can make a reservation on a day in the past
/// First makes a value for today in the same layout as the input from the DateTimePicker
var today = new Date();
                        var dd = today.getDate();
                        var mm = today.getMonth() + 1; // January is 0
                        var yyyy = today.getFullYear();
                        if (dd < 10) {
        dd = '0' + dd
    }
    if (mm < 10) {
        mm = '0' + mm
    }

    today = yyyy + '-' + mm + '-' + dd;
                        /// Changes the min attribute of calender_Date to today
                        /// Needs an Id attribute; getElementByName didn't work
                        //document.getElementById("calendar_Date").setAttribute("min", today);
                        document.getElementById("minimumdate").setAttribute("min", today);

//SCRIPT ONE ENDS


//SCRIPT TWO STARTS
// Script to not let you select a time (hour/minutes) in the past when today is selected in the DateTimePicker
// In calendar_Date input need: onchange="myFunction(),MyFunction2()      /// Changes the hours and minutes when the input value of DateTimePicker changes (e.g. when today is selected reset the comboboxes, cannot reserve in past (hour/minutes))
// In cmb_Time input need: onchange="MyFunction2()                        /// Changes the minutes when the input value of cmb_Time changes (e.g. for this hour cannot reserve in past (minutes))
// Need two new Date(); or else cannot use the step when the hour is almost over
    var today2 = new Date();
                    var today3 = new Date();
                    var dd2 = today2.getDate();
                    var mm2 = today2.getMonth() + 1; // January is 0
                    var yy2 = today2.getFullYear();
                    if (dd2 < 10) {
        dd2 = '0' + dd2
    }
    if (mm2 < 10) {
        mm2 = '0' + mm2
    }

    today2Date = yy2 + '-' + mm2 + '-' + dd2;

                    //For TEST command these three lines
                    var todayHour2 = today2.getHours();
                    var todayHour3 = today3.getHours();
                    var todayMin2 = today2.getMinutes();

                    ////// TEST ///
                    ////var todayHour2 = 12;
                    ////var todayHour3 = 12;
                    ////var todayMin2 = 24;
                    ////// TEST ///


                    // 12=[1]; 13=[2];...21=[10]
                    // Have to make number the same as the index of cmb_Time
                    todayHour2 = (todayHour2 - 11);

                    // have to make the index one index higher because the hour is almost over; can only choose future hours
                    var todayHour3 = (todayHour3 - 11) + 1;


                    // 00=[1]; 15=[2]; 30=[3]; 45=[4]
                    // Have to number the same as in the index of cmb_Minutes + 1, should only be able to choose future minutes
                    todayMin2 = Math.ceil((todayMin2 + 0.1) / 15) + 1;

                    function myFunction() {

                        // Reset first to disabled=false when Date is changed
                        for (var k = 1; k < 11; k++) {
        document.getElementById("cmb_Time").options[k].disabled = false;
    }

                        var date = document.getElementById("minimumdate").value;

                        // Chech if date in datepicker is the same as today
                        if (date == today2Date) {

        // Resets value in box
        document.getElementById("cmb_Time").value = null;

    // Check if the minutes in the time is above 45 minutes, which means the hour is almost over
    if (todayMin2 > 4) {

                                // For every index in cmb_Time, check if it is lower than the hour of the time now
                                // If true, then disable the option
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
                                // Because cannot reserve anymore. Could make a reservation for 21h at 20.59
                                if (todayHour2 == 10) {
                                    for (var z = 1; z < 11; z++) {
        document.getElementById("cmb_Time").options[z].disabled = true;
    }

                                }

                                // Don't have to make the index one index higher because the hour is isn't almost over
                                // uses todayHour2 = (todayHour2 - 11);
                                // For every idex in the cmb_Time, check if it is lower than the hour of the time now
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

                        // First reset
                        for (var x = 1; x < 5; x++) {
        document.getElementById("cmb_Minutes").options[x].disabled = false;
    }

                        var date2 = document.getElementById("minimumdate").value;
                        var selectedHour = document.getElementById("cmb_Time").selectedIndex;

                        // Cannot make a reservation for later than 21
                        if (selectedHour == 10) {
        document.getElementById("cmb_Minutes").options[1].disabled = false;
    document.getElementById("cmb_Minutes").options[2].disabled = true;
                            document.getElementById("cmb_Minutes").options[3].disabled = true;
                            document.getElementById("cmb_Minutes").options[4].disabled = true;

                        }

                        if (date2 == today2Date) {

        // Resets value in box
        document.getElementById("cmb_Minutes").value = null;

    // Cannot make a reservation for today after 21h
    if (todayHour2 >= 10) {
                                for (var a = 1; a < 11; a++) {
        document.getElementById("cmb_Minutes").options[a].disabled = true;
    }

                            }

                            // For every every index in cmb_Minutes, check if it is lower than index of the minutes now
                            // If true, disable the option
                            if (selectedHour == todayHour2) {

                                for (var j = 1; j < 5; j++) {

                                    if (j < todayMin2) {

        document.getElementById("cmb_Minutes").options[j].disabled = true;
    }
                                }

                            }
                        }

                    }
                                                                                                                                                                        ////+++++++++++++++++++++++++++++++++++++++++++++++++++++
//SCRIPT TWO ENDS

//SCRIPT THREE STARTS
 ///jQuery function that inputs the phonenumber and email and changes required to empty/false for one of them when the other one has a value

    jQuery(function ($) {

            var $inputs = $('input[name=txt_GuestPhoneNumber],input[name=e-box_GuestEmail]');
            $inputs.on('input', function () {
        $inputs.not(this).prop('required', !$(this).val().length);

    });
        });

//SCRIPT THREE ENDS

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
