type settable = {
    prop: string;
}

function setprop(obj: settable, str: string) {
    obj.prop = str
    if (obj.prop == "1") {
        >>>"prop is one"
    }
}

let obj: settable = {
    prop: "prop";
}

obj.setprop("1")
obj.setprop("2")
>>>obj