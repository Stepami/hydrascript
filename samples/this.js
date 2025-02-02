type ObjType = {
    num: number;
    flag: boolean;
    str: string;
}

function toString(obj: ObjType): string {
    let s = "object obj: "
    return s + (obj as string)
}

let obj: ObjType = {
    num: 1;
    flag: true;
    str: "field";
}

>>> obj.toString()