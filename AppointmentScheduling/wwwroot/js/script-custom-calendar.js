var RouteUrl =location.protocol+'/';
$(document).ready(function () {
    $("#appointmentDate").kendoDateTimePicker({
     
        DateInput: false,
        value: new Date()
    });
    InitializeCalendar();
});
/*
function InitializeCalendar() {
    try {
        $("#calendar").fullCalendar({
                 header: {
                left: 'prev,today,next',
                center: 'title',
                right: 'month,agendaWeek,agendaDay'
            },
            selectable: true,
            editable: false,
            select: function (event) {
                onShowModal(event, null)
            },
            events: function (fetchInfo, successCallback, failureCallBack) {
                $.ajax({
                    url: "https://localhost:7137/api/appointment/GetCalendarData?doctorId=" + $("#Doctorid").val(),
                    type: 'GET',
                    dataType: 'JSON',
                    success: function (response) {
                        if (response.status === 1) {
                            $.each(response.dataenum, function (i, data) {
                                var event = {
                                    title: data.title,
                                    description: data.description,
                                    start: data.startDate,
                                    end: data.endDate,
                                    duration: data.duration,
                                    doctorId: data.doctorId,
                                    patientId: data.patientId,
                                    backgroundColor: data.doctorApproved ? "#28a75" : "#dc3545",
                                    borderColor: "#162466",
                                    textColor: "white",
                                    id: data.id,
                                    allDay: true
                                };
                                $("#calendar").fullCalendar('renderEvent', event);
                            });
                            successCallback([]);
                        } else {
                            failureCallBack(response.message);
                        }
                    },
                    error: function (xhr, status, error) {
                        errorCallback(error);
                    }
                });
            },
           
            eventClick: function (info) {
                getEventDetailsbyEventId(info.event);
            }
        });
      
    } catch (e) {
        alert(e);
    }
}
*/
var calendar;
function InitializeCalendar() {
    try {


        var calendarEl = document.getElementById('calendar');
        if (calendarEl != null) {
            calendar = new FullCalendar.Calendar(calendarEl, {
                initialView: 'dayGridMonth',
                headerToolbar: {
                    left: 'prev,next,today',
                    center: 'title',
                    right: 'dayGridMonth,timeGridWeek,timeGridDay'
                },
                selectable: true,
                editable: false,
                select: function (event) {
                    onShowModal(event, null);
                },
                eventDisplay: 'block',
                events: function (fetchInfo, successCallback, failureCallback) {
                    $.ajax({
                        url: "https://localhost:7137/api/appointment/GetCalendarData?doctorId=" + $("#Doctorid").val(),
                        type: 'GET',
                        dataType: 'JSON',
                        success: function (response) {
                            var events = [];
                            if (response.status === 1) {
                                $.each(response.dataenum, function (i, data) {
                                    events.push({
                                        title: data.title,
                                        description: data.description,
                                        start: data.startDate,
                                        end: data.endDate,
                                        backgroundColor: data.doctorApproved ? "#28a745" : "#dc3545",
                                        borderColor: "#162466",
                                        textColor: "white",
                                        id: data.id
                                    });
                                })
                            }
                            successCallback(events);
                        },
                        error: function (xhr) {
                            $.notify("Error", "error");
                        }
                    });
                },
                eventClick: function (info) {
                    getEventDetailsByEventId(info.event);
                }
            });
            calendar.render();
        }

    }
    catch (e) {
        alert(e);
    }

}



