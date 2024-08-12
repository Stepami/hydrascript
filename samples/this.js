type ObjType = {
    num: number;
    flag: boolean;
    str: string;
}

function toString(obj: ObjType): string {
    let s = "object obj:\n"
    return s + (obj as string)
}

let obj: ObjType = {
    num: 1;
    flag: true;
    str: "field";
}

print(obj.toString())