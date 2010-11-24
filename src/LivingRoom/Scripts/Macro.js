

function ExecuteCommand(device, command, param) {
    // alert("Executing " + device + '.' + command + "(" + param + ")");
    $.get(ExecuteCommandURL,
                { Device: device, Command: command, Parameter: param },
                function (data) {
                    // alert(data);
                });
};

function ExecuteQuery(device, command, callback) {
    $.get(ExecuteQueryURL,
                { Device: device, Command: command },
                function (data) {
                    callback(data);
                });
};

function ExecuteMacro(command, params) {
    $.get(ExecuteMacroURL,
                { Command: command, Parameters: params },
                function (data) {
                    // alert(data);
                });
};

//pads left
String.prototype.lpad = function (padString, length) {
    var str = this;
    while (str.length < length)
        str = padString + str;
    return str;
}

//pads right
String.prototype.rpad = function (padString, length) {
    var str = this;
    while (str.length < length)
        str = str + padString;
    return str;
}