function onShowModal(obj, isEventDetail) {
    if (isEventDetail != null) {
        $("#title").val(obj.title);
        $("#description").val(obj.description);
        $("#appointmentDate").val(obj.startDate);
        $("#duration").val(obj.duration);
        $("#Doctorid").val(obj.doctorId);
        $("#PatientId").val(obj.patientId);
        $("#lblPatientName").html(obj.patientName);
        $("#lblDoctorName").html(obj.doctorName);
        if (obj.DoctorApproved) {
            $("#lblStatus").html('Approved')
            $("#btnConfirm").addClass("d-none");
            $("#btnSubmit").addClass("d-none");
        }
        else {
            $("#lblStatus").html('pending')
            $("#btnConfirm").removeClass("d-none");
            $("#btnSubmit").removeClass("d-none");

        }
        $("#btnDelete").removeClass("d-none");

    } else {
        $("#appointmentDate").val(obj.startStr + " " + new moment().format("hh:mm A"));
       
     //   $("#id").val(0);
        $("#btnDelete").addClass("d-none");
        $("#btnSubmit").removeClass("d-none");
    }
    $('#AppointmentInput').modal("show")
}
function OnCloseModal() {
   

    $('#AppointmentInput').modal("hide")
}
function onSubmitForm() {
    if (isValidation) {
        startDateString = $("#appointmentDate").val()
        startDate = new Date(startDateString)
        formattedStartDate = startDate.toISOString()
        var idString = $("#idInput").val();
        var id = parseInt(idString);
        var requestData = {
            Id: id,
            Title: $("#title").val(),
            Description: $("#description").val(),
            StartDate: formattedStartDate,
            Duration: $("#duration").val(),
            DoctorId: $("#Doctorid").val(),
            PatientId: $("#PatientId").val(),
            AdminId: null

        };
    }

    $.ajax({
        url: "https://localhost:7137/api/appointment/SaveCalendarDate",
        type: 'POST',
        data: JSON.stringify(requestData),
        contentType: 'application/json',
        success: function (response) {
            if (response.status == 1 || response.status == 2) {
                calendar.refetchEvents();
                $.notify(response.message, "success");
                OnCloseModal();
            } else {
                console.log(response.message)
                $.notify(response.message, "error");
            }
        },
        error: function (xhr) {
            console.log(xhr);
            if (xhr.status === 400) {
                var errorMessage = JSON.parse(xhr.responseText);
                console.log("Error:", errorMessage);
                $.notify("Bad Request: " + errorMessage, "error");
            } else {
                $.notify("Error", "error");
            }
        }
    }).fail(function (xhr, status, error) {
        console.log('Error :', error)
    });
}


function isValidation() {
    var isValid = true;
    if ($('#title').val() === undefined || $('#title').val() === "") {
        isValid = false;
        $('#title').addClass("error");
    } else {
        $("#title").removeClass("error")
    }
    if ($("#appointmentDate").val() === undefined || $("#appointmentDate").val() === "") {
        isValid = false;
        $('#appointmentDate').addClass("error");
    } else {
        $("#appointmentDate").removeClass("error")
    }
    return isValid;
}

function getEventDetailsByEventId(info) {
    $.ajax({
        url: "https://localhost:7137/api/appointment/GetCalenderDataById/"+ info.id,
        type: 'GET',
      
       dataType: 'JSON',
        success: function (response) {
            if (response.status == 1 && response.dataenum !== undefined) {
                onShowModal(response.dataenum,true)

            } else {
                $.notify("Error", "error");
            }
        }

    });
}
function OnChangeDoctor() {
    calendar.refetchEvents();
}

function onDeleteAppointment() {
    var idString = $("#idInput").val();
    var id = parseInt(idString);
    console.log(id)
    $.ajax({
        url: 'https://localhost:7137/api/appointment/DeleteAppoinment/' + id,
        type: 'GET',
        dataType: 'JSON',
        success: function (response) {

            if (response.status === 1) {
                $.notify(response.message, "success");
                calendar.refetchEvents();
                onCloseModal();
            }
            else {

                $.notify(response.message, "error");
            }
        },
        error: function (xhr) {
            $.notify("Error", "error");
        }
    });
}

    function onConfirm() {
        var id = $("#id").val();
        $.ajax({
            url: 'https://localhost:7137/api/appointment/ConfirmEvent/' + id ,
            type: 'GET',
            dataType: 'JSON',
            success: function (response) {
                if (response.status === 1) {
                    $.notify(response.message, "success");
                    calendar.refetchEvents();
                    onCloseModal();
                } else {
                    $.notify(response.message, "error");
                }
            },
            error: function (xhr) {
                $.notify("Error", "error");
            }
        });
    }
