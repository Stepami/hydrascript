type ObjType = {
    num: number;
    flag: boolean;
    str: string;
    arr: number[];
    nullableFlag: boolean?;
}

function toString(obj: ObjType): string {
    let s = "object obj: "
    return s + (obj as string)
}

let obj: ObjType = {
    num: 1;
    flag: true;
    str: "field";
    arr: [1, 2, 3];
    nullableFlag: null;
}

>>> obj.toString()