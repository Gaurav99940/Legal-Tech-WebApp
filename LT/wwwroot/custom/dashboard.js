//function showDropDownOption() {
//    //debugger;
//    $.ajax({
//        type: "GET",
//        url: '/SearchOption',
//        //data: { selectedValues: selectedValues },
//        contentType: "application.json",
//        dataType: "json",
//        success: function (response) {
//            var districtDropdown = $('#districtDropdown');
//            $.each(response, function (data, value) {
//                console.log(value.name)
//                districtDropdown.append('<option value="' + value.name + '">' + value.name + '</option>');
//            });
//        },
//        failure: function (response) {
//            alert(response);
//        }
//    });
//}

//$(document).ready(function () {
//    // Function to populate dropdown options
//    var selectedValues = "";
//    showDropDownOption();
//    // Function to handle search button click
//    $('#searchButton').click(function () {
//        selectedValues = $('#districtDropdown').val();
//        console.log(selectedValues);// Get selected values
//        searchInDatabase(selectedValues); // Call function to search in database
//    });
//});

//// Function to search in database
//function searchInDatabase(selectedValues) {
//    console.log(selectedValue);
//    $.ajax({
//        type: "GET", // Use POST method to send data to backend
//        url: '/SearchOption', // Endpoint for searching in database
//        contentType: "application/json",
//        data: JSON.stringify({ selectedValues: selectedValues }), // Send selected values as JSON
//        dataType: "json",
//        success: function (response) {
//            // Handle the response from the backend
//            console.log(response);
//        },
//        failure: function (response) {
//            alert(response);
//        }
//    });
//}
