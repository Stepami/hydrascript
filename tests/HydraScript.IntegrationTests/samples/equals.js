type withEquals = {
    prop: number;
}

function equals(obj: withEquals, that: withEquals) {
    return obj.prop == that.prop
}

let obj1: withEquals = {
    prop: 1;
}

let obj2: withEquals = {
    prop: 2;
}

>>> obj1.equals(obj2)