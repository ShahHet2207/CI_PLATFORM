﻿function GetCity() {
    console.log("het");
    var countryId = $('#countryId').find(":selected").val();
    /*    debugger;*/
    $.ajax({
        url: "/Platform/GetCitys",
        method: "GET",
        data: {
            "countryId": countryId
        },
        success: function (data) {
            data = JSON.parse(data);
            $("#selectCityList").empty();
            document.getElementById("selectCityList").innerHTML += `
        <option value=${name}> City </option>
        `;
            data.forEach((name) => {
                document.getElementById("selectCityList").innerHTML += `
        <option value=${name.CityId} >${name.Name}</option>
        `;
            })
        },
        error: function (e) {
            console.log("Bye city");
            alert('Error');
        },
    });
}
function temp(pg) {
    if (pg == undefined) {
        pg = 1;
    }
    var checkedcntryvalues = [];
    var div1 = document.getElementById("countryId");
    var list = div1.getElementsByTagName("option");
    for (i = 0; i < list.length; i++) {
        if (list[i].selected) {
            checkedcntryvalues.push(list[i].value);
        }

    }
    console.log(checkedcntryvalues);


    var checkedvalues = [];
    var div = document.getElementById("selectCityList");
    var list = div.getElementsByTagName("option");
    for (i = 0; i < list.length; i++) {
        if (list[i].selected) {
            checkedvalues.push(list[i].value);
        }

    }
    console.log(checkedvalues);





    var checkedthemevalues = [];
    var div2 = document.getElementById("theme");
    var list = div2.getElementsByTagName("input");
    for (i = 0; i < list.length; i++) {
        if (list[i].checked) {
            checkedthemevalues.push(list[i].value);
        }

    }
    console.log(checkedthemevalues);



    var checkedskillvalues = [];
    var div3 = document.getElementById("skill");
    var list = div3.getElementsByTagName("input");
    for (i = 0; i < list.length; i++) {
        if (list[i].checked) {
            checkedskillvalues.push(list[i].value);
        }

    }
    console.log(checkedskillvalues);



    var search = document.getElementById("searchb").value;
    console.log(search)


    var sort = document.getElementById("sort").value;
    console.log(sort)


    $.ajax({
        url: '/Platform/Filter',
        type: "POST", // POST

        data: {
            'cityId': checkedvalues,
            'countryId': checkedcntryvalues,
            'themeId': checkedthemevalues,
            'skillId': checkedskillvalues,
            'search': search,
            'sort': sort,
            'pg': pg,
        },
        dataType: "html", // return datatype like JSON and HTML
        success: function (data) {


            $("#filter").empty();
            console.log("grid Hii");
            $("#filter").html(data);


            //$("#grid-view").empty();
            //console.log("grid Hii");
            //$("#grid-view").html(data);
            //$("#list-view").empty();
            //console.log("list Hii");
            //$("#list-view").html(data);

            var div1 = document.getElementById("list-view");
            div1.style.display = 'none';

        },
        error: function (e) {
            console.log("Bye");
            alert('Error');
        },
    });
}
function story(pg) {
    if (pg == undefined) {
        pg = 1;
    }
    var search = document.getElementById("searchb").value;
    console.log(search)
    //debugger
    $.ajax({
        type: "POST", // POST
        url: '/Platform/StoryFilter',
        data: {
            'search': search,
            'pg': pg,
        },
        dataType: "html", // return datatype like JSON and HTML
        success: function (data) {

            console.log(data);
            $("#StoryFilter").empty();
            $("#StoryFilter").html(data);
        },
        error: function (e) {
            /*    debugger*/
            console.log("Bye");
            alert('Error');
        },
    });
}
window.onload = opengrid();
function opengrid() {
    console.log("Grid");
    var div1 = document.getElementById("list-view");
    var div2 = document.getElementById("grid-view");
    div1.style.display = 'none';
    div2.style.display = 'block';
    console.log("Done");
}
function openlist() {
    console.log("List");
    var div1 = document.getElementById("list-view");
    var div2 = document.getElementById("grid-view");
    div1.style.display = 'block';
    div2.style.display = 'none';
    console.log("Done");
}
function AddMissionToFavourite(missionId) {

    $.ajax({

        url: '/Platform/AddMissionToFavourite',
        method: "POST",
        data: {
            'missionId': missionId,
        },
        success: function (c) {

            console.log("hii")
            if (c == true) {
                $('#addToFav').removeClass();
                $('#addToFav').addClass("bi bi-heart-fill");
                $('#addToFav').css("color", "red");

                document.getElementById(missionId).className = "bi bi-heart-fill text-danger";
            }
            else {
                $('#addToFav').css("color", "black");
                $('#addToFav').removeClass();
                $('#addToFav').addClass("bi bi-heart");

                document.getElementById(missionId).className = "bi bi-heart";
            }
        },
        error: function (request, error) {
            console.log("Bye city");
            alert('Error');
        },
    });
}
function applyMission(missionId) {
    debugger
    $.ajax({

        url: '/Platform/applyMission',
        method: "POST",
        data: {
            'missionId': missionId,
        },
        success: function (missions) {
            debugger
            if (missions == true) {
                console.log("done");
                toastr.options = {
                    "closeButton": true,
                    "progressBar": true
                };
                $('#applyMission').prop('disabled', true);
                $('#applyMission').text("Your Request has been sent for Approve");
                $('#applyMission').css("color", "red");
                /*document.getElementById("ok").innerHTML += `Applied Successfully...`;*/


                toastr.success('Applied successfully');
            }
        },
        error: function (request, error) {
            console.log("function not working");
            /* document.getElementById("ok").innerHTML += `You've already Applied...`;*/
            toastr.options = {
                "closeButton": true,
                "progressBar": true
            };
            toastr.success('function not working');
            alert('Error');
        },

    });

}
function comment(missionid) {

    //var crd = document.getElementById("comment");
    var comnt = $("#comment_text").val();

    /*    var missionId = document.getElementsByClassName("mission_id").value;*/
    console.log("kkkkkkkkkkkkkkkkkkkkk");
    console.log(comnt);


    $.ajax({
        url: "/Platform/AddComment",
        type: "POST", // POST
        data: {
            'obj': missionid,
            'comnt': comnt
        },
        dataType: "html", // return datatype like JSON and HTML
        success: function (data) {


            toastr.options = {
                "closeButton": true,
                "progressBar": true
            };
            $("#comment").html();
            console.log("Added ");
            toastr.success('Comment Added  successfully');
            setTimeout(function () { window.location.reload(); }, 3000);
        },
        error: function (e) {
            console.log("Bye");
            alert('Error');
        },
    });
}
function recommandToCoWorker(x) {
    //var toUserId = $('#recommand').find(":checked").val();
    var Missiond = x;
    var toUserId = [];
    var recommand = document.getElementById("recommand");
    var list = recommand.getElementsByTagName("input");
    for (i = 0; i < list.length; i++) {
        if (list[i].checked) {
            toUserId.push(list[i].value);
        }

    }

    /* debugger;*/
    $.ajax({
        url: "/Platform/RecommandToCoWorker",
        method: "Post",
        data: {
            "toUserId": toUserId,
            "mid": Missiond
        },
        success: function (data) {
            toastr.options = {
                "closeButton": true,
                "progressBar": true
            };
            console.log(toUserId);
            toastr.success('Email send  successfully');

        }
        ,
        error: function (e) {
            console.log("Bye");
            alert('Error');
        },
    });
}
function recommandStory(x) {
    //var toUserId = $('#recommand').find(":checked").val();
    var Storyd = x;
    var toUserId = [];
    var recommand = document.getElementById("recommand");
    var list = recommand.getElementsByTagName("input");
    for (i = 0; i < list.length; i++) {
        if (list[i].checked) {
            toUserId.push(list[i].value);
        }

    }

    /* debugger;*/
    $.ajax({
        url: "/Platform/RecommandStory",
        method: "Post",
        data: {
            "toUserId": toUserId,
            "sid": Storyd
        },
        success: function (data) {
            toastr.options = {
                "closeButton": true,
                "progressBar": true
            };
            console.log(toUserId);
            toastr.success('Email send  successfully');


        }
        ,
        error: function (e) {
            console.log("Bye");
            alert('Error');
        },
    });
}

function Sdata() {
    var div1 = document.getElementById("getData");
    var selectedOption = div1.options[div1.selectedIndex];
    var selectedValue = selectedOption.value;
    console.log(selectedValue);
    debugger
    $.ajax({
        type: "POST", // POST
        url: '/Platform/CheckData',
        data: {
            'mid': selectedValue,
        },
        success: function (data) {
            console.log(data);
            data = JSON.parse(data);
            console.log(data);
            {
                let myDateObj = new Date(data.story.PublishedAt);
                const year = myDateObj.getFullYear();
                const month = String(myDateObj.getMonth() + 1).padStart(2, '0');
                const day = String(myDateObj.getDate()).padStart(2, '0');
                const formattedDate = `${year}-${month}-${day}`;
                data.PublishedAt = formattedDate;

                // The data exists, so populate the form with the retrieved data
                $("#formGroupExampleInput").val(data.story.Title);

                $("#sDate").val(data.PublishedAt);
                console.log(data.Description);
                var editor1 = CKEDITOR.instances.editor;

                // Set the content of the editor
                editor1.setData('Your content goes here');

            }
            debugger
        },
        error: function (e) {
            debugger
            alert('Error');
        },
    });
}