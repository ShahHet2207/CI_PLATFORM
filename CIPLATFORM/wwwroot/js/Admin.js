﻿function searchuser(pg, key) {
    if (pg == undefined) {
        pg = 1;
    }
    if (key == "user") {
        var search = document.getElementById("searchuser").value;
    }
    if (key == "cms") {
        var search = document.getElementById("searchcms").value;
    }
    if (key == "mission") {
        var search = document.getElementById("searchmission").value;
    }
    if (key == "theme") {
        var search = document.getElementById("searchtheme").value;
    }
    if (key == "skill") {
        var search = document.getElementById("searchskill").value;
    }
    if (key == "application") {
        var search = document.getElementById("searchapplication").value;
    }
    if (key == "story") {
        var search = document.getElementById("searchstory").value;
    }
    console.log(search)

    $.ajax({
        type: "POST", // POST
        url: '/Admin/UserFilter',
        data: {
            'search': search,
            'pg': pg,
            'key': key,
        },
        dataType: "html", // return datatype like JSON and HTML
        success: function (data) {

            console.log(data);
            if (key == "user") {
                $("#hi").empty();
                $("#hi").html(data);
            }
            else if (key == "cms") {
                $("#hi2").empty();
                $("#hi2").html(data);
            }
            else if (key == "mission") {
                $("#hi3").empty();
                $("#hi3").html(data);
            }
            else if (key == "theme") {
                $("#hi4").empty();
                $("#hi4").html(data);
            }
            else if (key == "skill") {
                $("#hi5").empty();
                $("#hi5").html(data);
            }
            else if (key == "application") {
                $("#hi6").empty();
                $("#hi6").html(data);
            }
            else if (key == "story") {
                $("#hi7").empty();
                $("#hi7").html(data);
            }
            else if (key == "banner") {                $("#hi8").empty();                $("#hi8").html(data);            }
        },
        error: function (e) {
            console.log("Bye");
            alert('Error');
        },
    });
}

function getdata(x, id) {

    var page = document.getElementById(x);

    var addForm = page.querySelector("#edit");



    $.ajax({
        url: "/Admin/EditForm",
        method: "Post",
        data:
        {
            "id": id,
            "page": x,
        },
        success: function (data) {

            console.log(data);
            var htmlObject = document.createElement('div');
            htmlObject.innerHTML = data;

            var abc = htmlObject.querySelector("#edit");
            abc.style.display = "block";

            if (x == "nav-mission") {
                var missionSkill = abc.querySelector("#photos");
                console.log(missionSkill);
                var ck = [];
                missionSkill.querySelectorAll(".Dimg").forEach((element) => {
                    ck.push(element.value);
                });
                console.log(ck);
            }

            console.log(abc);
            console.log(addForm);

            addForm.replaceWith(abc);
            if (x == "nav-cms") {
                var abc = document.getElementById("cms2");
                CKEDITOR.replace(abc);
            }
            if (x == "nav-user") {                $("#profileImageInput1").on('change', function () {
                    console.log("2");
                    readURL1(this);
                });            }
            if (x == "nav-mission") {                var abc = document.getElementById("mission2");
                CKEDITOR.replace(abc);
                const inputDiv = document.querySelector(".input-div")
                const input = document.querySelector("#imageupload")
                const output = document.querySelector("#preview")
                let imagesArray = []

                input.addEventListener("change", () => {

                    const files = input.files
                    for (let i = 0; i < files.length; i++) {
                        imagesArray.push(files[i])
                    }
                    console.log(files);
                    displayImages()

                })

                inputDiv.addEventListener("drop", () => {
                    e.preventDefault()
                    const files = e.dataTransfer.files
                    for (let i = 0; i < files.length; i++) {
                        if (!files[i].type.match("image")) continue;

                        if (imagesArray.every(image => image.name !== files[i].name))
                            imagesArray.push(files[i])
                    }
                    console.log(files);
                    displayImages()
                })
                function displayImages() {

                    let images = ""
                    imagesArray.forEach((image, index) => {
                        images += `<div class="image storyimages">
                                        <img src="${URL.createObjectURL(image)}" alt="image">
                                        <span onclick="deleteImg(${index})">&times;</span>
                                   </div>`
                    })
                    output.innerHTML = images
                }

                function deleteImg(index) {
                    imagesArray.splice(index, 1)
                    displayImages()
                }

                if (ck.length > 0) {

                    function toDataUrl(url, callback) {
                        console.log(url);
                        var newUrl = url;
                        var xhr = new XMLHttpRequest();
                        xhr.onload = function () {
                            callback(xhr.response);

                        };
                        xhr.open('GET', newUrl);
                        xhr.responseType = 'blob';
                        xhr.send();
                    }
                    const dT = new DataTransfer();
                    let image;
                    let images = ""
                    let returnImage = ""
                    ck.forEach((img, index) => {
                        returnImage = img;
                        img = "/images/" + img;
                        toDataUrl(img, function (x) {
                            image = x;
                            dT.items.add(new File([image], returnImage, {
                                type: "image/png"
                            }));
                            imagesArray.push(new File([image], returnImage, {
                                type: "image/png"
                            }));
                            document.querySelector('#imageupload').files = dT.files;
                            console.log(document.querySelector('#imageupload'));
                            console.log(document.querySelector('#imageupload').files);
                        });


                        images += `<div class="image">
                                        <img src="${img}" alt="image">
                                        <span onclick="deleteImg(${index})">&times;</span>
                                   </div>`

                    });

                    output.innerHTML = images;

                }
                else {
                    output.innerHTML = "No Images Are Choosen";
                }
            }


        },
        error: function (e) {
            debugger
            console.log("Bye");
            alert('Error');
        },
    });

}


function GetsCity(x) {
    if (x != undefined) {
        var countryId = $('#countryId' + x).find(":selected").val();
        var city = "selectCityList" + x;
    }
    else {
        var countryId = $('#countryId').find(":selected").val();
        var city = "selectCityList";
    }
    $.ajax({
        url: "/Platform/GetCitys",
        method: "GET",
        data: {
            "countryId": countryId
        },
        success: function (data) {
            data = JSON.parse(data);
            $("#" + city).empty();

            document.getElementById(city).innerHTML += `
        <option value=${name}> City </option>
        `;
            data.forEach((name) => {
                document.getElementById(city).innerHTML += `
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
