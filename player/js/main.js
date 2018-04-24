Testfile = "https://scenario2storage.blob.core.windows.net/asset-7a3e5529-b2ec-4bc2-816e-bfe7e7942cad/Movie on 4-23-18 at 12.36 PM_540x360_AACAudio_1000.mp4?sv=2015-07-08&sr=c&si=5fc7a7c0-209e-4408-bf21-f3401e7b0f0b&sig=7OAt%2F5DX6iTYGJ1hLSoWgkutBY%2FiQu0z16sd3Ih75YA%3D&st=2018-04-23T18%3A28%3A52Z&se=2118-04-23T18%3A28%3A52Z"


function takemethere(timeIndex) {
    playerInstance.currentTime(timeIndex);
}


function PopulateIndex(markerObj) {
    // var objl = document.getElementById('listIndex');
    for (var i = 0; i < markerObj.length; i++) {
        var jobj = markerObj[i];
        var strl = '<li class="list-group-item"><a href="#" onclick="takemethere(' + jobj['time'] + ')">' + jobj['text'] + '</a></li>';
        $('#listIndex').append(strl);
        // objl.appendChild(strl);
    }
}

function populatePatients(patients) {
    for (var i = 0; i < patients.length; i++) {
        var jobj = patients[i];
        var strl = '<a class="dropdown-item" href="#" onclick="patientSelected(' + jobj['id'] + ',\'' + jobj['name'] + '\')">' + jobj['name'] + '</a>';
        $('#dropdownOptions').append(strl);
    }
}

function patientSelected(pid, pname) {
    patient = {
        id: pid,
        name: pname
    };
    $('#dropdownMenuButton').text(pname);
}

function postData() {
    // get patient id from global
    var TimeStamp = $('#TimeStamp').val();
    // TimeStamp += TimeStamp+start time
    // var
    var pdata = {
        PatientID: patient.id,
        TimeStamp: TimeStamp,
        Skill: $('#skill').val(),
        Target: $('#target').val(),
        Result: $('#result').val(),
        Comments: $('#comments').val()
    };
    $.ajax({
        type: "POST",
        url: "https://jsonplaceholder.typicode.com/posts",
        data: JSON.stringify(pdata),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function(p) {
            console.log(p);
            alert("done");
        },
        failure: function(errMsg) {
            alert(errMsg);
        }
    });
}

function GetVideos() {
    // verify if patient is selected
    //
}

function addMarkpoint() {
    var pos = playerInstance.currentTime();
    addMarker(null, pos);
}

function loadJWPlayer() {
    playerInstance = jwplayer("myplayer");
    playerInstance.setup({


        file: Testfile,
        //mediaid: "xxxxYYYY",
    });
}

function loadAMP(aryTimes) {

    var myOptions = {
        autoplay: true,
        controls: true,
        width: "640",
        height: "400",
        poster: "",
        plugins: {
            //foobarbaz: {},
            timelineMarker: {
                markertime: aryTimes //["20","25", "30"]

            }
        }
    };
    playerInstance = amp("myplayer", myOptions);
    playerInstance.src([{
        src: Testfile
    }]);

}

$(document).ready(function functionName() {
    // $('#videoPlayback').hide();
    $('#postDataForm').hide();
    $('#selectPatient').hide();
    $('input[name="daterange"]').daterangepicker();

    populatePatients([{
        id: "1",
        name: "Dhishan"
    }, {
        id: "2",
        name: "Siddharth"
    }, {
        id: "3",
        name: "amulya"
    }, {
        id: "4",
        name: "Lambert"
    }]);


    // get the marker data
    var markerObj = [{
        time: "20",
        text: "Behaviour 1"
    }, {
        time: "25",
        text: "Behaviour 2"
    }, {
        time: "30",
        text: "Behaviour 3"
    }];
    PopulateIndex(markerObj);
    let ary = new Array();
    for (marker of markerObj) {
        ary.push(marker.time);
    }
    loadAMP(ary);
});
